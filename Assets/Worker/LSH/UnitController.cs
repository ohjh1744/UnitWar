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



    private Collider2D _detect;
    public Collider2D Detect { get { return _detect; } set { } }

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
        _currentState?.OnUpdate();

        Detect = Physics2D.OverlapCircle(this.transform.position, _outRadius);
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


}
