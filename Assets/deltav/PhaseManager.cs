using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Phase
{
    JoiningGame,
    Planning,
    WaitingOnOtherPlayers,
    Executing
}
public delegate void PhaseChange(Phase NewPhase);
public delegate void TurnResolved(SolarSystem NewState);

public class PhaseManager : MonoBehaviour {

    public static PhaseManager S;
    public Phase CurrentPhase = Phase.JoiningGame;
    public IEngineBridge EngineBridge;
    public PhasedCommands CurrentCommands;
    public TurnResolution LastTurnResolution, PendingTurnResolution;
    public SolarSystem State;
    public event PhaseChange OnPhaseChange;
    public event TurnResolved OnTurnResolved;
    public LocalAIEngineBridge LocalAIEngineBridge;
    internal Guid PlayerID;

    /// <summary>
    /// convenience getter for difficulty data
    /// </summary>
    internal DifficultyData DifficultyData
    {
        get
        {
            return GameplayData.DifficultyDataset[State.Players[PlayerID].Difficulty];
        }
    }
        

    // Use this for initialization
    void Start () {
        S = this;
        EngineBridge = LocalAIEngineBridge;
        EngineBridge.OnCommandsResolved += EngineBridge_OnCommandsResolved;
        StartCoroutine(GetSolarSystem());
        CurrentCommands = new PhasedCommands();
        CurrentPhase = Phase.Planning;
        if (OnPhaseChange != null)
            OnPhaseChange(CurrentPhase);
    }

    private IEnumerator GetSolarSystem()
    {
        yield return EngineBridge.GetSolarSystem();
        State = EngineBridge.CurrentState;
        State.Hydrate();
        PlayerID = State.Players.Keys.First();
    }

    private void EngineBridge_OnCommandsResolved(TurnResolution resolution)
    {
        CurrentCommands = new PhasedCommands();
        LastTurnResolution = resolution;
        PendingTurnResolution = resolution;
        CurrentPhase = Phase.Executing;
        if (OnPhaseChange != null)
            OnPhaseChange(CurrentPhase);
    }

    private IEnumerator RenderResolution(TurnResolution resolution)
    {
        yield return new WaitForSeconds(1f);
        State = LastTurnResolution.newSolarSystemState;
        CurrentPhase = Phase.Planning;
        if (OnPhaseChange != null)
            OnPhaseChange(CurrentPhase);
        if (OnTurnResolved != null)
            OnTurnResolved(State);
    }

    // Update is called once per frame
    void Update () {
		if (PendingTurnResolution != null)
        {
            StartCoroutine(RenderResolution(PendingTurnResolution));
            PendingTurnResolution = null;
        }
	}

    public void EndTurn()
    {
        StartCoroutine(SubmitCommands());
    }
    public IEnumerator SubmitCommands()
    {
        yield return StartCoroutine(EngineBridge.SubmitCommands(CurrentCommands));
        CurrentPhase = Phase.WaitingOnOtherPlayers;
        if (OnPhaseChange != null)
            OnPhaseChange(CurrentPhase);
    }

    internal void BuildInfrastructure(Guid focusedPlanetID, Guid focusedCityID, BuildableType selectedType)
    {
        CurrentCommands.BuildInfrastructure.Enqueue(new BuildInCityCommand(focusedPlanetID, PlayerID, focusedCityID, selectedType));
        State.GetEmpire(focusedPlanetID, PlayerID).CurrentResources.Subtract(DifficultyData[selectedType].Costs);
    }
}
