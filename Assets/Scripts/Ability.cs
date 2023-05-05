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
    //public int CooldownTime {get;set;}
    //private int turnsRemaining;
    

    public Ability() {
        AbilityName = "";
        //abilityDescription = "";
        ManaCost = -1;
        DamageMultiplier = 1;
        ClassName = stats.C_Class;
        //CooldownTime = 0;
    }

    /*public void turnsCooldown() {
        turnsRemaining = CooldownTurns;
        if(turnsRemaining > 0) {
            turnsRemaining--;
        }
        else {
            //can use ability
        }
    }*/

    public void Dispose(){}
    void Update() {}
    void Start() {
        //turnsRemaining = 0;
    }
}