using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Game;

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
            Shop,
            Battle
        }

        int m_yValue { get; set; }
        public TileType tileType;

        // public Sprite TileSprite;
        public string SpritePath;

        public bool IsClickable()
        {
            return (tileType == TileType.Shop || tileType == TileType.Battle);
        }

        public CellInfo(int yValue)
        {
            m_yValue = yValue;
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
        public List<Trinket> trinkets { get; set; }
        public List<Potion> potions { get; set; }
        public List<Weapon> weapons { get; set; }
        public List<Armor> armors { get; set; }

        public ShopInfo(int yValue, int tier)
            : base(yValue)
        {
            // tier 3 (lowest) 70% tier 3 items 20% tier 2 items 10% tier 1 i
            SpritePath = "Map/Trade_Post";
            tileType = TileType.Shop;
        }
    }

    public class BattleInfo : CellInfo
    {
        public List<Enemy> enemies = new List<Enemy>();

        public BattleInfo(int yValue, List<Enemy> enemies)
            : base(yValue)
        {
            this.enemies = enemies;
            // TileSprite = ResourceManager.LoadSprite("Sprites/Characters/Dragonborn cleric");
            SpritePath = "Sprites/Characters/Dragonborn cleric";
            tileType = TileType.Battle;
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
    }
}
