using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceButton : MonoBehaviour
{
    

    public string AssignedRace = "";


    void Start(){

        var found = FindObjectsOfType<CharacterSelection>();
        found[0].UpdateRace("");

    }

    public void RaceClicked(){

        var found = FindObjectsOfType<CharacterSelection>();
        found[0].UpdateRace(AssignedRace);

    }


}
