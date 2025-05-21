using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : UnitState
{
    private UnitData _data;

    private Animator _animator;

    private int _hashDead;

    private float _setFalsTime;

    private float _curTime;


    public DeadState(UnitController unit) : base(unit)
    {

        _data = Unit.UnitData;

        _animator = Unit.GetComponent<Animator>();

        _hashDead = Animator.StringToHash("Death");

        _setFalsTime = _data.SetFalseTime;

    }


    public override void OnEnter()
    {
        _curTime = 0;
        Debug.Log("Dead상태 진입");
        PlayDeadAnimation();
        Unit.Audio.PlayOneShot(_data.AudioCLips[(int)ESound.Dead]);
    }

    public override void OnUpdate()
    {
        _curTime += Time.deltaTime;
        // 애니메이션 동작 후 setfalse
        if(_curTime > _setFalsTime)
        {
            GameSceneManager.Instance.CurUnitCounts[(int)_data.UnitType]--;
            Unit.gameObject.SetActive(false);
        }
    }

    public override void OnExit()
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
