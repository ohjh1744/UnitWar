using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private UnitController _unitController;

    private UnitData _data;

    private Collider2D _detectEnemy;

    private IDamageable _damageAble;

    private float _curDamageRate;

    private Animator _animator;

    private int _hashAttackFront;

    private int _hashAttackBack;

    private int _hashAttackRight;

    private Vector3 _attackDir; //공격방향

    private SpriteRenderer _render;

    public AttackState(UnitController controller)
    {
        //생성자
        _unitController = controller;
        _data = _unitController.UnitData;

        _animator = _unitController.GetComponent<Animator>();
        _render = _unitController.GetComponent<SpriteRenderer>();

        _hashAttackFront = Animator.StringToHash("Attack_Front");
        _hashAttackBack = Animator.StringToHash("Attack_Back");
        _hashAttackRight = Animator.StringToHash("Attack_Right");
    }


    public void OnEnter()
    {
        Debug.Log("Attack상태 진입");        

        _curDamageRate = 0;        
    }

    public void OnUpdate()
    {
        Debug.Log("공격중!");

        if (_data.HP <= 0 || (GameSceneManager.Instance.IsFinish == true && _unitController.photonView.IsMine == true)) 
        {
            //_unitController.ChangeState(_unitController.States[(int)EStates.Dead]);
            _unitController.photonView.RPC("ChangeState", RpcTarget.All, (int)EStates.Dead);
        }
        if (_data.Path != null && _data.Path.Count == _data.PathIndex && _data.HitObject == null)
        {
            //_unitController.ChangeState(_unitController.States[(int)EStates.Idle]);
            _unitController.photonView.RPC("ChangeState", RpcTarget.All, (int)EStates.Idle);
        }
        if ((_data.HasReceivedMove == true || _data.HitObject == null) && _data.Path.Count > 0 && _data.PathIndex != _data.Path.Count)
        {
            //_unitController.ChangeState(_unitController.States[(int)EStates.Walk]);
            _unitController.photonView.RPC("ChangeState", RpcTarget.All, (int)EStates.Walk);
        }

        DoAttack();

    }

    public void OnExit()
    {
        Debug.Log("Attack상태 탈출");
        StopAni();
    }

    
    /// <summary>
    /// 항상, 유닛의 외부 테두리와 상대의 외부 테두리가 충돌하는지 검사합니다.
    /// 유닛의 바깥 테두리 안에 상대가 들어온 경우, 안쪽 테두리 까지 들어오면 공격합니다.
    /// </summary>
    public void DoAttack()
    {
        _curDamageRate += Time.deltaTime;

        if(_data.HitObject == null)
        {
            return;
        }

        if (_curDamageRate > _data.DamageRate)
        {
            IDamageable damageable = _data.HitObject.GetComponent<IDamageable>();
            if(damageable != null)
            {
                damageable.GetDamage(_data.Power);
            }
            _curDamageRate = 0;
            _unitController.Audio.PlayOneShot(_data.AudioCLips[(int)ESound.Attack]);
        }

        // 상대 unit과의 바라보는 방향 계산하여 애니메이션 동작.
        _attackDir = _data.HitObject.transform.position - _unitController.transform.position;
        PlayAttackAnimation();

    }

    //FIX ME: Walk 애니메이션은 방향에 따라 좌,우만 재생 (24.11/15 16:30)
    private void PlayAttackAnimation()
    {

        if (_attackDir.normalized.x >= 0)
        {
            //오른쪽 공격 애니메이션 작동
            _render.flipX = false;
            _animator.Play(_hashAttackRight);
        }
        else if (_attackDir.normalized.x < 0)
        {
            //왼쪽 공격 애니메이션 작동
            _render.flipX = true;
            _animator.Play(_hashAttackRight);
        }
    }
    private void StopAni()
    {
        _animator.StopPlayback();
    }

}
