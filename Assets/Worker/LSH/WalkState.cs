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
        if (_data.Path.Count > 0 && _data.PathIndex < _data.Path.Count)
        {
            DoWalk(_data.Path[_data.PathIndex]);
        }

        if (_data.HP <= 0)
        {
            _unitController.ChangeState(_unitController.States[(int)EStates.Dead]);
        }

        if (_data.Path.Count == _data.PathIndex)
        {
            _unitController.ChangeState(_unitController.States[(int)EStates.Idle]);
        }
        if (_data.DetectColider != null && _data.HitColider != null)
        {
            _unitController.ChangeState(_unitController.States[(int)EStates.Attack]);
        }

        

    }

    public void OnExit()
    {
        Debug.Log("Walk상태 탈출");
    }


    //TO DO: 애니메이션 추가
    public void DoWalk(Vector2Int pathPoint)
    {

        // 현재 위치에서 목표 지점으로 이동
        transform.position = Vector2.MoveTowards(transform.position, pathPoint, _data.MoveSpeed * Time.deltaTime);

        // 목표 지점에 도달했는지 확인
        if ((Vector2)transform.position == pathPoint)
        {
            _data.PathIndex++;  // 다음 지점으로 이동
        }


    }


}
