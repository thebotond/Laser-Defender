﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFormation : MonoBehaviour {

    public GameObject projectile;
    public float health = 100f;
    public float projectileSpeed = 10;
    public float firingRate = 0.5f;
    public int scoreValue = 150;

    public AudioClip fireSound;
    public AudioClip oofSound;

    private ScoreKeeper scoreKeeper;

    void Start()
    {
        scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
        //scoreKeeper.ResetScore();
    }

    private void Update()
    {
        float probability = firingRate * Time.deltaTime;
        if(Random.value < probability)
        {
            Fire();
        }
    }

    void Fire()
    {
        Vector3 startPosition = transform.position + new Vector3(0f, -1f, 0);
        GameObject missile = Instantiate(projectile, startPosition, Quaternion.identity) as GameObject;
        missile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("Hit " + collider);
        Projectile missile = collider.gameObject.GetComponent<Projectile>();
        if(missile)
        {
            health -= missile.GetDamage();
            missile.Hit();
            if(health <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        AudioSource.PlayClipAtPoint(oofSound, transform.position);
        Destroy(gameObject);
        scoreKeeper.UpdateScore(scoreValue);
    }
}
