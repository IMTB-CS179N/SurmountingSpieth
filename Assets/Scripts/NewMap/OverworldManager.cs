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
        GameObject[,] GridTiles = new GameObject[5, 15];
        public GameObject BaseTile;
        public Transform TilesParent;
        float moveSpeed = 32f;
        public Transform movePointX;
        public Transform movePointY;
        public Transform player;

        List<ColumnInfo> columns = new List<ColumnInfo>();

        void Start()
        {
            // movePoint.parent = null;
            movePointX.parent = null;
            movePointY.parent = null;
            for (int i = 0; i < 15; i++)
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
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    GridTiles[i, j] = Instantiate<GameObject>(
                        BaseTile,
                        new Vector3((j - 7) * UNIT_SIZE, (i - 2) * UNIT_SIZE, 0),
                        transform.rotation,
                        TilesParent
                    );
                    GridTiles[i, j].GetComponent<SpriteRenderer>().sprite = columns[j].GetSprite(i);
                    if (columns[j].GetCell(i).IsClickable())
                    {
                        GridTiles[i, j].AddComponent<Click>();
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
                movePointY.position,
                moveSpeed * Time.deltaTime
            );
            TilesParent.position = Vector3.MoveTowards(
                TilesParent.position,
                // new Vector3(-movePoint.position.x, 0f, 0f),
                -movePointX.position,
                moveSpeed * Time.deltaTime
            );
        }

        public void SetMovePoint(Vector3 newPos)
        {
            if (
                Mathf.Abs(player.position.y - movePointY.position.y) <= 0.05f
                && Mathf.Abs(TilesParent.position.x + movePointX.position.x) <= 0.05f
            )
            {
                // movePoint.position = newPos;
                // movePointX.position.x = newPos.x;
                movePointX.position = new Vector3(newPos.x, 0f, 0f);
                // movePointY.position.y = newPos.y;
                movePointY.position = new Vector3(0f, newPos.y, 0f);
            }
            else
            {
                Debug.Log("player:" + player.position);
                Debug.Log("TilesParent: " + TilesParent.position);
                Debug.Log("movePointX: " + movePointX.position);
                Debug.Log("movePointY: " + movePointY.position);
                Debug.Log("diff: " + Mathf.Abs(TilesParent.position.x + movePointX.position.x));
                Debug.Log("diffY: " + Mathf.Abs(player.position.y - movePointX.position.y));
            }
        }
    }
}
