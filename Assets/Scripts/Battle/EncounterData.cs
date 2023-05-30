using Project.Input;
using System;
using UnityEngine;

namespace Project{

public class EncounterData : IDisposable
{
    private string[] m_easyenemies;
    private string[] m_mediumenemies;
    private string[] m_hardenemies;

    private int[] m_easyhealth;
    private int[] m_mediumhealth;
    private int[] m_hardhealth;

    private int[] m_easydamage;
    private int[] m_mediumdamage;
    private int[] m_harddamage;



    [Order(0)]
    public string[] EasyEnemies 
    {   
        get => this.m_easyenemies; 
        set => this.m_easyenemies = value ?? Array.Empty<string>();
    }

    [Order(1)]
    public string[] MediumEnemies 
    { 
        get => this.m_mediumenemies; 
        set => this.m_mediumenemies = value ?? Array.Empty<string>(); 
    }

    [Order(2)]
    public string[] HardEnemies 
    { 
        get => this.m_hardenemies; 
        set => this.m_hardenemies = value ?? Array.Empty<string>(); 
    }

    [Order(3)]
    public int[] EasyHealth 
    { 
        get => this.m_easyhealth; 
        set => this.m_easyhealth = value ?? Array.Empty<int>(); 
    }

    [Order(4)]
    public int[] MediumHealth 
    { 
        get => this.m_mediumhealth;
        set => this.m_mediumhealth = value ?? Array.Empty<int>();
    }

    [Order(5)]
    public int[] HardHealth 
    {
        get => this.m_hardhealth; 
        set => this.m_hardhealth = value ?? Array.Empty<int>();
    }

    [Order(6)]
    public int[] EasyDamage 
    {
        get => this.m_easydamage; 
        set => this.m_easydamage = value ?? Array.Empty<int>(); 
    }

    [Order(7)]
    public int[] MediumDamage 
    {
        get => this.m_mediumdamage; 
        set => this.m_mediumdamage = value ?? Array.Empty<int>(); 
    }

    [Order(8)]
    public int[] HardDamage 
    {
        get => this.m_harddamage; 
        set => this.m_harddamage = value ?? Array.Empty<int>();
    }

    [Order(9)]
    public int Reward {get; set; }
    //

    public EncounterData(){

        /*
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
        */
        
    }

    public void Dispose(){}

}
}