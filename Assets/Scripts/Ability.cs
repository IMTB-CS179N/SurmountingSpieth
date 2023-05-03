using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ability : MonoBehaviour
{
    public Stats stats = new Stats();
    public string ClassName{get;set;}
    public string AbilityName {get; set;}
    //public string abilityDescription {get; set;}
    public int ManaCost {get; set;}
    public int DamageMultiplier {get; set;}
    
    [SerializeField]
    private int currentMana;

    private int currentDamage;
    public GameObject projectile;
    public float projectileForce;

    public Ability() {
        AbilityName = "Slash";
        //abilityDescription = "";
        ManaCost = 5;
        DamageMultiplier = 1;
        ClassName = stats.C_Class;
        stats.BaseDamage = 2;
        stats.BaseMana = 20;
    }

    public void manaReduction(int manaCost) {
        currentMana -= manaCost;
    }
    
    public void abilityDamageIncrease(int damageMultiplier) {
        currentDamage *= damageMultiplier;
    }

    public void attackAbility() {
        manaReduction(ManaCost);
        abilityDamageIncrease(DamageMultiplier);
        GameObject spell = Instantiate(projectile, transform.position, Quaternion.identity);
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 myPos = transform.position;
        Vector2 direction = (mousePos - myPos).normalized;
        spell.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
    }
    //public void Dispose(){}
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