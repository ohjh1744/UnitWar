using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitProduct
{
    public abstract void Spawn(Vector3 spanwPos);
}

public class MeleeUnit : UnitProduct
{
    public override void Spawn(Vector3 spawnPos)
    {
        //GameObject meleeUnit = ObjectPool.Instance.GetObject(ObjectPool.EUnitType.MELEE);
        //meleeUnit.transform.position = spawnPos;
    }
}