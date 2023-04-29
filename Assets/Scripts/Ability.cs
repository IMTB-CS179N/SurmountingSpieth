using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Ability : MonoBehaviour
{
    public string abilityName {get; set;}
    public string abilityDescription {get; set;}
    public int manaCost {get; set;}
    [SerializeField] 
    private int currentMana;
    public GameObject projectile;
    public float projectileForce;
    public Stats stats = new Stats();


    public Ability() {
        abilityName = "";
        abilityDescription = "";
        manaCost = 5;
        stats.BaseDamage = 2;
        stats.BaseMana = 20;
    }

    public void manaReduction(int manaRed) {
        currentMana -= manaRed;
    }

    void attackAbility() {
        manaReduction(manaCost);
        GameObject spell = Instantiate(projectile, transform.position, Quaternion.identity);
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 myPos = transform.position;
        Vector2 direction = (mousePos - myPos).normalized;
        spell.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
    }

    void Update() {
        if(Input.GetMouseButtonDown(0)) {
            if(currentMana < 5) {
                projectile.SetActive(false);
                Debug.Log("Not enough mana. Cannot use ability");
                currentMana += stats.BaseMana;
            }
            else {
                projectile.SetActive(true);
            }
            attackAbility();
        }
    }
    void Start() {
        currentMana = stats.BaseMana;
    }
}