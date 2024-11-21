using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WalkState : MonoBehaviour, IState
{
    private UnitController _unitController;
    private UnitData _data;

    public WalkState(UnitController controller)
    {
        //생성자
        _unitController = controller;
        _data = _unitController.UnitData;
    }


    public void OnEnter()
    {
        Debug.Log("Walk상태 진입");
    }

    public void OnUpdate()
    {
        if (_unitController.UnitData.Path.Count > 0 && _unitController.UnitData.PathIndex < _unitController.UnitData.Path.Count)
        {
            DoWalk(_unitController.UnitData.Path[_unitController.UnitData.PathIndex]);
        }

        if (_unitController.UnitData.HP <= 0)
        {
            _unitController.ChangeState(_unitController.States[(int)EStates.Dead]);
        }

        if (_unitController.UnitData.Path.Count == _unitController.UnitData.PathIndex)
        {
            _unitController.ChangeState(_unitController.States[(int)EStates.Idle]);
        }
        if (_unitController.Detect != null)
        {
            _unitController.ChangeState(_unitController.States[(int)EStates.Attack]);
        }

        

    }

    public void OnExit()
    {
        Debug.Log("Walk상태 탈출");
    }


    public void DoWalk(Vector2Int pathPoint)
    {
        // 현재 위치에서 목표 지점으로 이동
        transform.position = Vector2.MoveTowards(transform.position, pathPoint, _unitController.UnitData.MoveSpeed * Time.deltaTime);

        // 목표 지점에 도달했는지 확인
        if ((Vector2)transform.position == pathPoint)
        {
            _unitController.UnitData.PathIndex++;  // 다음 지점으로 이동
        }
    }


}
