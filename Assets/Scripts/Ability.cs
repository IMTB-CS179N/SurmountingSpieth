using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Ability : MonoBehaviour
{
    public string abilityName;
    public string abilityDescription;
    public int manaCost;
    public int baseDamage;
    public bool attacking = false;



    public Ability(string abilityName, string abilityDescription, int manaCost, int baseDamage) {
        abilityName = this.abilityName;
        abilityDescription = this.abilityDescription;
        manaCost = this.manaCost;
        baseDamage = this.baseDamage;
    }

    public Ability() {
        abilityName = "Generic";
        abilityDescription = "Generic";
        manaCost = 2;
        baseDamage = 2;
    }
     public void attack() {
        attacking = true;
    }

    void Update() {
        if(Input.GetMouseButtonDown(0)) {
            attack();
        }
        if(!Input.GetMouseButtonDown(0)) {
            attacking = false;
        }
    }
}