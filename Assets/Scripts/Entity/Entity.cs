using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Project
{
    public interface IEntity
    {
        int Health { get; set; }
        int ArmorValue { get; set; }
        int Mana { get; set; }

        int Damage { get; }

        int Precision { get; }
        float DodgeChance { get; }

        int DepleteArmor(int damage);

        int ComputeDamage();
    }

    public class Player : IEntity
    {
        public static Player Instance = new();

        Stats m_stats { get; set; }
        private Ability[] m_abilities;

        // private List<Weapon> m_weapons;
        private List<Armor> m_armors = new List<Armor>();
        private int m_precision;
        private float m_dodgeChance;

        public Player()
        {
            // this.m_stats = stats;

            // Health = stats.Health;
            m_armors.Add(new Armor());
            ArmorValue = m_armors[0].ArmorValue;
        }

        public void SetStats(FinishCharacter build)
        {
            this.m_stats = build.UserCharacter;
            Health = m_stats.Health;
            Debug.Log("Successfully bound stats to character");
        }

        public int Health { get; set; }
        public int ArmorValue { get; set; }
        public int Mana { get; set; }

        public int Damage { get; set; }

        public int Precision => this.m_precision;
        public float DodgeChance => this.m_dodgeChance;

        public int DepleteArmor(int damage)
        {
            var NewDamage = Math.Max(0, damage - ArmorValue);
            ArmorValue = Math.Max(0, ArmorValue - damage);

            return NewDamage;
        }

        public int ComputeDamage()
        {
            return 3;
        }
    }

    public class Enemy : IEntity
    {
        private int m_precision;
        private float m_dodgeChance;

        public int Health { get; set; }
        public int ArmorValue { get; set; }
        public int Mana { get; set; }

        public int Damage { get; set; }

        public int Precision => this.m_precision;
        public float DodgeChance => this.m_dodgeChance;

        public Enemy()
        {
            Health = 100;
            ArmorValue = 2;
        }

        public int DepleteArmor(int damage)
        {
            var NewDamage = Math.Max(0, damage - ArmorValue);
            ArmorValue = Math.Max(0, ArmorValue - damage);

            return NewDamage;
        }

        public int ComputeDamage()
        {
            return 3;
        }
    }

    public static class BattleUtil
    {
        public static bool Attack(IEntity attacker, IEntity receiver)
        {
            if (CheckIfHit(attacker, receiver))
            {
                var damage = attacker.ComputeDamage();

                receiver.Health -= receiver.DepleteArmor(damage);

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
