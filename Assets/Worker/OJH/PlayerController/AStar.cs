using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    [SerializeField] Transform _start;

    [SerializeField] Transform _end;
    private enum Edir { Straight = 0, Diagoanal = 4 }

    private Vector2Int[] _direction =
{
        new Vector2Int( 0, +1), // 상
        new Vector2Int( 0, -1), // 하
        new Vector2Int(-1,  0), // 좌
        new Vector2Int(+1,  0), // 우
        new Vector2Int(+1, +1), // 우상
        new Vector2Int(+1, -1), // 우하
        new Vector2Int(-1, +1), // 좌상
        new Vector2Int(-1, -1), // 좌하
    };

    private const int _constStraigtCost = 10;

    private const int _constDiagoanalCost = 14;

    private List<ASNode> _priorityQueue; //계산한 정점들

    private List<Vector2Int> _path; //A스타경로

    public List<Vector2Int> Path { get { return _path; } set { _path = value; } }

    private HashSet<Vector2Int> _checkNodes; // 방문한 정점들


    public bool DoAStar(Vector2Int start, Vector2Int end)
    {
         Collider2D hitCollider = Physics2D.OverlapCircle(new Vector2(end.x, end.y), 0.4f);

        // end변수가 애초에 벽과 같이 가지 못하는 Posion이라면 무한스택에러발생.
        if (hitCollider != null)
        {
            if(hitCollider.tag == "Obstacle")
            {
                Debug.Log("장애물로 이동할수없습니다.");
                return false;
            }
        }

        _priorityQueue = new List<ASNode>();

        _path = new List<Vector2Int>();

        _checkNodes = new HashSet<Vector2Int>();

        // 시작정점
        _priorityQueue.Add(new ASNode(start, null, 0, GetHeuristic(start, end)));

        while (_priorityQueue.Count > 0)
        {
            ASNode currentNode = MinNode(_priorityQueue);

            _priorityQueue.Remove(currentNode);

            //현재 노드가 목표노드와 같다면 종료.
            if (currentNode.pos == end)
            {
                ASNode current = currentNode;
                while (current != null)
                {
                    _path.Add(current.pos);
                    current = current.parent;
                }
                _path.Reverse();
                return true;
            }

            //이미 방문한 정점이라면 무시.
            if (_checkNodes.Contains(currentNode.pos) == true)
            {
                continue;
            }

            _checkNodes.Add(currentNode.pos);

            for (int i = 0; i < _direction.Length; i++)
            {
                Vector2Int nextPos = currentNode.pos + _direction[i];

                hitCollider = Physics2D.OverlapCircle(nextPos, 0.4f);
                //장애물 있을시 무시
                if (hitCollider != null && hitCollider.CompareTag("Obstacle"))
                {
                    continue;
                }
                // 대각선 중에 둘다 막힌 경우도 무시.

                Collider2D diagoanalObject1 = Physics2D.OverlapCircle(new Vector2(nextPos.x, currentNode.pos.y), 0.4f);
                Collider2D diagoanalObject2 = Physics2D.OverlapCircle(new Vector2(nextPos.x, currentNode.pos.y), 0.4f);

                if (i >= 4 &&  (diagoanalObject1 != null) &&  (diagoanalObject2 != null))
                {
                    continue;
                }

                int nextF;
                int nextH;
                int nextG;

                //대각선이동경우
                if (i >= (int)Edir.Diagoanal)
                {
                    nextG = _constDiagoanalCost + currentNode.g;
                }
                else //직선이동 경우
                {
                    nextG = _constStraigtCost + currentNode.g;
                }

                nextH = GetHeuristic(nextPos, end);

                nextF = nextH + nextG;

                ASNode findNode = FindNode(_priorityQueue, nextPos);

                // 전에 계산한 노드가 없다면
                if (findNode == null)
                {
                    _priorityQueue.Add(new ASNode(nextPos, currentNode, nextG, nextH));
                } // 있다면 F를 비교해서 이전 F보다 작다면
                else if (findNode.f > nextF)
                {
                    findNode.f = nextF;
                    findNode.g = nextG;
                    findNode.h = nextH;
                    findNode.parent = currentNode;
                }


            }

        }

        return false;
    }
    private static ASNode MinNode(List<ASNode> openList)
    {
        // F가 가장 낮은, F가 같다면 H가 가장 낮은 선택
        int curF = int.MaxValue;
        int curH = int.MaxValue;
        ASNode minNode = null;

        for (int i = 0; i < openList.Count; i++)
        {
            if (curF > openList[i].f)
            {
                curF = openList[i].f;
                curH = openList[i].h;
                minNode = openList[i];
            }
            else if (curF == openList[i].f &&
                curH > openList[i].h)
            {
                curF = openList[i].f;
                curH = openList[i].h;
                minNode = openList[i];
            }
        }

        return minNode;
    }

    private ASNode FindNode(List<ASNode> openList, Vector2Int pos)
    {
        for (int i = 0; i < openList.Count; i++)
        {
            if (openList[i].pos == pos)
            {
                return openList[i];
            }
        }

        return null;
    }

    private int GetHeuristic(Vector2Int start, Vector2Int end)
    {
        int xSize = Mathf.Abs(start.x - end.x);
        int ySize = Mathf.Abs(start.y - end.y);

        int straightCost = Mathf.Abs(xSize - ySize);
        int diagonalCost = Mathf.Max(xSize, ySize) - straightCost;
        return _constStraigtCost * straightCost + _constDiagoanalCost * diagonalCost;

    }
}

public class ASNode
{
    public Vector2Int pos;  // 현재 정점 위치
    public ASNode parent;   // 이 정점을 탐색한 정점

    public int f;           // 예상 최종 거리 => f = g + h
    public int g;           // 걸린 거리
    public int h;           // 예상 남은 거리

    public ASNode(Vector2Int pos, ASNode parent, int g, int h)
    {
        this.pos = pos;
        this.parent = parent;
        this.f = g + h;
        this.g = g;
        this.h = h;
    }
}