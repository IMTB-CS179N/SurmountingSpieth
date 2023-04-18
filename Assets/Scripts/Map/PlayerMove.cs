using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Vector3 destination;

    [SerializeField]
    float moveSpeed;
    bool moving = false;

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
    }

    public void SetDestination(float newY)
    {
        Vector3 new_pos = this.transform.position;
        new_pos.y = newY;
        destination = new_pos;
        moveSpeed = Mathf.Abs(newY - this.transform.position.y) * 2;
        moving = true;
    }
}
