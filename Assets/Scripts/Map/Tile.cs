using Project.Game;
using Project.Items;

using System;
using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;

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
        public readonly int Tier;
        public readonly HashSet<ArmorData> Armors;
        public readonly HashSet<WeaponData> Weapons;
        public readonly HashSet<PotionData> Potions;
        public readonly HashSet<TrinketData> Trinkets;

        public ShopInfo(int yValue) : base(yValue)
        {
            this.Tier = Random.Range(1, 4);

            this.Weapons = new();
            this.Trinkets = new();
            this.Potions = new();
            this.Armors = new();

            ShopInfo.GenerateItems(ResourceManager.Armors, this.Armors, this.Tier);
            ShopInfo.GenerateItems(ResourceManager.Weapons, this.Weapons, this.Tier);
            ShopInfo.GenerateItems(ResourceManager.Potions, this.Potions, this.Tier);
            ShopInfo.GenerateItems(ResourceManager.Trinkets, this.Trinkets, this.Tier);

            this.SpritePath = "Map/Trade_Post"; // #TODO different tier based sprites
            this.tileType = TileType.Shop;
        }

        private static void GenerateItems<T>(IReadOnlyList<T> src, HashSet<T> dst, int tier) where T : IItem
        {
            var tier1Array = ShopInfo.GetItemsOfTier<T>(src, 1);
            var tier2Array = ShopInfo.GetItemsOfTier<T>(src, 2);
            var tier3Array = ShopInfo.GetItemsOfTier<T>(src, 3);

            int tier1Count = Mathf.Min(tier1Array.Length, tier1Array.Length >> (tier == 1 ? 1 : 2));
            int tier2Count = Mathf.Min(tier2Array.Length, tier2Array.Length >> (tier == 2 ? 1 : 2));
            int tier3Count = Mathf.Min(tier3Array.Length, tier3Array.Length >> (tier == 3 ? 1 : 2));

            ShopInfo.AddRandomItemsIntoSet(tier1Array, dst, tier1Count);
            ShopInfo.AddRandomItemsIntoSet(tier2Array, dst, tier2Count);
            ShopInfo.AddRandomItemsIntoSet(tier3Array, dst, tier3Count);
        }

        private static T[] GetItemsOfTier<T>(IReadOnlyList<T> list, int tier) where T : IItem
        {
            int count = 0;

            for (int i = list.Count - 1; i >= 0; --i)
            {
                if (list[i].Tier == tier)
                {
                    count++;
                }
            }

            var array = new T[count];

            for (int i = list.Count - 1; i >= 0; --i)
            {
                var item = list[i];

                if (item.Tier == tier)
                {
                    array[--count] = item;
                }
            }

            return array;
        }

        private static void AddRandomItemsIntoSet<T>(T[] src, HashSet<T> dst, int count) where T : IItem
        {
            Span<bool> table = stackalloc bool[src.Length];

            table.Fill(false);

            while (count > 0)
            {
                int index = Random.Range(0, src.Length);

                if (!table[index])
                {
                    table[index] = true;

                    bool added = dst.Add(src[index]);

                    Debug.Assert(added);

                    count--;
                }
            }
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
                    return new ShopInfo(yValue);
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
