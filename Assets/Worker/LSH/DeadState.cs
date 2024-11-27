using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : MonoBehaviourPun, IState
{
    private UnitController _unitController;

    private UnitData _data;

    private Animator _animator;

    private int _hashDead;

    private float _setFalsTime;

    private float _curTime;


    public DeadState(UnitController controller)
    {
        //생성자
        _unitController = controller;

        _data = _unitController.UnitData;

        _animator = _unitController.GetComponent<Animator>();

        _hashDead = Animator.StringToHash("Death");

        _setFalsTime = _data.SetFalseTime;

    }


    public void OnEnter()
    {
        _curTime = 0;
        Debug.Log("Dead상태 진입");
        PlayDeadAnimation();
    }

    public void OnUpdate()
    {
        _curTime += Time.deltaTime;
        // 애니메이션 동작 후 setfalse
        if(_curTime > _setFalsTime)
        {
            _unitController.gameObject.SetActive(false);
            //TO DO: 머지 후 위쪽 코드를 밑 코드로 변경
            //ObjectPool.Instance.ReturnObject(_unitController.gameObject);
        }
    }

    public void OnExit()
    {
        Debug.Log("Dead상태 탈출");
        StopAni();
    }

    // 죽는 애니메이션 실행 후 false 해주기
    // 죽는 애니메이션은 반복이 필요없으므로 roof false 해주기.
    private void  PlayDeadAnimation()
    {
        _animator.Play(_hashDead);
    }

    private void StopAni()
    {
        _animator.StopPlayback();
    }


}
