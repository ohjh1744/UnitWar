using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitFactory
{
    public abstract void Spawn(Vector3 spawnPos);
}

public class MeleeUnitFactory : UnitFactory
{
    public override void Spawn(Vector3 spawnPos)
    {
        UnitProduct meleeUnit = new MeleeUnit();
        meleeUnit.Spawn(spawnPos);
    }
}
