using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "User Character")]

public class CharacterSelection : ScriptableObject
{
    
    public string ClassChoice = "";
    //cleric = 1
    //paladin = 2
    //bard = 3
    //barbarian = 4
    //rogue = 5
    //wizard = 6
    
    public string RaceChoice = "";
    //dwarf = 1
    //elf = 2 
    //dragonborn = 3
    //human = 4 
    //orc = 5

    public void UpdateClass(string choice){

        ClassChoice = choice;
        Debug.Log(ClassChoice);

    }

    public void UpdateRace(string choice){

        RaceChoice = choice;
        Debug.Log(RaceChoice);

    }
}
