using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Input;
using Project.Battle;
using Project.Game;

namespace Project.Overworld
{
    public class OverworldManager : MonoBehaviour
    {
        private static OverworldManager ms_instance;
        public static OverworldManager Instance =>
            ms_instance == null
                ? (ms_instance = MapManager.Instance.CreateOverworld())
                : ms_instance;
        public static OverworldInfo[] stages;
        int UNIT_SIZE = 16;

        // Start is called before the first frame update
        GameObject[,] GridTiles = new GameObject[5, 13];
        public GameObject BaseTile;
        public Transform TilesParent;

        // float moveSpeed = 32f;
        float moveSpeed = 48f;
        public float movePointX;
        public float movePointY;
        public Transform player;
        int currentX = 0;
        bool destination = false;

        private UI.InGameBuilder.ActionType m_action;

        List<ColumnInfo> columns = new List<ColumnInfo>();
        ColumnInfo blank = new ColumnInfo();
        Queue<Vector3> moveQueue = new Queue<Vector3>();

        void Awake()
        {
            MapManager.Instance.InGameUI.OnUIEnabled += this.InGameUICallback;
        }

        void OnDestroy()
        {
            if (MapManager.Instance != null && MapManager.Instance.InGameUI != null)
            {
                MapManager.Instance.InGameUI.OnUIEnabled -= this.InGameUICallback;
            }
        }

        void Start()
        {
            // Instantiate GameObjects to act as tiles
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    GridTiles[i, j] = Instantiate<GameObject>(
                        BaseTile,
                        new Vector3((j - 6) * UNIT_SIZE, (i - 2) * UNIT_SIZE, 0),
                        transform.rotation,
                        TilesParent
                    );
                    GridTiles[i, j].AddComponent<Click>();
                }
            }
            // Add blanks to the left side of character starting point
            for (int i = 0; i < 6; i++)
            {
                columns.Add(new ColumnInfo());
                currentX++;
            }
            ColumnInfo starterColumn = new ColumnInfo();
            starterColumn.SetCell(new BackgroundInfo(2, CellInfo.TileType.Horizontal));
            columns.Add(starterColumn);

            List<CellInfo> newCells = new List<CellInfo>();
            newCells.Add(new ShopInfo(0, 1));
            newCells.Add(new BattleInfo(1, CellInfo.TileType.BattleEasy));
            newCells.Add(new ShopInfo(2, 1));
            newCells.Add(new ShopInfo(3, 1));
            newCells.Add(new ShopInfo(4, 1));
            // GenerateLevel(newCells);
            GenerateBattle();
            // GenerateShop();

            SetSprites();
        }

        // Update is called once per frame
        void Update()
        {
            var collider = InputProcessor.Instance.RaycastLeftSingular();

            if (collider)
            {
                var clickObj = collider.transform.gameObject.GetComponent<Click>();

                if (clickObj)
                {
                    clickObj.TriggerClick();
                }
            }

            player.position = Vector3.MoveTowards(
                player.position,
                new Vector3(0f, movePointY, 0f),
                moveSpeed * Time.deltaTime
            );
            TilesParent.position = Vector3.MoveTowards(
                TilesParent.position,
                new Vector3(-movePointX, 0f, 0f),
                moveSpeed * Time.deltaTime
            );
            if (!Moving() && moveQueue.Count != 0)
            {
                SetMovePoint(moveQueue.Dequeue());
            }
            else if (!Moving() && moveQueue.Count == 0 && destination)
            {
                destination = false;
                int playerY = (int)(player.position.y) / UNIT_SIZE + 2;
                Debug.Log("Arrived at position " + GridTiles[playerY, 6].transform.position);
                // int rand = Random.Range(0, 2);
                // if (rand == 0)
                // {
                //     // GenerateLevel(newCells);
                //     GenerateBattle();
                // }
                // else
                // {
                //     GenerateShop();
                // }
                SetSprites();
                switch (this.columns[this.currentX].GetCell(playerY).tileType)
                {
                    case CellInfo.TileType.Shop:
                        MapManager.Instance.UpdateAction(UI.InGameBuilder.ActionType.Enter);
                        m_action = UI.InGameBuilder.ActionType.Enter;
                        GenerateBattle();
                        break;

                    case CellInfo.TileType.BattleEasy:
                        MapManager.Instance.difficulty = MapManager.Difficulty.Easy;
                        m_action = UI.InGameBuilder.ActionType.Battle;
                        MapManager.Instance.UpdateAction(UI.InGameBuilder.ActionType.Battle);
                        break;
                    case CellInfo.TileType.BattleMedium:
                        MapManager.Instance.difficulty = MapManager.Difficulty.Medium;
                        m_action = UI.InGameBuilder.ActionType.Battle;
                        MapManager.Instance.UpdateAction(UI.InGameBuilder.ActionType.Battle);
                        break;
                    case CellInfo.TileType.BattleHard:
                        MapManager.Instance.difficulty = MapManager.Difficulty.Hard;
                        m_action = UI.InGameBuilder.ActionType.Battle;
                        MapManager.Instance.UpdateAction(UI.InGameBuilder.ActionType.Battle);
                        break;

                    default:
                        m_action = UI.InGameBuilder.ActionType.None;
                        MapManager.Instance.UpdateAction(UI.InGameBuilder.ActionType.None);
                        break;
                }
            }
        }

        public void SetMovePoint(Vector3 newPos)
        {
            if (Moving())
            {
                return;
            }
            // move left -> shift everything right by 1 tile
            if (newPos.x > movePointX)
            {
                ShiftGameObjects((newPos.x - movePointX) / UNIT_SIZE);
            }
            // move right -> shift everything left by 1 tile
            else if (newPos.x < movePointX)
            {
                ShiftGameObjects((newPos.x - movePointX) / UNIT_SIZE);
            }
            movePointX = newPos.x;
            movePointY = newPos.y;
        }

        public void SetDestination(Vector3 newPos)
        {
            // UpdateAction(ActionType None) while moving

            int tilesX = -(int)TilesParent.position.x / UNIT_SIZE;
            int playerY = (int)player.position.y / UNIT_SIZE;
            int newX = (int)newPos.x / UNIT_SIZE;
            if (newX <= tilesX)
            {
                return;
            }
            int newY = (int)newPos.y / UNIT_SIZE;

            int deltaX = newX - tilesX;
            int deltaY = newY - playerY;

            int i = 0;
            int j = playerY;
            for (; i <= deltaX / 2; i++)
            {
                moveQueue.Enqueue(new Vector3((tilesX + i) * UNIT_SIZE, playerY * UNIT_SIZE, 0f));
            }
            if (deltaY > 0)
            {
                for (; j <= newY; j++)
                {
                    moveQueue.Enqueue(new Vector3((tilesX + i - 1) * UNIT_SIZE, j * UNIT_SIZE, 0f));
                }
                j--;
            }
            else if (deltaY < 0)
            {
                for (; j >= newY; j--)
                {
                    moveQueue.Enqueue(new Vector3((tilesX + i - 1) * UNIT_SIZE, j * UNIT_SIZE, 0f));
                }
                j++;
            }
            for (; i <= deltaX; i++)
            {
                moveQueue.Enqueue(new Vector3((tilesX + i) * UNIT_SIZE, j * UNIT_SIZE, 0f));
            }
            destination = true;
        }

        void ShiftGameObjects(float delta)
        {
            currentX += (int)delta;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    GridTiles[i, j].transform.position += new Vector3(UNIT_SIZE * delta, 0f, 0f);
                    SetSprites();
                }
            }
        }

        void GenerateLevel(List<CellInfo> newCells)
        {
            // heights range from 0 to 4
            // newCells in ascending y_value order
            /*
                4
                3
                2
                1
                0
            */
            if (!Moving() && currentX == (columns.Count - 1))
            {
                Debug.Log("test");
            }
            ColumnInfo playerSideTurns = new ColumnInfo();
            ColumnInfo tileColumn = new ColumnInfo();
            int playerHeight = (int)(player.position.y + (UNIT_SIZE * 2)) / UNIT_SIZE;
            if (newCells.Count == 1)
            {
                if (newCells[0].yValue > playerHeight)
                {
                    playerSideTurns.SetCell(
                        new BackgroundInfo(playerHeight, CellInfo.TileType.BottomRight)
                    );
                    // tileSideTurns.SetCell(
                    playerSideTurns.SetCell(
                        new BackgroundInfo(newCells[0].yValue, CellInfo.TileType.TopLeft)
                    );
                }
                else if (newCells[0].yValue == playerHeight)
                {
                    playerSideTurns.SetCell(
                        new BackgroundInfo(playerHeight, CellInfo.TileType.Horizontal)
                    );
                    // tileSideTurns.SetCell(
                    playerSideTurns.SetCell(
                        new BackgroundInfo(newCells[0].yValue, CellInfo.TileType.Horizontal)
                    );
                }
                else if (newCells[0].yValue < playerHeight)
                {
                    playerSideTurns.SetCell(
                        new BackgroundInfo(playerHeight, CellInfo.TileType.TopRight)
                    );
                    // tileSideTurns.SetCell(
                    playerSideTurns.SetCell(
                        new BackgroundInfo(newCells[0].yValue, CellInfo.TileType.BottomLeft)
                    );
                }
            }
            else if (newCells.Count > 1)
            {
                bool above = false;
                bool below = false;
                bool same = false;
                // bottom
                int bottomHeight = newCells[0].yValue;
                if (bottomHeight > playerHeight)
                {
                    // tileSideTurns.SetCell(
                    playerSideTurns.SetCell(
                        new BackgroundInfo(newCells[0].yValue, CellInfo.TileType.ForkRight)
                    );
                    above = true;
                }
                else if (bottomHeight == playerHeight)
                {
                    // tileSideTurns.SetCell(
                    playerSideTurns.SetCell(
                        new BackgroundInfo(newCells[0].yValue, CellInfo.TileType.ForkUp)
                    );
                    same = true;
                }
                else if (bottomHeight < playerHeight)
                {
                    // tileSideTurns.SetCell(
                    playerSideTurns.SetCell(
                        new BackgroundInfo(newCells[0].yValue, CellInfo.TileType.BottomLeft)
                    );
                    below = true;
                }
                int topIndex = newCells.Count - 1;
                int topHeight = newCells[topIndex].yValue;
                /*
                    * top and above -> top_left
                    * top and same -> fork_down
                    * top and below -> fork_right
                */
                if (topHeight > playerHeight)
                {
                    // tileSideTurns.SetCell(
                    playerSideTurns.SetCell(
                        new BackgroundInfo(newCells[topIndex].yValue, CellInfo.TileType.TopLeft)
                    );
                    above = true;
                }
                else if (topHeight == playerHeight)
                {
                    // tileSideTurns.SetCell(
                    playerSideTurns.SetCell(
                        new BackgroundInfo(newCells[topIndex].yValue, CellInfo.TileType.ForkDown)
                    );
                    same = true;
                }
                else if (topHeight < playerHeight)
                {
                    // tileSideTurns.SetCell(
                    playerSideTurns.SetCell(
                        new BackgroundInfo(newCells[topIndex].yValue, CellInfo.TileType.ForkRight)
                    );
                    below = true;
                }
                /*
                    * middle and above -> fork_right
                    * middle and same -> crossroads
                    * middle and below -> fork_right
                */
                for (int i = 1; i < topIndex; i++)
                {
                    var currentCell = newCells[i];
                    int height = currentCell.yValue;
                    if (height > playerHeight)
                    {
                        // tileSideTurns.SetCell(
                        playerSideTurns.SetCell(
                            new BackgroundInfo(newCells[i].yValue, CellInfo.TileType.ForkRight)
                        );
                        above = true;
                    }
                    else if (height == playerHeight)
                    {
                        // tileSideTurns.SetCell(
                        playerSideTurns.SetCell(
                            new BackgroundInfo(newCells[i].yValue, CellInfo.TileType.Crossroads)
                        );
                        same = true;
                    }
                    else if (height < playerHeight)
                    {
                        // tileSideTurns.SetCell(
                        playerSideTurns.SetCell(
                            new BackgroundInfo(newCells[i].yValue, CellInfo.TileType.ForkRight)
                        );
                        below = true;
                    }
                }

                if (above && !same && !below)
                {
                    playerSideTurns.SetCell(
                        new BackgroundInfo(playerHeight, CellInfo.TileType.BottomRight)
                    );
                }
                else if (!above && same && !below)
                {
                    Debug.Log("There should not be 2 cells that match player height");
                }
                else if (!above && !same && below)
                {
                    playerSideTurns.SetCell(
                        new BackgroundInfo(playerHeight, CellInfo.TileType.TopRight)
                    );
                }
                else if (above && same && !below)
                {
                    playerSideTurns.SetCell(
                        new BackgroundInfo(playerHeight, CellInfo.TileType.ForkUp)
                    );
                }
                else if (!above && same && below)
                {
                    playerSideTurns.SetCell(
                        new BackgroundInfo(playerHeight, CellInfo.TileType.ForkDown)
                    );
                }
                else if (above && !same && below)
                {
                    playerSideTurns.SetCell(
                        new BackgroundInfo(playerHeight, CellInfo.TileType.ForkLeft)
                    );
                }
                else if (above && same && below)
                {
                    playerSideTurns.SetCell(
                        new BackgroundInfo(playerHeight, CellInfo.TileType.Crossroads)
                    );
                }
            }
            else
            {
                Debug.Log("This should never happen");
            }
            int numDist = Random.Range(2, 4);
            for (int i = 0; i < numDist / 2; i++)
            {
                ColumnInfo spacer = new ColumnInfo();
                spacer.SetCell(new BackgroundInfo(playerHeight, CellInfo.TileType.Horizontal));
                columns.Add(spacer);
            }
            for (
                int i = playerSideTurns.nonBlank[0] + 1;
                i < playerSideTurns.nonBlank[playerSideTurns.nonBlank.Count - 1];
                i++
            )
            {
                if (!playerSideTurns.nonBlank.Contains(i))
                {
                    playerSideTurns.SetCell(new BackgroundInfo(i, CellInfo.TileType.Vertical));
                }
            }
            columns.Add(playerSideTurns);
            for (int i = numDist / 2; i < numDist; i++)
            {
                ColumnInfo spacer = new ColumnInfo();
                for (int j = 0; j < newCells.Count; j++)
                {
                    spacer.SetCell(
                        new BackgroundInfo(newCells[j].yValue, CellInfo.TileType.Horizontal)
                    );
                }
                columns.Add(spacer);
            }

            for (int i = 0; i < newCells.Count; i++)
            {
                tileColumn.SetCell(newCells[i]);
            }
            columns.Add(tileColumn);
            /*
            player-side tile logic
            -* all above -> bottom_right
            -* all below -> top_right
            -* exactly 1 and same -> horizontal
            -* all above and same -> fork_up
            -* all below and same -> fork_down
            -* above and below but not same -> fork_left
            -* above, below, and same -> crossroads

            
            tile-side tile logic
            -* exactly 1 and above -> top_left
            -* exactly 1 and same -> horizontal
            -* exactly 1 and below -> bottom_left

            -* top and above -> top_left
            -* top and same -> fork_down
            -* top and below -> fork_right

            -* middle and above -> fork_right
            -* middle and same -> crossroads
            -* middle and below -> fork_right

            -* bottom and above -> fork_right
            -* bottom and same -> fork_up
            -* bottom and below -> bottom_left
            */
            SetSprites();
        }

        bool Moving()
        {
            return player.position.y != movePointY || TilesParent.position.x != -movePointX;
        }

        public void GenerateShop()
        {
            Debug.Log("Inside GenerateShop");
            List<CellInfo> level = new List<CellInfo>();
            int height = Random.Range(0, 5);
            ShopInfo shop = new ShopInfo(height, 0);
            level.Add(shop);
            GenerateLevel(level);
        }

        void GenerateBattle()
        {
            Encounter currentEncounter = ResourceManager.Campaign[MapManager.Instance.levelIndex];

            List<CellInfo> level = new List<CellInfo>();
            List<int> yValues = new List<int>();
            HashSet<int> heights = new HashSet<int>();
            int random = Random.Range(0, 5);
            List<CellInfo.TileType> battleTypes = new List<CellInfo.TileType>();
            if (currentEncounter.EasyEnemyList.Length > 0)
            {
                battleTypes.Add(CellInfo.TileType.BattleEasy);
            }
            if (currentEncounter.NormalEnemyList.Length > 0)
            {
                battleTypes.Add(CellInfo.TileType.BattleMedium);
            }
            if (currentEncounter.HardEnemyList.Length > 0)
            {
                battleTypes.Add(CellInfo.TileType.BattleHard);
            }
            for (int i = 0; i < battleTypes.Count; i++)
            {
                while (heights.Contains(random))
                {
                    random = Random.Range(0, 5);
                }
                heights.Add(random);
                level.Add(new BattleInfo(random, battleTypes[i]));
            }

            yValues.Sort();
            level.Sort((x, y) => x.yValue.CompareTo(y.yValue));

            GenerateLevel(level);
            MapManager.Instance.levelIndex++;
        }

        void SetSprites()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    int offset = currentX + j - 6;
                    if (offset >= columns.Count)
                    {
                        GridTiles[i, j].GetComponent<SpriteRenderer>().sprite = blank.GetSprite(i);
                        GridTiles[i, j].GetComponent<Click>().clickable = false;
                    }
                    else
                    {
                        GridTiles[i, j].GetComponent<SpriteRenderer>().sprite = columns[
                            offset
                        ].GetSprite(i);
                        if (columns[offset].GetCell(i).IsClickable())
                        {
                            GridTiles[i, j].GetComponent<Click>().clickable = true;
                        }
                        else
                        {
                            GridTiles[i, j].GetComponent<Click>().clickable = false;
                        }
                    }
                }
            }
        }

        public void InGameUICallback()
        {
            MapManager.Instance.UpdateAction(this.m_action);
        }
    }
}
