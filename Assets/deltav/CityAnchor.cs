using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CityAnchor : MonoBehaviour {
    public string CityName;
    public float RotX, RotY;

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public City GetTiedCity()
    {
        var planetName = transform.parent.parent.parent.parent.parent.name;
        var planet = PhaseManager.S.State.Planets.Values.FirstOrDefault(x => x.Name.ToLowerInvariant() == planetName.ToLowerInvariant());
        return planet.Empires[PhaseManager.S.PlayerID].Cities.Values.FirstOrDefault(x => x.Name.ToLowerInvariant() == CityName.ToLowerInvariant());

    }
}
