using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Project.Game
{
    public interface IEntity
    {
        int Health { get; }

        int Armor { get; }
        
        int Damage { get; }

        int Precision { get; }

        float DodgeChance { get; }

        float CriticalChance { get; }
        
        float CriticalDamage { get; }

        int DealDamage(int damage);

        int ComputeDamage();
    }
}
