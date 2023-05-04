using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFade : MonoBehaviour
{
    // Start is called before the first frame update
    float opacity;
    float BOUNDS = 96f;

    void Start() { }

    // Update is called once per frame
    void Update()
    {
        Color newColor = this.GetComponent<SpriteRenderer>().color;
        if (Mathf.Abs(this.transform.position.x) > BOUNDS)
        {
            newColor.a = 0f;
        }
        else
        {
            newColor.a = 1f;
        }
        this.GetComponent<SpriteRenderer>().color = newColor;
    }
}
