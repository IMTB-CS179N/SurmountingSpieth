using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int currentHealth;
    public GameObject Player;
    public GameObject projectile;
    public Stats stats = new Stats();

    public Enemy() {
        stats.C_Class = "Rogue";
        stats.Health = 10;
    }

    void Start() {
        currentHealth = stats.Health;
        Debug.Log("Enemy's current health is 10");
    }

    public void takeDamage(int damage) {
        currentHealth -= damage;
        if(currentHealth >= 0) {
            Debug.Log("Enemy's health is: " + currentHealth);
        }
        if(currentHealth <= 0) {
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject) {
            takeDamage(Player.GetComponent<Ability>().stats.BaseDamage);
        }
    }

    void Die() {
        Debug.Log("Enemy Dead!");
        Destroy(GetComponent<SpriteRenderer>());
    }

    // Update is called once per frame
    void Update() {}
}
