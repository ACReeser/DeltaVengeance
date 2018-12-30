using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class City{
    public Guid ID;
    public string Name;
    public Agent Player;

    public Dictionary<Guid, Infrastructure> Infrastructure;

    public Register GetResourcesPerTurnSummary()
    {
        return Infrastructure.Values.Aggregate(new Register(), (sum, building) =>
        {
            sum.Add(GameplayData.DifficultyDataset[Player.Difficulty][building.Type].Outputs);
            return sum;
        });
    }

    public decimal GetUnusedEnergy()
    {
        return Infrastructure.Values.Aggregate(0m, (sum, building) =>
        {
            sum += GameplayData.DifficultyDataset[Player.Difficulty][building.Type].Outputs.Energy;
            sum -= GameplayData.DifficultyDataset[Player.Difficulty][building.Type].Costs.Energy;
            return sum;
        });
    }

    internal bool CanBuild(BuildableType type)
    {
        decimal energyRequired = GameplayData.DifficultyDataset[Player.Difficulty][type].Costs.Energy;
        if (energyRequired != 0)
        {
            return energyRequired <= GetUnusedEnergy();
        }
        return true;
    }
}
