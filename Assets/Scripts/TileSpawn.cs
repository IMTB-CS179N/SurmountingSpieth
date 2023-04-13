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
    List<GameObject> tiles = new List<GameObject>();
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
            for (int i = 1; i < numTiles; i++)
            {
                Vector3 offset = new Vector3(
                    player.transform.position.x + PATH_X_LENGTH,
                    2 * Y_BOUNDS * (float)i / (float)numTiles + -Y_BOUNDS,
                    0
                );
                GameObject a = Instantiate<GameObject>(
                    shop,
                    transform.position + offset,
                    transform.rotation,
                    this.transform
                );
                a.AddComponent<SetAsDestination>();
                a.GetComponent<SetAsDestination>().player = this.player;
                tiles.Add(a);
            }
            random_num = Random.Range(0, numTiles);
            tiles[random_num].GetComponent<SetAsDestination>().chosen = true;
        }
    }
}
