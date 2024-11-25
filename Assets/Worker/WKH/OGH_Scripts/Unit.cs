using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public abstract class Unit : MonoBehaviourPun
{
    public abstract void Spawn(Vector3 spanwPos);
}

public class Zealot : Unit
{
    public override void Spawn(Vector3 spawnPos)
    {
        GameObject meleeUnit = ObjectPool.Instance.GetObject((int)EUnit.Zealot);
        Debug.Log(meleeUnit);
        meleeUnit.transform.position = spawnPos;
    }
}

public class Juggling: Unit
{
    public override void Spawn(Vector3 spawnPos)
    {
        GameObject meleeUnit = ObjectPool.Instance.GetObject((int)EUnit.Juggling);
        meleeUnit.transform.position = spawnPos;
    }
}

public class Ultra : Unit
{
    public override void Spawn(Vector3 spawnPos)
    {
        GameObject meleeUnit = ObjectPool.Instance.GetObject((int)EUnit.Ultralisk);
        meleeUnit.transform.position = spawnPos;
    }
}

public class DarkTempler : Unit
{
    public override void Spawn(Vector3 spawnPos)
    {
        GameObject meleeUnit = ObjectPool.Instance.GetObject((int)EUnit.DarkTemplar);
        meleeUnit.transform.position = spawnPos;
    }
}