using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rocket : Buildable
{
    public int PayloadToLEO;
    public int DeltaV;
    public int MassTon;
    public int Impulse;
    public RocketFlight Flight;
    public Guid? OrbitingPlanet;

    internal bool CanLift(Dictionary<Guid, Payload> packedPayloads, Guid fromPlanetID, Guid toPlanetID)
    {
        return true;
    }
}

public class RocketFlight
{
    public Guid PlayerID, RocketID, FromPlanetID, ToPlanetID;
    public float FromPlanetAngle, ToPlanetAngle;
    public int LaunchTurn, ArrivalTurn;
    public Dictionary<Guid, Payload> Payloads;
}
