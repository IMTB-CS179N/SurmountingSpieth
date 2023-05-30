using Project.Input;
using System;
using UnityEngine;

namespace Project.Battle{

public class EncounterData : IDisposable
{
    [Order(0)]
    public int NumberEnemies { get; set; }

    [Order(1)]
    public string Enemy1 { get; set; }

    [Order(2)]
    public string Enemy2 { get; set; }

    [Order(3)]
    public string Enemy3 { get; set; }

    [Order(4)]
    public double BaseModifier { get; set; }

    [Order(5)]
    public double EliteModifier {get; set; }

    [Order(6)]
    public int Reward {get; set; }

    public EncounterData(){

        this.NumberEnemies = 0;
        this.Enemy1 = String.Empty;
        this.Enemy2 = String.Empty;
        this.Enemy3 = String.Empty;
        this.BaseModifier = 1.0;
        this.EliteModifier = 1.0;
        this.Reward = 0;
        
    }

    public void Dispose(){}

}
}