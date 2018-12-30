using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityClick : MonoBehaviour {

    public MeshRenderer render;
    // Use this for initialization
    void Start ()
    {
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
