using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShowText : MonoBehaviour
{
 

    [SerializeField] public CharacterSelection data;

    public string ClassElement;
    public string RaceElement;

    void Update(){
        
        ClassElement = data.ClassChoice;
        RaceElement = data.RaceChoice;

    }
}
