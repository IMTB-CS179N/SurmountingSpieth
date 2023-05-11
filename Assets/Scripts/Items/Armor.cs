using Project.Input;

using System;

using UnityEngine;

namespace Project.Items
{
    public class Armor : IItem, IDisposable
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
        public int Value { get; set; }

        [Order(2)]
        public int Weight { get; set; }

        [Order(3)]
        public int Unknown { get; set; }

        [Order(4)]
        public int Price { get; set; }

        [Order(4)]
        public Sprite Sprite
        {
            get => this.m_sprite;
            set => this.m_sprite = value == null ? null : ResourceManager.DefaultSprite;
        }

        public Armor()
        {
            this.m_name = String.Empty;
            this.Value = 0;
            this.Weight = 0;
            this.Unknown = 0;
            this.Price = 0;
            this.m_sprite = ResourceManager.DefaultSprite;
        }

        public void Dispose()
        {
        }
    }
}
