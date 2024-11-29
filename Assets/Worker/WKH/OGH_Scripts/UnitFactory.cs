using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitFactory
{
    public abstract void Create(Vector3 spawnPos);
}

public class ZealotFactory : UnitFactory
{
    public override void Create(Vector3 spawnPos)
    {
        Unit unit = new Zealot();
        unit.Spawn(spawnPos);
    
    }
}

public class UltraFactory : UnitFactory
{
    public override void Create(Vector3 spawnPos)
    {
        Unit unit = new Ultra();
        unit.Spawn(spawnPos);

    }
}

public class JugglingFactory : UnitFactory
{
    public override void Create(Vector3 spawnPos)
    {
        Unit unit = new Juggling();
        unit.Spawn(spawnPos);

    }
}

public class DarkTemplerFactory : UnitFactory
{
    public override void Create(Vector3 spawnPos)
    {
        Unit unit = new DarkTempler();
        unit.Spawn(spawnPos);

    }
}
