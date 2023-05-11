using Project.Items;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Game
{
    public class Player
    {
        public enum AbilityUsage
        {
            CanUse,
            OnCooldown,
            NotEnoughMana,
            DoesNotExist,
        }

        private static Player ms_instance;

        private readonly List<Weapon> m_weapons;
        private readonly List<Armor> m_armors;
        private readonly List<Effect> m_effects;
        private readonly List<Potion> m_potions;
        private readonly Ability[] m_abilities;
        private readonly Stats m_stats;

        private int m_maxHealth;
        private int m_maxMana;
        private int m_curHealth;
        private int m_curMana;
        private int m_armor;
        private int m_damage;
        private float m_evasion;
        private float m_precision;
        private float m_critChance;
        private float m_critMultiplier;
        private int m_money;

        public static readonly float SellMultiplier = 0.8f;

        public static Player Instance => ms_instance ?? throw new Exception("Cannot access player when not in game");

        public bool IsDead => this.m_curHealth <= 0;

        public int MaximumHealth => this.m_maxHealth;

        public int CurrentHealth => this.m_curHealth;

        public int MaximumMana => this.m_maxMana;

        public int CurrentMana => this.m_curMana;

        public int Money => this.m_money;

        public IReadOnlyList<Armor> Armors => this.m_armors;

        public IReadOnlyList<Weapon> Weapons => this.m_weapons;

        public IReadOnlyList<Potion> Potions => this.m_potions;

        public IReadOnlyList<Effect> Effects => this.m_effects;

        public IReadOnlyList<Ability> Abilities => this.m_abilities;

        public Player(string race, string @class)
        {
            var stats = ResourceManager.Stats.Find(_ => _.Race == race && _.Class == @class);

            if (stats is null)
            {
                throw new ArgumentException($"Player initialization failure: cannot find stats for race \"{race}\" and class \"{@class}\"");
            }

            this.m_stats = stats;
            this.m_armors = new();
            this.m_weapons = new();
            this.m_potions = new();
            this.m_effects = new();
            this.m_abilities = ResourceManager.Abilities.Where(_ => _.Class == @class).Select(_ => new Ability(_)).ToArray();
        }



        private void RecalculateStats()
        {
            this.RecalculateHealth();
            this.RecalculateMana();
            this.RecalculateArmor();
            this.RecalculateDamage();
            this.RecalculateEvasion();
            this.RecalculatePrecision();
            this.RecalculateCritChance();
            this.RecalculateCritMultiplier();
        }

        private void RecalculateHealth()
        {
            this.m_maxHealth = this.m_stats.BaseHealth; // #TODO calculate with effects, armor, weapons and such

        }

        private void RecalculateMana()
        {
            this.m_maxMana = this.m_stats.BaseMana; // #TODO calculate with effects, armor, weapons and such
        }

        private void RecalculateArmor()
        {
            this.m_armor = 0; // #TODO calculate with effects, armor, weapons and such
        }

        private void RecalculateDamage()
        {
            this.m_damage = this.m_stats.BaseDamage; // #TODO calculate with effects, armor, weapons and such
        }

        private void RecalculateEvasion()
        {
            this.m_evasion = this.m_stats.BaseEvasion; // #TODO calculate with effects, armor, weapons and such
        }

        private void RecalculatePrecision()
        {
            this.m_precision = this.m_stats.BasePrecision; // #TODO calculate with effects, armor, weapons and such
        }

        private void RecalculateCritChance()
        {
            this.m_critChance = this.m_stats.BaseCritChance; // #TODO calculate with effects, armor, weapons and such
        }

        private void RecalculateCritMultiplier()
        {
            this.m_critMultiplier = this.m_stats.BaseCritMultiplier; // #TODO calculate with effects, armor, weapons and such
        }



        public void Update()
        {
            // THIS IS CALLED ONLY WHEN PLAYER'S TURN

            for (int i = 0; i < this.m_abilities.Length; ++i)
            {
                this.m_abilities[i].Cooldown();
            }

            for (int i = this.m_effects.Count - 1; i >= 0; --i)
            {
                var effect = this.m_effects[i];

                effect.Cooldown();

                if (!effect.IsLasting)
                {
                    this.m_effects.RemoveAt(i);
                }
            }

            // #TODO other stuff


            // recalculate stats only at the end

            this.RecalculateStats();
        }

        public AbilityUsage CanUseAbility(int abilityIndex)
        {
            if (abilityIndex >= 0 && abilityIndex < this.m_abilities.Length)
            {
                var ability = this.m_abilities[abilityIndex];

                if (ability.IsOnCooldown)
                {
                    return AbilityUsage.OnCooldown;
                }

                if (this.m_curMana < ability.ManaCost)
                {
                    return AbilityUsage.NotEnoughMana;
                }

                return AbilityUsage.CanUse;
            }

            return AbilityUsage.DoesNotExist;
        }

        public AbilityUsage CanUseAbility(Ability ability)
        {
            if (Array.IndexOf(this.m_abilities, ability) >= 0)
            {
                if (ability.IsOnCooldown)
                {
                    return AbilityUsage.OnCooldown;
                }

                if (this.m_curMana < ability.ManaCost)
                {
                    return AbilityUsage.NotEnoughMana;
                }

                return AbilityUsage.CanUse;
            }

            return AbilityUsage.DoesNotExist;
        }

        public void ComputeDamage(int abilityIndex, out int damage, out bool canMiss)
        {
            if (this.CanUseAbility(abilityIndex) == AbilityUsage.CanUse)
            {
                var ability = this.m_abilities[abilityIndex];

                damage = (int)(this.m_damage * ability.DamageMultiplier);

                canMiss = ability.CanMiss;

                ability.PutOnCooldown();
            }
            else
            {
                damage = this.m_damage;

                canMiss = true;
            }
        }

        public void DealDamage(int damage)
        {
            this.m_curHealth -= (int)(damage * (100.0f / (100.0f + this.m_armor)));

            if (this.m_curHealth < 0)
            {
                this.m_curHealth = 0;
            }
        }



        public int PurchaseArmor(Armor armor)
        {
            if (armor is null)
            {
                throw new ArgumentNullException(nameof(armor));
            }

            if (armor.Price > this.m_money)
            {
                throw new ArgumentException("Unable to purchase because not enough money");
            }

            this.m_money -= armor.Price;

            int index = Player.BinarySearchInsertionPlace(armor, this.m_armors);

            this.m_armors.Insert(index, armor);

            this.RecalculateStats();

            return index;
        }

        public int PurchaseWeapon(Weapon weapon)
        {
            if (weapon is null)
            {
                throw new ArgumentNullException(nameof(weapon));
            }

            if (weapon.Price > this.m_money)
            {
                throw new ArgumentException("Unable to purchase because not enough money");
            }

            this.m_money -= weapon.Price;

            int index = Player.BinarySearchInsertionPlace(weapon, this.m_weapons);

            this.m_weapons.Insert(index, weapon);

            this.RecalculateStats();

            return index;
        }

        public int PurchasePotion(Potion potion)
        {
            if (potion is null)
            {
                throw new ArgumentNullException(nameof(potion));
            }

            if (potion.Price > this.m_money)
            {
                throw new ArgumentException("Unable to purchase because not enough money");
            }

            this.m_money -= potion.Price;

            int index = Player.BinarySearchInsertionPlace(potion, this.m_potions);

            this.m_potions.Insert(index, potion);

            return index; // no need to recalculate stats since potions don't affect them unless used
        }

        public void SellArmor(Armor armor)
        {
            if (armor is not null && this.m_armors.Remove(armor))
            {
                this.m_money += (int)(armor.Price * Player.SellMultiplier);
            }
        }

        public void SellWeapon(Weapon weapon)
        {
            if (weapon is not null && this.m_weapons.Remove(weapon))
            {
                this.m_money += (int)(weapon.Price * Player.SellMultiplier);
            }
        }

        public void SellPotion(Potion potion)
        {
            if (potion is not null && this.m_potions.Remove(potion))
            {
                this.m_money += (int)(potion.Price * Player.SellMultiplier);
            }
        }



        private static int BinarySearchInsertionPlace<T>(T item, IReadOnlyList<T> list) where T : IItem
        {
            int end = list.Count - 1;

            if (end < 0)
            {
                return 0;
            }

            int start = 0;

            while (start <= end)
            {
                int middle = start + ((end - start) >> 1);
                var evaled = list[middle].Name;
                int result = String.CompareOrdinal(item.Name, evaled);

                if (result == 0)
                {
                    return middle + 1; // AFTER item with the same name (less elements to copy)
                }

                if (result < 0)
                {
                    end = middle - 1;
                }
                else
                {
                    start = middle + 1;
                }
            }

            return start;
        }

        public static void Initialize(string race, string @class)
        {
            ms_instance = new Player(race, @class);
        }

        public static void InitializeFromSaveData()
        {
            // #TODO
        }

        public static void Deinitialize()
        {
            ms_instance = null;
        }
    }
}
