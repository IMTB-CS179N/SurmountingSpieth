using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Vector3 destination;

    [SerializeField]
    float moveSpeed = 30f;
    bool moving = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moving) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, destination, moveSpeed * Time.deltaTime);
            if (this.transform.position == destination) {
                moving = false;
            }
        }
    }

    public void SetDestination(Vector3 newDestination) {
        destination = newDestination;
        moving = true;
    }

    public bool IsMoving() {
        return moving;
    }
}
