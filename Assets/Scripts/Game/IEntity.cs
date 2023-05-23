using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Project.Game
{
    public enum AbilityUsage
    {
        CanUse,
        OnCooldown,
        NotEnoughMana,
        DoesNotExist,
    }

    public interface IEntity
    {
        bool IsPlayer { get; }

        bool IsAlive { get; }

        ref readonly TurnStats TurnStats { get; }

        ref readonly EntityStats EntityStats { get; }

        IReadOnlyList<Effect> Effects { get; }

        IReadOnlyList<Ability> Abilities { get; }

        IReadOnlyList<Potion> EquippedPotions { get; }

        void AddEffect(Effect effect);

        AbilityUsage CanUseAbility(int abilityIndex);

        AbilityUsage CanUseAbility(Ability ability);
    }

    public struct TurnStats
    {
        public bool BlockCurrentMove;
        public bool RemovePositiveEffects;
        public bool RemoveNegativeEffects;
    }

    public class BattleStats
    {
    }
}
