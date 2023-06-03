using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Game;
using System.Linq;

namespace Project
{
    public class CellInfo
    {
        public enum TileType
        {
            Blank,
            Horizontal,
            Vertical,
            BottomRight,
            TopRight,
            BottomLeft,
            TopLeft,
            ForkRight,
            ForkUp,
            ForkDown,
            ForkLeft,
            Crossroads,
            Shop,
            BattleEasy,
            BattleMedium,
            BattleHard
        }

        public int yValue { get; set; }
        public TileType tileType;

        // public Sprite TileSprite;
        public string SpritePath;

        public bool IsClickable()
        {
            return (
                tileType == TileType.Shop
                || tileType == TileType.BattleEasy
                || tileType == TileType.BattleMedium
                || tileType == TileType.BattleHard
            );
        }

        public CellInfo(int yValue)
        {
            this.yValue = yValue;
        }

        public Sprite GetSprite()
        {
            return ResourceManager.LoadSprite(SpritePath);
        }
    }

    public class BackgroundInfo : CellInfo
    {
        public BackgroundInfo(int yValue, TileType type)
            : base(yValue)
        {
            var SpriteName = type.ToString("G");
            // string path = "Map/PathTiles/" + SpriteName;
            SpritePath = "Map/PathTiles/" + SpriteName;
            // TileSprite = Resources.Load<Sprite>(path);
            // TileSprite = ResourceManager.LoadSprite(path);
            tileType = type;
        }
    }

    public class ShopInfo : CellInfo
    {
        public readonly HashSet<TrinketData> Trinkets;
        public readonly HashSet<PotionData> Potions;
        public readonly HashSet<WeaponData> Weapons;
        public readonly HashSet<ArmorData> Armors;

        public ShopInfo(int yValue, int tier)
            : base(yValue)
        {
            // tier 3 (lowest) 70% tier 3 items 20% tier 2 items 10% tier 1 i
            weapons = new();
            trinkets = new();
            potions = new();
            armors = new();

            GenerateWeapons(tier);
            GenerateTrinkets(tier);
            GeneratePotions(tier);
            GenerateArmors(tier);

            SpritePath = "Map/Trade_Post";
            tileType = TileType.Shop;
        }

        public HashSet<WeaponData> GenerateWeapons(int tier)
        {
            int numTier1Items = 0;
            int numTier2Items = 0;
            int numTier3Items = 0;

            // Calculate the distribution percentages based on the shop tier
            if (tier == 1)
            {
                numTier1Items = 3;
                numTier2Items = 2;
                numTier3Items = 1;
            }
            else if (tier == 2)
            {
                numTier1Items = 1;
                numTier2Items = 3;
                numTier3Items = 2;
            }
            else if (tier == 3)
            {
                numTier1Items = 1;
                numTier2Items = 2;
                numTier3Items = 3;
            }

            int numWeapons = Random.Range(4, 7);

            var allWeapons = ResourceManager.Weapons;

            for (
                int i = 0;
                i < numWeapons; /* empty */

            )
            {
                var weapon = allWeapons[Random.Range(0, allWeapons.Count)];

                ref int count = ref weapon.Tier switch
                {
                    1 => numTier1Items,
                    2 => numTier2Items,
                    3 => numTier3Items,
                    _ => throw new System.Exception($"Invalid tier {weapon.Tier}"),
                };

                if (count > 0 && this.Weapons.Add(weapon))
                {
                    --count;
                }

                continue;

                if (i < numTier1Items)
                {
                    // Select a random tier 1 weapon from the available options
                    var tier1Weapons = ResourceManager.Weapons
                        .Where(weaponData => weaponData.Tier == 1)
                        .ToList();
                    var randomWeapon = tier1Weapons[Random.Range(0, tier1Weapons.Count)];
                    weapons.Add(randomWeapon);
                }
                else if (i < (numTier1Items + numTier2Items))
                {
                    // Select a random tier 2 weapon from the available options
                    var tier2Weapons = ResourceManager.Weapons
                        .Where(weaponData => weaponData.Tier == 2)
                        .ToList();
                    var randomWeapon = tier2Weapons[Random.Range(0, tier2Weapons.Count)];
                    weapons.Add(randomWeapon);
                }
                else if (i < (numTier1Items + numTier2Items + numTier3Items))
                {
                    // Select a random tier 3 weapon from the available options
                    var tier3Weapons = ResourceManager.Weapons
                        .Where(weaponData => weaponData.Tier == 3)
                        .ToList();
                    var randomWeapon = tier3Weapons[Random.Range(0, tier3Weapons.Count)];
                    weapons.Add(randomWeapon);
                }
            }
        }

        public List<Trinket> GenerateTrinkets(int tier)
        {
            int numTier1Items = 0;
            int numTier2Items = 0;
            int numTier3Items = 0;

            // Calculate the distribution percentages based on the shop tier
            if (tier == 1)
            {
                numTier1Items = 3;
                numTier2Items = 2;
                numTier3Items = 1;
            }
            else if (tier == 2)
            {
                numTier1Items = 1;
                numTier2Items = 3;
                numTier3Items = 2;
            }
            else if (tier == 3)
            {
                numTier1Items = 1;
                numTier2Items = 2;
                numTier3Items = 3;
            }
            int numTrinkets = Random.Range(4, 7);
            for (int i = 0; i <= numTrinkets; i++)
            {
                if (i < numTier1Items)
                {
                    // Select a random tier 1 weapon from the available options
                    var tier1Trinkets = ResourceManager.Trinkets
                        .Where(trinketData => trinketData.Tier == 1)
                        .ToList();
                    var randomTrinket = tier1Trinkets[Random.Range(0, tier1Trinkets.Count)];
                    trinkets.Add(TrinketFactory.Create(randomTrinket));
                }
                else if (i < (numTier1Items + numTier2Items))
                {
                    // Select a random tier 2 weapon from the available options
                    var tier2Trinkets = ResourceManager.Trinkets
                        .Where(trinketData => trinketData.Tier == 2)
                        .ToList();
                    var randomTrinket = tier2Trinkets[Random.Range(0, tier2Trinkets.Count)];
                    trinkets.Add(TrinketFactory.Create(randomTrinket));
                }
                else if (i < (numTier1Items + numTier2Items + numTier3Items))
                {
                    // Select a random tier 3 weapon from the available options
                    var tier3Trinkets = ResourceManager.Trinkets
                        .Where(trinketData => trinketData.Tier == 3)
                        .ToList();
                    var randomTrinket = tier3Trinkets[Random.Range(0, tier3Trinkets.Count)];
                    trinkets.Add(TrinketFactory.Create(randomTrinket));
                }
            }
            return trinkets;
        }

        /*public List <Potion> GeneratePotions(int tier) {
            float numItemsPerCategory = 5; // Number of items per category (weapons, armors, etc.)
            int numTier1Items = 0;
            int numTier2Items = 0;
            int numTier3Items = 0;
    
            // Calculate the distribution percentages based on the shop tier
            if(tier == 1) {
                numTier1Items = 3;
                numTier2Items = 2;
                numTier3Items = 1;
            }
            else if(tier == 2) {
                numTier1Items = 1;
                numTier2Items = 3;
                numTier3Items = 2;
            }
            else if(tier == 3) {
                numTier1Items = 1;
                numTier2Items = 2;
                numTier3Items = 3;
            }
            int numPotions = Random.Range(4,7);
             for (int i = 0; i <= numPotions; i++) {
                if (i < numTier1Items) {
                    // Select a random tier 1 weapon from the available options
    
                    var tier1Potions = ResourceManager.Potions.Where(potionData => potionData.Tier == 1).ToList();
                    var randomPotion = tier1Potions[Random.Range(0, tier1Potions.Count)];
    
                    potions.Add(PotionFactory.Create(randomPotion));
                }
                else if (i < (numTier1Items + numTier2Items)) {
                    // Select a random tier 2 weapon from the available options
                    var tier2Potions = ResourceManager.Potions.Where(potionData => potionData.Tier == 2).ToList();
                    var randomPotion = tier2Potions[Random.Range(0, tier2Potions.Count)];
    
                    potions.Add(PotionFactory.Create(randomPotion));
                }
                else if(i < (numTier1Items + numTier2Items + numTier3Items)) {
                    // Select a random tier 3 weapon from the available options
    
                    var tier3Potions = ResourceManager.Potions.Where(potionData => potionData.Tier == 3).ToList();
                    var randomPotion = tier3Potions[Random.Range(0, tier3Potions.Count)];
    
                    potions.Add(PotionFactory.Create(randomPotion));
                }
            }
            return potions;
        }*/

        public List<Armor> GenerateArmors(int tier)
        {
            int numTier1Items = 0;
            int numTier2Items = 0;
            int numTier3Items = 0;

            // Calculate the distribution percentages based on the shop tier
            if (tier == 1)
            {
                numTier1Items = 3;
                numTier2Items = 2;
                numTier3Items = 1;
            }
            else if (tier == 2)
            {
                numTier1Items = 1;
                numTier2Items = 3;
                numTier3Items = 2;
            }
            else if (tier == 3)
            {
                numTier1Items = 1;
                numTier2Items = 2;
                numTier3Items = 3;
            }
            int numArmors = Random.Range(4, 7);
            for (int i = 0; i <= numArmors; i++)
            {
                if (i < numTier1Items)
                {
                    // Select a random tier 1 weapon from the available options
                    var tier1Armors = ResourceManager.Armors
                        .Where(armorData => armorData.Tier == 1)
                        .ToList();
                    var randomArmor = tier1Armors[Random.Range(0, tier1Armors.Count)];
                    armors.Add(new Armor(randomArmor));
                }
                else if (i < (numTier1Items + numTier2Items))
                {
                    // Select a random tier 2 weapon from the available options
                    var tier2Armors = ResourceManager.Armors
                        .Where(armorData => armorData.Tier == 2)
                        .ToList();
                    var randomArmor = tier2Armors[Random.Range(0, tier2Armors.Count)];
                    armors.Add(new Armor(randomArmor));
                }
                else if (i < (numTier1Items + numTier2Items + numTier3Items))
                {
                    // Select a random tier 3 weapon from the available options
                    var tier3Armors = ResourceManager.Armors
                        .Where(armorData => armorData.Tier == 3)
                        .ToList();
                    var randomArmor = tier3Armors[Random.Range(0, tier3Armors.Count)];
                    armors.Add(new Armor(randomArmor));
                }
            }
            return armors;
        }
    }

    public class BattleInfo : CellInfo
    {
        public List<Enemy> enemies = new List<Enemy>();

        public BattleInfo(int yValue, TileType difficulty)
            : base(yValue)
        {
            var SpriteName = difficulty.ToString("G");
            SpritePath = "Sprites/Battle/" + SpriteName;
            tileType = difficulty;
        }

        public List<Enemy> GetEnemies()
        {
            return enemies;
        }
    }

    public class ColumnInfo
    {
        CellInfo[] cells = new CellInfo[5];
        int m_index { get; set; }

        public List<int> nonBlank = new List<int>();

        public ColumnInfo()
        {
            for (int i = 0; i < 5; i++)
            {
                cells[i] = new BackgroundInfo(i, CellInfo.TileType.Blank);
            }
        }

        public ColumnInfo(int index, CellInfo[] newCells)
        {
            // cells[0] = new BackgroundInfo(0, CellInfo.TileType.BottomLeft);
            if (newCells.Length != 5)
            {
                Debug.Log(
                    "Something went wrong creating a column. Check how many tile types you pass in."
                );
            }
            for (int i = 0; i < 5; i++)
            {
                cells[i] = newCells[i];
            }

            m_index = index;
        }

        public Sprite GetSprite(int tileRow)
        {
            return cells[tileRow].GetSprite();
        }

        public CellInfo GetCell(int index)
        {
            return cells[index];
        }

        public static CellInfo RandomCell(int yValue)
        {
            int seed = Random.Range(0, 2);
            switch (seed)
            {
                case 0:
                    // return new BattleInfo(yValue, new List<Enemy>());
                    return new BackgroundInfo(yValue, CellInfo.TileType.Blank);
                case 1:
                    return new ShopInfo(yValue, 0);
                default:
                    break;
            }
            return new BackgroundInfo(yValue, CellInfo.TileType.Blank);
        }

        public void SetCell(CellInfo newCell)
        {
            cells[newCell.yValue] = newCell;
            nonBlank.Add(newCell.yValue);
            nonBlank.Sort();
        }
    }
}
