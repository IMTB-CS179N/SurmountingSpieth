using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    int UNIT_SIZE = 16;
    public GameObject foregroundTiles;
    public GameObject backgroundTiles;

    public GameObject shop;
    public GameObject bottomLeft;
    public GameObject bottomRight;
    public GameObject topLeft;
    public GameObject topRight;
    public GameObject vertical;
    public GameObject horizontal;
    public GameObject blank;

    public GameObject starter;

    public Character character;
    public TileSpawnerMove spawner;

    List<GameObject> path = new List<GameObject>();
    List<GameObject> currLevel;
    List<GameObject> backLevel;

    // Start is called before the first frame update
    void Start()
    {
        path.Add(starter);
    }

    // Update is called once per frame
    void Update()
    {
        if (!character.IsMoving() && character.GetTileIndex() == path.Count - 1)
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                CreateLevel(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                CreateLevel(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                CreateLevel(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                CreateLevel(4);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                CreateLevel(5);
            }
    }

    void CreateLevel(int numTiles)
    {
        currLevel = new List<GameObject>();
        backLevel = new List<GameObject>();
        GameObject newTile;
        GameObject backTile;
        int rand;
        switch (numTiles)
        {
            case 1:
                // random any tile
                rand = Random.Range(-2, 3);
                newTile = CreateTile(
                    "foreground",
                    shop,
                    new Vector3(2 * UNIT_SIZE, rand * UNIT_SIZE, 0)
                );
                backTile = CreateTile(
                    "background",
                    blank,
                    new Vector3(2 * UNIT_SIZE, rand * UNIT_SIZE, 0)
                );
                currLevel.Add(newTile);
                backLevel.Add(backTile);

                break;

            case 2:
                // hard code to surround middle
                newTile = CreateTile(
                    "foreground",
                    shop,
                    new Vector3(2 * UNIT_SIZE, 1 * UNIT_SIZE, 0)
                );
                backTile = CreateTile(
                    "background",
                    blank,
                    new Vector3(2 * UNIT_SIZE, 1 * UNIT_SIZE, 0)
                );
                currLevel.Add(newTile);
                backLevel.Add(backTile);

                newTile = CreateTile(
                    "foreground",
                    shop,
                    new Vector3(2 * UNIT_SIZE, -1 * UNIT_SIZE, 0)
                );
                backTile = CreateTile(
                    "background",
                    blank,
                    new Vector3(2 * UNIT_SIZE, -1 * UNIT_SIZE, 0)
                );
                currLevel.Add(newTile);
                backLevel.Add(backTile);

                break;

            case 3:
                // hard code to 1 3 5
                newTile = CreateTile(
                    "foreground",
                    shop,
                    new Vector3(2 * UNIT_SIZE, 2 * UNIT_SIZE, 0)
                );
                backTile = CreateTile(
                    "background",
                    blank,
                    new Vector3(2 * UNIT_SIZE, 2 * UNIT_SIZE, 0)
                );
                currLevel.Add(newTile);
                backLevel.Add(backTile);

                newTile = CreateTile(
                    "foreground",
                    shop,
                    new Vector3(2 * UNIT_SIZE, 0 * UNIT_SIZE, 0)
                );
                backTile = CreateTile(
                    "background",
                    blank,
                    new Vector3(2 * UNIT_SIZE, 0 * UNIT_SIZE, 0)
                );
                currLevel.Add(newTile);
                backLevel.Add(backTile);

                newTile = CreateTile(
                    "foreground",
                    shop,
                    new Vector3(2 * UNIT_SIZE, -2 * UNIT_SIZE, 0)
                );
                backTile = CreateTile(
                    "background",
                    blank,
                    new Vector3(2 * UNIT_SIZE, -2 * UNIT_SIZE, 0)
                );
                currLevel.Add(newTile);
                backLevel.Add(backTile);

                break;

            case 4:
                // hard code 1 3 5, random 2 4
                rand = Random.Range(0, 2) * 2 - 1;
                Debug.Log("random: " + rand);
                newTile = CreateTile(
                    "foreground",
                    shop,
                    new Vector3(2 * UNIT_SIZE, -2 * UNIT_SIZE, 0)
                );
                backTile = CreateTile(
                    "background",
                    blank,
                    new Vector3(2 * UNIT_SIZE, -2 * UNIT_SIZE, 0)
                );
                currLevel.Add(newTile);
                backLevel.Add(backTile);

                newTile = CreateTile(
                    "foreground",
                    shop,
                    new Vector3(2 * UNIT_SIZE, 0 * UNIT_SIZE, 0)
                );
                backTile = CreateTile(
                    "background",
                    blank,
                    new Vector3(2 * UNIT_SIZE, 0 * UNIT_SIZE, 0)
                );
                currLevel.Add(newTile);
                backLevel.Add(backTile);

                newTile = CreateTile(
                    "foreground",
                    shop,
                    new Vector3(2 * UNIT_SIZE, 2 * UNIT_SIZE, 0)
                );
                backTile = CreateTile(
                    "background",
                    blank,
                    new Vector3(2 * UNIT_SIZE, 2 * UNIT_SIZE, 0)
                );
                currLevel.Add(newTile);
                backLevel.Add(backTile);

                newTile = CreateTile(
                    "foreground",
                    shop,
                    new Vector3(2 * UNIT_SIZE, rand * UNIT_SIZE, 0)
                );
                backTile = CreateTile(
                    "background",
                    blank,
                    new Vector3(2 * UNIT_SIZE, rand * UNIT_SIZE, 0)
                );
                currLevel.Add(newTile);
                backLevel.Add(backTile);

                break;
            case 5:
                // all 5
                newTile = CreateTile(
                    "foreground",
                    shop,
                    new Vector3(2 * UNIT_SIZE, 2 * UNIT_SIZE, 0)
                );
                backTile = CreateTile(
                    "background",
                    blank,
                    new Vector3(2 * UNIT_SIZE, 2 * UNIT_SIZE, 0)
                );
                currLevel.Add(newTile);
                backLevel.Add(backTile);

                newTile = CreateTile(
                    "foreground",
                    shop,
                    new Vector3(2 * UNIT_SIZE, 1 * UNIT_SIZE, 0)
                );
                backTile = CreateTile(
                    "background",
                    blank,
                    new Vector3(2 * UNIT_SIZE, 1 * UNIT_SIZE, 0)
                );
                currLevel.Add(newTile);
                backLevel.Add(backTile);

                newTile = CreateTile(
                    "foreground",
                    shop,
                    new Vector3(2 * UNIT_SIZE, 0 * UNIT_SIZE, 0)
                );
                backTile = CreateTile(
                    "background",
                    blank,
                    new Vector3(2 * UNIT_SIZE, 0 * UNIT_SIZE, 0)
                );
                currLevel.Add(newTile);
                backLevel.Add(backTile);

                newTile = CreateTile(
                    "foreground",
                    shop,
                    new Vector3(2 * UNIT_SIZE, -1 * UNIT_SIZE, 0)
                );
                backTile = CreateTile(
                    "background",
                    blank,
                    new Vector3(2 * UNIT_SIZE, -1 * UNIT_SIZE, 0)
                );
                currLevel.Add(newTile);
                backLevel.Add(backTile);

                newTile = CreateTile(
                    "foreground",
                    shop,
                    new Vector3(2 * UNIT_SIZE, -2 * UNIT_SIZE, 0)
                );
                backTile = CreateTile(
                    "background",
                    blank,
                    new Vector3(2 * UNIT_SIZE, -2 * UNIT_SIZE, 0)
                );
                currLevel.Add(newTile);
                backLevel.Add(backTile);

                break;
            default:
                break;
        }
        return;
    }

    void PathNextTile(GameObject tile)
    {
        int tileY = (int)tile.transform.position.y / UNIT_SIZE;
        int charY = (int)character.transform.position.y / UNIT_SIZE;

        if (tileY > charY)
        {
            path.Add(
                CreateTile("foreground", bottomRight, new Vector3(UNIT_SIZE, charY * UNIT_SIZE, 0))
            );
            for (int i = charY + 1; i < tileY; i++)
            {
                path.Add(
                    CreateTile("background", vertical, new Vector3(UNIT_SIZE, i * UNIT_SIZE, 0))
                );
            }
            path.Add(
                CreateTile("foreground", topLeft, new Vector3(UNIT_SIZE, tileY * UNIT_SIZE, 0))
            );
        }
        else if (tileY < charY)
        {
            path.Add(
                CreateTile("foreground", topRight, new Vector3(UNIT_SIZE, charY * UNIT_SIZE, 0))
            );
            for (int i = charY - 1; i > tileY; i--)
            {
                path.Add(
                    CreateTile("background", vertical, new Vector3(UNIT_SIZE, i * UNIT_SIZE, 0))
                );
            }
            path.Add(
                CreateTile("background", bottomLeft, new Vector3(UNIT_SIZE, tileY * UNIT_SIZE, 0))
            );
        }
        else
        {
            path.Add(
                CreateTile("background", horizontal, new Vector3(UNIT_SIZE, charY * UNIT_SIZE, 0))
            );
        }
    }

    GameObject CreateTile(string type, GameObject tile, Vector3 position)
    {
        GameObject newObject;
        if (string.Equals(type, "foreground"))
        {
            newObject = Instantiate<GameObject>(
                tile,
                position,
                transform.rotation,
                foregroundTiles.transform
            );
            newObject.AddComponent<TileClick>();
            newObject.GetComponent<TileClick>().character = this.character;
            newObject.GetComponent<TileClick>().manager = this;
            newObject.AddComponent<SetAsDestination>();
            newObject.GetComponent<SetAsDestination>().character = this.character;
            newObject.GetComponent<SetAsDestination>().spawner = this.spawner;
            return newObject;
        }
        else
        {
            newObject = Instantiate<GameObject>(
                tile,
                position,
                transform.rotation,
                backgroundTiles.transform
            );
            newObject.AddComponent<SetAsDestination>();
            newObject.GetComponent<SetAsDestination>().character = this.character;
            newObject.GetComponent<SetAsDestination>().spawner = this.spawner;
            return newObject;
        }
    }

    public GameObject GetPathElement(int index)
    {
        Debug.Log(index);
        return this.path[index];
    }

    public bool IsSpawnerMoving()
    {
        return this.spawner.moving;
    }

    public void SetAsNext(GameObject nextTile)
    {
        for (int i = 0; i < this.currLevel.Count; i++)
        {
            if (currLevel[i] != nextTile)
            {
                GameObject.Destroy(currLevel[i]);
                GameObject.Destroy(backLevel[i]);
            }
        }
        PathNextTile(nextTile);
        path.Add(nextTile);
    }
}
