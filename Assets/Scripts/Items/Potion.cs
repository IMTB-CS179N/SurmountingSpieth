using System;

using UnityEngine;

namespace Project.Items
{
    public class Potion : IItem, IDisposable
    {
        private Sprite m_sprite;
        private string m_name;

        public string Name
        {
            get => this.m_name;
            set => this.m_name = value ?? String.Empty;
        }

        public int Price { get; set; }

        public Sprite Sprite
        {
            get => this.m_sprite;
            set => this.m_sprite = value == null ? ResourceManager.DefaultSprite : value;
        }

        public Potion()
        {
            this.m_name = String.Empty;
            this.Price = 0;
            this.m_sprite = ResourceManager.DefaultSprite;
        }

        public void Dispose()
        {
        }
    }
}
