using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : MonoBehaviour, IState
{
    private UnitController _unitController;

    private Collider2D _detectEnemy;    

    

    public AttackState(UnitController controller)
    {
        //생성자
        _unitController = controller;
    }


    public void OnEnter()
    {
        Debug.Log("Attack상태 진입");
    }

    public void OnUpdate()
    {
        if (_unitController.UnitData.HP <= 0)
        {
            _unitController.ChangeState(_unitController.States[(int)EStates.Dead]);
        }

        if (_unitController.Detect == null &&
            _unitController.UnitData.Path.Count == _unitController.UnitData.PathIndex)
        {
            _unitController.ChangeState(_unitController.States[(int)EStates.Idle]);
        }
        if (_unitController.Detect == null &&
            _unitController.UnitData.Path.Count > 0)
        {
            _unitController.ChangeState(_unitController.States[(int)EStates.Walk]);
        }

        OverlapCircle();

    }

    public void OnExit()
    {
        Debug.Log("Attack상태 탈출");
    }

    /// <summary>
    /// 항상, 유닛의 외부 테두리와 상대의 외부 테두리가 충돌하는지 검사합니다.
    /// </summary>
    public void OverlapCircle()
    {        
        _detectEnemy = Physics2D.OverlapCircle(this.transform.position, _unitController.InRadius);

        if (_detectEnemy != null)
        {
            DoAttack();
        }
        
    }

    /// <summary>
    /// 유닛의 바깥 테두리 안에 상대가 들어온 경우, 안쪽 테두리 까지 들어오면 공격합니다.
    /// </summary>
    public void DoAttack()
    {
        float damageRate = 0;
        damageRate += Time.deltaTime;

        if (damageRate > _unitController.UnitData.DamageRate)
        {
            _unitController.GetDamage();
            damageRate = 0f;
        }
    }

}
