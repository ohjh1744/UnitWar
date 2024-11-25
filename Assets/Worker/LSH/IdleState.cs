using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class IdleState : MonoBehaviour, IState
{
    private UnitController _unitController;
    private UnitData _data;
    private int _hashIdle;

    public IdleState(UnitController controller)
    {
        //생성자
        _unitController = controller;
        _data = _unitController.UnitData;
    }


    public void OnEnter()
    {
        Debug.Log("Idle상태 진입");
    }

    public void OnUpdate()
    {
        if (_data.HP <= 0)
        {
            _unitController.ChangeState(_unitController.States[(int)EStates.Dead]);
        }

        if (_data.Path.Count > 0 && _data.PathIndex != _data.Path.Count)        
        {
            _unitController.ChangeState(_unitController.States[(int)EStates.Walk]);
        }
        if (_data.HitObject != null)
        {
            _unitController.ChangeState(_unitController.States[(int)EStates.Attack]);
        }

        DoIdle();

    }

    public void OnExit()
    {
        Debug.Log("Idle상태 탈출");
    }


    public void DoIdle()
    {
        Debug.Log("Idle 상태 진행중");
        PlayIdleAnimation();
    }

    //FIX ME: Idle 애니메이션에 관한 회의 필요
    public void PlayIdleAnimation()
    {

    }

}
