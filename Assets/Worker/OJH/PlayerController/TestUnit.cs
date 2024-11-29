using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnit : MonoBehaviour, IDamageable
{
    [SerializeField] private int _hp;
    public void GetDamage(int damage)
    {
        _hp -= damage;
    }
}
