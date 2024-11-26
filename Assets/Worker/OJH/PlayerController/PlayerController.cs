using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;


public enum EOrder { Move, Attack };
public class PlayerController : MonoBehaviourPun
{
    [SerializeField] private EUnit _unitType;

    [SerializeField] private AStar _aStar;

    private List<UnitData> _units;

    private const int _linePosCount = 5;

    private Vector2 _lineStartPoint;

    private Vector2 _lineEndPoint;

    private Vector2 _movePoint;

    [SerializeField] private GameObject _target;

    private Vector2 _recentTargetPos;

    [SerializeField] private LineRenderer _lineRenderer;

    [SerializeField] private float _lineZPos;

    [SerializeField] private float _lineWidth;

    [SerializeField] private float _findAttackPathTime;

    [SerializeField] private UnitSpawner _unitSpawner;

    private float _time;                                              // Unit별 생성 주기 체크.

    [SerializeField] private float _spawnTime;                        // Unit별 Spawn Time 설정.

    [SerializeField] private int _unitCounts;                         // Unit별 현재 개수.

    [SerializeField] private Vector3 _spawnPos;                     // 종족별 기본 Spawn 위치

    private Vector2Int[] _endPosDir =
    {
        new Vector2Int( 0, +2), // 상
        new Vector2Int( 0, -2), // 하
        new Vector2Int(-2,  0), // 좌
        new Vector2Int(+2,  0), // 우
        new Vector2Int(+2, +2), // 우상
        new Vector2Int(+2, -2), // 우하
        new Vector2Int(-2, +2), // 좌상
        new Vector2Int(-2, -2), // 좌하
    };


    void Start()
    {
        _units = new List<UnitData>();
        _lineRenderer = GetComponent<LineRenderer>();

        _lineRenderer.startWidth = _lineWidth;
        _lineRenderer.endWidth = _lineWidth;

        _lineRenderer.startColor = Color.green;
        _lineRenderer.endColor = Color.green;
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            Debug.Log($"{PhotonNetwork.LocalPlayer.IsLocal},{gameObject.name}");
            SelectUnits();
            CheckCommand();
            CreateUnit();
        }
    }

    // Unit선택하기
    private void SelectUnits()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //선택 유닛 마우스 버튼누를 때 초기화.
            _units.Clear();
            _lineRenderer.positionCount = _linePosCount;
            _lineStartPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        }

        if (Input.GetMouseButton(0))
        {
            _lineEndPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            DrawRectangle();
        }

        if (Input.GetMouseButtonUp(0))
        {
            CheckUnits();
            //초기화
            _lineRenderer.positionCount = 0;
            _lineStartPoint = Vector3.zero;
            _lineEndPoint = Vector3.zero;
        }
    }


    // 사각형 라인 그리기
    private void DrawRectangle()
    {
        //왼쪽위, 오른쪽위, 오른쪽아래, 아래 모서리.
        Vector3 vertex1 = new Vector3(_lineStartPoint.x, _lineStartPoint.y, _lineZPos);
        Vector3 vertex2 = new Vector3(_lineEndPoint.x, _lineStartPoint.y, _lineZPos);
        Vector3 vertex3 = new Vector3(_lineEndPoint.x, _lineEndPoint.y, _lineZPos);
        Vector3 vertex4 = new Vector3(_lineStartPoint.x, _lineEndPoint.y, _lineZPos);

        _lineRenderer.SetPosition(0, vertex1);
        _lineRenderer.SetPosition(1, vertex2);
        _lineRenderer.SetPosition(2, vertex3);
        _lineRenderer.SetPosition(3, vertex4);
        _lineRenderer.SetPosition(4, vertex1);
    }

    // 사각형안에 Unit이 존재하는지.
    private void CheckUnits()
    {
        Vector2 centerPos = new Vector2((_lineStartPoint.x + _lineEndPoint.x) / 2, (_lineStartPoint.y + _lineEndPoint.y) / 2);
        float width = Mathf.Abs(_lineStartPoint.x - _lineEndPoint.x);
        float height = Mathf.Abs(_lineStartPoint.y - _lineEndPoint.y);
        Vector2 size = new Vector3(width, height);

        Collider2D[] coliders = Physics2D.OverlapBoxAll(centerPos, size, 0);

        foreach (Collider2D hitCollider in coliders)
        {
            UnitData unit = hitCollider.GetComponent<UnitData>();
            // 내 종족 Unit만 건들 수 있도록 함.
            if (unit != null && unit.UnitType == _unitType)
            {
                _units.Add(unit);
            }
        }

    }

    // 공격인지 이동오더인지 체크.
    private void CheckCommand()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _movePoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

            Collider2D target = Physics2D.OverlapCircle(_movePoint, 0.4f);

            // 공격할 대상이 있따면
            if (target != null)
            {
                UnitData _unit = target.GetComponent<UnitData>();
                // 장애물은 공격 대상으로 취급 안하고 Unit이어야 하며, 그 Unit은 아군종족이 아닌 다른 종족이어야함.
                if (target.tag != "Obstacle" && _unit != null && _unit.UnitType != _unitType)
                {
                    _target = target.gameObject;
                    CommandUnits(_target.transform.position.x, _target.transform.position.y, (int)EOrder.Attack);
                }
            }
            else
            {
                //공격할 대상이 없는 땅이라면 
                _target = null;
                CommandUnits(_movePoint.x, _movePoint.y, (int)EOrder.Move);
            }
        }
    }

    // 오더에 따라서 유닛 작동.
    private void CommandUnits(float movePosX, float movePosY, int orderNum)
    {
        Debug.Log("Move!");
        if (_movePoint == Vector2.zero)
        {
            return;
        }

        int xPos = 0;
        int yPos = 0;

        int dirIndex = 0;

        for (int i = 0; i < _units.Count; i++)
        {
            UnitData unit = _units[i];
            // 공격order인 경우 타겟 지정
            if (orderNum == (int)EOrder.Attack)
            {
                unit.AttackTarget = _target;
                unit.HasReceivedMove = false;
            }
            else if (orderNum == (int)EOrder.Move)
            {
                unit.AttackTarget = null;
                unit.HasReceivedMove = true;
            }

            Vector2Int startPos = new Vector2Int((int)_units[i].transform.position.x, (int)_units[i].transform.position.y);
            Vector2Int endPos = new Vector2Int((int)movePosX + xPos, (int)movePosY + yPos);

            if (_aStar.DoAStar(startPos, endPos) == true)
            {
                unit.Path.Clear();

                foreach (Vector2Int path in _aStar.Path)
                {
                    unit.PathIndex = 0;
                    unit.Path.Add(path);
                }
            }

            // 단체로 이동 오더 시 한곳에 모여지 않도록 구현.
            if (orderNum == (int)EOrder.Move)
            {
                if (dirIndex == _endPosDir.Length - 1)
                {
                    dirIndex = 0;
                }
                else
                {
                    dirIndex++;
                }

                xPos = _endPosDir[dirIndex].x;
                yPos = _endPosDir[dirIndex].y;
            }
        }

    }

    private void CreateUnit()
    {
        if(ObjectPool.Instance != null)
        {
            _time += Time.deltaTime;

            if (_time >= _spawnTime)
            {
                _unitSpawner.Spawn((int)_unitType, _unitCounts, _spawnPos);
                _unitCounts++;
                _time = 0;
            }
        }
    }
}
