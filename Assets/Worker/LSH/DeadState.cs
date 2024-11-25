using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : MonoBehaviour, IState
{
    private UnitController _unitController;

    private UnitData _data;

    private Animator _animator;

    private int _hashDead;
    public DeadState(UnitController controller)
    {
        //생성자
        _unitController = controller;
        _data = _unitController.UnitData;

        _animator = _unitController.GetComponent<Animator>();

        _hashDead = Animator.StringToHash("Death");
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
        StopAni();
    }


    //TO DO: 죽음 상태 완성하기
    public void Dead()
    {
        Debug.Log("Dead 상태 (죽음)");
        _unitController.gameObject.SetActive(false);
        PlayDeadAnimation();
    }

    public void PlayDeadAnimation()
    {
        _animator.Play(_hashDead);
    }

    public void StopAni()
    {
        _animator.StopPlayback();
    }


}
