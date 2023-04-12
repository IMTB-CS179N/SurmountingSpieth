using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawn : MonoBehaviour
{
    public GameObject shop;
    public PlayerMove player;
    // float moveSpeed = 1f;
    List<GameObject> tiles = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            float new_x = player.transform.position.x + 50;
            GameObject a = Instantiate<GameObject>(shop, transform.position + new Vector3(new_x, 25, 0), transform.rotation, this.transform);
            GameObject b = Instantiate<GameObject>(shop, transform.position + new Vector3(new_x, 0, 0), transform.rotation, this.transform);
            GameObject c = Instantiate<GameObject>(shop, transform.position + new Vector3(new_x, -25, 0), transform.rotation, this.transform);
            tiles.Add(a);
            tiles.Add(b);
            tiles.Add(c);
        }   
        if (Input.GetKeyDown(KeyCode.Alpha1) && !player.IsMoving()){
            player.SetDestination(tiles[0].transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !player.IsMoving()) {
            player.SetDestination(tiles[1].transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && !player.IsMoving()) {
            player.SetDestination(tiles[2].transform.position);
        }
    }
}
