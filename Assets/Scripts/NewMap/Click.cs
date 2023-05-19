using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Overworld;

public class Click : MonoBehaviour
{
    public bool clickable { get; set; }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void TriggerClick()
    {
        Debug.Log(
            "Moving to " + (this.transform.position - this.transform.parent.transform.position)
        );
        OverworldManager.Instance.SetMovePoint(
            this.transform.position - this.transform.parent.transform.position
        );
    }
}
