using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitData : MonoBehaviour
{
    [SerializeField] private int _hp;
    public int HP { get { return _hp; }  set { _hp = value; } }

    [SerializeField] private int _power;
    public int Power { get { return _power; } private set { } }


    [SerializeField] private float _damageRate; // 데미지 주기 시간
    public float DamageRate { get { return _damageRate; } private set { } }


    [SerializeField] private List<Vector2Int> _path = new List<Vector2Int>();
    public List<Vector2Int> Path { get { return _path; } set { _path = value; } }

    [SerializeField] private int _pathIndex;
    public int PathIndex { get { return _pathIndex; } set { _pathIndex = value; } }

    [SerializeField] private float _moveSpeed;
    public float MoveSpeed { get{ return _moveSpeed; } private set { } }


    [SerializeField] private Collider2D[] _detectColider;
    public Collider2D[] DetectColider { get { return _detectColider; }  set { _detectColider = value; } }

    [SerializeField] private Collider2D _detectObject;
    public Collider2D DetectObject { get { return _detectObject; } set { _detectObject = value; } }

    [SerializeField] private Collider2D[] _hitColider;
    public Collider2D[] HitColider { get { return _hitColider; }  set { _hitColider = value; } }

    [SerializeField] private Collider2D _hitObject;
    public Collider2D HitObject {  get { return _hitObject; } set { _hitObject = value; } }
    

    [SerializeField] private float _detectRadius;
    public float DetectRadius { get { return _detectRadius; } private set { } }

    [SerializeField] private float _hitRadius;
    public float HitRadius { get { return _hitRadius; } private set { } }

    [SerializeField] private GameObject _attackTarget;
    public GameObject AttackTarget { get { return _attackTarget; }  set { _attackTarget = value; } }

    [SerializeField] private float _findLoadTime;
    public float FindLoadTime { get { return _findLoadTime; } private set { } }


}
