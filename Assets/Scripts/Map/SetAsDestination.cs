using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAsDestination : MonoBehaviour
{
    public bool chosen = false;

    public PlayerMove player;
    public TileSpawnerMove spawner;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (chosen && Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.SetDestination(this.transform.localPosition.y);
            spawner.SetDestination(-this.transform.localPosition.x);
            chosen = false;
        }
    }

    public void Set()
    {
        player.SetDestination(this.transform.localPosition.y);
        spawner.SetDestination(-this.transform.localPosition.x);
        chosen = false;
    }
}
