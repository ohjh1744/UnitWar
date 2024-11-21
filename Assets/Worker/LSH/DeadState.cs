using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : MonoBehaviour, IState
{
    private UnitController _unitController;

    public DeadState(UnitController controller)
    {
        //»ý¼ºÀÚ
        _unitController = controller;
    }


    public void OnEnter()
    {

    }

    public void OnUpdate()
    {
        Dead();
    }

    public void OnExit()
    {

    }


    public void Dead()
    {

    }


}
