using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public CameraManager cameraManager;
    public CanvasManager canvasMan;
    private CelestialBodyClickTarget lastTarget;
    private CityClick lastCityTarget;

    // Update is called once per frame
    void Update () {
        if (!cameraManager.Focused)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider && hit.collider != null)
                {
                    var cbct = hit.collider.transform.GetComponent<CelestialBodyClickTarget>();
                    if (cbct != null)
                    {
                        cbct.MouseOver();
                        lastTarget = cbct;
                        if (Input.GetMouseButtonUp(0))
                        {
                            WhenNotHoverOverCelestialBody();
                            cameraManager.FocusOn(cbct);
                        }
                    }
                    else
                    {
                        WhenNotHoverOverCelestialBody();
                    }
                }
                else
                {
                    WhenNotHoverOverCelestialBody();
                }
            }
            else
                WhenNotHoverOverCelestialBody();
        }
        else
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider && hit.collider != null)
                {
                    var cityClick = hit.collider.transform.GetComponent<CityClick>();
                    if (cityClick != null)
                    {
                        cityClick.MouseOver();

                        if (Input.GetMouseButtonUp(0))
                        {
                            WhenNotHoverOverCity();

                            canvasMan.FocusCity(cityClick.transform.parent.parent.GetComponent<CityAnchor>().GetTiedCity());
                            //cameraManager.FocusOn(cbct);
                        }
                        lastCityTarget = cityClick;
                    }
                    else
                    {
                        WhenNotHoverOverCity();
                    }
                }
                else
                {
                    WhenNotHoverOverCity();
                }
            }
            else
            {
                WhenNotHoverOverCity();
            }
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            cameraManager.ExitPlanetFocus();
        }

        cameraManager.Rotate();
    }

    private void WhenNotHoverOverCity()
    {
        if (lastCityTarget != null)
        {
            lastCityTarget.MouseOut();
            lastCityTarget = null;
        }
    }

    private void WhenNotHoverOverCelestialBody()
    {
        if (lastTarget != null)
        {
            lastTarget.MouseOut();
            lastTarget = null;
        }
    }
}
