using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UnitSpawner : MonoBehaviourPunCallbacks
{

    [SerializeField] private int _unitsInLine;          // 한 라인당 유닛 생성 수

    [SerializeField] private float _interval;

    public void Spawn(int unitNum, int unitCount, Vector3 spawnPos) // spawnPos이 커멘드센터 위치.
    {
        int col = unitCount / _unitsInLine;                // 열
        int row = unitCount % _unitsInLine;                // 행
        float colRatio = col * _interval;                        // 생성 열 간격 조절
        float rowRatio = row * _interval;                        // 생성 행 간격 조절

        Vector3 newSpawnPos = spawnPos + new Vector3(rowRatio, -colRatio, 0);

        switch (unitNum)
        {
            case (int)EUnit.Zealot:
                UnitFactory meleeFact = new ZealotFactory();
                meleeFact.Create(newSpawnPos);
                break;
            case (int)EUnit.DarkTemplar:
                meleeFact = new DarkTemplerFactory();
                meleeFact.Create(newSpawnPos);
                break;
            case (int)EUnit.Juggling:
                meleeFact = new JugglingFactory();
                meleeFact.Create(newSpawnPos);
                break;
            case (int)EUnit.Ultralisk:
                meleeFact = new UltraFactory();
                meleeFact.Create(newSpawnPos);
                break;
        }

        unitCount++;   
    }
}
