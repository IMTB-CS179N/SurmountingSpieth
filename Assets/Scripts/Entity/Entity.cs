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
        float CriticalChance { get; }
        float CriticalDamage { get; }

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
        private List<Weapons> m_weapons = new List<Weapons>();

        private Armor e_armor;
        private Weapons e_weapon;

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
        public float CriticalChance => 0.5f;
        public float CriticalDamage => 1.5f;

        public int DepleteArmor(int damage)
        {
            var NewDamage = Math.Max(0, damage - ArmorValue);
            var temp = ArmorValue;
            ArmorValue = Math.Max(0, ArmorValue - damage);
            Debug.Log(
                "Depleting armor from "
                    + temp
                    + " to "
                    + ArmorValue
                    + " by taking "
                    + damage
                    + " damage, with Health taking "
                    + NewDamage
                    + " damage"
            );

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
        public float CriticalChance => 0.5f;
        public float CriticalDamage => 1.5f;

        public Enemy()
        {
            Health = 100;
            ArmorValue = 2;
            Damage = 3;
        }

        public int DepleteArmor(int damage)
        {
            var NewDamage = Math.Max(0, damage - ArmorValue);
            var temp = ArmorValue;
            ArmorValue = Math.Max(0, ArmorValue - damage);
            Debug.Log(
                "Depleting armor from "
                    + temp
                    + " to "
                    + ArmorValue
                    + " by taking "
                    + damage
                    + " damage, with Health taking "
                    + NewDamage
                    + " damage"
            );

            return NewDamage;
        }

        public int ComputeDamage()
        {
            return 3;
        }
    }
}
