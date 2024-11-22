using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum EStates { Idle, Walk, Attack, Dead, Size}
public class UnitController : MonoBehaviour, IDamageable
{

    [SerializeField] private UnitData _unitData;
    public UnitData UnitData { get { return _unitData; } set { } }

    private IState _currentState;
    private IState[] _states = new IState[(int)EStates.Size];    
    public IState[] States { get { return _states; } set { } }

    private GameObject _attackTarget;
    public GameObject AttackTarget { get { return _attackTarget; } set { } }






    private void Awake()
    {
        _states[(int)EStates.Idle] = new IdleState(this);
        _states[(int)EStates.Walk] = new IdleState(this);
        _states[(int)EStates.Attack] = new IdleState(this);
        _states[(int)EStates.Dead] = new IdleState(this);

    }

    private void Start()
    {
        ChangeState(_states[(int)EStates.Idle]);
        //DetectColider = _unitData.DetectColider;
        //HitColider = _unitData.HitColider;

    }


    private void Update()
    {
        _currentState?.OnUpdate();

        _unitData.DetectColider = Physics2D.OverlapCircle(this.transform.position, _unitData.OutRadius);
        _unitData.HitColider = Physics2D.OverlapCircle(this.transform.position, _unitData.InRadius);

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

    
    public void GetDamage()
    {
        _unitData.HP -= _unitData.Power;
    }


    //TO DO: PlayerController쪽에서 attackTarget을 지정해줌. 
    public void GetTarget()
    {
        //AttackTarget
    }

}
