
using System;
using System.Collections.Generic;


public abstract class Command
{
    public abstract void Execute(SolarSystem system);
}

public class LaunchPayloadCommand : Command
{
    public LaunchPayloadCommand(Payload p, Planet planet)
    {

    }

    public override void Execute(SolarSystem system)
    {

    }
}
public class BuildInfrastructureInCityCommand: Command
{
    public BuildInfrastructureInCityCommand(Infrastructure i, City c)
    {

    }

    public override void Execute(SolarSystem system)
    {

    }
}

public class BuildPayloadOnPlanetCommand : Command
{
    public BuildPayloadOnPlanetCommand(Payload p, Planet planet)
    {

    }

    public override void Execute(SolarSystem system)
    {

    }
}

public class Agent {
    public string Name;
    public List<Command> PreviousMoves;
    public List<Command> MoveQueue;
    public Guid ID;
}
