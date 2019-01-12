using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// the view in the right hand side that shows the constructed infrastructure for a city
/// </summary>
[Serializable]
public class ConstructionView
{
    public RectTransform ParentFrame;
    public RectTransform InfrastructureButtonPrefab;
    public RectTransform EmptyButtonPrefab;
    public RectTransform BuildingButtonPrefab;

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
                InfrastructureButtons[i].SetSiblingIndex(i);

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
                    EmptyButtons[i].SetSiblingIndex(i);
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
        Energy.text = c.GetUsedEnergy() + "/" + sum.Energy + " Energy";
    }
}

[Serializable]
public class CostBenefitView
{
    public Text Assembly, Metal, Energy, Populace, Research;

    public void Fill(Register costBenefit, bool negative)
    {
        string op = negative ? "-" : "+";
        Assembly.text = costBenefit.Assembly == 0 ? "" : op + costBenefit.Assembly;
        Metal.text = costBenefit.Metal == 0 ? "" : op + costBenefit.Metal;
        Energy.text = costBenefit.Energy == 0 ? "" : op + costBenefit.Energy;
        if (Populace != null)
            Populace.text = costBenefit.Population == 0 ? "" : op + costBenefit.Population;
        if (Research != null)
            Research.text = costBenefit.Research == 0 ? "" : op + costBenefit.Research;
    }
}

[Serializable]
public class OrbitalView
{
    public RectTransform ParentFrame;
    public ConstructionView Infrastructure;
    public ConstructionView Payloads;
}

[Serializable]
public class NewInfrastructureView
{
    public RectTransform NewCityConstructionPanel, ChooseKindParent;

    public RectTransform NewInfrastructureButtonPrefab;
    public CostBenefitView Cost, Benefit;
    public Button BeginConstructionButton;
    internal BuildableType SelectedType;

    public void Toggle(bool state)
    {
        NewCityConstructionPanel.gameObject.SetActive(state);
        if (state)
        {
            BeginConstructionButton.interactable = false;
            int i = 0;
            foreach(var buildableKVP in PhaseManager.S.DifficultyData)
            {
                if (buildableKVP.Key > BuildableType.AluminiumSmelter)
                {
                    break;
                }
                if (buildableKVP.Key <= BuildableType.Unknown)
                    continue;

                Transform child = null;
                if (i < ChooseKindParent.childCount)
                {
                    child = ChooseKindParent.GetChild(i);
                }
                else
                { 
                    child = GameObject.Instantiate(NewInfrastructureButtonPrefab, ChooseKindParent);
                }
                i++;
                child.GetChild(0).GetComponent<Text>().text = CanvasManager.ToSentenceCase(buildableKVP.Key.ToString());
                child.GetComponent<ButtonHoverTextColorChange>().OnHover.AddListener(() => UpdateCostBenefit(buildableKVP.Key));
                child.GetComponent<Button>().onClick.AddListener(() => SelectConstruction(buildableKVP.Key));
            }
        }
    }

    public void UpdateCostBenefit(BuildableType type)
    {
        Cost.Fill(PhaseManager.S.DifficultyData[type].Costs, true);
        Benefit.Fill(PhaseManager.S.DifficultyData[type].Outputs, false);
    }
    public void SelectConstruction(BuildableType type)
    {
        SelectedType = type;
        BeginConstructionButton.interactable = true;
    }
    public void ConfirmConstruction(CanvasManager parent)
    {
        PhaseManager.S.BuildInfrastructure(parent.FocusedPlanet.ID, parent.FocusedCity.ID, SelectedType);
        Toggle(false);
    }
}

public class CanvasManager : MonoBehaviour {
    public CameraManager cameraMan;
    public Text PhaseText;
    public RectTransform EndTurnButton;

    public RectTransform PlanetPanel;
    public RegisterView PlanetRegister;


    public RectTransform CityPanel;
    public RegisterView CityRegister;
    public ConstructionView CityConstruction;
    public NewInfrastructureView NewCityInfrastructure;

    public Planet FocusedPlanet;
    public City FocusedCity;
    public OrbitalView FocusedOrbital;

    // Use this for initialization
    void Start () {
        PhaseManager.S.OnPhaseChange += S_OnPhaseChange;
        PhaseManager.S.OnTurnResolved += S_OnTurnResolved;
        S_OnPhaseChange(PhaseManager.S.CurrentPhase);
        UnFocusCity();
        UnFocusPlanet();
        CityConstruction.CanvasMan = this;
        ToggleNewBuild(false);
    }

    private void S_OnTurnResolved(SolarSystem NewState)
    {
        if (FocusedPlanet != null)
        {
            PlanetRegister.Fill(FocusedPlanet);
        }

        if (FocusedCity != null)
        {
            FocusCity(NewState.GetCity(FocusedPlanet.ID, PhaseManager.S.PlayerID, FocusedCity.ID));
        }
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
        FocusedPlanet = planet;
        PlanetPanel.gameObject.SetActive(true);
        PlanetRegister.Fill(FocusedPlanet);
    }

    internal void UnFocusPlanet()
    {
        FocusedPlanet = null;
        PlanetPanel.gameObject.SetActive(false);
        UnFocusCity();
        FocusedOrbital.ParentFrame.gameObject.SetActive(false);
        cameraMan.ToggleDrydock(false);
    }

    internal void FocusCity(City c)
    {
        FocusedCity = c;
        CityPanel.gameObject.SetActive(true);
        CityRegister.Fill(FocusedCity);
        CityConstruction.Fill(FocusedCity);
        FocusedOrbital.ParentFrame.gameObject.SetActive(false);
        cameraMan.FocusTerrestrial();
    }

    internal void UnFocusCity()
    {
        FocusedCity = null;
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
        NewCityInfrastructure.Toggle(state);
    }

    public void ConfirmConstruction()
    {
        NewCityInfrastructure.ConfirmConstruction(this);
        PlanetRegister.Fill(FocusedPlanet);
        CityRegister.Fill(FocusedCity);
        CityConstruction.Fill(FocusedCity);
    }

    public void OpenOrbitalAssets()
    {
        FocusedOrbital.ParentFrame.gameObject.SetActive(true);
        CityPanel.gameObject.SetActive(false);
        cameraMan.FocusOrbital();
        cameraMan.ToggleDrydock(false);
    }

    public void OpenInterplanetaryLaunch()
    {
        cameraMan.ToggleDrydock(true);
    }
}
