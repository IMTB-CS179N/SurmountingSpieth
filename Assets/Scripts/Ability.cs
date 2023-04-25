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
    public GameObject projectile;
    public float projectileForce;


    public Ability() {
        abilityName = "Generic";
        abilityDescription = "Generic";
        manaCost = 2;
        baseDamage = 2;
    }
 
    


    void Update() {
        if(Input.GetMouseButtonDown(1)) {
            GameObject spell = Instantiate(projectile, transform.position, Quaternion.identity);
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 myPos = transform.position;
            Vector2 direction = (mousePos - myPos).normalized;
            spell.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
        }
    }
}