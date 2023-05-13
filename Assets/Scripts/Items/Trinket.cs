using Project.Input;

using System;

using UnityEngine;

namespace Project.Items
{
    public class Trinket : IItem, IDisposable
    {
        private Sprite m_sprite;
        private string m_stat;
        private string m_desc;
        private string m_name;

        [Order(0)]
        public string Name
        {
            get => this.m_name;
            set => this.m_name = value ?? String.Empty;
        }

        [Order(1)]
        public bool IsWeaponTrinket { get; set; }

        [Order(2)]
        public string StatName
        {
            get => this.m_stat;
            set => this.m_stat = value ?? String.Empty;
        }

        [Order(3)]
        public float StatModify { get; set; }

        [Order(4)]
        public int Price { get; set; }

        [Order(5)]
        public Sprite Sprite
        {
            get => this.m_sprite;
            set => this.m_sprite = value == null ? ResourceManager.DefaultSprite : value;
        }

        [Order(6)]
        public string Description
        {
            get => this.m_desc;
            set => this.m_desc = value ?? String.Empty;
        }

        public Trinket()
        {
            this.m_name = String.Empty;
            this.m_desc = String.Empty;
            this.m_stat = String.Empty;
            this.StatModify = 0.0f;
            this.Price = 0;
            this.m_sprite = ResourceManager.DefaultSprite;
        }

        public void Dispose()
        {
        }
    }
}
