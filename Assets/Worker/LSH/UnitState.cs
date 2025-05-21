using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public abstract class UnitState : IState
{
    private UnitController _unit;
    public UnitController Unit {  get { return _unit; } set { _unit = value; } }

    public UnitState(UnitController unit)
    {
        _unit = unit;
    }

    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();
}
