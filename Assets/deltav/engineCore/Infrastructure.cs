using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DestructionType
{
    None,
    Fire
}

public class Buildable
{
    public Register Cost;
    public int WeightTon;
}

public class Infrastructure : Buildable {
    public Register ResourcesPerTurn;
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

public class Rocket : Buildable
{
    public int PayloadToLEO;
    public int DeltaV;
}

public class Upgrade : Buildable
{

}
