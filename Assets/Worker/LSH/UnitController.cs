using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum EStates { Idle, Walk, Attack, Dead, Size}
public class UnitController : MonoBehaviour, IDamageable
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

    private void Start()
    {
        ChangeState(_states[(int)EStates.Idle]);

    }


    private void Update()
    {
        if (_unitData.AttackTarget != null && _unitData.AttackTarget.activeSelf == false)
        {
            _unitData.AttackTarget = null;
        }

        // 감지 대상 체크
        _unitData.DetectColider = Physics2D.OverlapCircleAll(this.transform.position, _unitData.DetectRadius);

        // Length == 1 즉, 본인은 대상으로 x.
        if(_unitData.DetectColider.Length == 1)
        {
            _unitData.DetectObject = null;
        }
        else
        {
            for (int i = 0; i < _unitData.DetectColider.Length; i++)
            {
                // 자기 자신이나 장애물은 감지대상으로 x.
                if (_unitData.DetectColider[i].gameObject != gameObject || _unitData.DetectColider[i].tag != "Obstacle")
                {
                    _unitData.DetectObject = _unitData.DetectColider[i];
                    break;
                }
            }
        }

        // Hit 대상 체크
        _unitData.HitColider = Physics2D.OverlapCircleAll(this.transform.position, _unitData.HitRadius);

        // Length == 1 즉, 본인은 대상으로 x. Player가 직접적으로 공격대상을 지정해준 경우. 랜덤공격대상은 null 로
        if (_unitData.HitColider.Length == 1)
        {
            _unitData.HitObject = null;
        } 
        else //  Hit할수 있는 대상들이 많은 경우
        {
            // 상대Unit이 먼저 때린 경우로, 이런 경우 랜덤으로 대상 정해서 공격하기
            for (int i = 0; i < _unitData.HitColider.Length; i++)
            {
                // 자기 자신이나 장애물은 Hit대상으로 x.
                if (_unitData.HitColider[i].gameObject != gameObject && _unitData.HitColider[i].tag != "Obstacle")
                {
                    _unitData.HitObject = _unitData.HitColider[i];
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

    public void ChangeState(IState newState)
    {
        if (_currentState != null)
        {
            _currentState.OnExit();
        }

        _currentState = newState;
        _currentState.OnEnter();
    }

    public void GetDamage(int damage)
    {
        _unitData.HP -= damage;
    }

}
