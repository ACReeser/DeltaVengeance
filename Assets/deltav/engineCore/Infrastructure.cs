using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
    Pacifist,
    Avenger
}

public class DifficultyDatasets: Dictionary<Difficulty, DifficultyData>
{
}

public static class GameplayData
{
    public static DifficultyDatasets DifficultyDataset = new DifficultyDatasets()
    {
        {
            Difficulty.Pacifist, new DifficultyData(){
                {
                    BuildableType.Mine, new BuildableData(){
                        Class = typeof(Infrastructure),
                        Outputs = new Register(){
                            Metal = 10
                        },
                        Costs = new Register(){
                            Metal = 5,
                            Energy = 0.5m,
                            Assembly = 5
                        }
                    }
                }, {
                    BuildableType.Propellant, new BuildableData(){
                        Class = typeof(Payload),
                        Outputs = new Register(){
                        },
                        Costs = new Register(){
                            Assembly = 5
                        },
                        WeightTon = 10
                    }
                },{
                    BuildableType.Cargo, new BuildableData(){
                        Class = typeof(Payload),
                        Outputs = new Register(){
                        },
                        Costs = new Register(){
                            Metal = 10,
                            Assembly = 5
                        },
                        WeightTon = 10
                    }
                },
            }
        }
    };
}

public enum BuildableType
{
    Unknown = -1,
    //infrastructure 0-99
    Mine,
    UraniumWell,
    NuclearPlant,
    Farm,
    SolarPlant,
    Factory,
    LaunchFacility,
    AluminiumSmelter,
    //orbital infrastructure 100-199
    //payloads 200-299
    Colonist = 200,
    PackedInfrastructure,
    Cargo,
    Propellant,
    NuclearCore,
    NuclearMissile
    //rocket? 300-399
    //upgrades 400-499
}

public enum DestructionType
{
    None,
    Fire
}
public class DifficultyData : Dictionary<BuildableType, BuildableData>
{
}
public class BuildableData
{
    public Register Outputs;
    public Register Costs;
    public int WeightTon = 1;
    public Type Class;
}


public class Buildable
{
    public BuildableType Type = BuildableType.Unknown;
    public Guid ID;
}

public class Infrastructure : Buildable {
    public DestructionType Destroyed;
}

public class OrbitalInfrastructure : Infrastructure
{

}

public class Payload : Buildable
{

}

public class InfrastructurePayload : Payload
{
    public Infrastructure Packed;
}

public class Upgrade : Buildable
{

}
