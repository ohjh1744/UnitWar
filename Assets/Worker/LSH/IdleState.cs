using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;

public class IdleState : UnitState
{
    private UnitData _data;

    private Animator _animator;

    private int _hashIdleFront;

    private int _hashIdleBack;

    private int _hashIdleRight;

    public IdleState(UnitController unit) : base(unit)
    {

        _data = Unit.UnitData;

        _animator = Unit.GetComponent<Animator>();

        _hashIdleFront = Animator.StringToHash("Idle_Front");
    }


    public override void OnEnter()
    {
        Debug.Log("Idle상태 진입");
        
    }

    public override void OnUpdate()
    {
        if (_data.HP <= 0 || (GameSceneManager.Instance.IsFinish == true && Unit.photonView.IsMine == true))
        {
            //_unitController.ChangeState(_unitController.States[(int)EStates.Dead]);
            Unit.photonView.RPC("ChangeState", RpcTarget.All, (int)EStates.Dead);
        }

        if (_data.Path.Count > 0 && _data.PathIndex != _data.Path.Count)        
        {
            // _unitController.ChangeState(_unitController.States[(int)EStates.Walk]);
            Unit.photonView.RPC("ChangeState", RpcTarget.All, (int)EStates.Walk);
        }
        if (_data.HitObject != null)
        {
            // _unitController.ChangeState(_unitController.States[(int)EStates.Attack]);
            Unit.photonView.RPC("ChangeState", RpcTarget.All, (int)EStates.Attack);
        }

        DoIdle();

    }

    public override void OnExit()
    {
        Debug.Log("Idle상태 탈출");
    }


    private void DoIdle()
    {
        Debug.Log("Idle 상태 진행중");
        PlayIdleAnimation();

    }

    //FIX ME: Idle 애니메이션은 아래 내려보는 방향만 재생 (24.11/15 16:30)
    private void PlayIdleAnimation()
    {
        _animator.Play(_hashIdleFront);             
    }

    private void StopAni()
    {
        _animator.StopPlayback();
    }


}
