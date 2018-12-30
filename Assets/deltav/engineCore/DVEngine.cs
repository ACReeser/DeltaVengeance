using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TurnResolvedEvent(TurnResolution resolution);

public interface IEngineBridge
{
    SolarSystem CurrentState { get; }

    IEnumerator SubmitCommands(PhasedCommands commands);
    IEnumerator GetSolarSystem();

    event TurnResolvedEvent OnCommandsResolved;
}

public class TurnResolution
{
    public SolarSystem newSolarSystemState;
    public PhasedCommands[] commandResolutions;
}

public class DVEngine {
    
    public TurnResolution Tick(SolarSystem s, PhasedCommands[] commands)
    {
        foreach(var command in commands)
        {
            command.Execute(s);
        }

        return new TurnResolution()
        {
            newSolarSystemState = s,
            commandResolutions = commands
        };
    }

    public static SolarSystem GetNewGame(int humanPlayers, int aiPlayers)
    {
        var s = new SolarSystem()
        {
            Planets = new Dictionary<Guid, Planet>(),
            Players = new Dictionary<Guid, Agent>()
        };
        AddPlanet(s, "Mercury");
        AddPlanet(s, "Venus");
        var e = AddPlanet(s, "Earth");
        AddPlanet(s, "Mars");

        var a = AddPlayer(s, "Alex");

        e.Empires[a.ID] = new Empire()
        {
            Cities = new Dictionary<Guid, City>(),
            OrbitalInfrastructure = new Dictionary<Guid, OrbitalInfrastructure>(),
            CurrentResources = new Register(),
            Name = "UN",
            GroundedPayloads = new Dictionary<Guid, Payload>(),
            OrbitingPayloads = new Dictionary<Guid, Payload>(),
            PlannedResources = new Register(),
            Player = a
        };
        AddCity(s, a.ID, e.ID, "NYC");
        AddCity(s, a.ID, e.ID, "Moscow");

        return s;
    }
    public static void AddInfra(SolarSystem s, City c, BuildableType b)
    {
        var i = new Infrastructure()
        {
            ID = Guid.NewGuid(),
            Type = b,
            Destroyed = DestructionType.None
        };
        c.Infrastructure[i.ID] = i;        
    }
    public static City AddCity(SolarSystem s, Guid playerID, Guid planetID, string name)
    {
        var e = s.GetEmpire(planetID, playerID);
        var city = new City()
        {
            ID = Guid.NewGuid(),
            Name = name,
            Infrastructure = new Dictionary<Guid, Infrastructure>()
        };
        e.Cities[city.ID] = city;
        AddInfra(s, city, BuildableType.NuclearPlant);
        AddInfra(s, city, BuildableType.SolarPlant);
        AddInfra(s, city, BuildableType.SolarPlant);
        AddInfra(s, city, BuildableType.Factory);
        AddInfra(s, city, BuildableType.LaunchFacility);
        AddInfra(s, city, BuildableType.Farm);
        AddInfra(s, city, BuildableType.Mine);
        return city;
    }
    public static Planet AddPlanet(SolarSystem s, string name)
    {
        var p = GetPlanet(name);
        s.Planets[p.ID] = p;
        return p;
    }
    public static Agent AddPlayer(SolarSystem s, string name)
    {
        var p = new Agent()
        {
            ID = Guid.NewGuid(),
            Name = name,
            Difficulty = Difficulty.Pacifist,
            Rockets = new Dictionary<Guid, Rocket>(),
            PreviousMoves = new Stack<Command>(),
            Commands = new PhasedCommands()
        };
        p.Commands.PlayerID = p.ID;
        s.Players[p.ID] = p;
        return p;
    }
    private static Planet GetPlanet(string name)
    {
        return new Planet()
        {
            ID = Guid.NewGuid(),
            Name = name,
            Empires = new Dictionary<Guid, Empire>()
        };
    }
}
