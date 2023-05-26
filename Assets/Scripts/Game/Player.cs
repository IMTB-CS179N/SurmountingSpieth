using Project.Items;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Project.Game
{
    public class Player : IEntity
    {
        private static Player ms_instance;

        private readonly List<TrinketData> m_trinkets;
        private readonly List<PotionData> m_potions;
        private readonly List<WeaponData> m_weapons;
        private readonly List<ArmorData> m_armors;
        private readonly List<Effect> m_effects;
        private readonly Ability[] m_abilities;
        private readonly BaseStats m_baseStats;

        private readonly Potion[] m_equippedPotions;

        private readonly WeaponTrinket[] m_weaponTrinkets;
        private Weapon m_weapon;

        private readonly ArmorTrinket[] m_leggingsTrinkets;
        private Armor m_leggings;

        private readonly ArmorTrinket[] m_chestplateTrinkets;
        private Armor m_chestplate;

        private readonly ArmorTrinket[] m_helmetTrinkets;
        private Armor m_helmet;

        private EntityStats m_stats;
        private TurnStats m_turn;
        private int m_money;

        public const int MaxPotionSlots = 3;

        public static readonly float SellMultiplier = 0.8f;

        public static readonly int HealthRegeneration = 20;

        public static readonly int ManaRegeneration = 10;

        public static readonly int InitialPlayerBank = 10000;

        public static Player Instance => ms_instance ?? throw new Exception("Cannot access player when not in game");

        public Sprite Sprite => this.m_baseStats.Sprite;

        public bool IsPlayer => true;

        public bool IsAlive => this.m_stats.CurHealth > 0;

        public ref readonly EntityStats EntityStats => ref this.m_stats;

        public ref readonly TurnStats TurnStats => ref this.m_turn;

        public int Money => this.m_money;

        public Armor Helmet => this.m_helmet;

        public Armor Chestplate => this.m_chestplate;

        public Armor Leggings => this.m_leggings;

        public Weapon Weapon => this.m_weapon;

        public IReadOnlyList<ArmorTrinket> HelmetTrinkets => this.m_helmetTrinkets;

        public IReadOnlyList<ArmorTrinket> ChestplateTrinkets => this.m_chestplateTrinkets;

        public IReadOnlyList<ArmorTrinket> LeggingsTrinkets => this.m_leggingsTrinkets;

        public IReadOnlyList<WeaponTrinket> WeaponTrinkets => this.m_weaponTrinkets;

        public IReadOnlyList<Potion> EquippedPotions => this.m_equippedPotions;

        public IReadOnlyList<ArmorData> Armors => this.m_armors;

        public IReadOnlyList<WeaponData> Weapons => this.m_weapons;

        public IReadOnlyList<PotionData> Potions => this.m_potions;

        public IReadOnlyList<TrinketData> Trinkets => this.m_trinkets;

        public IReadOnlyList<Effect> Effects => this.m_effects;

        public IReadOnlyList<Ability> Abilities => this.m_abilities;

        public Player(string race, string @class)
        {
            var stats = ResourceManager.Stats.Find(_ => _.Race == race && _.Class == @class);

            if (stats is null)
            {
                throw new ArgumentException($"Player initialization failure: cannot find stats for race \"{race}\" and class \"{@class}\"");
            }

            this.m_baseStats = stats;
            this.m_armors = new();
            this.m_weapons = new();
            this.m_potions = new();
            this.m_effects = new();
            this.m_trinkets = new();
            this.m_abilities = ResourceManager.Abilities.Where(_ => _.Class == @class).Select(_ => new Ability(this, _)).ToArray();

            this.m_equippedPotions = new Potion[Player.MaxPotionSlots];
            this.m_helmetTrinkets = new ArmorTrinket[Armor.MaxTrinketSlots];
            this.m_chestplateTrinkets = new ArmorTrinket[Armor.MaxTrinketSlots];
            this.m_leggingsTrinkets = new ArmorTrinket[Armor.MaxTrinketSlots];
            this.m_weaponTrinkets = new WeaponTrinket[Weapon.MaxTrinketSlots];

            this.m_money = Player.InitialPlayerBank;

            this.RecalculateStats();
        }



        private void RecalculateStats()
        {
            this.m_stats.MaxHealth = this.m_baseStats.Health;
            this.m_stats.MaxMana = this.m_baseStats.Mana;
            this.m_stats.Armor = this.m_baseStats.Armor;
            this.m_stats.Damage = this.m_baseStats.Damage;
            this.m_stats.Evasion = this.m_baseStats.Evasion;
            this.m_stats.Precision = this.m_baseStats.Precision;
            this.m_stats.CritChance = this.m_baseStats.CritChance;
            this.m_stats.CritMultiplier = this.m_baseStats.CritMultiplier;

            if (this.m_helmet is not null)
            {
                var stats = this.m_helmet.Stats;

                var trinkets = this.m_helmetTrinkets;

                if (trinkets is not null)
                {
                    for (int i = 0; i < trinkets.Length; ++i)
                    {
                        trinkets[i]?.ModifyStats(ref stats);
                    }
                }

                this.m_stats.Armor += stats.Armor;
                this.m_stats.Evasion += stats.Evasion;
                this.m_stats.Precision += stats.Precision;
            }

            if (this.m_chestplate is not null)
            {
                var stats = this.m_chestplate.Stats;

                var trinkets = this.m_chestplateTrinkets;

                if (trinkets is not null)
                {
                    for (int i = 0; i < trinkets.Length; ++i)
                    {
                        trinkets[i]?.ModifyStats(ref stats);
                    }
                }

                this.m_stats.Armor += stats.Armor;
                this.m_stats.Evasion += stats.Evasion;
                this.m_stats.Precision += stats.Precision;
            }

            if (this.m_leggings is not null)
            {
                var stats = this.m_leggings.Stats;

                var trinkets = this.m_leggingsTrinkets;

                if (trinkets is not null)
                {
                    for (int i = 0; i < trinkets.Length; ++i)
                    {
                        trinkets[i]?.ModifyStats(ref stats);
                    }
                }

                this.m_stats.Armor += stats.Armor;
                this.m_stats.Evasion += stats.Evasion;
                this.m_stats.Precision += stats.Precision;
            }

            if (this.m_weapon is not null)
            {
                var stats = this.m_weapon.Stats;

                var trinkets = this.m_weaponTrinkets;

                if (trinkets is not null)
                {
                    for (int i = 0; i < trinkets.Length; ++i)
                    {
                        trinkets[i]?.ModifyStats(ref stats);
                    }
                }

                this.m_stats.Damage += stats.Damage;
                this.m_stats.Precision += stats.Precision;
                this.m_stats.CritChance += stats.CritChance;
                this.m_stats.CritMultiplier += stats.CritMultiplier;
            }

            for (int i = 0; i < this.m_effects.Count; ++i)
            {
                this.m_effects[i].ModifyStats(ref this.m_stats);
            }

            this.m_stats.CurHealth = Mathf.Clamp(this.m_stats.CurHealth, 0, this.m_stats.MaxHealth);
            this.m_stats.CurMana = Mathf.Clamp(this.m_stats.CurMana, 0, this.m_stats.MaxMana);
            this.m_stats.Evasion = Mathf.Clamp01(this.m_stats.Evasion);
            this.m_stats.Precision = Mathf.Clamp(this.m_stats.Precision, 0.0f, Single.PositiveInfinity);
            this.m_stats.CritChance = Mathf.Clamp01(this.m_stats.CritChance);
        }

        private void UnattachPotion(PotionData potion)
        {
            for (int i = 0; i < this.m_equippedPotions.Length; ++i)
            {
                if (this.m_equippedPotions[i]?.IsPotionDataSame(potion) ?? false)
                {
                    this.m_equippedPotions[i] = null;
                }
            }
        }

        private bool UnattachTrinket(TrinketData trinket)
        {
            for (int i = 0; i < this.m_helmetTrinkets.Length; ++i)
            {
                if (this.m_helmetTrinkets[i]?.IsTrinketDataSame(trinket) ?? false)
                {
                    this.m_helmetTrinkets[i] = null;

                    return true;
                }
            }

            for (int i = 0; i < this.m_chestplateTrinkets.Length; ++i)
            {
                if (this.m_chestplateTrinkets[i]?.IsTrinketDataSame(trinket) ?? false)
                {
                    this.m_chestplateTrinkets[i] = null;

                    return true;
                }
            }

            for (int i = 0; i < this.m_leggingsTrinkets.Length; ++i)
            {
                if (this.m_leggingsTrinkets[i]?.IsTrinketDataSame(trinket) ?? false)
                {
                    this.m_leggingsTrinkets[i] = null;

                    return true;
                }
            }

            for (int i = 0; i < this.m_weaponTrinkets.Length; ++i)
            {
                if (this.m_weaponTrinkets[i]?.IsTrinketDataSame(trinket) ?? false)
                {
                    this.m_weaponTrinkets[i] = null;

                    return true;
                }
            }

            return false;
        }



        public void InitBattle()
        {
            this.RecalculateStats();

            this.m_stats.CurHealth = this.m_stats.MaxHealth;
            this.m_stats.CurMana = this.m_stats.MaxMana;
        }

        public void InitTurn()
        {
            this.m_turn = default;
        }

        public void Cooldown()
        {
            int count = this.m_effects.Count;

            for (int i = 0; i < this.m_abilities.Length; ++i)
            {
                this.m_abilities[i].Cooldown();
            }

            for (int i = this.m_effects.Count - 1; i >= 0; --i)
            {
                var effect = this.m_effects[i];

                effect.Cooldown(ref this.m_stats);

                if (!effect.IsLasting)
                {
                    this.m_effects.RemoveAt(i);
                }
            }

            if (count > this.m_effects.Count)
            {
                this.RecalculateStats();
            }
        }

        public void UsePotion(int potionIndex)
        {
            if (potionIndex >= 0 && potionIndex < this.m_equippedPotions.Length)
            {
                var potion = this.m_equippedPotions[potionIndex];

                var effect = potion.Use(in this.m_stats);

                this.m_equippedPotions[potionIndex] = null;

                if (effect.Type == EffectType.IsImmediate)
                {
                    effect.ApplyImmediate(ref this.m_stats);
                }
                else // #TODO is cleanse potion, handle super
                {
                    this.m_effects.Add(effect);

                    this.RecalculateStats();
                }
            }
        }



        public void ApplyDamage(int damage)
        {
            this.m_stats.CurHealth -= damage;

            if (this.m_stats.CurHealth < 0)
            {
                this.m_stats.CurHealth = 0;
            }
        }

        public void AddEffect(Effect effect)
        {
            this.m_effects.Add(effect);

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

                if (this.m_stats.CurMana < ability.ManaCost)
                {
                    return AbilityUsage.NotEnoughMana;
                }

                return AbilityUsage.CanUse;
            }

            return AbilityUsage.DoesNotExist;
        }

        public AbilityUsage CanUseAbility(Ability ability)
        {
            if (ability is not null && ability.Owner == this)
            {
                if (ability.IsOnCooldown)
                {
                    return AbilityUsage.OnCooldown;
                }

                if (this.m_stats.CurMana < ability.ManaCost)
                {
                    return AbilityUsage.NotEnoughMana;
                }

                return AbilityUsage.CanUse;
            }

            return AbilityUsage.DoesNotExist;
        }

        public void RemoveMana(int mana)
        {
            this.m_stats.CurMana -= mana;

            if (this.m_stats.CurMana < 0)
            {
                this.m_stats.CurMana = 0;
            }
        }

        public void ApplyImmediateEffects()
        {
            for (int i = this.m_effects.Count - 1; i >= 0; --i)
            {
                var effect = this.m_effects[i];

                if (effect.Type == EffectType.IsImmediate)
                {
                    effect.ApplyImmediate(ref this.m_stats);

                    this.m_effects.RemoveAt(i);
                }
                else if (effect.Type == EffectType.SuperEffect)
                {
                    effect.SuperAffect(ref this.m_turn);

                    this.m_effects.RemoveAt(i);
                }
            }
        }

        public void RemoveEffectsOfSide(EffectSide side)
        {
            int initial = this.m_effects.Count;

            for (int i = initial - 1; i >= 0; --i)
            {
                if (this.m_effects[i].Side == side)
                {
                    this.m_effects.RemoveAt(i);
                }
            }

            if (initial != this.m_effects.Count)
            {
                this.RecalculateStats();
            }
        }



        public void EquipHelmet(int index)
        {
            if (index < 0 || index >= this.m_armors.Count)
            {
                this.m_helmet = null;
            }
            else
            {
                var armor = this.m_armors[index];

                if (armor.SlotType != ArmorSlotType.Helmet)
                {
                    throw new Exception($"Armor at index {index} is not a Helmet armor");
                }

                this.m_helmet = new Armor(armor);
            }

            this.m_helmetTrinkets.AsSpan().Clear();

            this.RecalculateStats();
        }

        public void EquipHelmet(ArmorData armor)
        {
            if (armor is null)
            {
                this.m_helmet = null;
            }
            else
            {
                int index = this.m_armors.IndexOf(armor);

                if (index < 0)
                {
                    throw new Exception("Armor is not in the inventory");
                }

                if (armor.SlotType != ArmorSlotType.Helmet)
                {
                    throw new Exception($"Armor {armor.Name} is not a Helmet armor");
                }

                this.m_helmet = new Armor(armor);
            }

            this.m_helmetTrinkets.AsSpan().Clear();

            this.RecalculateStats();
        }

        public void EquipChestplate(int index)
        {
            if (index < 0 || index >= this.m_armors.Count)
            {
                this.m_chestplate = null;
            }
            else
            {
                var armor = this.m_armors[index];

                if (armor.SlotType != ArmorSlotType.Chestplate)
                {
                    throw new Exception($"Armor at index {index} is not a Chestplate armor");
                }

                this.m_chestplate = new Armor(armor);
            }

            this.m_chestplateTrinkets.AsSpan().Clear();

            this.RecalculateStats();
        }

        public void EquipChestplate(ArmorData armor)
        {
            if (armor is null)
            {
                this.m_chestplate = null;
            }
            else
            {
                int index = this.m_armors.IndexOf(armor);

                if (index < 0)
                {
                    throw new Exception("Armor is not in the inventory");
                }

                if (armor.SlotType != ArmorSlotType.Chestplate)
                {
                    throw new Exception($"Armor {armor.Name} is not a chestplate armor");
                }

                this.m_chestplate = new Armor(armor);
            }

            this.m_chestplateTrinkets.AsSpan().Clear();

            this.RecalculateStats();
        }

        public void EquipLeggings(int index)
        {
            if (index < 0 || index >= this.m_armors.Count)
            {
                this.m_leggings = null;
            }
            else
            {
                var armor = this.m_armors[index];

                if (armor.SlotType != ArmorSlotType.Leggings)
                {
                    throw new Exception($"Armor at index {index} is not a leggings armor");
                }

                this.m_leggings = new Armor(armor);
            }

            this.m_leggingsTrinkets.AsSpan().Clear();

            this.RecalculateStats();
        }

        public void EquipLeggings(ArmorData armor)
        {
            if (armor is null)
            {
                this.m_leggings = null;
            }
            else
            {
                int index = this.m_armors.IndexOf(armor);

                if (index < 0)
                {
                    throw new Exception("Armor is not in the inventory");
                }

                if (armor.SlotType != ArmorSlotType.Leggings)
                {
                    throw new Exception($"Armor {armor.Name} is not a leggings armor");
                }

                this.m_leggings = new Armor(armor);
            }

            this.m_leggingsTrinkets.AsSpan().Clear();

            this.RecalculateStats();
        }

        public void EquipWeapon(int index)
        {
            if (index < 0 || index >= this.m_weapons.Count)
            {
                this.m_weapon = null;
            }
            else
            {
                this.m_weapon = new Weapon(this.m_weapons[index]);
            }

            this.m_weaponTrinkets.AsSpan().Clear();

            this.RecalculateStats();
        }

        public void EquipWeapon(WeaponData weapon)
        {
            if (weapon is null)
            {
                this.m_weapon = null;
            }
            else
            {
                int index = this.m_weapons.IndexOf(weapon);

                if (index < 0)
                {
                    throw new Exception("Weapon is not in the inventory");
                }

                this.m_weapon = new Weapon(weapon);
            }

            this.m_weaponTrinkets.AsSpan().Clear();

            this.RecalculateStats();
        }

        public void EquipPotion(int slot, int index)
        {
            if (slot < 0)
            {
                throw new Exception("Potion slot number cannot be negative");
            }

            if (slot >= this.m_equippedPotions.Length)
            {
                throw new Exception($"Trying to set potion for slot {slot} when maximum slot count is {this.m_equippedPotions.Length}");
            }

            if (index < 0 || index >= this.m_potions.Count)
            {
                this.m_equippedPotions[slot] = null;
            }
            else
            {
                var potion = this.m_potions[slot];

                this.UnattachPotion(potion);

                this.m_equippedPotions[slot] = new Potion(potion);
            }
        }

        public void EquipPotion(int slot, PotionData potion)
        {
            if (slot < 0)
            {
                throw new Exception("Potion slot number cannot be negative");
            }

            if (slot >= this.m_equippedPotions.Length)
            {
                throw new Exception($"Trying to set potion for slot {slot} when maximum slot count is {this.m_equippedPotions.Length}");
            }

            if (potion is null)
            {
                this.m_equippedPotions[slot] = null;
            }
            else
            {
                int index = this.m_potions.IndexOf(potion);

                if (index < 0)
                {
                    throw new Exception("Potion is not in the inventory");
                }

                this.UnattachPotion(potion);

                this.m_equippedPotions[slot] = new Potion(potion);
            }
        }

        public void EquipHelmetTrinket(int slot, int index)
        {
            if (this.m_helmet is null)
            {
                throw new Exception("Unable to equip helmet trinket because helmet is not equipped");
            }

            if (slot < 0)
            {
                throw new Exception("Trinket slot number cannot be negative");
            }

            if (slot >= this.m_helmet.MaxTrinketCount)
            {
                throw new Exception($"Trying to set helmet trinket for slot {slot} when maximum slot count is {this.m_helmet.MaxTrinketCount}");
            }

            if (index < 0 || index >= this.m_trinkets.Count)
            {
                this.m_helmetTrinkets[slot] = null;
            }
            else
            {
                var trinket = this.m_trinkets[index];

                if (trinket.IsWeaponTrinket)
                {
                    throw new Exception($"Trinket {trinket.Name} cannot be applied to helmet because it is a weapon-only trinket");
                }

                this.UnattachTrinket(trinket);

                this.m_helmetTrinkets[slot] = TrinketFactory.Create(trinket) as ArmorTrinket;
            }

            this.RecalculateStats();
        }

        public void EquipHelmetTrinket(int slot, TrinketData trinket)
        {
            if (this.m_helmet is null)
            {
                throw new Exception("Unable to equip helmet trinket because helmet is not equipped");
            }

            if (slot < 0)
            {
                throw new Exception("Trinket slot number cannot be negative");
            }

            if (slot >= this.m_helmet.MaxTrinketCount)
            {
                throw new Exception($"Trying to set helmet trinket for slot {slot} when maximum slot count is {this.m_helmet.MaxTrinketCount}");
            }

            if (trinket is null)
            {
                this.m_helmetTrinkets[slot] = null;
            }
            else
            {
                int index = this.m_trinkets.IndexOf(trinket);

                if (index < 0)
                {
                    throw new Exception("Trinket is not in the inventory");
                }

                if (trinket.IsWeaponTrinket)
                {
                    throw new Exception($"Trinket {trinket.Name} cannot be applied to helmet because it is a weapon-only trinket");
                }

                this.UnattachTrinket(trinket);

                this.m_helmetTrinkets[slot] = TrinketFactory.Create(trinket) as ArmorTrinket;
            }

            this.RecalculateStats();
        }

        public void EquipChestplateTrinket(int slot, int index)
        {
            if (this.m_chestplate is null)
            {
                throw new Exception("Unable to equip chestplate trinket because chestplate is not equipped");
            }

            if (slot < 0)
            {
                throw new Exception("Trinket slot number cannot be negative");
            }

            if (slot >= this.m_chestplate.MaxTrinketCount)
            {
                throw new Exception($"Trying to set chestplate trinket for slot {slot} when maximum slot count is {this.m_chestplate.MaxTrinketCount}");
            }

            if (index < 0 || index >= this.m_trinkets.Count)
            {
                this.m_chestplateTrinkets[slot] = null;
            }
            else
            {
                var trinket = this.m_trinkets[index];

                if (trinket.IsWeaponTrinket)
                {
                    throw new Exception($"Trinket {trinket.Name} cannot be applied to chestplate because it is a weapon-only trinket");
                }

                this.UnattachTrinket(trinket);

                this.m_chestplateTrinkets[slot] = TrinketFactory.Create(trinket) as ArmorTrinket;
            }

            this.RecalculateStats();
        }

        public void EquipChestplateTrinket(int slot, TrinketData trinket)
        {
            if (this.m_chestplate is null)
            {
                throw new Exception("Unable to equip chestplate trinket because chestplate is not equipped");
            }

            if (slot < 0)
            {
                throw new Exception("Trinket slot number cannot be negative");
            }

            if (slot >= this.m_chestplate.MaxTrinketCount)
            {
                throw new Exception($"Trying to set chestplate trinket for slot {slot} when maximum slot count is {this.m_chestplate.MaxTrinketCount}");
            }

            if (trinket is null)
            {
                this.m_chestplateTrinkets[slot] = null;
            }
            else
            {
                int index = this.m_trinkets.IndexOf(trinket);

                if (index < 0)
                {
                    throw new Exception("Trinket is not in the inventory");
                }

                if (trinket.IsWeaponTrinket)
                {
                    throw new Exception($"Trinket {trinket.Name} cannot be applied to chestplate because it is a weapon-only trinket");
                }

                this.UnattachTrinket(trinket);

                this.m_chestplateTrinkets[slot] = TrinketFactory.Create(trinket) as ArmorTrinket;
            }

            this.RecalculateStats();
        }

        public void EquipLeggingsTrinket(int slot, int index)
        {
            if (this.m_leggings is null)
            {
                throw new Exception("Unable to equip leggings trinket because leggings is not equipped");
            }

            if (slot < 0)
            {
                throw new Exception("Trinket slot number cannot be negative");
            }

            if (slot >= this.m_leggings.MaxTrinketCount)
            {
                throw new Exception($"Trying to set leggings trinket for slot {slot} when maximum slot count is {this.m_leggings.MaxTrinketCount}");
            }

            if (index < 0 || index >= this.m_trinkets.Count)
            {
                this.m_leggingsTrinkets[slot] = null;
            }
            else
            {
                var trinket = this.m_trinkets[index];

                if (trinket.IsWeaponTrinket)
                {
                    throw new Exception($"Trinket {trinket.Name} cannot be applied to leggings because it is a weapon-only trinket");
                }

                this.UnattachTrinket(trinket);

                this.m_leggingsTrinkets[slot] = TrinketFactory.Create(trinket) as ArmorTrinket;
            }

            this.RecalculateStats();
        }

        public void EquipLeggingsTrinket(int slot, TrinketData trinket)
        {
            if (this.m_leggings is null)
            {
                throw new Exception("Unable to equip leggings trinket because leggings is not equipped");
            }

            if (slot < 0)
            {
                throw new Exception("Trinket slot number cannot be negative");
            }

            if (slot >= this.m_leggings.MaxTrinketCount)
            {
                throw new Exception($"Trying to set leggings trinket for slot {slot} when maximum slot count is {this.m_leggings.MaxTrinketCount}");
            }

            if (trinket is null)
            {
                this.m_leggingsTrinkets[slot] = null;
            }
            else
            {
                int index = this.m_trinkets.IndexOf(trinket);

                if (index < 0)
                {
                    throw new Exception("Trinket is not in the inventory");
                }

                if (trinket.IsWeaponTrinket)
                {
                    throw new Exception($"Trinket {trinket.Name} cannot be applied to leggings because it is a weapon-only trinket");
                }

                this.UnattachTrinket(trinket);

                this.m_leggingsTrinkets[slot] = TrinketFactory.Create(trinket) as ArmorTrinket;
            }

            this.RecalculateStats();
        }

        public void EquipWeaponTrinket(int slot, int index)
        {
            if (this.m_weapon is null)
            {
                throw new Exception("Unable to equip weapon trinket because weapon is not equipped");
            }

            if (slot < 0)
            {
                throw new Exception("Trinket slot number cannot be negative");
            }

            if (slot >= this.m_weapon.MaxTrinketCount)
            {
                throw new Exception($"Trying to set weapon trinket for slot {slot} when maximum slot count is {this.m_weapon.MaxTrinketCount}");
            }

            if (index < 0 || index >= this.m_trinkets.Count)
            {
                this.m_weaponTrinkets[slot] = null;
            }
            else
            {
                var trinket = this.m_trinkets[index];

                if (!trinket.IsWeaponTrinket)
                {
                    throw new Exception($"Trinket {trinket.Name} cannot be applied to helmet because it is an armor-only trinket");
                }

                this.UnattachTrinket(trinket);

                this.m_weaponTrinkets[slot] = TrinketFactory.Create(trinket) as WeaponTrinket;
            }

            this.RecalculateStats();
        }

        public void EquipWeaponTrinket(int slot, TrinketData trinket)
        {
            if (this.m_weapon is null)
            {
                throw new Exception("Unable to equip weapon trinket because weapon is not equipped");
            }

            if (slot < 0)
            {
                throw new Exception("Trinket slot number cannot be negative");
            }

            if (slot >= this.m_weapon.MaxTrinketCount)
            {
                throw new Exception($"Trying to set weapon trinket for slot {slot} when maximum slot count is {this.m_weapon.MaxTrinketCount}");
            }

            if (trinket is null)
            {
                this.m_weaponTrinkets[slot] = null;
            }
            else
            {
                int index = this.m_trinkets.IndexOf(trinket);

                if (index < 0)
                {
                    throw new Exception("Trinket is not in the inventory");
                }

                if (!trinket.IsWeaponTrinket)
                {
                    throw new Exception($"Trinket {trinket.Name} cannot be applied to weapon because it is an armor-only trinket");
                }

                this.UnattachTrinket(trinket);

                this.m_weaponTrinkets[slot] = TrinketFactory.Create(trinket) as WeaponTrinket;
            }

            this.RecalculateStats();
        }



        public int PurchaseArmor(ArmorData armor)
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

            this.m_armors.Insert(index, armor.Clone());

            return index;
        }

        public int PurchaseWeapon(WeaponData weapon)
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

            this.m_weapons.Insert(index, weapon.Clone());

            return index;
        }

        public int PurchasePotion(PotionData potion)
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

            this.m_potions.Insert(index, potion.Clone());

            return index;
        }

        public int PurchaseTrinket(TrinketData trinket)
        {
            if (trinket is null)
            {
                throw new ArgumentNullException(nameof(trinket));
            }
            
            if (trinket.Price > this.m_money)
            {
                throw new ArgumentException("Unable to purchase because not enough money");
            }

            this.m_money -= trinket.Price;

            int index = Player.BinarySearchInsertionPlace(trinket, this.m_trinkets);

            this.m_trinkets.Insert(index, trinket.Clone());

            return index;
        }

        public void SellArmor(ArmorData armor)
        {
            if (armor is not null && this.m_armors.Remove(armor))
            {
                this.m_money += (int)(armor.Price * Player.SellMultiplier);

                if (this.m_helmet is not null && this.m_helmet.IsArmorDataSame(armor))
                {
                    this.EquipHelmet(null);
                }

                if (this.m_chestplate is not null && this.m_chestplate.IsArmorDataSame(armor))
                {
                    this.EquipChestplate(null);
                }

                if (this.m_leggings is not null && this.m_leggings.IsArmorDataSame(armor))
                {
                    this.EquipLeggings(null);
                }
            }
        }

        public void SellWeapon(WeaponData weapon)
        {
            if (weapon is not null && this.m_weapons.Remove(weapon))
            {
                this.m_money += (int)(weapon.Price * Player.SellMultiplier);

                if (this.m_weapon is not null && this.m_weapon.IsWeaponDataSame(weapon))
                {
                    this.EquipWeapon(null);
                }
            }
        }

        public void SellPotion(PotionData potion)
        {
            if (potion is not null && this.m_potions.Remove(potion))
            {
                this.m_money += (int)(potion.Price * Player.SellMultiplier);

                this.UnattachPotion(potion);
            }
        }

        public void SellTrinket(TrinketData trinket)
        {
            if (trinket is not null && this.m_trinkets.Remove(trinket))
            {
                this.m_money += (int)(trinket.Price * Player.SellMultiplier);

                if (this.UnattachTrinket(trinket))
                {
                    this.RecalculateStats();
                }
            }
        }

        public void SellArmor(int index)
        {
            if (index >= 0 && index < this.m_armors.Count)
            {
                var armor = this.m_armors[index];

                this.m_money += (int)(armor.Price * Player.SellMultiplier);

                if (this.m_helmet is not null && this.m_helmet.IsArmorDataSame(armor))
                {
                    this.EquipHelmet(null);
                }

                if (this.m_chestplate is not null && this.m_chestplate.IsArmorDataSame(armor))
                {
                    this.EquipChestplate(null);
                }

                if (this.m_leggings is not null && this.m_leggings.IsArmorDataSame(armor))
                {
                    this.EquipLeggings(null);
                }

                this.m_armors.RemoveAt(index);
            }
        }

        public void SellWeapon(int index)
        {
            if (index >= 0 && index < this.m_weapons.Count)
            {
                var weapon = this.m_weapons[index];

                this.m_money += (int)(weapon.Price * Player.SellMultiplier);

                if (this.m_weapon is not null && this.m_weapon.IsWeaponDataSame(weapon))
                {
                    this.EquipWeapon(null);
                }

                this.m_weapons.RemoveAt(index);
            }
        }

        public void SellPotion(int index)
        {
            if (index >= 0 && index < this.m_potions.Count)
            {
                var potion = this.m_potions[index];

                this.m_money += (int)(potion.Price * Player.SellMultiplier);

                this.m_potions.RemoveAt(index);
            }
        }

        public void SellTrinket(int index)
        {
            if (index >= 0 && index < this.m_trinkets.Count)
            {
                var trinket = this.m_trinkets[index];

                this.m_money += (int)(trinket.Price * Player.SellMultiplier);

                if (this.UnattachTrinket(trinket))
                {
                    this.RecalculateStats();
                }

                this.m_trinkets.RemoveAt(index);
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
