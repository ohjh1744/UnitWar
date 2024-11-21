using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : MonoBehaviour, IState
{
    private UnitController _unitController;

    public AttackState(UnitController controller)
    {
        //생성자
        _unitController = controller;
    }


    public void OnEnter()
    {

    }

    public void OnUpdate()
    {
        if (_unitController.UnitData.HP <= 0)
        {
            _unitController.ChangeState(_unitController.States[(int)EStates.Dead]);
        }

        if (true /*상대방이 외부 테두리를 벗어남*/ &&
            _unitController.UnitData.Path.Count == _unitController.UnitData.PathIndex)
        {
            _unitController.ChangeState(_unitController.States[(int)EStates.Idle]);
        }
        if (true/*상대방이 외부 테두리를 벗어남*/ &&
            _unitController.UnitData.Path.Count > 0)
        {
            _unitController.ChangeState(_unitController.States[(int)EStates.Walk]);
        }

        OverlapCircle();

    }

    public void OnExit()
    {

    }

    /// <summary>
    /// 항상, 유닛의 외부 테두리와 상대의 외부 테두리가 충돌하는지 검사합니다.
    /// </summary>
    public void OverlapCircle()
    {

    }

    /// <summary>
    /// 유닛의 바깥 테두리 안에 상대가 들어온 경우, 안쪽 테두리 까지 들어오면 공격합니다.
    /// </summary>
    public void DoAttack()
    {

    }

}
