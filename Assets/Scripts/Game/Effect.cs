using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using UnityEngine;

namespace Project.Game
{
    public enum EffectType
    {
        IsImmediate,
        ModifyStats,
        AffectStats,
        SuperEffect,
    }

    public abstract class Effect
    {
        private EffectType m_type;
        private Sprite m_sprite;
        private string m_description;
        private string m_name;
        private float m_value;
        private int m_remainingDuration;

        public EffectType Type => this.m_type;

        public bool IsLasting => this.m_remainingDuration > 0;

        public int RemainingDuration => this.m_remainingDuration;

        public float Value => this.m_value;

        public string Name => this.m_name;

        public string Description => this.m_description;

        public Sprite Sprite => this.m_sprite;

        public Effect(EffectType type, float value, int duration, string name, string description, string spritePath)
        {
            this.m_type = type;
            this.m_value = value;
            this.m_remainingDuration = duration;
            this.m_name = name ?? String.Empty;
            this.m_description = description ?? String.Empty;
            this.m_sprite = ResourceManager.LoadSprite(spritePath);
        }

        protected virtual void AffectStatsInternal(ref EntityStats stats)
        {
        }

        protected virtual void ModifyStatsInternal(ref EntityStats stats)
        {
        }

        protected virtual void ApplyAllNowInternal(ref EntityStats stats)
        {
        }

        protected virtual void SuperEffectInternal(ref TurnStats stats)
        {
        }

        public void ModifyStats(ref EntityStats stats)
        {
            if (this.m_type == EffectType.ModifyStats)
            {
                this.ModifyStatsInternal(ref stats);
            }
        }

        public void ApplyImmediate(ref EntityStats stats)
        {
            if (this.m_type == EffectType.IsImmediate)
            {
                this.ApplyAllNowInternal(ref stats);
            }
        }

        public void Cooldown(ref EntityStats stats)
        {
            if (!this.IsLasting)
            {
                this.m_remainingDuration--;

                if (this.m_type == EffectType.ModifyStats)
                {
                    this.AffectStatsInternal(ref stats);
                }
            }
        }

        public void SuperAffect(ref TurnStats stats)
        {
            if (this.m_type == EffectType.SuperEffect)
            {
                this.SuperEffectInternal(ref stats);
            }
        }

        public Effect Clone()
        {
            return (Effect)this.MemberwiseClone();
        }
    }

    public class PoisonEffect : Effect
    {
        public const string EffectName = "Poison";

        public const string SpritePath = "Sprites/Effects/PoisonEffect";

        public PoisonEffect(float value, int duration, ref EntityStats initial) : base(EffectType.AffectStats, value * initial.Damage, duration, EffectName, GetDescription(value * initial.Damage, duration), SpritePath)
        {
        }

        protected override void AffectStatsInternal(ref EntityStats stats)
        {
            stats.CurHealth -= (int)this.Value;

            if (stats.CurHealth < 0)
            {
                stats.CurHealth = 0;
            }
        }

        private static string GetDescription(float value, int duration)
        {
            return $"Reduces health by {(int)value} points per turn over {duration} turns.";
        }

        public static void RegisterForFactory()
        {
            EffectFactory.Register(EffectName, Create);

            static Effect Create(float value, int duration, ref EntityStats initial)
            {
                return new PoisonEffect(value, duration, ref initial);
            }
        }
    }

    public class BurnEffect : Effect
    {
        public const string EffectName = "Burn";

        public const string SpritePath = "Sprites/Effects/BurnEffect";

        public BurnEffect(float value, int duration, ref EntityStats initial) : base(EffectType.AffectStats, value * initial.Damage, duration, EffectName, GetDescription(value * initial.Damage, duration), SpritePath)
        {
        }

        protected override void AffectStatsInternal(ref EntityStats stats)
        {
            stats.CurHealth -= (int)this.Value;

            if (stats.CurHealth < 0)
            {
                stats.CurHealth = 0;
            }
        }

        private static string GetDescription(float value, int duration)
        {
            return $"Reduces health by {(int)value} points per turn over {duration} turns.";
        }

        public static void RegisterForFactory()
        {
            EffectFactory.Register(EffectName, Create);

            static Effect Create(float value, int duration, ref EntityStats initial)
            {
                return new BurnEffect(value, duration, ref initial);
            }
        }
    }

    public class ShredEffect : Effect
    {
        public const string EffectName = "Shred";

        public const string SpritePath = "Sprites/Effects/ShredEffect";

        public ShredEffect(float value, int duration) : base(EffectType.AffectStats, value, duration, EffectName, GetDescription(value, duration), SpritePath)
        {
        }

        protected override void ModifyStatsInternal(ref EntityStats stats)
        {
            stats.Armor -= (int)(stats.Armor * this.Value);

            if (stats.Armor < 0)
            {
                stats.Armor = 0;
            }
        }

        private static string GetDescription(float value, int duration)
        {
            return $"Reduces armor by {value}% for {duration} turns.";
        }

        public static void RegisterForFactory()
        {
            EffectFactory.Register(EffectName, Create);

            static Effect Create(float value, int duration, ref EntityStats initial)
            {
                return new ShredEffect(value, duration);
            }
        }
    }

    public class EmpowerEffect : Effect
    {
        public const string EffectName = "Empower";

        public const string SpritePath = "Sprites/Effects/EmpowerEffect";

        public EmpowerEffect(float value, int duration) : base(EffectType.ModifyStats, value, duration, EffectName, GetDescription(value, duration), SpritePath)
        {
        }

        protected override void ModifyStatsInternal(ref EntityStats stats)
        {
            stats.Damage += (int)this.Value;
        }

        private static string GetDescription(float value, int duration)
        {
            return $"Increases damage by {(int)value} points for {duration} turns.";
        }

        public static void RegisterForFactory()
        {
            EffectFactory.Register(EffectName, Create);

            static Effect Create(float value, int duration, ref EntityStats initial)
            {
                return new EmpowerEffect(value, duration);
            }
        }
    }

    public class AmplifyEffect : Effect
    {
        public const string EffectName = "Amplify";

        public const string SpritePath = "Sprites/Effects/AmplifyEffect";

        public AmplifyEffect(float value, int duration) : base(EffectType.ModifyStats, value, duration, EffectName, GetDescription(value, duration), SpritePath)
        {
        }

        protected override void ModifyStatsInternal(ref EntityStats stats)
        {
            stats.Damage += (int)(stats.Damage * this.Value);
        }

        private static string GetDescription(float value, int duration)
        {
            return $"Increases damage by {value}% for {duration} turns.";
        }

        public static void RegisterForFactory()
        {
            EffectFactory.Register(EffectName, Create);

            static Effect Create(float value, int duration, ref EntityStats initial)
            {
                return new AmplifyEffect(value, duration);
            }
        }
    }

    public class HealingEffect : Effect
    {
        public const string EffectName = "Healing";

        public const string SpritePath = "Sprites/Effects/HealingEffect";

        public HealingEffect(float value) : base(EffectType.IsImmediate, value, 0, EffectName, GetDescription(value), SpritePath)
        {
        }

        protected override void ApplyAllNowInternal(ref EntityStats stats)
        {
            stats.CurHealth += (int)this.Value;

            if (stats.CurHealth > stats.MaxHealth)
            {
                stats.CurHealth = stats.MaxHealth;
            }
        }

        private static string GetDescription(float value)
        {
            return $"Restores {(int)value} health points immediately.";
        }

        public static void RegisterForFactory()
        {
            EffectFactory.Register(EffectName, Create);

            static Effect Create(float value, int duration, ref EntityStats initial)
            {
                return new HealingEffect(value);
            }
        }
    }

    public class SurgeEffect : Effect
    {
        public const string EffectName = "Surge";

        public const string SpritePath = "Sprites/Effects/SurgeEffect";

        public SurgeEffect(float value) : base(EffectType.IsImmediate, value, 0, EffectName, GetDescription(value), SpritePath)
        {
        }

        protected override void ApplyAllNowInternal(ref EntityStats stats)
        {
            stats.CurMana += (int)this.Value;

            if (stats.CurMana > stats.MaxMana)
            {
                stats.CurMana = stats.MaxMana;
            }
        }

        private static string GetDescription(float value)
        {
            return $"Restores {value} mana points immediately.";
        }

        public static void RegisterForFactory()
        {
            EffectFactory.Register(EffectName, Create);

            static Effect Create(float value, int duration, ref EntityStats initial)
            {
                return new SurgeEffect(value);
            }
        }
    }

    public class RegenerationEffect : Effect
    {
        public const string EffectName = "Regeneration";

        public const string SpritePath = "Sprites/Effects/RegenerationEffect";

        public RegenerationEffect(float value, int duration) : base(EffectType.AffectStats, value, duration, EffectName, GetDescription(value, duration), SpritePath)
        {
        }

        protected override void AffectStatsInternal(ref EntityStats stats)
        {
            stats.CurHealth += (int)this.Value;

            if (stats.CurHealth > stats.MaxHealth)
            {
                stats.CurHealth = stats.MaxHealth;
            }
        }

        private static string GetDescription(float value, int duration)
        {
            return $"Regenerates health for {(int)value} points per turn over {duration} turns.";
        }

        public static void RegisterForFactory()
        {
            EffectFactory.Register(EffectName, Create);

            static Effect Create(float value, int duration, ref EntityStats initial)
            {
                return new RegenerationEffect(value, duration);
            }
        }
    }

    public class RenewalEffect : Effect
    {
        public const string EffectName = "Renewal";

        public const string SpritePath = "Sprites/Effects/RenewalEffect";

        public RenewalEffect(float value, int duration) : base(EffectType.AffectStats, value, duration, EffectName, GetDescription(value, duration), SpritePath)
        {
        }

        protected override void AffectStatsInternal(ref EntityStats stats)
        {
            stats.CurMana += (int)this.Value;

            if (stats.CurMana > stats.MaxMana)
            {
                stats.CurMana = stats.MaxMana;
            }
        }

        private static string GetDescription(float value, int duration)
        {
            return $"Regenerates mana for {(int)value} points per turn over {duration} turns.";
        }

        public static void RegisterForFactory()
        {
            EffectFactory.Register(EffectName, Create);

            static Effect Create(float value, int duration, ref EntityStats initial)
            {
                return new RenewalEffect(value, duration);
            }
        }
    }

    public class WeakenEffect : Effect
    {
        public const string EffectName = "Weaken";

        public const string SpritePath = "Sprites/Effects/WeakenEffect";

        public WeakenEffect(float value, int duration) : base(EffectType.ModifyStats, value, duration, EffectName, GetDescription(value, duration), SpritePath)
        {
        }

        protected override void ModifyStatsInternal(ref EntityStats stats)
        {
            stats.Damage -= (int)(this.Value * stats.Damage);

            if (stats.Damage < 0)
            {
                stats.Damage = 0;
            }
        }

        private static string GetDescription(float value, int duration)
        {
            return $"Decreases damage by {value}% for {duration} turns.";
        }

        public static void RegisterForFactory()
        {
            EffectFactory.Register(EffectName, Create);

            static Effect Create(float value, int duration, ref EntityStats initial)
            {
                return new WeakenEffect(value, duration);
            }
        }
    }

    public class LethalityEffect : Effect
    {
        public const string EffectName = "Lethality";

        public const string SpritePath = "Sprites/Effects/LethalityEffect";

        public readonly float LethalityValue;

        public LethalityEffect(float value, int duration) : base(EffectType.ModifyStats, duration, EffectName, GetDescription(value, duration), SpritePath)
        {
            this.LethalityValue = value;
        }

        protected override void ModifyStatsInternal(ref EntityStats stats)
        {
            stats.CritChance += this.LethalityValue;

            if (stats.CritChance > 1.0f)
            {
                stats.CritChance = 1.0f;
            }
        }

        private static string GetDescription(float value, int duration)
        {
            return $"Increases critical chance by {value}% for {duration} turns.";
        }
    }

    public class ShatterEffect : Effect
    {
        public const string EffectName = "Shatter";

        public const string SpritePath = "Sprites/Effects/ShatterEffect";

        public readonly float ShatterValue;

        public ShatterEffect(float value, int duration) : base(EffectType.ModifyStats, duration, EffectName, GetDescription(value, duration), SpritePath)
        {
            this.ShatterValue = value;
        }

        protected override void ModifyStatsInternal(ref EntityStats stats)
        {
            stats.CritMultiplier += stats.CritMultiplier * this.ShatterValue;
        }

        private static string GetDescription(float value, int duration)
        {
            return $"Increases critical multiplier by {value}% for {duration} turns.";
        }
    }

    public class SlowEffect : Effect
    {
        public const string EffectName = "Slow";

        public const string SpritePath = "Sprites/Effects/SlowEffect";

        public readonly float SlowValue;

        public SlowEffect(float value, int duration) : base(EffectType.ModifyStats, duration, EffectName, GetDescription(value, duration), SpritePath)
        {
            this.SlowValue = value;
        }

        protected override void ModifyStatsInternal(ref EntityStats stats)
        {
            stats.Evasion -= this.SlowValue;

            if (stats.Evasion < 0.0f)
            {
                stats.Evasion = 0.0f;
            }
        }

        private static string GetDescription(float value, int duration)
        {
            return $"Decreases evasion by {value}% for {duration} turns.";
        }
    }

    public class DizzynessEffect : Effect
    {
        public const string EffectName = "Dizzyness";

        public const string SpritePath = "Sprites/Effects/DizzynessEffect";

        public readonly float DizzynessValue;

        public DizzynessEffect(float value, int duration) : base(EffectType.ModifyStats, duration, EffectName, GetDescription(value, duration), SpritePath)
        {
            this.DizzynessValue = value;
        }

        protected override void ModifyStatsInternal(ref EntityStats stats)
        {
            stats.Precision -= this.DizzynessValue;

            if (stats.Precision < 0.0f)
            {
                stats.Precision = 0.0f;
            }
        }

        private static string GetDescription(float value, int duration)
        {
            return $"Decreases precision by {value} points for {duration} turns.";
        }
    }

    public class FlowEffect : Effect
    {
        public const string EffectName = "Flow";

        public const string SpritePath = "Sprites/Effects/FlowEffect";

        public readonly float FlowValue;

        public FlowEffect(float value, int duration) : base(EffectType.ModifyStats, duration, EffectName, GetDescription(value, duration), SpritePath)
        {
            this.FlowValue = value;
        }

        protected override void ModifyStatsInternal(ref EntityStats stats)
        {
            stats.Evasion += this.FlowValue;

            if (stats.Evasion > 1.0f)
            {
                stats.Evasion = 1.0f;
            }
        }

        private static string GetDescription(float value, int duration)
        {
            return $"Increases evasion by {value}% for {duration} turns.";
        }
    }

    public class SharpnessEffect : Effect
    {
        public const string EffectName = "Sharpness";

        public const string SpritePath = "Sprites/Effects/SharpnessEffect";

        public readonly float SharpnessValue;

        public SharpnessEffect(float value, int duration) : base(EffectType.ModifyStats, duration, EffectName, GetDescription(value, duration), SpritePath)
        {
            this.SharpnessValue = value;
        }

        protected override void ModifyStatsInternal(ref EntityStats stats)
        {
            stats.Precision += this.SharpnessValue;
        }

        private static string GetDescription(float value, int duration)
        {
            return $"Increases precision by {value} points for {duration} turns.";
        }
    }

    public static class EffectFactory
    {
        public delegate Effect Activator(float value, int duration, ref EntityStats initial);

        private static readonly Dictionary<string, Activator> ms_activatorMap = new();

        public static void Register(string name, Activator activator)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (activator is null)
            {
                throw new ArgumentNullException(nameof(activator));
            }

            ms_activatorMap[name] = activator;
        }

        public static Effect CreateEffect(string name, float value, int duration, ref EntityStats initial)
        {
            if (ms_activatorMap.TryGetValue(name, out var activator))
            {
                return activator(value, duration, ref initial);
            }

            throw new Exception($"Unable to create effect named {name}");
        }

        public static void Initialize()
        {

        }
    }
}
