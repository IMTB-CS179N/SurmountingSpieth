using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    public Stats stats = new Stats();
    public Ability ability = new Ability();
    
    [SerializeField]
    public int currentMana;
    [SerializeField]
    private int currentDamage;
    public GameObject projectile;
    public float projectileForce;

    public PlayerAbility() {
        stats.BaseDamage = 2;
        stats.BaseMana = 20;
    }

    public void manaReduction() {
        currentMana -= ability.ManaCost;
    }
    
    public void abilityDamageIncrease() {
        currentDamage *= ability.DamageMultiplier;
    }

    public void attackAbility() {
        manaReduction();
        abilityDamageIncrease();
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
        currentDamage = stats.BaseDamage;
        currentMana = stats.BaseMana;
    }
}