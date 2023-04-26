using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    bool moving = false;

    float moveSpeed;

    int tileIndex = 0;
    public MovementManager manager;

    [SerializeField]
    GameObject destinationTile;

    Vector3 destination;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            this.transform.position = Vector3.MoveTowards(
                this.transform.position,
                destination,
                moveSpeed * Time.deltaTime
            );
            if (this.transform.position == destination)
            {
                moving = false;
            }
        }
        else if (
            !manager.IsSpawnerMoving()
            && this.transform.position != destinationTile.transform.position
        )
        {
            if (destinationTile.transform.position.x > this.transform.position.x)
            {
                tileIndex++;
            }
            else
            {
                tileIndex--;
            }
            manager.GetPathElement(tileIndex).GetComponent<SetAsDestination>().Set();
        }
    }

    public void SetDestination(float newY)
    {
        Vector3 new_pos = this.transform.position;
        new_pos.y = newY;
        destination = new_pos;
        moveSpeed = Mathf.Abs(newY - this.transform.position.y) * 4;
        moving = true;
    }

    public void SetDestinationTile(GameObject tile)
    {
        destinationTile = tile;
    }

    // void SetIntermediateDestination()

    public bool IsMoving()
    {
        return this.moving;
    }

    public int GetTileIndex()
    {
        return this.tileIndex;
    }
}
