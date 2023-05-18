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

        public bool IsClickable()
        {
            return (m_tileType == TileType.Shop || m_tileType == TileType.Battle);
        }

        public CellInfo(int yValue)
        {
            m_yValue = yValue;
        }

        public TileType m_tileType;
        public Sprite TileSprite;
    }

    public class BackgroundInfo : CellInfo
    {
        public BackgroundInfo(int yValue, TileType type)
            : base(yValue)
        {
            var SpriteName = type.ToString("G");
            string path = "Map/PathTiles/" + SpriteName;
            // TileSprite = Resources.Load<Sprite>(path);
            TileSprite = ResourceManager.LoadSprite(path);
            m_tileType = type;
        }
    }

    public class ShopInfo : CellInfo
    {
        public ShopInfo(int yValue)
            : base(yValue)
        {
            // TileSprite = Resources.Load<Sprite>("Map/Trade_Post");
            TileSprite = ResourceManager.LoadSprite("Map/Trade_Post");
            m_tileType = TileType.Shop;
        }
    }

    public class BattleInfo : CellInfo
    {
        public List<Enemy> m_enemies = new List<Enemy>();

        public BattleInfo(int yValue, List<Enemy> enemies)
            : base(yValue)
        {
            m_enemies = enemies;
            TileSprite = ResourceManager.LoadSprite("Sprites/Characters/Dragonborn cleric");
            m_tileType = TileType.Battle;
        }
    }

    public class ColumnInfo
    {
        CellInfo[] cells = new CellInfo[5];
        int m_index { get; set; }

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
            return cells[tileRow].TileSprite;
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
    }
}
