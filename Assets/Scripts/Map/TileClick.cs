using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileClick : OnClick
{
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public override void Click()
    {
        // Debug.Log("Clkdjfklsjfsdlkjf");
        this.GetComponent<SetAsDestination>().Set();
    }
}
