using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class IdleState : MonoBehaviour, IState
{
    private UnitController _unitController;
    private UnitData _data;

    private Animator _animator;

    private int _hashIdleFront;

    private int _hashIdleBack;

    private int _hashIdleRight;

    public IdleState(UnitController controller)
    {
        //생성자
        _unitController = controller;
        _data = _unitController.UnitData;

        _animator = _unitController.GetComponent<Animator>();

        _hashIdleFront = Animator.StringToHash("Idle_Front");
        _hashIdleBack = Animator.StringToHash("Idle_Back");
        _hashIdleRight = Animator.StringToHash("Idle_Right");
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

    //FIX ME: Idle 애니메이션은 아래 내려보는 방향만 재생 (24.11/15 16:30)
    public void PlayIdleAnimation()
    {
        _animator.Play(_hashIdleFront);             
        //_data.Animator.Play(_hashIdleBack);
        //_data.Animator.Play(_hashIdleRight);
    }

    public void StopAni()
    {
        _animator.StopPlayback();
    }


}
