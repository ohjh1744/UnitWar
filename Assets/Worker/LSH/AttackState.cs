using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackState : MonoBehaviour, IState
{
    private UnitController _unitController;
    private UnitData _data;

    private Collider2D _detectEnemy;
    private IDamageable _damageAble;

    public AttackState(UnitController controller)
    {
        //생성자
        _unitController = controller;
        _data = _unitController.UnitData;
    }


    public void OnEnter()
    {
        Debug.Log("Attack상태 진입");
    }

    public void OnUpdate()
    {
        if (_data.HP <= 0)
        {
            _unitController.ChangeState(_unitController.States[(int)EStates.Dead]);
        }

        if (_data.DetectColider == null &&
            _data.Path.Count == _data.PathIndex)
        {
            _unitController.ChangeState(_unitController.States[(int)EStates.Idle]);
        }
        if (_data.DetectColider == null &&
            _data.Path.Count > 0 && _data.PathIndex != _data.Path.Count)
        {
            _unitController.ChangeState(_unitController.States[(int)EStates.Walk]);
        }

        DoAttack();

    }

    public void OnExit()
    {
        Debug.Log("Attack상태 탈출");
    }

    
    /// <summary>
    /// 항상, 유닛의 외부 테두리와 상대의 외부 테두리가 충돌하는지 검사합니다.
    /// 유닛의 바깥 테두리 안에 상대가 들어온 경우, 안쪽 테두리 까지 들어오면 공격합니다.
    /// </summary>
    public void DoAttack()
    {
        float damageRate = 0;
        damageRate += Time.deltaTime;

        if (_unitController.AttackTarget != null)
        {
            _unitController.GetDamage();
            damageRate = 0f;
        }
        else
        {
            _unitController.GetDamage();
            damageRate = 0f;
        }

        
    }



}
