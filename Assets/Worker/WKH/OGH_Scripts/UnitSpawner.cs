using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UnitSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] Vector3 _spawnPos;                      // 생성 위치

    [SerializeField] private float _unitDistance = 0.1f;    // 유닛간의 거리

    [SerializeField] private int _unitCount = 0;            // 현재 유닛 카운트
    public int UnitCount { get { return _unitCount; } set { _unitCount = value; } }

    [SerializeField] private int _unitsInLine = 5;          // 한 라인당 유닛 생성 수

    [SerializeField] private float _time;                   // 현재 시간용 타임

    [SerializeField] private float _spawnTime;              // 생산 시간용 타임

    private void Update()
    {
        // if(!photonView.IsMine)
        // return;
        _time += Time.deltaTime;
        if(_spawnTime <= _time && _unitCount < 25)
        {
            _time = 0;
            Spawn();
        }
    }

    public void Spawn()
    {
        int col = _unitCount / _unitsInLine;                // 열
        int row = _unitCount % _unitsInLine;                // 행
        float colRatio = col * 0.3f;                        // 생성 열 간격 조절
        float rowRatio = row * 0.3f;                        // 생성 행 간격 조절

        _spawnPos = transform.position + new Vector3(1, 0.7f, 0);
        Vector3 newSpawnPos = _spawnPos + new Vector3(rowRatio, -colRatio, 0);
        UnitFactory meleeFact = new MeleeUnitFactory();
        meleeFact.Spawn(newSpawnPos);
        _unitCount++;   
    }
}
