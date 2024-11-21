using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public enum E_UnitType 
    {
        NONE = 0,
        MELEE = 1
    }

    public static ObjectPool Instance;

    [SerializeField] private GameObject[] _poolingObj;                          // «Æ∏µ«“ ø¿∫Í¡ß∆Æ

    private Dictionary<E_UnitType, List<GameObject>> _poolDict;      // ø¿∫Í¡ß∆Æ µÒº≈≥ ∏Æ

    private void Awake()
    {
        Instance = this;

        Init(25);
    }

    private void Init(int count)
    {
        _poolDict = new Dictionary<E_UnitType, List<GameObject>>();
        for (int j = 0; j < _poolingObj.Length; j++)
        {
            E_UnitType type = (E_UnitType)(j + 1);
            _poolDict[type] = new List<GameObject>();

            for (int i = 0; i < count; i++)
            {
                GameObject poolObj = Instantiate(_poolingObj[j]);
                poolObj.SetActive(false);
                _poolDict[type].Add(poolObj);
            }
        }
    }

    public GameObject GetObject(E_UnitType type)
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
        return null;
    }

    public void ReturnObject(GameObject gameObj)
    {
        gameObj.SetActive(false);

        // TODO : º±«˝¥‘«—≈◊ ¿Ø¥÷ ¡◊¿ª ∂ß ObjectPool.Instance.ReturnObject(gameObject) «ÿ¥ﬁ∂Û∞Ì «œ±‚
    }
}
