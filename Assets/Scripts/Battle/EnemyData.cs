using Project.Input;
using System;
using UnityEngine;

namespace Project{
public class EnemyData : IDisposable
{

    private Sprite m_sprite;
    
    [Order(0)]
    public string EnemyName { get; set; }

    [Order(1)]
    public int EnemyHealth { get; set; }

    [Order(2)]
    public int EnemyDamage { get; set; }

    [Order(3)]
    public int EnemyArmor { get; set; }

    [Order(4)]
    public int EnemyPrecision { get; set; }

    [Order(5)]
    public double EnemyEvasion { get; set; }
    
    [Order(6)]
    public double EnemyCritChance { get; set; }

    [Order(7)]
    public double EnemyCritMult { get; set; }

    [Order(8)]
    public string PlayerAbility { get; set; }

    [Order(9)]
    public string AbilityName { get; set; }

    [Order(10)]
    public Sprite Sprite
        {
            get => this.m_sprite;
            set => this.m_sprite = value == null ? ResourceManager.DefaultSprite : value;
        }


    public EnemyData(){

        this.EnemyName = String.Empty;
        this.EnemyHealth = 0;
        this.EnemyDamage = 0;
        this.EnemyArmor = 0;
        this.EnemyPrecision = 0;
        this.EnemyEvasion = 0.0;
        this.EnemyCritChance = 0.0;
        this.EnemyCritMult = 0.0;
        this.PlayerAbility = String.Empty;
        this.AbilityName = String.Empty;

    }

    public void Dispose(){}

}
}
