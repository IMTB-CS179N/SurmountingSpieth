using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    bool run = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) {
            run = true;
        }
        if (run) {
            this.transform.position = this.transform.position + Vector3.left * Time.deltaTime * 10;
        }
    }
}
