using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitFactory
{
    public abstract void Spawn(GameObject gameObject, Vector3 spawnPos);
}

public class MeleeUnitFactory : UnitFactory
{
    public override void Spawn(GameObject gameObject, Vector3 spawnPos)
    {
        UnitProduct meleeUnit = new MeleeUnit();
        meleeUnit.Spawn(gameObject, spawnPos);
    }
}
