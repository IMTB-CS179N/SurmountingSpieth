using Project.Input;

using System;

using UnityEngine;

namespace Project.Game
{
    public class AbilityData : IDisposable
    {
        private Sprite m_sprite;
        private string m_description;
        private string m_class;
        private string m_name;

        [Order(0)]
        public string Class
        {
            get => this.m_class;
            set => this.m_class = value ?? String.Empty;
        }

        [Order(1)]
        public string Name
        {
            get => this.m_name;
            set => this.m_name = value ?? String.Empty;
        }
        
        [Order(2)]
        public int ManaCost { get; set; }
        
        [Order(3)]
        public float DamageMultiplier { get; set; }

        [Order(4)]
        public bool CanMiss { get; set; }

        [Order(5)]
        public bool IsAOE { get; set; } // area of effect

        [Order(6)]
        public int CooldownTime { get; set; }

        [Order(7)]
        public Sprite Sprite
        {
            get => this.m_sprite;
            set => this.m_sprite = value == null ? ResourceManager.DefaultSprite : value;
        }

        [Order(8)]
        public string Description
        {
            get => this.m_description;
            set => this.m_description = value ?? String.Empty;
        }

        public AbilityData()
        {
            this.m_class = String.Empty;
            this.m_name = String.Empty;
            this.ManaCost = 0;
            this.DamageMultiplier = 0.0f;
            this.m_sprite = ResourceManager.DefaultSprite;
            this.m_description = String.Empty;
        }

        public void Dispose()
        {
        }
    }

    public class Ability
    {
        private readonly AbilityData m_data;

        private int m_remainingCooldown;

        public bool IsOnCooldown => this.m_remainingCooldown > 0;

        public int RemainingCooldown => this.m_remainingCooldown;

        public string Name => this.m_data.Name;

        public int ManaCost => this.m_data.ManaCost;

        public bool CanMiss => this.m_data.CanMiss;

        public float DamageMultiplier => this.m_data.DamageMultiplier;

        public int CooldownTime => this.m_data.CooldownTime;

        public string Description => this.m_data.Description;

        public Sprite Sprite => this.m_data.Sprite;

        public Ability(AbilityData data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            this.m_data = data;

            this.m_remainingCooldown = 0;
        }

        public void Cooldown()
        {
            if (!this.IsOnCooldown)
            {
                this.m_remainingCooldown--;
            }
        }

        public void PutOnCooldown()
        {
            this.m_remainingCooldown = this.m_data.CooldownTime;
        }
    }
}
