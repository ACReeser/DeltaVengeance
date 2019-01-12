using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CelestialBodyClickTarget : MonoBehaviour {
    public MeshRenderer render;
    public Transform OrbitAnchor;
    
    public Planet GetTiedPlanet()
    {
        var planetName = transform.parent.parent.parent.name;
        return PhaseManager.S.State.Planets.Values.FirstOrDefault(x => x.Name.ToLowerInvariant() == planetName.ToLowerInvariant());
    }

	// Use this for initialization
	void Start () {
	    if (render == null)
        {
            render = transform.GetComponent<MeshRenderer>();
            render.enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private bool hovering = false;

    internal void MouseOver()
    {
        if (!hovering)
        {
            hovering = true;
            render.enabled = true;
        }
    }

    internal void MouseOut()
    {
        if (hovering)
        {
            hovering = false;
            render.enabled = false;
        }
    }
}
