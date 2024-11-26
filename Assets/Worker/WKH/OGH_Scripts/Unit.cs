using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public abstract class Unit
{
    public abstract void Spawn(Vector3 spanwPos);
}

public class Zealot : Unit
{
    public override void Spawn(Vector3 spawnPos)
    {
        GameObject meleeUnit = ObjectPool.Instance.GetObject((int)EUnit.Zealot, spawnPos);
    }
}

public class Juggling: Unit
{
    public override void Spawn(Vector3 spawnPos)
    {
        GameObject meleeUnit = ObjectPool.Instance.GetObject((int)EUnit.Juggling, spawnPos);
    }
}

public class Ultra : Unit
{
    public override void Spawn(Vector3 spawnPos)
    {
        GameObject meleeUnit = ObjectPool.Instance.GetObject((int)EUnit.Ultralisk, spawnPos);
    }
}

public class DarkTempler : Unit
{
    public override void Spawn(Vector3 spawnPos)
    {
        GameObject meleeUnit = ObjectPool.Instance.GetObject((int)EUnit.DarkTemplar, spawnPos);
    }
}