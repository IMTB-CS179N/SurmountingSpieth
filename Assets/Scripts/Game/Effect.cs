using System;

using UnityEngine;

namespace Project.Game
{
    public class Effect
    {
        // #TODO what status effects can there be? dodge, health, damage, etc.?

        private Sprite m_sprite;
        private string m_description;
        private string m_name;
        private int m_remainingDuration;

        public bool IsLasting => this.m_remainingDuration > 0;

        public int RemainingDuration => this.m_remainingDuration;

        public string Name
        {
            get => this.m_name;
            set => this.m_name = value ?? String.Empty;
        }

        public string Description
        {
            get => this.m_description;
            set => this.m_description = value ?? String.Empty;
        }

        public Sprite Sprite
        {
            get => this.m_sprite;
            set => this.m_sprite = value == null ? ResourceManager.DefaultSprite : value;
        }

        public Effect(int duration, string name, string description, Sprite sprite)
        {
            this.m_remainingDuration = duration;
            this.m_name = name ?? String.Empty;
            this.m_description = description ?? String.Empty;
            this.m_sprite = sprite == null ? ResourceManager.DefaultSprite : sprite;
        }

        public void Cooldown()
        {
            if (!this.IsLasting)
            {
                this.m_remainingDuration--;
            }
        }
    }
}
