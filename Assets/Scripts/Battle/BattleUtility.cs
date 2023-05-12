using Project.Game;

using UnityEngine;

namespace Project.Battle
{
    public struct OutcomeInfo
    {
        public bool HitMissed;
        public int DamageDealt;
        public bool KilledAnyEnemy;

    }

    public static class BattleUtility
    {
        public static bool CheckIfHits(float precision, float evasion)
        {
            // evasion is 0.0f to 1.0f
            // precision is 0.0f to infinity

            if (evasion <= 0.0f)
            {
                return true; // if dodge is 0%, always hits
            }

            if (evasion >= 1.0f)
            {
                return false; // if dodge is 100%, never hits (should never happen though?)
            }

            var chance = precision * (1.0f - evasion);

            if (chance >= 1.0f)
            {
                return true; // if too much precision, always hits
            }

            return Random.Range(0, 100) < (int)(chance * 100.0f);
        }

        public static int CalculateDamage(int damage, int armor, float critChance, float critMultiplier, bool trueDamage)
        {
            float applied = damage;

            if (critChance >= 1.0f || (critChance > 0.0f && Random.Range(0, 100) < (int)(critChance * 100.0f)))
            {
                applied *= critMultiplier; // if crit chance is 100% or if it applies in general
            }

            return (int)(trueDamage ? applied : (applied * (100.0f / (100.0f + armor))));
        }

        public static void Attack(IEntity attacker, IEntity receiver)
        {
            // information we have to receive:
            //   - entity information, attacker and receiver
            //   - information about entities
            //   - whether an ability should be used by an attacker
            
            // information we have to send back:
            //   - whether an attack hit
            //   - how much damage was dealt

        }
    }
}
