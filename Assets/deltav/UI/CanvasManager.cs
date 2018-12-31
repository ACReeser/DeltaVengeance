using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ConstructionView
{
    public RectTransform ParentFrame;
    public RectTransform InfrastructureButtonPrefab;
    public RectTransform EmptyButtonPrefab;

    private RectTransform[] InfrastructureButtons = new RectTransform[12];
    private RectTransform[] EmptyButtons = new RectTransform[12];

    public CanvasManager CanvasMan;

    public void Fill(City c)
    {
        var infras = c.Infrastructure.Values.ToArray();
        for (int i = 0; i < 12; i++)
        {
            Infrastructure structure = null;
            if (i < infras.Length)
            {
                structure = infras[i];
                if (InfrastructureButtons[i] == null)
                {
                    InfrastructureButtons[i] = GameObject.Instantiate(InfrastructureButtonPrefab, ParentFrame);
                }
                else
                {
                    InfrastructureButtons[i].gameObject.SetActive(true);
                }
                InfrastructureButtons[i].GetChild(0).GetComponent<Text>().text = CanvasManager.ToSentenceCase(structure.Type.ToString());

                if (EmptyButtons[i] != null)
                {
                    EmptyButtons[i].gameObject.SetActive(false);
                }
            }
            else
            {
                if (EmptyButtons[i] == null)
                {
                    EmptyButtons[i] = GameObject.Instantiate(EmptyButtonPrefab, ParentFrame);
                    EmptyButtons[i].GetComponent<Button>().onClick.AddListener(() =>
                    {
                        CanvasMan.ToggleNewBuild(true);
                    });
                }
                else
                {
                    EmptyButtons[i].gameObject.SetActive(true);
                }

                if (InfrastructureButtons[i] != null)
                {
                    InfrastructureButtons[i].gameObject.SetActive(false);
                }
            }
        }
    }
}

[Serializable]
public class RegisterView
{
    public Text Name, Population;
    public Text Growth, Assembly, Metal, Energy;

    public void Fill(Planet p)
    {
        Name.text = " > " + p.Name.ToUpperInvariant();
        Assembly.text = p.Empires[PhaseManager.S.PlayerID].CurrentResources.Assembly + " Assembly";
        Metal.text = p.Empires[PhaseManager.S.PlayerID].CurrentResources.Metal + " Metal";
        Energy.text = p.Empires[PhaseManager.S.PlayerID].CurrentResources.Energy + " Energy";
    }
    public void Fill(City c)
    {
        Name.text = " > " + c.Name.ToUpperInvariant();
        var sum = c.GetResourcesPerTurnSummary();
        Assembly.text = "+"+sum.Assembly + " Assembly";
        Metal.text = "+" + sum.Metal + " Metal";
        Energy.text = sum.Energy + " Energy";
    }
}

public class CanvasManager : MonoBehaviour {
    public Text PhaseText;
    public RectTransform EndTurnButton;

    public RectTransform PlanetPanel;
    public RegisterView PlanetRegister;


    public RectTransform CityPanel;
    public RegisterView CityRegister;
    public ConstructionView CityConstruction;
    public RectTransform NewCityConstructionPanel;

    // Use this for initialization
    void Start () {
        PhaseManager.S.OnPhaseChange += S_OnPhaseChange;
        S_OnPhaseChange(PhaseManager.S.CurrentPhase);
        UnFocusCity();
        UnFocusPlanet();
        CityConstruction.CanvasMan = this;
        ToggleNewBuild(false);
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
        PlanetRegister.Fill(planet);
    }

    internal void UnFocusPlanet()
    {
        PlanetPanel.gameObject.SetActive(false);
        UnFocusCity();
    }

    internal void FocusCity(City c)
    {
        CityPanel.gameObject.SetActive(true);
        CityRegister.Fill(c);
        CityConstruction.Fill(c);
    }

    internal void UnFocusCity()
    {
        CityPanel.gameObject.SetActive(false);
    }

    public void EndTurnButtonClick()
    {
        PhaseManager.S.EndTurn();
    }

    public static string ToSentenceCase(string str)
    {
        return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + m.Value[1]);
    }

    public void ToggleNewBuild(bool state)
    {
        NewCityConstructionPanel.gameObject.SetActive(state);
    }
}
