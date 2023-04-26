using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileClick : OnClick
{
    // Start is called before the first frame update
    // public MovementManager manager;
    public Character character;
    public MovementManager manager;

    bool isNew = true;

    void Start() { }

    // Update is called once per frame
    void Update() { }

    public override void Click()
    {
        // this.GetComponent<SetAsDestination>().Set();
        if (this.isNew)
        {
            manager.SetAsNext(this.gameObject);
            this.isNew = false;
        }
        character.SetDestinationTile(this.gameObject);
    }
}
