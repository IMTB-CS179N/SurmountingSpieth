using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Project.Input;

public class Ability : IDisposable
{
    [Order(0)]
    public string C_Class {get;set;}
    [Order(1)]
    public string AbilityName {get; set;}
    [Order(2)]
    public int ManaCost {get; set;}
    [Order(3)]
    public int DamageMultiplier {get; set;}

    //public int CooldownTime {get;set;}
    //private int turnsRemaining;
    

    public Ability() {
        C_Class = "";
        AbilityName = "";
        ManaCost = -1;
        DamageMultiplier = -1;
        //CooldownTime = 0;
    }

    public void manaReduction(int currentMana) {
        currentMana -= ManaCost;
    }
    
    public void abilityDamageIncrease(int currentDamage) {
        currentDamage *= DamageMultiplier;
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
    void Start(
    ) {
        //turnsRemaining = 0;
    }
}
