using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAsDestination : MonoBehaviour
{
    public Character character;
    public TileSpawnerMove spawner;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void Set()
    {
        character.SetDestination(this.transform.localPosition.y);
        spawner.SetDestination(-this.transform.localPosition.x);
    }
}
