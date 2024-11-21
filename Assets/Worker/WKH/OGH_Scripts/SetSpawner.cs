using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SetSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private Vector3[] _spawnerPos;                     // 스폰위치 (네트워크 도입시 유저마다 다르게 설정)
    [SerializeField] private GameObject _spawnerPrefab;                 // 스포너 프리팹

    private void Start()
    {
        //if(PhotonNetwork.IsConnected)
        //{
        //    int playerNum = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        //    Vector3 pos = _spawnerPos[playerNum];
        //
        //    PhotonNetwork.Instantiate("GameObject/Spawner", pos, Quaternion.identity);
        //}

        Instantiate(_spawnerPrefab, _spawnerPos[0], Quaternion.identity);
    }
}
