using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private AStar _aStar;

    private List<Unit> _units;

    private const int _linePosCount = 5;

    private Vector2 _lineStartPoint;

    private Vector2 _lineEndPoint;

    private Vector2 _movePoint;

    [SerializeField]private GameObject _target;

    private Vector2 _recentTargetPos;

    [SerializeField] private LineRenderer _lineRenderer;

    [SerializeField] private float _lineZPos;

    [SerializeField] private float _lineWidth;

    [SerializeField] private float _findAttackPathTime;

    private float time;

    private bool _isAttack;



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
        _units = new List<Unit>();
        _lineRenderer = GetComponent<LineRenderer>();

        _lineRenderer.startWidth = _lineWidth;
        _lineRenderer.endWidth = _lineWidth;

        _lineRenderer.startColor = Color.green;
        _lineRenderer.endColor = Color.green;
    }

    void Update()
    {
        time += Time.deltaTime;

        SelectUnits();
        OrderUnits();

        if(_isAttack == true && time > _findAttackPathTime)
        {
            time = 0f;
            UpdateTargetPos();
            OrderAttack();
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

    private void OrderUnits()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CheckOrder();
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

        foreach(Collider2D hitCollider in coliders)
        {
            Unit unit = hitCollider.GetComponent<Unit>();
            if(unit != null)
            {
                _units.Add(unit);
            }
        }

    }

    private void CheckOrder()
    {
        _movePoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

        Collider2D target = Physics2D.OverlapCircle(_movePoint, 0.4f);
        // 공격할 대상이 있따면
        if (target != null)
        {
            _target = target.gameObject;
            _isAttack = true;
            UpdateTargetPos();
        }
        else
        {
            //공격할 대상이 없는 땅이라면 
            _target = null;
            _isAttack = false;
            OrderMove();
        }
    }

    private void UpdateTargetPos()
    {
        _recentTargetPos.x = _target.transform.position.x;
        _recentTargetPos.y = _target.transform.position.y;
    }
    //이동 명령.
    private void OrderMove()
    {
        Debug.Log("Move!");
        if(_movePoint == Vector2.zero)
        {
            return;
        }

        int xPos = 0;
        int yPos = 0;

        int dirIndex = 0;

        for(int i = 0; i < _units.Count; i++)
        {
            Unit unit = _units[i];      

            Vector2Int startPos = new Vector2Int((int)_units[i].transform.position.x, (int)_units[i].transform.position.y);
            Vector2Int endPos = new Vector2Int((int)_movePoint.x + xPos, (int)_movePoint.y + yPos);

            Debug.Log(endPos);

            if(_aStar.DoAStar(startPos, endPos) == true)
            {
                unit.Path.Clear();

                foreach (Vector2Int path in _aStar.Path)
                {
                    unit.PathIndex = 0;
                    unit.Path.Add(path);
                }
            }

            // 단체로 unit이 이동 시 한곳에 모여지 않도록 구현.
            if(dirIndex == _endPosDir.Length - 1)
            {
                dirIndex = 0;
            }
            else
            {
                dirIndex++;
            }

            xPos += _endPosDir[dirIndex].x;
            yPos += _endPosDir[dirIndex].y;


        }

    }

    //공격 명령
    private void  OrderAttack()
    {
        Debug.Log("Attack!");
        if (_movePoint == Vector2.zero)
        {
            return;
        }

        for (int i = 0; i < _units.Count; i++)
        {
            Unit unit = _units[i];

            Vector2Int startPos = new Vector2Int((int)_units[i].transform.position.x, (int)_units[i].transform.position.y);
            Vector2Int endPos = new Vector2Int((int)_target.transform.position.x, (int)_target.transform.position.y);

            Debug.Log(endPos);

            if (_aStar.DoAStar(startPos, endPos) == true)
            {
                unit.Path.Clear();

                foreach (Vector2Int path in _aStar.Path)
                {
                    unit.PathIndex = 0;
                    unit.Path.Add(path);
                }
            }

        }
    }




}
