using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    public Guid PlayerID;
    public Command(Guid playerID) { this.PlayerID = playerID; }
    public abstract void Execute(SolarSystem system);
}

public class LaunchPayloadCommand : Command
{
    public Guid planetID, payloadID;
    public LaunchPayloadCommand(Guid planet, Guid player, Guid payload) : base(player)
    {
        planetID = planet;
        payloadID = payload;
    }

    public override void Execute(SolarSystem system)
    {
        var empire = system.GetEmpire(planetID, PlayerID);
        var payload = empire.GroundedPayloads[payloadID];
        int payloadTonnes = GameplayData.DifficultyDataset[system.Players[PlayerID].Difficulty][payload.Type].WeightTon;
        if (empire.CurrentResources.TonnesToOrbit >= payloadTonnes)
        {
            empire.CurrentResources.TonnesToOrbit -= payloadTonnes;
            empire.OrbitingPayloads.Add(payloadID, payload);
            empire.GroundedPayloads.Remove(payloadID);
        }
    }
}

public abstract class BuildOnPlanetCommand : Command
{
    public BuildableType type;
    public Guid planetID;
    public BuildOnPlanetCommand(Guid planet, Guid player, BuildableType type) : base(player)
    {
        this.type = type;
        planetID = planet;
    }

    public override void Execute(SolarSystem system)
    {
        var empire = system.GetEmpire(planetID, PlayerID);
        if (CanBuild(empire, type))
        {
            Build(system, empire, GameplayData.DifficultyDataset[system.Players[PlayerID].Difficulty][type]);
        }
    }

    internal virtual bool CanBuild(Empire empire, BuildableType type)
    {
        return empire.CanBuild(type);
    }
    internal abstract void Build(SolarSystem system, Empire empire, BuildableData data);
}
public class BuildPayloadOnPlanetCommand : BuildOnPlanetCommand
{
    public BuildPayloadOnPlanetCommand(Guid planet, Guid player, BuildableType type) : base(planet, player, type) { }

    internal override void Build(SolarSystem system, Empire empire, BuildableData data)
    {
        var newBuildable = Activator.CreateInstance(data.Class) as Buildable;
        newBuildable.ID = Guid.NewGuid();
        empire.GroundedPayloads.Add(newBuildable.ID, newBuildable as Payload);
    }
}
public class BuildInCityCommand : BuildOnPlanetCommand
{
    public Guid city;
    public BuildInCityCommand(Guid planet, Guid player, Guid c, BuildableType type) : base(planet, player, type)
    {
        city = c;
    }

    internal override void Build(SolarSystem system, Empire empire, BuildableData data)
    {
        var newBuildable = Activator.CreateInstance(data.Class) as Buildable;
        newBuildable.ID = Guid.NewGuid();
        empire.Cities[city].Infrastructure.Add(newBuildable.ID, newBuildable as Infrastructure);
    }
    internal override bool CanBuild(Empire empire, BuildableType type)
    {
        return empire.CanBuild(type) && empire.CanBuildInCity(city, type);
    }
}
public class LaunchInterplanetaryRocketCommand : Command
{
    public Guid fromPlanetID, toPlanetID, rocketID;
    public Guid[] payloadIDs;
    public LaunchInterplanetaryRocketCommand(Guid fromPlanet, Guid toPlanet, Guid player, Guid rocket, Guid[] payloads) : base(player)
    {
        fromPlanetID = fromPlanet;
        toPlanetID = toPlanet;
        rocketID = rocket;
        payloadIDs = payloads;
    }

    public override void Execute(SolarSystem system)
    {
        var rocket = system.Players[PlayerID].Rockets[rocketID];
        var empire = system.GetEmpire(fromPlanetID, PlayerID);
        var packedPayloads = new Dictionary<Guid, Payload>();
        foreach (var payloadID in payloadIDs)
        {
            packedPayloads[payloadID] = empire.OrbitingPayloads[payloadID];
        }
        if (rocket.OrbitingPlanet.HasValue && rocket.CanLift(packedPayloads, fromPlanetID, toPlanetID))
        {
            foreach (var payloadID in payloadIDs)
            {
                empire.OrbitingPayloads.Remove(payloadID);
            }
            var newFlight = new RocketFlight()
            {
                PlayerID = PlayerID,
                RocketID = rocketID,
                FromPlanetID = fromPlanetID,
                ToPlanetID = toPlanetID,
                Payloads = packedPayloads
            };
            rocket.Flight = newFlight;
            rocket.OrbitingPlanet = null;
        }
    }
}