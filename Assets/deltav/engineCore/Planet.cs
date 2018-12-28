using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Planet  {
    public Dictionary<Guid, Empire> Empires;
}

public class Empire
{
    public List<City> Cities;

    public List<OrbitalInfrastructure> OrbitalInfrastructure;

    public List<Payload> GroundedPayloads;
    public List<Payload> OrbitingPayloads;

    public Agent Player;

    public string Name;

    public Register CurrentResources;
}
