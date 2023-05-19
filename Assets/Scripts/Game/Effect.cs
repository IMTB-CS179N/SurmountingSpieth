using System;

using UnityEngine;

namespace Project.Game
{
    public abstract class Effect
    {
        private readonly bool m_modifiesStats;
        private readonly bool m_affectsStats;
        private readonly bool m_isImmediate;
        private readonly Sprite m_sprite;
        private readonly string m_description;
        private readonly string m_name;
        private int m_remainingDuration;

        public bool IsImmediate => this.m_isImmediate;

        public bool ModifiesStats => this.m_modifiesStats;

        public bool AffectsStats => this.m_affectsStats;

        public bool IsLasting => this.m_remainingDuration > 0;

        public int RemainingDuration => this.m_remainingDuration;

        public string Name => this.m_name;

        public string Description => this.m_description;

        public Sprite Sprite => this.m_sprite;

        public Effect(bool isImmediate, bool modifiesStats, bool affectsStats, int duration, string name, string description, Sprite sprite)
        {
            this.m_isImmediate = isImmediate;
            this.m_modifiesStats = modifiesStats;
            this.m_affectsStats = affectsStats;
            this.m_remainingDuration = duration;
            this.m_name = name ?? String.Empty;
            this.m_description = description ?? String.Empty;
            this.m_sprite = sprite == null ? ResourceManager.DefaultSprite : sprite;
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

        public void ModifyStats(ref EntityStats stats)
        {
            if (this.m_modifiesStats)
            {
                this.ModifyStatsInternal(ref stats);
            }
        }

        public void Cooldown(ref EntityStats stats)
        {
            if (!this.IsLasting)
            {
                this.m_remainingDuration--;

                if (this.m_affectsStats)
                {
                    this.AffectStatsInternal(ref stats);
                }
            }
        }

        public void ApplyImmediate(ref EntityStats stats)
        {
            if (this.m_isImmediate)
            {
                this.ApplyAllNowInternal(ref stats);
            }
        }
    }

    public class PoisonEffect : Effect
    {
        public const string EffectName = "Poison";

        public const string SpritePath = "Sprites/Effects/PoisonEffect";

        public readonly int PoisonValue;

        public PoisonEffect(int value, int duration) : base(false, false, true, duration, EffectName, GetDescription(value, duration), ResourceManager.LoadSprite(SpritePath))
        {
            this.PoisonValue = value;
        }

        protected override void AffectStatsInternal(ref EntityStats stats)
        {
            stats.CurHealth -= this.PoisonValue;

            if (stats.CurHealth < 0)
            {
                stats.CurHealth = 0;
            }
        }

        private static string GetDescription(int value, int duration)
        {
            return $"Reduces health for {value} points per turn over {duration} turns";
        }
    }

    public class BurnEffect : Effect
    {
        public const string EffectName = "Burn";

        public const string SpritePath = "Sprites/Effects/BurnEffect";

        public readonly int BurnValue;

        public BurnEffect(int value, int duration) : base(false, false, true, duration, EffectName, GetDescription(value, duration), ResourceManager.LoadSprite(SpritePath))
        {
            this.BurnValue = value;
        }

        protected override void AffectStatsInternal(ref EntityStats stats)
        {
            stats.CurHealth -= this.BurnValue;

            if (stats.CurHealth < 0)
            {
                stats.CurHealth = 0;
            }
        }

        private static string GetDescription(int value, int duration)
        {
            return $"Reduces health for {value} points per turn over {duration} turns";
        }
    }

    public class ShredEffect : Effect
    {
        public const string EffectName = "Shred";

        public const string SpritePath = "Sprites/Effects/ShredEffect";

        public readonly float ShredValue;

        public ShredEffect(float value, int duration) : base(false, true, false, duration, EffectName, GetDescription(value, duration), ResourceManager.LoadSprite(SpritePath))
        {
            this.ShredValue = value;
        }

        protected override void ModifyStatsInternal(ref EntityStats stats)
        {
            stats.Armor -= (int)(stats.Armor * this.ShredValue);

            if (stats.Armor < 0)
            {
                stats.Armor = 0;
            }
        }

        private static string GetDescription(float value, int duration)
        {
            return $"Reduces armor by {value}% for {duration} turns";
        }
    }

    public class EmpowerEffect : Effect
    {
        public const string EffectName = "Empower";

        public const string SpritePath = "Sprites/Effects/EmpowerEffect";

        public readonly int DamageValue;

        public EmpowerEffect(int value, int duration) : base(false, true, false, duration, EffectName, GetDescription(value, duration), ResourceManager.LoadSprite(SpritePath))
        {
            this.DamageValue = value;
        }

        protected override void ModifyStatsInternal(ref EntityStats stats)
        {
            stats.Damage += this.DamageValue;
        }

        private static string GetDescription(int value, int duration)
        {
            return $"Increases damage by {value} points for {duration} turns";
        }
    }

    public class AmplifyEffect : Effect
    {
        public const string EffectName = "Amplify";

        public const string SpritePath = "Sprites/Effects/AmplifyEffect";

        public readonly float DamageValue;

        public AmplifyEffect(float value, int duration) : base(false, true, false, duration, EffectName, GetDescription(value, duration), ResourceManager.LoadSprite(SpritePath))
        {
            this.DamageValue = value;
        }

        protected override void ModifyStatsInternal(ref EntityStats stats)
        {
            stats.Damage += (int)(stats.Damage * this.DamageValue);
        }

        private static string GetDescription(float value, int duration)
        {
            return $"Increases damage by {value}% for {duration} turns";
        }
    }

    public class HealingEffect : Effect
    {
        public const string EffectName = "Healing";

        public const string SpritePath = "Sprites/Effects/HealingEffect";

        public readonly int HealingValue;

        public HealingEffect(int value) : base(true, false, false, 0, EffectName, GetDescription(value), ResourceManager.LoadSprite(SpritePath))
        {
            this.HealingValue = value;
        }

        protected override void ApplyAllNowInternal(ref EntityStats stats)
        {
            stats.CurHealth += this.HealingValue;

            if (stats.CurHealth > stats.MaxHealth)
            {
                stats.CurHealth = stats.MaxHealth;
            }
        }

        private static string GetDescription(int value)
        {
            return $"Restores {value} health points immediately";
        }
    }

    public class SurgeEffect : Effect
    {
        public const string EffectName = "Surge";

        public const string SpritePath = "Sprites/Effects/SurgeEffect";

        public readonly int SurgeValue;

        public SurgeEffect(int value) : base(true, false, false, 0, EffectName, GetDescription(value), ResourceManager.LoadSprite(SpritePath))
        {
            this.SurgeValue = value;
        }

        protected override void ApplyAllNowInternal(ref EntityStats stats)
        {
            stats.CurMana += this.SurgeValue;

            if (stats.CurMana > stats.MaxMana)
            {
                stats.CurMana = stats.MaxMana;
            }
        }

        private static string GetDescription(int value)
        {
            return $"Restores {value} mana points immediately";
        }
    }

    public class RegenerationEffect : Effect
    {
        public const string EffectName = "Regeneration";

        public const string SpritePath = "Sprites/Effects/RegenerationEffect";

        public readonly int RegenerationValue;

        public RegenerationEffect(int value, int duration) : base(false, false, true, duration, EffectName, GetDescription(value, duration), ResourceManager.LoadSprite(SpritePath))
        {
            this.RegenerationValue = value;
        }

        protected override void AffectStatsInternal(ref EntityStats stats)
        {
            stats.CurHealth += this.RegenerationValue;

            if (stats.CurHealth > stats.MaxHealth)
            {
                stats.CurHealth = stats.MaxHealth;
            }
        }

        private static string GetDescription(int value, int duration)
        {
            return $"Regenerates health for {value} points per turn over {duration} turns";
        }
    }

    public class RenewalEffect : Effect
    {
        public const string EffectName = "Renewal";

        public const string SpritePath = "Sprites/Effects/RenewalEffect";

        public readonly int RenewalValue;

        public RenewalEffect(int value, int duration) : base(false, false, true, duration, EffectName, GetDescription(value, duration), ResourceManager.LoadSprite(SpritePath))
        {
            this.RenewalValue = value;
        }

        protected override void AffectStatsInternal(ref EntityStats stats)
        {
            stats.CurMana += this.RenewalValue;

            if (stats.CurMana > stats.MaxMana)
            {
                stats.CurMana = stats.MaxMana;
            }
        }

        private static string GetDescription(int value, int duration)
        {
            return $"Regenerates mana for {value} points per turn over {duration} turns";
        }
    }

    public class WeakenEffect : Effect
    {
        public const string EffectName = "Weaken";

        public const string SpritePath = "Sprites/Effects/WeakenEffect";

        public readonly float WeakenValue;

        public WeakenEffect(float value, int duration) : base(false, true, false, duration, EffectName, GetDescription(value, duration), ResourceManager.LoadSprite(SpritePath))
        {
            this.WeakenValue = value;
        }

        protected override void ModifyStatsInternal(ref EntityStats stats)
        {
            stats.Damage -= (int)(this.WeakenValue * stats.Damage);

            if (stats.Damage < 0)
            {
                stats.Damage = 0;
            }
        }

        private static string GetDescription(float value, int duration)
        {
            return $"Decreases damage by {value}% for {duration} turns";
        }
    }

    public class LethalityEffect : Effect
    {
        public const string EffectName = "Lethality";

        public const string SpritePath = "Sprites/Effects/LethalityEffect";

        public readonly float LethalityValue;

        public LethalityEffect(float value, int duration) : base(false, true, false, duration, EffectName, GetDescription(value, duration), ResourceManager.LoadSprite(SpritePath))
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
            return $"Increases critical chance by {value}% for {duration} turns";
        }
    }

    public class SlowEffect : Effect
    {
        public const string EffectName = "Slow";

        public const string SpritePath = "Sprites/Effects/SlowEffect";

        public readonly float SlowValue;

        public SlowEffect(float value, int duration) : base(false, true, false, duration, EffectName, GetDescription(value, duration), ResourceManager.LoadSprite(SpritePath))
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
            return $"Decreases evasion by {value}% for {duration} turns";
        }
    }

    public class DizzynessEffect : Effect
    {
        public const string EffectName = "Dizzyness";

        public const string SpritePath = "Sprites/Effects/DizzynessEffect";

        public readonly float DizzynessValue;

        public DizzynessEffect(float value, int duration) : base(false, true, false, duration, EffectName, GetDescription(value, duration), ResourceManager.LoadSprite(SpritePath))
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
            return $"Decreases precision by {value} points for {duration} turns";
        }
    }

    public class FlowEffect : Effect
    {
        public const string EffectName = "Flow";

        public const string SpritePath = "Sprites/Effects/FlowEffect";

        public readonly float FlowValue;

        public FlowEffect(float value, int duration) : base(false, true, false, duration, EffectName, GetDescription(value, duration), ResourceManager.LoadSprite(SpritePath))
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
            return $"Increases evasion by {value}% for {duration} turns";
        }
    }

    public class SharpnessEffect : Effect
    {
        public const string EffectName = "Sharpness";

        public const string SpritePath = "Sprites/Effects/SharpnessEffect";

        public readonly float SharpnessValue;

        public SharpnessEffect(float value, int duration) : base(false, true, false, duration, EffectName, GetDescription(value, duration), ResourceManager.LoadSprite(SpritePath))
        {
            this.SharpnessValue = value;
        }

        protected override void ModifyStatsInternal(ref EntityStats stats)
        {
            stats.Precision += this.SharpnessValue;
        }

        private static string GetDescription(float value, int duration)
        {
            return $"Increases precision by {value} points for {duration} turns";
        }
    }
}
