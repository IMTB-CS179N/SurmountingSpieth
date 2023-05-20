using Project.Input;

using System;

using UnityEngine;

namespace Project.Game
{
    public class AbilityData : IDisposable
    {
        private float[] m_enemyModifiers;
        private float[] m_allyModifiers;
        private string[] m_enemyEffects;
        private string[] m_allyEffects;
        private int[] m_enemyDurations;
        private int[] m_allyDurations;
        private string m_description;
        private string m_class;
        private string m_name;
        private Sprite m_sprite;

        [Order(0)]
        public string Class
        {
            get => this.m_class;
            set => this.m_class = value ?? String.Empty;
        }

        [Order(1)]
        public string Name
        {
            get => this.m_name;
            set => this.m_name = value ?? String.Empty;
        }

        [Order(2)]
        public string[] EnemyEffects
        {
            get => this.m_enemyEffects;
            set => this.m_enemyEffects = value ?? Array.Empty<string>();
        }

        [Order(3)]
        public string[] AllyEffects
        {
            get => this.m_allyEffects;
            set => this.m_allyEffects = value ?? Array.Empty<string>();
        }

        [Order(4)]
        public float[] EnemyModifiers
        {
            get => this.m_enemyModifiers;
            set => this.m_enemyModifiers = value ?? Array.Empty<float>();
        }

        [Order(5)]
        public float[] AllyModifiers
        {
            get => this.m_allyModifiers;
            set => this.m_allyModifiers = value ?? Array.Empty<float>();
        }

        [Order(6)]
        public int[] EnemyDurations
        {
            get => this.m_enemyDurations;
            set => this.m_enemyDurations = value ?? Array.Empty<int>();
        }

        [Order(7)]
        public int[] AllyDurations
        {
            get => this.m_allyDurations;
            set => this.m_allyDurations = value ?? Array.Empty<int>();
        }

        [Order(8)]
        public float DamageMultiplier { get; set; }

        [Order(9)]
        public int ManaCost { get; set; }
        
        [Order(10)]
        public bool IsAOE { get; set; }

        [Order(11)]
        public int CooldownTime { get; set; }

        [Order(12)]
        public Sprite Sprite
        {
            get => this.m_sprite;
            set => this.m_sprite = value == null ? ResourceManager.DefaultSprite : value;
        }

        [Order(13)]
        public string Description
        {
            get => this.m_description;
            set => this.m_description = value ?? String.Empty;
        }

        public AbilityData()
        {
            this.m_class = String.Empty;
            this.m_name = String.Empty;
            this.m_enemyEffects = Array.Empty<string>();
            this.m_allyEffects = Array.Empty<string>();
            this.m_enemyModifiers = Array.Empty<float>();
            this.m_allyModifiers = Array.Empty<float>();
            this.m_enemyDurations = Array.Empty<int>();
            this.m_allyDurations = Array.Empty<int>();
            this.ManaCost = 0;
            this.IsAOE = false;
            this.CooldownTime = 0;
            this.m_sprite = ResourceManager.DefaultSprite;
            this.m_description = String.Empty;
        }

        public void Dispose()
        {
        }
    }

    public class Ability
    {
        private readonly AbilityData m_data;

        private int m_remainingCooldown;

        public bool IsOnCooldown => this.m_remainingCooldown > 0;

        public int RemainingCooldown => this.m_remainingCooldown;

        public string Name => this.m_data.Name;

        public int ManaCost => this.m_data.ManaCost;

        public int CooldownTime => this.m_data.CooldownTime;

        public bool DoesDamage => this.m_data.DamageMultiplier > 0.0f;

        public float DamageMultiplier => this.m_data.DamageMultiplier;

        public bool IsAreaOfEffect => this.m_data.IsAOE;

        public string Description => this.m_data.Description;

        public Sprite Sprite => this.m_data.Sprite;

        public Ability(AbilityData data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.EnemyEffects.Length != data.EnemyModifiers.Length)
            {
                throw new Exception($"Enemy effect mismatch: number of enemy effects is {data.EnemyEffects.Length} while number of enemy modifiers {data.EnemyModifiers.Length}");
            }

            if (data.EnemyEffects.Length != data.EnemyDurations.Length)
            {
                throw new Exception($"Enemy effect mismatch: number of enemy effects is {data.EnemyEffects.Length} while number of enemy durations {data.EnemyDurations.Length}");
            }

            if (data.AllyEffects.Length != data.AllyModifiers.Length)
            {
                throw new Exception($"Ally effect mismatch: number of ally effects is {data.AllyEffects.Length} while number of ally modifiers {data.AllyModifiers.Length}");
            }

            if (data.AllyEffects.Length != data.AllyDurations.Length)
            {
                throw new Exception($"Ally effect mismatch: number of ally effects is {data.AllyEffects.Length} while number of ally durations {data.AllyDurations.Length}");
            }

            this.m_data = data;

            this.m_remainingCooldown = 0;
        }

        public void Cooldown()
        {
            if (!this.IsOnCooldown)
            {
                this.m_remainingCooldown--;
            }
        }

        public void PutOnCooldown()
        {
            this.m_remainingCooldown = this.m_data.CooldownTime;
        }

        public void ApplyAllyEffects(IEntity ally, IEntity[] enemies)
        {

        }

        public void ApplyEnemyEffects(IEntity ally, IEntity[] enemies, int targetEnemy)
        {
            if (ally is null)
            {
                throw new ArgumentNullException(nameof(ally));
            }

            if (enemies is null)
            {
                throw new ArgumentNullException(nameof(enemies));
            }

            if (targetEnemy < 0 || targetEnemy >= enemies.Length)
            {
                throw new IndexOutOfRangeException($"Out of range target enemy index, number of enemies is {enemies.Length}, target is {targetEnemy}");
            }

            if (enemies.Length != 0)
            {
                var effects = this.m_data.EnemyEffects;
                var modifys = this.m_data.EnemyModifiers;
                var duratio = this.m_data.EnemyDurations;

                for (int i = 0; i < effects.Length; ++i)
                {
                    var effect = EffectFactory.CreateEffect(effects[i], modifys[i], duratio[i], ref ally.EntityStats);

                    if (this.IsAreaOfEffect)
                    {
                        for (int k = 0; k < enemies.Length; ++k)
                        {
                            enemies[k].AddEffect(effect.Clone());
                        }
                    }
                    else
                    {
                        enemies[targetEnemy].AddEffect(effect);
                    }
                }
            }
        }
    }
}
