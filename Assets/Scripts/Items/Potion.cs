﻿using Project.Input;

using System;

using UnityEngine;

namespace Project.Items
{
    public class Potion : IItem, IDisposable
    {
        private Sprite m_sprite;
        private string m_effect;
        private string m_desc;
        private string m_name;

        [Order(0)]
        public string Name
        {
            get => this.m_name;
            set => this.m_name = value ?? String.Empty;
        }

        [Order(1)]
        public string Effect
        {
            get => this.m_effect;
            set => this.m_effect = value ?? String.Empty;
        }

        [Order(2)]
        public float Modifier { get; set; }

        [Order(3)]
        public int Price { get; set; }

        [Order(4)]
        public Sprite Sprite
        {
            get => this.m_sprite;
            set => this.m_sprite = value == null ? ResourceManager.DefaultSprite : value;
        }

        [Order(5)]
        public string Description
        {
            get => this.m_desc;
            set => this.m_desc = value ?? String.Empty;
        }

        public Potion()
        {
            this.Price = 0;
            this.Modifier = 0.0f;
            this.m_name = String.Empty;
            this.m_desc = String.Empty;
            this.m_effect = String.Empty;
            this.m_sprite = ResourceManager.DefaultSprite;
        }

        public void Dispose()
        {
        }
    }
}