using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum EStates { Idle, Walk, Attack, Dead, Size}
public class UnitController : MonoBehaviour
{

    [SerializeField] private UnitData _unitData;
    public UnitData UnitData { get; set; } //天天天天天天天天天天天

    private IState _currentState; //============================
    private IState[] _states = new IState[(int)EStates.Size];    
    public IState[] States { get; set; } //天天天天天天天天天天天天


    private Collider2D _hitEnemy;
    public Collider2D HitEnemy { get { return _hitEnemy} set { } }

    [SerializeField] private float _outRadius;
    public float OutRadius { get { return _outRadius; } set { } }

    [SerializeField] private float _inRadius;
    public float InRadius { get { return _inRadius; } set { } }



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
    }


    private void Update()
    {
        _currentState.OnUpdate(); //---------------------------------------

        HitEnemy = Physics2D.OverlapCircle(this.transform.position, _outRadius);
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


}
