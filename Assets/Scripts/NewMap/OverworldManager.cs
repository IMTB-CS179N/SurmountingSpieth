using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Input;

namespace Project.Overworld
{
    public class OverworldManager : MonoBehaviour
    {
        private static OverworldManager ms_instance;
        public static OverworldManager Instance =>
            ms_instance == null
                ? (ms_instance = FindFirstObjectByType<OverworldManager>())
                : ms_instance;
        int UNIT_SIZE = 16;

        // Start is called before the first frame update
        GameObject[,] GridTiles = new GameObject[5, 13];
        public GameObject BaseTile;
        public Transform TilesParent;
        float moveSpeed = 32f;
        public float movePointX;
        public float movePointY;
        public Transform player;
        int currentX = 0;

        List<ColumnInfo> columns = new List<ColumnInfo>();

        void Start()
        {
            // movePoint.parent = null;
            for (int i = 0; i < 6; i++)
            {
                columns.Add(new ColumnInfo());
                currentX++;
            }
            // for (int i = 6; i < 13; i++)
            for (int i = 6; i < 17; i++)
            {
                CellInfo[] newCells = new CellInfo[5];
                // Type[] CellTypes = { typeof(BackgroundInfo), typeof(BattleInfo) };
                for (int j = 0; j < 5; j++)
                {
                    // newCells[j] = new BackgroundInfo(j, CellInfo.TileType.BottomLeft);
                    newCells[j] = ColumnInfo.RandomCell(j);
                }
                columns.Add(new ColumnInfo(i, newCells));
            }
            for (int j = 0; j < 13; j++)
            {
                for (int i = 0; i < 5; i++)
                {
                    GridTiles[i, j] = Instantiate<GameObject>(
                        BaseTile,
                        new Vector3((j - 6) * UNIT_SIZE, (i - 2) * UNIT_SIZE, 0),
                        transform.rotation,
                        TilesParent
                    );
                    GridTiles[i, j].GetComponent<SpriteRenderer>().sprite = columns[j].GetSprite(i);
                    GridTiles[i, j].AddComponent<Click>();
                    if (columns[j].GetCell(i).IsClickable())
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

        // Update is called once per frame
        void Update()
        {
            player.position = Vector3.MoveTowards(
                player.position,
                // new Vector3(0f, movePoint.position.y, 0f),
                new Vector3(0f, movePointY, 0f),
                moveSpeed * Time.deltaTime
            );
            TilesParent.position = Vector3.MoveTowards(
                TilesParent.position,
                // new Vector3(-movePoint.position.x, 0f, 0f),
                new Vector3(-movePointX, 0f, 0f),
                moveSpeed * Time.deltaTime
            );
        }

        public void SetMovePoint(Vector3 newPos)
        {
            if (
                Mathf.Abs(player.position.y - movePointY) <= 0.05f
                && Mathf.Abs(TilesParent.position.x + movePointX) <= 0.05f
            )
            {
                // move left -> shift everything right by 1 tile
                if (newPos.x > movePointX)
                {
                    ShiftGameObjects("right");
                }
                // move right -> shift everything left by 1 tile
                else if (newPos.x < movePointX)
                {
                    ShiftGameObjects("left");
                }
                movePointX = newPos.x;
                movePointY = newPos.y;
            }
            else
            {
                Debug.Log("player:" + player.position);
                Debug.Log("TilesParent: " + TilesParent.position);
                Debug.Log("movePointX: " + movePointX);
                Debug.Log("movePointY: " + movePointY);
                Debug.Log("diff: " + Mathf.Abs(TilesParent.position.x + movePointX));
                Debug.Log("diffY: " + Mathf.Abs(player.position.y - movePointY));
            }
        }

        void ShiftGameObjects(string direction)
        {
            switch (direction)
            {
                case "left":
                    currentX--;
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < 13; j++)
                        {
                            GridTiles[i, j].transform.position -= new Vector3(16f, 0f, 0f);
                            int offset = currentX + j - 6;
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

                    break;
                case "right":
                    currentX++;
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < 13; j++)
                        {
                            GridTiles[i, j].transform.position += new Vector3(16f, 0f, 0f);
                            int offset = currentX + j - 6;
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

                    break;
            }
        }

        void GenerateLevel() { }
    }
}
