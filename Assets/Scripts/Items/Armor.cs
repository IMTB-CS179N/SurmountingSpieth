using Project.Input;

using System;

using UnityEngine;

namespace Project.Items
{
    public enum ArmorSlotType
    {
        Helmet,
        Chestplate,
        Leggings,
    }

    public class Armor : IItem, IDisposable
    {
        private Sprite m_sprite;
        private string m_desc;
        private string m_name;

        [Order(0)]
        public string Name
        {
            get => this.m_name;
            set => this.m_name = value ?? String.Empty;
        }

        [Order(1)]
        public ArmorSlotType SlotType { get; set; }

        [Order(2)]
        public int MaxTrinketCount { get; set; }

        [Order(3)]
        public int Value { get; set; }

        [Order(4)]
        public float PrecisionReduction { get; set; }

        [Order(5)]
        public float EvasionAdditive { get; set; }

        [Order(6)]
        public int Price { get; set; }

        [Order(7)]
        public Sprite Sprite
        {
            get => this.m_sprite;
            set => this.m_sprite = value == null ? null : ResourceManager.DefaultSprite;
        }

        [Order(8)]
        public string Description
        {
            get => this.m_desc;
            set => this.m_desc = value ?? String.Empty;
        }

        public Armor()
        {
            this.m_name = String.Empty;
            this.m_desc = String.Empty;
            this.Value = 0;
            this.Price = 0;
            this.PrecisionReduction = 0.0f;
            this.EvasionAdditive = 0.0f;
            this.m_sprite = ResourceManager.DefaultSprite;
        }

        public void Dispose()
        {
        }
    }
}
