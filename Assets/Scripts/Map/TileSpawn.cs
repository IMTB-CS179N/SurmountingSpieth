using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawn : MonoBehaviour
{
    [SerializeField]
    int numTiles = 4;
    public GameObject shop;
    public PlayerMove player;

    // float moveSpeed = 1f;
    List<List<GameObject>> tiles = new List<List<GameObject>>();
    int curr_level = 0;
    int random_num;

    int PATH_X_LENGTH = 50;

    // int X_BOUNDS = 100;
    int Y_BOUNDS = 40;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            numTiles = Random.Range(1, 4);
            tiles.Add(new List<GameObject>());
            for (int i = 1; i < numTiles + 1; i++)
            {
                Vector3 newPos = this.transform.position;
                newPos.x = PATH_X_LENGTH;
                newPos.y = 2 * Y_BOUNDS * i / (float)(numTiles + 1) - Y_BOUNDS;
                GameObject _ = CreateTile(newPos, shop);
            }
            random_num = Random.Range(0, numTiles);
            tiles[curr_level][random_num].GetComponent<SetAsDestination>().chosen = true;
            curr_level++;
        }
    }

    public GameObject CreateTile(Vector3 position, GameObject tile)
    {
        GameObject newObject = Instantiate<GameObject>(
            shop,
            position,
            transform.rotation,
            this.transform
        );
        newObject.AddComponent<SetAsDestination>();
        newObject.GetComponent<SetAsDestination>().player = this.player;
        newObject.GetComponent<SetAsDestination>().spawner = this.GetComponent<TileSpawnerMove>();
        newObject.name = curr_level.ToString();
        tiles[curr_level].Add(newObject);
        return newObject;
    }
}
