using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {
    public Text PhaseText;
    public RectTransform EndTurnButton;

    public RectTransform PlanetPanel;


    public RectTransform CityPanel;

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
    }

    internal void UnFocusPlanet()
    {
        PlanetPanel.gameObject.SetActive(false);
    }

    internal void FocusCity(City c)
    {
        CityPanel.gameObject.SetActive(true);
    }

    internal void UnFocusCity()
    {
        CityPanel.gameObject.SetActive(false);
    }
}
