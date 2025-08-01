using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Analytics;
using Photon.Pun;

public class WalkState : UnitState
{
    private UnitData _data;

    private AStar _aStar;

    private Vector3 _currentAttackTarget;

    private float _checkAttackTargetTime;

    private Animator _animator;

    private int _hashWalkFront;

    private int _hashWalkBack;

    private int _hashWalkRight;

    private Vector2Int _currentDir;

    private SpriteRenderer _render;

    public WalkState(UnitController unit): base(unit)
    {

        _data = Unit.UnitData;
        _aStar = Unit.AStar;

        _animator = Unit.GetComponent<Animator>();
        _render = Unit.GetComponent<SpriteRenderer>();

        _hashWalkFront = Animator.StringToHash("Walk_Front");
        _hashWalkBack = Animator.StringToHash("Walk_Back");
        _hashWalkRight = Animator.StringToHash("Walk_Right");
    }


    public override void OnEnter()
    {
        Debug.Log("Walk상태 진입");        

        // 경로 재탐색을 위한 초기화.
        if (_data.AttackTarget != null)
        {
            _currentAttackTarget = _data.AttackTarget.transform.position;
        }
        _checkAttackTargetTime = _data.FindLoadTime;
    }

    //순서 바꾸지 말기!
    public override void OnUpdate()
    {
        _checkAttackTargetTime += Time.deltaTime;

        // 이동 마지막 경로까지 이동을 끝냈다면, HasReceivedMove false.
        if (_data.Path != null && _data.PathIndex == _data.Path.Count)
        {
            _data.HasReceivedMove = false;
        }

        //상태전환 
        if (_data.HP <= 0 || (GameSceneManager.Instance.IsFinish == true && Unit.photonView.IsMine == true))
        {
            //_unitController.ChangeState(_unitController.States[(int)EStates.Dead]);
            Unit.photonView.RPC("ChangeState", RpcTarget.All, (int)EStates.Dead);
        }

        if (_data.HasReceivedMove == false)
        {
            if (_data.Path.Count == _data.PathIndex && _data.AttackTarget == null)
            {
                //_unitController.ChangeState(_unitController.States[(int)EStates.Idle]);
                Unit.photonView.RPC("ChangeState", RpcTarget.All, (int)EStates.Idle);
            }
            if (_data.HitObject != null)
            {
                //_unitController.ChangeState(_unitController.States[(int)EStates.Attack]);
                Unit.photonView.RPC("ChangeState", RpcTarget.All, (int)EStates.Attack);
            }
        }

        // 재탐색
        if (_data.AttackTarget != null && (_data.AttackTarget.transform.position != _currentAttackTarget && _checkAttackTargetTime > _data.FindLoadTime))
        {
            Debug.Log("aaaaaaaaaah");
            ReSearchPath();
        }

        // 이동
        if(_data.PathIndex < _data.Path.Count)
        {
            DoWalk(_data.Path[_data.PathIndex]);
        }


    }

    public override void OnExit()
    {
        Debug.Log("Walk상태 탈출");
        StopAni();
    }

    // 경로 재탐색
    private void ReSearchPath()
    {
        Debug.Log("경로 변환!!!");
        _currentAttackTarget = _data.AttackTarget.transform.position;
        Vector2Int startPos = new Vector2Int((int)Unit.transform.position.x, (int)Unit.transform.position.y);
        Vector2Int endPos = new Vector2Int((int)_data.AttackTarget.transform.position.x, (int)_data.AttackTarget.transform.position.y);
        _aStar.DoAStar(startPos, endPos);
        _data.Path.Clear();
        _data.PathIndex = 0;
        _data.Path = _aStar.Path;
        _checkAttackTargetTime = 0;
    }

    private void DoWalk(Vector2Int pathPoint)
    {
        Debug.Log("Walk중!");
        // Unit 위치 vector2int로 변환
        Vector2Int unitPos = new Vector2Int((int)Unit.transform.position.x, (int)Unit.transform.position.y);

        // 바라보고 있는 방향 정하기
        _currentDir = pathPoint - unitPos;

        // 현재 위치에서 목표 지점으로 이동
        Unit.transform.position = Vector2.MoveTowards(Unit.transform.position, pathPoint, _data.MoveSpeed * Time.deltaTime);

        // 목표 지점에 도달했는지 확인
        if ((Vector2)Unit.transform.position == pathPoint)
        {
            _data.PathIndex++;  // 다음 지점으로 이동
        }

        PlayWalkAnimation();

    }

    //FIX ME: Walk 애니메이션은 방향에 따라 상하좌우 재생 (24.11/15 16:30)
    private void PlayWalkAnimation()
    {
        //방향벡터를 얻기 위한 Vector2 변환
        Vector2 newDir = _currentDir;
        Vector2 direction = newDir.normalized;

        if (direction.x > 0)
        {
            // 오른쪽 이동 애니메이션 Play
            _render.flipX = false;
            _animator.Play(_hashWalkRight);
        }
        else if (direction.x < 0)
        {
            // 왼쪽 이동 애니메이션 Play
            _render.flipX = true;
            _animator.Play(_hashWalkRight);
        }
        else if (direction == Vector2.up)
        {
            // 위 이동 애니메이션 Play
            _animator.Play(_hashWalkBack);
        }
        else if (direction == Vector2.down)
        {
            // 아래 이동 애니메이션 Play
            _animator.Play(_hashWalkFront);
        }
        
        
    }

    private void StopAni()
    {
        _animator.StopPlayback();
    }

}
