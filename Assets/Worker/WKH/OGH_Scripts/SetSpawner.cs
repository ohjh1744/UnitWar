using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SetSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private Vector3[] _spawnerPos;
    [SerializeField] private GameObject _spawnerPrefab;

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
