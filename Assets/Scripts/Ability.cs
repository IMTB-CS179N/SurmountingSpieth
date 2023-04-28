using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Ability : MonoBehaviour
{
    public string abilityName {get; set;}
    public string abilityDescription {get; set;}
    public int manaCost {get; set;}
    public int baseDamage {get; set;}
    public int baseMana;
    public GameObject projectile;
    public float projectileForce;


    public Ability() {
        abilityName = "";
        abilityDescription = "";
        manaCost = 5;
        baseDamage = 2;
        baseMana = 20;
    }

    public void manaReduction(int manaRed) {
        baseMana -= manaRed;
    }

    void Update() {
        if(Input.GetMouseButtonDown(1)) {
            if(baseMana < 5) {
                projectile.SetActive(false);
                Debug.Log("Not enough mana. Cannot use ability");
                baseMana += 20;
            }
            else {
                projectile.SetActive(true);
            }
            manaReduction(manaCost);
            GameObject spell = Instantiate(projectile, transform.position, Quaternion.identity);
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 myPos = transform.position;
            Vector2 direction = (mousePos - myPos).normalized;
            spell.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
        }
    }
}