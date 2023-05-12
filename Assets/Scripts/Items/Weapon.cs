using Project.Input;

using System;

using UnityEngine;

namespace Project.Items
{
    public class Weapon : IItem, IDisposable
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
        public float Damage { get; set; } // base weapon dmg
        
        [Order(2)]
        public float CritChance { get; set; } // crit chance
        
        [Order(3)]
        public float CritMultiplier { get; set; } // crit modifier
        
        [Order(4)]
        public float Precision { get; set; } // precision

        [Order(5)]
        public int Price { get; set; }

        [Order(6)]
        public Sprite Sprite
        {
            get => this.m_sprite;
            set => this.m_sprite = value == null ? ResourceManager.DefaultSprite : value;
        }

        [Order(7)]
        public string Description
        {
            get => this.m_desc;
            set => this.m_desc = value ?? String.Empty;
        }

        public Weapon()
        {
            this.m_name = String.Empty;
            this.m_desc = String.Empty;
            this.Damage = 7.0f;
            this.CritChance = 0.5f;
            this.CritMultiplier = 0.5f;
            this.Precision = 0.0f;
            this.Price = 100;
            this.m_sprite = ResourceManager.DefaultSprite;
        }

        public void Dispose()
        {
        }
    }
}
