
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Agent {
    public string Name;
    public Stack<Command> PreviousMoves;
    public PhasedCommands Commands;
    public Guid ID;
    public Difficulty Difficulty;
    public Dictionary<Guid, Rocket> Rockets;
}

public class PhasedCommands
{
    public Guid PlayerID;
    public Queue<LaunchPayloadCommand> LaunchPayloads = new Queue<LaunchPayloadCommand>();
    public Queue<BuildInCityCommand> BuildInfrastructure = new Queue<BuildInCityCommand>();
    public Queue<BuildPayloadOnPlanetCommand> BuildPayloads = new Queue<BuildPayloadOnPlanetCommand>();

    public void ExecuteLaunchPayloads(SolarSystem system)
    {
        foreach (var e in LaunchPayloads) { e.Execute(system); }
    }
    public void ExecuteBuildPayloads(SolarSystem system)
    {
        foreach (var e in BuildPayloads) { e.Execute(system); }
    }
    public void ExecuteBuildInfrastructure(SolarSystem system)
    {
        foreach (var e in BuildInfrastructure) { e.Execute(system); }
    }
}