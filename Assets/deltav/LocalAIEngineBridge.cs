using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LocalAIEngineBridge : MonoBehaviour, IEngineBridge
{
    public SolarSystem CurrentState { get; private set; }
    public PhasedCommands PlayerCommands;
    public IEnumerator SubmitCommands(PhasedCommands commands)
    {
        PlayerCommands = commands;
        yield return true;
    }
    public event TurnResolvedEvent OnCommandsResolved;

    void Start()
    {
    }

    void Update()
    {
        if (PlayerCommands != null)
        {
            List<PhasedCommands> commands = new List<PhasedCommands>() { PlayerCommands };
            var aiCommands = GetAICommands();
            if (aiCommands != null)
            {
                commands.Add(aiCommands);
            }
            var resolution = new DVEngine().Tick(CurrentState, commands.ToArray());
            PlayerCommands = null;
            if (OnCommandsResolved != null)
                OnCommandsResolved(resolution);
        }
    }

    private PhasedCommands GetAICommands()
    {
        return null;
    }

    public IEnumerator GetSolarSystem()
    {
        if (CurrentState == null)
        {
            CurrentState = DVEngine.GetNewGame(1, 0);
        }
        yield return null;
    }
}