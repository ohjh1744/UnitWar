using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public enum EUnitType 
    {
        Dark,
        Zealut,
        Ultra,
        Zergling
    }

    public static ObjectPool Instance { get; set; }

    //[SerializeField] private GameObject[] _poolingObj;                  // 풀링할 오브젝트

    private Dictionary<EUnitType, List<GameObject>> _poolDict;         // 오브젝트 딕셔너리

    //[SerializeField] GameObject[] _minimapRender;                         // 미니맵에 표시될 도형


    private void Awake()
    {
        Instance = this;
        //if(PhotonNetwork.IsMasterClient)
        //{
        //    Init(5);
        //}
        
    }

    public void Init(int count)
    {
        _poolDict = new Dictionary<EUnitType, List<GameObject>>();
        for (int j = 0; j < 4; j++)
        {
            EUnitType type = (EUnitType)j;
            _poolDict[type] = new List<GameObject>();

            
            for (int i = 0; i < count; i++)
            {
                GameObject poolObj = PhotonNetwork.Instantiate($"Prefabs/Unit{j}", Vector3.zero, Quaternion.identity);
                //GameObject poolObj = Instantiate(_poolingObj[j]);
                //GameObject minimapRender = Instantiate(_minimapRender[j]);
                //minimapRender.transform.parent = poolObj.transform;
                //minimapRender.transform.position = poolObj.transform.position;
                poolObj.SetActive(false);
                _poolDict[type].Add(poolObj);
            }
        }
    }

    public GameObject GetObject(EUnitType type)
    {
        if (!_poolDict.ContainsKey(type))
        {
            return null;
        }
        
        foreach (GameObject poolObj in _poolDict[type])
        {
            if(!poolObj.activeSelf)
            {
                poolObj.SetActive(true);
                return poolObj;
            }
        }
        GameObject newObj = PhotonNetwork.Instantiate($"Prefabs/Unit{(int)type}", Vector3.zero, Quaternion.identity);
        newObj.SetActive(false);
        _poolDict[type].Add(newObj);

        newObj.SetActive(true);
        return newObj;
    }

    public void ReturnObject(GameObject gameObj)
    {
        gameObj.SetActive(false);

        // TODO : 선혜님한테 유닛 죽을 때 ObjectPool.Instance.ReturnObject(gameObject) 해달라고 하기
    }
}
