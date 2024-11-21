using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData : MonoBehaviour
{
    [SerializeField] private int _hp;
    public int HP { get { return _hp; } set { } }


    [SerializeField] private float _attackRange;
    public float AttackRange { get { return _attackRange; } set { } }

    [SerializeField] private int _power;
    public int Power { get { return _power; } set { } }


    [SerializeField] private float _damageRate;
    public float DamageRate { get { return _damageRate; } set { } }


    public List<Vector2Int> Path = new List<Vector2Int>();

    public int PathIndex = 0;

    [SerializeField] private float _moveSpeed;
    public float MoveSpeed { get{ return _moveSpeed; } private set { } }



    

}
