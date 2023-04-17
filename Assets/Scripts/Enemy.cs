using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int maxHealth = 10;
    public int currentHealth;

    void Start() {
        currentHealth = maxHealth;
        Debug.Log("Enemy's current health is 10");
    }

    public void takeDamage(int damage) {
        currentHealth -= damage;

        if(currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        Debug.Log("Enemy Dead!");
    }

    // Update is called once per frame
    void Update()
    {
    }
}
