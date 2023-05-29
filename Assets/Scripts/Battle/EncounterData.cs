using Project.Input;
using System;
using UnityEngine;

namespace Project{

public class EncounterData : IDisposable
{
    [Order(0)]
    public string[] EasyEnemies 
    {   
        get => this.EasyEnemies; 
        set => this.EasyEnemies = new string[4];
    }

    [Order(1)]
    public string[] MediumEnemies 
    { 
        get => this.MediumEnemies; 
        set => this.MediumEnemies = new string[4]; 
    }

    [Order(2)]
    public string[] HardEnemies 
    { 
        get => this.HardEnemies; 
        set => this.HardEnemies = new string[4]; 
    }

    [Order(3)]
    public int[] EasyHealth 
    { 
        get => this.EasyHealth; 
        set => this.EasyHealth = new int[4]; 
    }

    [Order(4)]
    public int[] MediumHealth 
    { 
        get => this.MediumHealth;
        set => this.MediumHealth = new int[4]; 
    }

    [Order(5)]
    public int[] HardHealth 
    {
        get => this.HardHealth; 
        set => this.HardHealth = new int[4]; 
    }

    [Order(6)]
    public int[] EasyDamage 
    {
        get => this.EasyDamage; 
        set => this.EasyDamage = new int[4]; 
    }

    [Order(7)]
    public int[] MediumDamage 
    {
        get => this.MediumDamage; 
        set => this.MediumDamage = new int[4]; 
    }

    [Order(8)]
    public int[] HardDamage 
    {
        get => this.HardDamage; 
        set => this.HardDamage = new int[4];
    }

    [Order(9)]
    public int Reward {get; set; }

    public EncounterData(){

        this.EasyEnemies[0] = String.Empty;
        this.MediumEnemies[0] = String.Empty;
        this.HardEnemies[0] = String.Empty;

        this.EasyHealth[0] = 0;
        this.MediumHealth[0] = 0;
        this.HardHealth[0] = 0;

        this.EasyDamage[0] = 0;
        this.MediumDamage[0] = 0;
        this.HardDamage[0] = 0;

        this.Reward = 0;
        
    }

    public void Dispose(){}

}
}