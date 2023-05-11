using Project.Input;

using System;

using UnityEngine;

namespace Project.Items
{
    public class Weapon : IItem, IDisposable
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
        public float Damage { get; set; } // base weapon dmg
        
        [Order(2)]
        public float CritChance { get; set; } // crit chance
        
        [Order(3)]
        public float CritMultiplier { get; set; } // crit modifier
        
        [Order(4)]
        public float DamageMultipler { get; set; } // weapon dmg modifier
        
        [Order(5)]
        public float SPDMultiplier { get; set; } // weapon spd modifier

        [Order(6)]
        public int Price { get; set; }

        [Order(7)]
        public Sprite Sprite
        {
            get => this.m_sprite;
            set => this.m_sprite = value == null ? ResourceManager.DefaultSprite : value;
        }

        public Weapon()
        {
            this.m_name = String.Empty;
            this.Damage = 7.0f;
            this.CritChance = 0.5f;
            this.CritMultiplier = 0.5f;
            this.DamageMultipler = 2.0f;
            this.SPDMultiplier = 2.0f;
            this.Price = 100;
            this.m_sprite = ResourceManager.DefaultSprite;
        }

        public void Dispose()
        {
        }
    }
}
