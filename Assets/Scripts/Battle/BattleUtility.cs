using Project.Game;

using System;

using UnityEngine;

using Random = UnityEngine.Random;

namespace Project.Battle
{
    public struct PlayerOutcomeInfo
    {
        public struct EnemyInfo
        {
            public int DamageDealt;
            public bool Engaged;
            public bool Missed;
            public bool Killed;
            public bool Dispel;
        }

        public bool Cleansed;
        public int HealReceived;
        public EnemyInfo[] EnemyInfos;

        public PlayerOutcomeInfo(int enemyCount)
        {
            this.Cleansed = false;
            this.HealReceived = 0;
            this.EnemyInfos = enemyCount == 0 ? Array.Empty<EnemyInfo>() : new EnemyInfo[enemyCount];
        }
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

            var chance = (precision * 0.5f) * (1.0f - evasion);

            if (chance >= 1.0f)
            {
                return true; // if too much precision, always hits
            }

            return Random.Range(0, 100) < (int)(chance * 100.0f);
        }

        public static int CalculateDamage(int damage, int armor, float critChance, float critMultiplier)
        {
            float applied = damage;

            if (critChance >= 1.0f || (critChance > 0.0f && Random.Range(0, 100) < (int)(critChance * 100.0f)))
            {
                applied *= critMultiplier; // if crit chance is 100% or if it applies in general
            }

            return (int)(applied * (100.0f / (100.0f + armor)));
        }

        public static PlayerOutcomeInfo Attack(Player player, Enemy[] enemies, int enemyIndex, int abilityUsed)
        {
            // information we have to receive:
            //   - entity information, attacker and receiver
            //   - information about entities
            //   - whether an ability should be used by an attacker
            
            // information we have to send back:
            //   - whether an attack hit
            //   - how much damage was dealt

            if (player is null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            if (enemies is null)
            {
                throw new ArgumentNullException(nameof(enemies));
            }

            var outcome = new PlayerOutcomeInfo(enemies.Length);

            if (abilityUsed < 0 || abilityUsed > player.Abilities.Count)
            {
                var enemy = enemies[enemyIndex];

                Debug.Assert(enemy is not null && enemy.IsAlive);

                ref readonly var playerStats = ref player.EntityStats;
                ref readonly var enemysStats = ref enemy.EntityStats;

                ref var info = ref outcome.EnemyInfos[enemyIndex];

                info.Engaged = true;

                if (CheckIfHits(playerStats.Precision, enemysStats.Evasion))
                {
                    int damage = CalculateDamage(playerStats.Damage, enemysStats.Armor, playerStats.CritChance, playerStats.CritMultiplier);

                    enemy.ApplyDamage(damage);

                    info.DamageDealt = damage;

                    info.Killed = !enemy.IsAlive;
                }
                else
                {
                    info.Missed = true;
                }
            }
            else
            {
                if (player.CanUseAbility(abilityUsed) != AbilityUsage.CanUse)
                {
                    throw new Exception($"Cannot use ability at index {abilityUsed}");
                }

                var ability = player.Abilities[abilityUsed];

                ability.PutOnCooldown();

                player.RemoveMana(ability.ManaCost);

                int currentHealtth = player.EntityStats.CurHealth;

                ability.ApplyAllyEffects(player);

                player.ApplyImmediateEffects();

                if (player.TurnStats.RemoveNegativeEffects)
                {
                    player.RemoveEffectsOfSide(EffectSide.Negative);

                    outcome.Cleansed = true;
                }

                if (currentHealtth < player.EntityStats.CurHealth)
                {
                    outcome.HealReceived = player.EntityStats.CurHealth - currentHealtth;
                }

                if (ability.DoesDamage)
                {
                    Debug.Assert(enemyIndex >= 0);

                    for (int i = 0; i < enemies.Length; ++i)
                    {
                        var enemy = enemies[i];

                        if (enemy is not null && enemy.IsAlive)
                        {
                            int count = enemy.Effects.Count;

                            ability.ApplyEnemyEffects(player, enemy, i == enemyIndex);

                            if (count != enemy.Effects.Count)
                            {
                                enemy.ApplyImmediateEffects();

                                if (enemy.TurnStats.RemovePositiveEffects)
                                {
                                    enemy.RemoveEffectsOfSide(EffectSide.Positive);

                                    outcome.EnemyInfos[i].Dispel = true;
                                }
                            }
                        }
                    }

                    ref readonly var stats = ref player.EntityStats;
                    
                    if (ability.IsAreaOfEffect)
                    {
                        for (int i = 0; i < enemies.Length; ++i)
                        {
                            var enemy = enemies[i];

                            if (enemy is not null && enemy.IsAlive)
                            {
                                ref var info = ref outcome.EnemyInfos[i];

                                int damage = CalculateDamage(stats.Damage, enemy.EntityStats.Armor, stats.CritChance, stats.CritMultiplier);

                                enemy.ApplyDamage(damage);

                                info.Engaged = true;

                                info.DamageDealt = damage;

                                info.Killed = !enemy.IsAlive;
                            }
                        }
                    }
                    else
                    {
                        var enemy = enemies[enemyIndex];

                        Debug.Assert(enemy is not null && enemy.IsAlive);

                        ref var info = ref outcome.EnemyInfos[enemyIndex];

                        int damage = CalculateDamage(stats.Damage, enemy.EntityStats.Armor, stats.CritChance, stats.CritMultiplier);

                        enemy.ApplyDamage(damage);

                        info.Engaged = true;

                        info.DamageDealt = damage;

                        info.Killed = !enemy.IsAlive;
                    }
                }
            }

            return outcome;
        }
    }
}
