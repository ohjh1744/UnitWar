using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Unit : MonoBehaviour
{

    public List<Vector2Int> Path = new List<Vector2Int>();

    public int PathIndex = 0;

    [SerializeField] private float _moveSpeed;


    void Update()
    {
        // 목표 지점에 도달했는지 확인
        if (Path.Count > 0 && PathIndex < Path.Count)
        {
            MoveTowardsTarget(Path[PathIndex]);
        }
    }

    // 목표 지점으로 이동하는 함수
    void MoveTowardsTarget(Vector2Int pathPoint)
    {
        // 현재 위치에서 목표 지점으로 이동
        transform.position = Vector2.MoveTowards(transform.position, pathPoint, _moveSpeed * Time.deltaTime);

        // 목표 지점에 도달했는지 확인
        if ((Vector2)transform.position == pathPoint)
        {
            PathIndex++;  // 다음 지점으로 이동
        }
    }
}
