using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Project.Game
{
    public interface IEntity
    {
        ref TurnStats TurnStats { get; }

        ref EntityStats EntityStats { get; }

        void AddEffect(Effect effect);
    }

    public struct TurnStats
    {
        public bool CanMove;
    }

    public class BattleStats
    {
    }
}
