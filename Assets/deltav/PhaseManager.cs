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

public class PhaseManager : MonoBehaviour {

    public static PhaseManager S;
    public Phase CurrentPhase = Phase.JoiningGame;
    public IEngineBridge EngineBridge;
    public PhasedCommands CurrentCommands;
    public TurnResolution LastTurnResolution, PendingTurnResolution;
    public SolarSystem State;
    public event PhaseChange OnPhaseChange;
    public LocalAIEngineBridge LocalAIEngineBridge;
    internal Guid PlayerID;

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
        yield return null;
        CurrentPhase = Phase.Planning;
        if (OnPhaseChange != null)
            OnPhaseChange(CurrentPhase);
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
        EngineBridge.SubmitCommands(CurrentCommands);
        CurrentPhase = Phase.WaitingOnOtherPlayers;
        if (OnPhaseChange != null)
            OnPhaseChange(CurrentPhase);
    }
}
