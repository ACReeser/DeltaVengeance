using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Planet  {
    public Guid ID;
    public string Name;
    public Dictionary<Guid, Empire> Empires;
}

public class Empire
{
    public Dictionary<Guid, City> Cities;

    public Dictionary<Guid, OrbitalInfrastructure> OrbitalInfrastructure;

    public Dictionary<Guid, Payload> GroundedPayloads;
    public Dictionary<Guid, Payload> OrbitingPayloads;

    public Agent Player;

    public string Name;

    public Register CurrentResources;
    public Register PlannedResources;

    internal bool CanBuild(BuildableType type)
    {
        return CurrentResources.EqualToOrGreaterThan(GameplayData.DifficultyDataset[Player.Difficulty][type].Costs);
    }
    internal bool CanBuildInCity(Guid cityID, BuildableType type)
    {
        var city = Cities[cityID];
        return CanBuild(type) && city.CanBuild(type);
    }
}
