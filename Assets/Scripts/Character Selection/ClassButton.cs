using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClassButton : MonoBehaviour
{

    public string AssignedClass = "";

    void Start(){

        var found = FindObjectsOfType<CharacterSelection>();
        found[0].UpdateClass("");

    }


    public void ClassClicked(){
        
        
        var found = FindObjectsOfType<CharacterSelection>();
        found[0].UpdateClass(AssignedClass);
        

    }
}
//