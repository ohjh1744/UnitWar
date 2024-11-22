using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : MonoBehaviour, IState
{
    private UnitController _unitController;

    public DeadState(UnitController controller)
    {
        //생성자
        _unitController = controller;
    }


    public void OnEnter()
    {
        Debug.Log("Dead상태 진입");
    }

    public void OnUpdate()
    {
        Dead();
    }

    public void OnExit()
    {
        Debug.Log("Dead상태 탈출");
    }


    //TO DO: 죽음 상태 완성하기
    public void Dead()
    {
        Debug.Log("Dead 상태 (죽음)");
        //못움직이고
        //애니메이션 추가
        //오브젝트 사라짐
    }


}
