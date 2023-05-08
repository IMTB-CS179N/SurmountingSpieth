using UnityEngine;
// using Project.Entity
using System;

namespace Project
{
    public static class BattleUtility
    {
        public static bool CalculateHit(float Precision, float DodgeChance)
        {
            if (DodgeChance == 0.0)
            {
                return true;
            }
            else if (DodgeChance == 1.0)
            {
                return false;
            }
            float ChanceToHit = (1f - DodgeChance) + (Precision * 0.01f);
            int Hit = (int)(ChanceToHit * 100);
            return UnityEngine.Random.Range(0, 100) < Hit;
        }

        public static float CalculateDamage(
            float BaseDamage,
            float CriticalChance,
            float CriticalDamage
        )
        {
            float Damage = BaseDamage;
            if (UnityEngine.Random.Range(0, 100) < CriticalChance)
            {
                Damage *= CriticalDamage;
            }
            return Damage;
        }

        public static bool Attack(IEntity attacker, IEntity receiver)
        {
            if (CalculateHit(attacker.Precision, receiver.DodgeChance))
            {
                var damage = CalculateDamage(
                    attacker.Damage,
                    attacker.CriticalChance,
                    attacker.CriticalDamage
                );
                Debug.Log(
                    "Doing " + damage + " damage to entity with " + receiver.Health + " health"
                );
                receiver.Health -= receiver.DepleteArmor((int)damage);
                Debug.Log("New health: " + receiver.Health);
                if (receiver.Health <= 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
