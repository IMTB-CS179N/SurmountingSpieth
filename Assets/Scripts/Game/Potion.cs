using Project.Items;

using System;
using System.Collections.Generic;

using UnityEngine;

using Object = System.Object;

namespace Project.Game
{
    public abstract class Potion
    {
        private readonly PotionData m_data;

        public string Name => this.m_data.Name;

        public int Price => this.m_data.Price;

        public Sprite Sprite => this.m_data.Sprite;

        public string Description => this.m_data.Description;

        public float Modifier => this.m_data.Modifier;

        public int Duration => this.m_data.Duration;

        public PotionData Data => this.m_data;

        public Potion(PotionData data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            this.m_data = data;
        }

        public bool IsPotionDataSame(PotionData data)
        {
            return Object.ReferenceEquals(this.m_data, data);
        }

        public abstract Effect[] Use();
    }

    public class HealingPotion : Potion
    {
        public const string Effect = "Healing";

        public HealingPotion(PotionData data) : base(data)
        {
        }

        public override Effect[] Use()
        {
            return new Effect[]
            {
                new HealingEffect((int)this.Modifier),
            };
        }

        public static void RegisterForFactory()
        {
            PotionFactory.Register(Effect, data => new HealingPotion(data));
        }
    }

    public class RegenerationPotion : Potion
    {
        public const string Effect = "Regeneration";

        public RegenerationPotion(PotionData data) : base(data)
        {
        }

        public override Effect[] Use()
        {
            return new Effect[]
            {
                new RegenerationEffect((int)this.Modifier, this.Duration),
            };
        }

        public static void RegisterForFactory()
        {
            PotionFactory.Register(Effect, data => new RegenerationPotion(data));
        }
    }

    public static class PotionFactory
    {
        private static readonly Dictionary<string, Func<PotionData, Potion>> ms_activatorMap = new();

        public static void Register(string effect, Func<PotionData, Potion> activator)
        {
            if (activator is null)
            {
                throw new ArgumentNullException(nameof(activator));
            }

            if (String.IsNullOrEmpty(effect))
            {
                throw new ArgumentNullException(nameof(effect));
            }

            ms_activatorMap[effect] = activator;
        }

        public static Potion Create(PotionData potion)
        {
            if (ms_activatorMap.TryGetValue(potion.Effect, out var activator))
            {
                return activator(potion);
            }

            throw new Exception($"Unable to create potion with the following parameters: Effect = {potion.Effect}");
        }

        public static void Initialize()
        {
            HealingPotion.RegisterForFactory();
            RegenerationPotion.RegisterForFactory();
        }
    }
}
