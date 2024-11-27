using Photon.Pun;
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
        photonView.RPC("ChangeState", RpcTarget.All, (int)EStates.Idle);
        //ChangeState(_states[(int)EStates.Idle]);
    }

    private void Update()
    {
        // AttackTarget이 정해져 있는 상태에서 그 Attack Target이 죽었다면 AttackTarget 초기화.
        if (_unitData.AttackTarget != null && _unitData.AttackTarget.activeSelf == false)
        {
            _unitData.AttackTarget = null;
        }

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
                // 자기 자신이나 장애물, 아군 종족은  Hit대상으로 x,
                if (_unitData.HitColiders[i].gameObject != gameObject && _unitData.HitColiders[i].tag != "Obstacle" && otherUnit.UnitType != _unitData.UnitType)
                {
                    _unitData.HitObject = _unitData.HitColiders[i];
                    // 만약 공격대상이 지정된 경우, 공격대상을 HitObject로 변경.
                    if (_unitData.AttackTarget != null)
                    {
                        _unitData.HitObject = _unitData.AttackTarget.GetComponent<Collider2D>();
                    }
                    break;
                }
            }
        }

        _currentState?.OnUpdate();
    }

    private void ResetData()
    {
        _unitData.HP = 100;
        _unitData.Path.Clear();
        _unitData.PathIndex = 0;
        _unitData.HitObject = null;
        _unitData.AttackTarget = null;
        _unitData.HasReceivedMove = false;
        
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
