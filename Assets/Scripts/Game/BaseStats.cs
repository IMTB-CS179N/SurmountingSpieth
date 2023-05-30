using Project.Input;

using System;

using UnityEngine;

namespace Project.Game
{
    public class BaseStats : IDisposable
    {
        private Sprite m_sprite;
        private string m_class;
        private string m_race;

        [Order(0)]
        public string Race
        {
            get => this.m_race;
            set => this.m_race = value ?? String.Empty;
        }

        [Order(1)]
        public string Class
        {
            get => this.m_class;
            set => this.m_class = value ?? String.Empty;
        }

        [Order(2)]
        public int Health { get; set; }
        
        [Order(3)]
        public int Damage { get; set; }

        [Order(4)]
        public int Mana { get; set; }

        [Order(5)]
        public int Armor { get; set; }

        [Order(6)]
        public float Evasion { get; set; }
        
        [Order(7)]
        public float Precision { get; set; }
        
        [Order(8)]
        public float CritChance { get; set; }
        
        [Order(9)]
        public float CritMultiplier { get; set; }
        
        [Order(10)]
        public Sprite Sprite
        {
            get => this.m_sprite;
            set => this.m_sprite = value == null ? ResourceManager.DefaultSprite : value;
        }

        public BaseStats()
        {
            this.m_race = String.Empty;
            this.m_class = String.Empty;
            this.Health = 0;
            this.Damage = 0;
            this.Mana = 0;
            this.Armor = 0;
            this.Evasion = 0.0f;
            this.Precision = 0.0f;
            this.CritChance = 0.0f;
            this.CritMultiplier = 0.0f;
            this.m_sprite = ResourceManager.DefaultSprite;

        }

        public void Dispose()
        {
        }
    }
}
