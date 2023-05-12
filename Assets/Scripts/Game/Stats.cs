using Project.Input;

using System;

using UnityEngine;

namespace Project.Game
{
    public class Stats : IDisposable
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
        public int BaseHealth { get; set; }
        
        [Order(3)]
        public int BaseDamage { get; set; }

        [Order(4)]
        public int BaseMana { get; set; }
        
        [Order(5)]
        public float BaseEvasion { get; set; }
        
        [Order(6)]
        public float BasePrecision { get; set; }
        
        [Order(7)]
        public float BaseCritChance { get; set; }
        
        [Order(8)]
        public float BaseCritMultiplier { get; set; }
        
        [Order(9)]
        public Sprite Sprite
        {
            get => this.m_sprite;
            set => this.m_sprite = value == null ? ResourceManager.DefaultSprite : value;
        }

        public Stats()
        {
            this.m_race = String.Empty;
            this.m_class = String.Empty;
            this.BaseHealth = 0;
            this.BaseDamage = 0;
            this.BaseMana = 0;
            this.BaseEvasion = 0.0f;
            this.BasePrecision = 0.0f;
            this.BaseCritChance = 0.0f;
            this.BaseCritMultiplier = 0.0f;
            this.m_sprite = ResourceManager.DefaultSprite;

        }

        public void Dispose()
        {
        }
    }
}
