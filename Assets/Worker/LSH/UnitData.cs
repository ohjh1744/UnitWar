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

    private int _pathIndex = 0;
    public int PathIndex { get { return _pathIndex; } set { } }

    [SerializeField] private float _moveSpeed;
    public float MoveSpeed { get{ return _moveSpeed; } private set { } }


    private Collider2D _detectColider;
    public Collider2D DetectColider { get { return _detectColider; } set { } }

    private Collider2D _hitColider;
    public Collider2D HitColider { get { return _hitColider; } set { } }
    

    [SerializeField] private float _outRadius;
    public float OutRadius { get { return _outRadius; } set { } }

    [SerializeField] private float _inRadius;
    public float InRadius { get { return _inRadius; } set { } }



}
