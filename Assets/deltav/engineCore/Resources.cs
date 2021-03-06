﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStockpile
{
    decimal NuclearCores { get; set; }
    int Population { get; set; }
    int Research { get; set; }
    int Metal { get; set; }
}
public interface ICapability
{
    int Assembly { get; set; }
    decimal Energy { get; set; }
    int TonnesToOrbit { get; set; }
}

/// <summary>
/// a register of stockpile and capability
/// </summary>
public class Register : IStockpile, ICapability
{
    public int Assembly { get; set; }
    public decimal Energy { get; set; }
    public int Metal { get; set; }
    public decimal NuclearCores { get; set; }
    public int TonnesToOrbit { get; set; }
    public int Population { get; set; }
    public int Research { get; set; }

    internal void Add(Register other)
    {
        Assembly += other.Assembly;
        Energy += other.Energy;
        Metal += other.Metal;
        NuclearCores += other.NuclearCores;
        TonnesToOrbit += other.TonnesToOrbit;
        Population += other.Population;
        Research += other.Research;
    }

    internal void ResetCapability()
    {
        Assembly = 0;
        Energy = 0;
        TonnesToOrbit = 0;
    }

    internal bool EqualToOrGreaterThan(Register other)
    {
        return Assembly >= other.Assembly &&
            Energy >= other.Energy &&
            NuclearCores >= other.NuclearCores &&
            TonnesToOrbit >= other.TonnesToOrbit &&
            Population >= other.Population &&
            Research >= other.Research;
    }

    internal void AddToCapability(ICapability other)
    {
        Assembly += other.Assembly;
        Energy += other.Energy;
        TonnesToOrbit += other.TonnesToOrbit;
    }

    internal void AddToStockpile(IStockpile other)
    {
        Metal += other.Metal;
        NuclearCores += other.NuclearCores;
        Population += other.Population;
        Research += other.Research;
    }

    internal void SetCapability(ICapability other)
    {
        Assembly = other.Assembly;
        Energy = other.Energy;
        TonnesToOrbit = other.TonnesToOrbit;
    }

    internal void Subtract(Register other)
    {
        Assembly -= other.Assembly;
        Energy -= other.Energy;
        Metal -= other.Metal;
        NuclearCores -= other.NuclearCores;
        TonnesToOrbit -= other.TonnesToOrbit;
        Population -= other.Population;
        Research -= other.Research;
    }
}
