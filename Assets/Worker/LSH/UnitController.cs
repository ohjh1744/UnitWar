using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum EStates { Idle, Walk, Attack, Dead, Size}
public class UnitController : MonoBehaviourPun, IDamageable
{

    [SerializeField] private AStar _aStar;
    public AStar AStar { get { return _aStar; } set { _aStar = value; } }

    [SerializeField] private UnitData _unitData;
    public UnitData UnitData { get { return _unitData; } set { } }

    private IState _currentState;

    private IState[] _states = new IState[(int)EStates.Size];
    public IState[] States { get { return _states; } set { } }

    [SerializeField] private AudioSource _audio;

    public AudioSource Audio { get { return _audio; } private set { } }

    private GameObject _curAttacktarget;
    private void Awake()
    {
        _states[(int)EStates.Idle] = new IdleState(this);
        _states[(int)EStates.Walk] = new WalkState(this);
        _states[(int)EStates.Attack] = new AttackState(this);
        _states[(int)EStates.Dead] = new DeadState(this);
       
    }
    private void OnEnable()
    {
        //죽고 Pull에서 다시 생성될때, Data Reset해주기
        ResetData();
        _curAttacktarget = _unitData.AttackTarget;
        photonView.RPC("SetTrue", RpcTarget.All);
        photonView.RPC("ChangeState", RpcTarget.All, (int)EStates.Idle);
    }

    private void Update()
    {
        UpdateAttackTarget();
        CheckHitObject();
        _currentState?.OnUpdate();
    }

    private void UpdateAttackTarget()
    {
        //어택타겟이 달라진다면 Rpc를 통해 attackTarget바꿔주기
        if (_curAttacktarget != UnitData.AttackTarget)
        {
            int viewid;
            if (_unitData.AttackTarget == null)
            {
                viewid = 0;
            }
            else
            {
                viewid = _unitData.AttackTarget.GetComponent<PhotonView>().ViewID;
            }
            photonView.RPC("SetAttackTarget", RpcTarget.All, viewid);
        }

        // AttackTarget이 정해져 있는 상태에서 그 Attack Target이 죽었다면 AttackTarget 초기화.
        if (_unitData.AttackTarget != null && _unitData.AttackTarget.activeSelf == false)
        {
            _unitData.AttackTarget = null;
        }
    }

    private void CheckHitObject()
    {
        // 공격 가능한 대상들 체크
        _unitData.HitColiders = Physics2D.OverlapCircleAll(this.transform.position, _unitData.HitRadius);

        // Length == 1 즉, 본인은 대상으로 x. Player가 직접적으로 공격대상을 지정해준 경우. 랜덤공격대상은 null 로
        if (_unitData.HitColiders.Length == 1)
        {
            _unitData.HitObject = null;
        }
        else //  Hit할수 있는 대상들이 많은 경우
        {
            // 상대Unit이 먼저 때린 경우로, 이런 경우 랜덤으로 대상 정해서 공격하기
            for (int i = 0; i < _unitData.HitColiders.Length; i++)
            {
                UnitData otherUnit = _unitData.HitColiders[i].GetComponent<UnitData>();
                PlayerData otherPlayer = _unitData.HitColiders[i].GetComponent<PlayerData>();
                // 자기 자신이나 장애물, 아군 종족 및 Player은  Hit대상으로 x,
                if (_unitData.HitColiders[i].gameObject != gameObject && _unitData.HitColiders[i].tag != "Obstacle" && ((otherUnit != null && otherUnit.UnitType != _unitData.UnitType) || (otherPlayer != null && otherPlayer.UnitType != _unitData.UnitType)))
                {
                    _unitData.HitObject = _unitData.HitColiders[i];
                    // 만약 공격대상이 지정된 경우, 공격대상을 HitObject로 변경.  -> 지금 문제는 AttackTarget이 HitColiders에 없어도, AttackTarget을 때리는 문제 발생.
                    if (_unitData.AttackTarget != null && Array.Exists(_unitData.HitColiders, c => c.gameObject == _unitData.AttackTarget))
                    {
                        _unitData.HitObject = _unitData.AttackTarget.GetComponent<Collider2D>();
                    }
                    break;
                }
                else
                {
                    _unitData.HitObject = null;
                }

            }
        }
    }


    private void ResetData()
    {
        _unitData.HP = _unitData.OriginHp;
        _unitData.Path.Clear();
        _unitData.PathIndex = 0;
        _unitData.HitObject = null;
        _unitData.AttackTarget = null;
        _unitData.HasReceivedMove = false;
        
    }

    [PunRPC]
    public void SetAttackTarget(int otherid)
    {
        if(otherid == 0)
        {
            _unitData.AttackTarget = null;
            _curAttacktarget = _unitData.AttackTarget;
        }
        else
        {
            PhotonView other = PhotonView.Find(otherid);
            _unitData.AttackTarget = other.gameObject;
            _curAttacktarget = _unitData.AttackTarget;
        }
    }

    [PunRPC]
    public void SetTrue()
    {
        gameObject.SetActive(true);
    }

    [PunRPC]
    public void ChangeState(int newStateIndex)
    {
        if (_currentState != null)
        {
            _currentState.OnExit();
        }

        _currentState = _states[newStateIndex];
        _currentState.OnEnter();

    }

    public void GetDamage(int damage)
    {
        _unitData.HP -= damage;
    }

}
