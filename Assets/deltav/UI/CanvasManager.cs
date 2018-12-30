using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {
    public Text PhaseText;
    public RectTransform EndTurnButton;

    public RectTransform PlanetPanel;
    public Text PlanetName;


    public RectTransform CityPanel;
    public Text CityName;

    // Use this for initialization
    void Start () {
        PhaseManager.S.OnPhaseChange += S_OnPhaseChange;
        S_OnPhaseChange(PhaseManager.S.CurrentPhase);
        UnFocusCity();
        UnFocusPlanet();
    }

    private void S_OnPhaseChange(Phase NewPhase)
    {
        PhaseText.text = NewPhase.ToString();
        EndTurnButton.gameObject.SetActive(NewPhase == Phase.Planning);
    }

    // Update is called once per frame
    void Update () {
		
	}

    internal void FocusPlanet(Planet planet)
    {
        PlanetPanel.gameObject.SetActive(true);
        PlanetName.text = " > " +planet.Name.ToUpperInvariant();
    }

    internal void UnFocusPlanet()
    {
        PlanetPanel.gameObject.SetActive(false);
        UnFocusCity();
    }

    internal void FocusCity(City c)
    {
        CityPanel.gameObject.SetActive(true);
        CityName.text = " > "+c.Name.ToUpperInvariant();
    }

    internal void UnFocusCity()
    {
        CityPanel.gameObject.SetActive(false);
    }

    public void EndTurnButtonClick()
    {
        PhaseManager.S.EndTurn();
    }
}
