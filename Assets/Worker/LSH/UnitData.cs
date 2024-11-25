using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum EUnit {Zealot, DarkTemplar, Juggling, Ultralisk}
public class UnitData : MonoBehaviour
{
    [SerializeField] private EUnit _unitType;
    public EUnit UnitType { get { return _unitType;} private set { } }

    [SerializeField] private int _hp;  // 체력
    public int HP { get { return _hp; }  set { _hp = value; } }

    [SerializeField] private int _power;  //공격력 
    public int Power { get { return _power; } private set { } }

    [SerializeField] private float _damageRate; // 데미지 주기 시간
    public float DamageRate { get { return _damageRate; } private set { } }

    [SerializeField] private List<Vector2Int> _path = new List<Vector2Int>();  // 이동 경로
    public List<Vector2Int> Path { get { return _path; } set { _path = value; } }

    [SerializeField] private int _pathIndex; // 현재 어떤 경로로 이동 중인지 여부.
    public int PathIndex { get { return _pathIndex; } set { _pathIndex = value; } }

    [SerializeField] private float _moveSpeed; // 이동 속도
    public float MoveSpeed { get{ return _moveSpeed; } private set { } }

    [SerializeField] private Collider2D[] _hitColiders; // 공격가능한 대상들 
    public Collider2D[] HitColiders { get { return _hitColiders; }  set { _hitColiders = value; } }

    [SerializeField] private Collider2D _hitObject; // 랜덤 공격 대상 
    public Collider2D HitObject {  get { return _hitObject; } set { _hitObject = value; } }

    [SerializeField] private GameObject _attackTarget; // Player에 의해 지정된 공격 대상 
    public GameObject AttackTarget { get { return _attackTarget; } set { _attackTarget = value; } }

    [SerializeField] private float _hitRadius; // 공격 가능한 범위
    public float HitRadius { get { return _hitRadius; } private set { } }

    [SerializeField] private float _findLoadTime; // 길 재찾기 주기
    public float FindLoadTime { get { return _findLoadTime; } private set { } }

    [SerializeField] private bool _hasReceivedMove; // 이동명령을 받았는지 여부 
    public bool HasReceivedMove { get { return _hasReceivedMove; } set { _hasReceivedMove = value; } }

    [SerializeField] private Animator _animator; // 애니메이터
    public Animator Animator { get { return _animator; } set { } }

}
