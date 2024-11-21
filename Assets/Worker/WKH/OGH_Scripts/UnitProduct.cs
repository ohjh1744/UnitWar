using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitProduct
{
    public abstract void Spawn(GameObject gameObject, Vector3 spanwPos);
}

public class MeleeUnit : UnitProduct
{
    public override void Spawn(GameObject gameObject, Vector3 spawnPos)
    {
        GameObject.Instantiate(gameObject, spawnPos, Quaternion.identity);
    }
}
