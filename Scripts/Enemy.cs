using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    public float startSpeed = 10f;
    public float startHealth = 100f;

    [HideInInspector]
    public float speed;
    public int dmg = 1;
    public int reward = 50;
    private float health = 100;
    public GameObject deathEffect;
    public int ownedBy = 2;

    [Header("Unity Ref")]
    public Image healthBar;

    void Start()
    {
        speed = startSpeed;
        health = startHealth;

    }

    public void TakeDamage (float amount)
    {
        health -= amount;
        healthBar.fillAmount = health/startHealth;

        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        PlayerStats.Money += reward;

        GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);

        WaveSpawner.EnemiesAlive--;

        Destroy(gameObject);
    }

    public void Slow(float amount)
    {
        speed = startSpeed - (startSpeed * amount);
    }

}
