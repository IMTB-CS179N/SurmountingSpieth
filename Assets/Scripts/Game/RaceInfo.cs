using Project.Input;

using System;

using UnityEngine;

namespace Project.Game
{
    public class RaceInfo : IDisposable
    {
        private static readonly string ms_defaultPath = "Sprites/Races/DefaultRace";

        private Sprite m_sprite;
        private string m_name;

        [Order(0)]
        public string Name
        {
            get => this.m_name;
            set => this.m_name = value ?? String.Empty;
        }

        [Order(1)]
        public Sprite Sprite
        {
            get
            {
                return this.m_sprite;
            }
            set
            {
                if (value == null || value == ResourceManager.DefaultSprite)
                {
                    this.m_sprite = ResourceManager.LoadSprite(ms_defaultPath);
                }
                else
                {
                    this.m_sprite = value;
                }
            }
        }

        public RaceInfo()
        {
            this.m_name = String.Empty;
            this.m_sprite = ResourceManager.LoadSprite(ms_defaultPath);
        }

        public void Dispose()
        {
        }
    }
}
