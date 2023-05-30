using Project.Input;

using System;
using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;

namespace Project.Game
{
    public class EnemyData : IDisposable
    {
        private Sprite m_sprite;
        private string m_name;

        [Order(0)]
        public string Name
        {
            get => this.m_name;
            set => this.m_name = value ?? String.Empty;
        }

        [Order(1)]
        public int Health { get; set; }

        [Order(2)]
        public int Damage { get; set; }

        [Order(3)]
        public int Armor { get; set; }

        [Order(4)]
        public float Precision { get; set; }

        [Order(5)]
        public float Evasion { get; set; }

        [Order(6)]
        public float CritChance { get; set; }

        [Order(7)]
        public float CritMultiplier { get; set; }

        [Order(8)]
        public string AbilityName { get; set; }

        [Order(9)]
        public string[] AbilityEnemyEffects { get; set; }

        [Order(10)]
        public string[] AbilityAllyEffects { get; set; }

        [Order(11)]
        public float[] AbilityEnemyModifiers { get; set; }

        [Order(12)]
        public float[] AbilityAllyModifiers { get; set; }

        [Order(13)]
        public int[] AbilityEnemyDurations { get; set; }

        [Order(14)]
        public int[] AbilityAllyDurations { get; set; }

        [Order(15)]
        public float AbilityDamageMultiplier { get; set; }

        [Order(16)]
        public int AbilityCooldown { get; set; }

        [Order(17)]
        public Sprite Sprite
        {
            get => this.m_sprite;
            set => this.m_sprite = value == null ? ResourceManager.DefaultSprite : value;
        }

        public EnemyData()
        {
            this.m_name = String.Empty;
            this.m_sprite = ResourceManager.DefaultSprite;
        }

        public void Dispose()
        {
        }
    }

    public class Enemy : IEntity
    {
        private readonly static string[] ms_cowNames = new string[]
        {
            "GrayRasterCow",
            "RedRasterCow",
            "BlueRasterCow",
            "GreenRasterCow",
        };

        private readonly List<Effect> m_effects;
        private readonly Ability[] m_abilities;
        private readonly EnemyData m_data;
        private EntityStats m_stats;
        private TurnStats m_turn;

        public Sprite Sprite => this.m_data.Sprite;

        public bool IsPlayer => false;

        public bool IsAlive => this.m_stats.CurHealth > 0;

        public bool IsMelee => true;

        public ref readonly EntityStats EntityStats => ref this.m_stats;

        public ref readonly TurnStats TurnStats => ref this.m_turn;

        public IReadOnlyList<Effect> Effects => this.m_effects;

        public IReadOnlyList<Ability> Abilities => this.m_abilities;

        public IReadOnlyList<Potion> EquippedPotions => Array.Empty<Potion>();

        public Enemy(EnemyData data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            this.m_data = data;

            this.m_abilities = new Ability[1]
            {
                new Ability(this, new AbilityData()
                {
                    Class = "Enemy",
                    Name = data.AbilityName,
                    EnemyEffects = data.AbilityEnemyEffects,
                    AllyEffects = data.AbilityAllyEffects,
                    EnemyModifiers = data.AbilityEnemyModifiers,
                    AllyModifiers = data.AbilityAllyModifiers,
                    EnemyDurations = data.AbilityEnemyDurations,
                    AllyDurations = data.AbilityAllyDurations,
                    IsAOE = false,
                    DamageMultiplier = data.AbilityDamageMultiplier,
                    ManaCost = 0,
                    CooldownTime = data.AbilityCooldown,
                    Sprite = null,
                    Description = null,
                }),
            };

            this.m_effects = new List<Effect>();

            this.RecalculateStats();
        }

        private void RecalculateStats()
        {
            this.m_stats.MaxMana = 0;
            this.m_stats.MaxHealth = this.m_data.Health;
            this.m_stats.Armor = this.m_data.Armor;
            this.m_stats.Damage = this.m_data.Damage;
            this.m_stats.Evasion = this.m_data.Evasion;
            this.m_stats.Precision = this.m_data.Precision;
            this.m_stats.CritChance = this.m_data.CritChance;
            this.m_stats.CritMultiplier = this.m_data.CritMultiplier;

            for (int i = 0; i < this.m_effects.Count; ++i)
            {
                this.m_effects[i].ModifyStats(ref this.m_stats, ref this.m_turn);
            }

            this.m_stats.CurMana = 0;
            this.m_stats.CurHealth = Mathf.Clamp(this.m_stats.CurHealth, 0, this.m_stats.MaxHealth);
            this.m_stats.Evasion = Mathf.Clamp01(this.m_stats.Evasion);
            this.m_stats.Precision = Mathf.Clamp(this.m_stats.Precision, 0.0f, Single.PositiveInfinity);
            this.m_stats.CritChance = Mathf.Clamp01(this.m_stats.CritChance);
        }



        public void InitBattle()
        {
            this.RecalculateStats();

            this.m_stats.CurHealth = this.m_stats.MaxHealth;
            this.m_stats.CurMana = this.m_stats.MaxMana;

            this.m_effects.Clear();

            for (int i = 0; i < this.m_abilities.Length; ++i)
            {
                this.m_abilities[i].Reset();
            }
        }

        public void InitTurn()
        {
            this.m_turn = default;
        }

        public void Cooldown(out int totalHeal, out int totalMana, out int totalDmgs)
        {
            int count = this.m_effects.Count;

            for (int i = 0; i < this.m_abilities.Length; ++i)
            {
                this.m_abilities[i].Cooldown();
            }

            totalHeal = 0;
            totalMana = 0;
            totalDmgs = 0;

            // positive, then neutral, then negative

            for (int i = this.m_effects.Count - 1; i >= 0; --i)
            {
                var effect = this.m_effects[i];

                if (effect.Side == EffectSide.Positive)
                {
                    int curHealth = this.m_stats.CurHealth;

                    effect.Cooldown(ref this.m_stats, ref this.m_turn);

                    totalHeal += this.m_stats.CurHealth - curHealth;

                    if (!effect.IsLasting)
                    {
                        this.m_effects.RemoveAt(i);
                    }
                }
            }

            for (int i = this.m_effects.Count - 1; i >= 0; --i)
            {
                var effect = this.m_effects[i];

                if (effect.Side == EffectSide.Neutral)
                {
                    effect.Cooldown(ref this.m_stats, ref this.m_turn);

                    if (!effect.IsLasting)
                    {
                        this.m_effects.RemoveAt(i);
                    }
                }
            }

            for (int i = this.m_effects.Count - 1; i >= 0; --i)
            {
                var effect = this.m_effects[i];

                if (effect.Side == EffectSide.Negative)
                {
                    int curHealth = this.m_stats.CurHealth;

                    effect.Cooldown(ref this.m_stats, ref this.m_turn);

                    totalDmgs += curHealth - this.m_stats.CurHealth;

                    if (!effect.IsLasting)
                    {
                        this.m_effects.RemoveAt(i);
                    }
                }
            }

            if (count > this.m_effects.Count)
            {
                this.RecalculateStats();
            }
        }



        public void ApplyDamage(int damage)
        {
            this.m_stats.CurHealth -= damage;

            if (this.m_stats.CurHealth < 0)
            {
                this.m_stats.CurHealth = 0;
            }
        }

        public void AddEffect(Effect effect)
        {
            this.m_effects.Add(effect);

            this.RecalculateStats();
        }

        public AbilityUsage CanUseAbility(int abilityIndex)
        {
            if (abilityIndex >= 0 && abilityIndex < this.m_abilities.Length)
            {
                var ability = this.m_abilities[abilityIndex];

                if (ability.IsOnCooldown)
                {
                    return AbilityUsage.OnCooldown;
                }

                return AbilityUsage.CanUse;
            }

            return AbilityUsage.DoesNotExist;
        }

        public AbilityUsage CanUseAbility(Ability ability)
        {
            if (ability is not null && ability.Owner == this)
            {
                if (ability.IsOnCooldown)
                {
                    return AbilityUsage.OnCooldown;
                }

                return AbilityUsage.CanUse;
            }

            return AbilityUsage.DoesNotExist;
        }

        public void ApplyImmediateEffects()
        {
            for (int i = this.m_effects.Count - 1; i >= 0; --i)
            {
                var effect = this.m_effects[i];

                if (effect.Type == EffectType.IsImmediate)
                {
                    effect.ApplyImmediate(ref this.m_stats, ref this.m_turn);

                    this.m_effects.RemoveAt(i);
                }
            }
        }

        public void RemoveEffectsOfSide(EffectSide side)
        {
            int initial = this.m_effects.Count;

            for (int i = initial - 1; i >= 0; --i)
            {
                if (this.m_effects[i].Side == side)
                {
                    this.m_effects.RemoveAt(i);
                }
            }

            if (initial != this.m_effects.Count)
            {
                this.RecalculateStats();
            }
        }



        public static Enemy CreateDefaultEnemy()
        {
            return new Enemy(new EnemyData()
            {
                Name = "Cow",
                Health = 10 * Random.Range(10, 30),
                Damage = Random.Range(8, 16),
                Armor = 5 * Random.Range(4, 9),
                Precision = 30.0f,
                Evasion = 0.10f,
                CritChance = 0.05f,
                CritMultiplier = 1.5f,
                AbilityName = "Milkdrop",
                AbilityEnemyEffects = new string[]
                {
                    "Weaken",
                    "Poison",
                },
                AbilityAllyEffects = new string[]
                {
                    "Healing",
                    "Amplify",
                    "Sharpness",
                },
                AbilityEnemyModifiers = new float[]
                {
                    0.20f,
                    0.30f,
                },
                AbilityAllyModifiers = new float[]
                {
                    15.0f,
                    0.20f,
                    30.0f,
                },
                AbilityEnemyDurations = new int[]
                {
                    2,
                    2,
                },
                AbilityAllyDurations = new int[]
                {
                    0,
                    3,
                    3,
                },
                AbilityDamageMultiplier = 1.2f,
                AbilityCooldown = 5,
                Sprite = ResourceManager.LoadSprite("Sprites/Monsters/" + ms_cowNames[Random.Range(0, ms_cowNames.Length)]),
            });
        }
    }
}
