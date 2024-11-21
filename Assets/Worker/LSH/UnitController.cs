using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States { Idle, Walk, Attack, Dead, Size}
public class UnitController : MonoBehaviour
{
    [SerializeField] private UnitData _unitData;
    public UnitData UnitData { get; set; }


    private void Update()
    {
        //IState에 있는 OnUpdate를 계속 호출함
    }

}
