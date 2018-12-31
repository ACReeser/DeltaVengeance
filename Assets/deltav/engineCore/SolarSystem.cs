using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem {
    public ISolarSystemBridge Bridge;

    public Dictionary<Guid, Planet> Planets;
    public Dictionary<Guid, Agent> Players;

    public City GetCity(Guid planet, Guid player, Guid city)
    {
        return Planets[planet].Empires[player].Cities[city];
    }

    public Empire GetEmpire(Guid planet, Guid player)
    {
        return Planets[planet].Empires[player];
    }

    public Payload GetGroundedPayload(Guid planet, Guid player, Guid payload)
    {
        return Planets[planet].Empires[player].GroundedPayloads[payload];
    }

    public Payload GetOrbitingPayload(Guid planet, Guid player, Guid payload)
    {
        return Planets[planet].Empires[player].OrbitingPayloads[payload];
    }

    public Infrastructure GetGroundInfrastructure(Guid planet, Guid player, Guid city, Guid infra)
    {
        return Planets[planet].Empires[player].Cities[city].Infrastructure[infra];
    }

    internal void Hydrate()
    {
        foreach(Planet p in Planets.Values)
        {
            foreach(var kvp in p.Empires)
            {
                var player = Players[kvp.Key];
                kvp.Value.Player = player;

                foreach(City c in kvp.Value.Cities.Values)
                {
                    c.Player = player;
                }
            }
        }
    }
}

