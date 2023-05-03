using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using System.Dispose;

public class Ability : IDisposable
{
    public Stats stats = new Stats();
    public string ClassName{get;set;}
    public string AbilityName {get; set;}
    //public string abilityDescription {get; set;}
    public int ManaCost {get; set;}
    public int DamageMultiplier {get; set;}
    

    public Ability() {
        AbilityName = "";
        //abilityDescription = "";
        ManaCost = -1;
        DamageMultiplier = 1;
        ClassName = stats.C_Class;
    }

    public void Dispose(){}
    void Update() {}
    void Start() {
    }
}