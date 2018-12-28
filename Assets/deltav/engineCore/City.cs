using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class City{

    public string Name;

    public List<Infrastructure> Infrastructure;

    public Register GetResourcesPerTurnSummary()
    {
        return Infrastructure.Aggregate(new Register(), (sum, building) =>
        {
            sum.Add(building.ResourcesPerTurn);
            return sum;
        });
    }
}
