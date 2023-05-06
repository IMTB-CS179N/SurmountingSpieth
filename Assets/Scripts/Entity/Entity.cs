using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public interface IEntity
    {
        int Health { get; set; }
        int Mana { get; set; }

        int Damage { get; }

        int Precision { get; }
        float DodgeChance { get; }

        int ComputeDamage();
    }

    public class Player : IEntity
    {
        public static Player Instance = new();

        private Stats m_stats;
        private Ability[] m_abilities;

        // private List<Weapon> m_weapons;
        // private List<Armor> m_armors;
        private int m_precision;
        private float m_dodgeChance;

        public Player()
        {
            // this.m_stats = stats;

            // Health = stats.Health;
            Health = 0;
        }

        public int Health { get; set; }
        public int Mana { get; set; }

        public int Damage { get; set; }

        public int Precision => this.m_precision;
        public float DodgeChance => this.m_dodgeChance;

        public int ComputeDamage()
        {
            return 0;
        }
    }

    public class SomeClass
    {
        private Stats m_stats;

        public int CurrentHealth;

        public SomeClass(Stats stats)
        {
            this.m_stats = stats;
        }
    }

    public class Enemy : IEntity
    {
        private int m_precision;
        private float m_dodgeChance;

        public int Health { get; set; }
        public int Mana { get; set; }

        public int Damage { get; set; }

        public int Precision => this.m_precision;
        public float DodgeChance => this.m_dodgeChance;

        public int ComputeDamage()
        {
            return 0;
        }
    }

    public static class BattleUtil
    {
        public static bool Attack(IEntity attacker, IEntity receiver)
        {
            if (CheckIfHit(attacker, receiver))
            {
                receiver.Health -= attacker.ComputeDamage();

                if (receiver.Health <= 0)
                {
                    return true; // kill
                }
            }

            return false;
        }

        public static bool CheckIfHit(IEntity attacker, IEntity receiver)
        {
            return true; // take into account precision and dodge chance, return true if hit
        }
    }
}
