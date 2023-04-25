using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClassButton : MonoBehaviour
{

    public string AssignedClass = "";

    public void ClassClicked(){
        
        
        var found = FindObjectsOfType<CharacterSelection>();
        found[0].UpdateRace(AssignedClass);
        

    }
}
//