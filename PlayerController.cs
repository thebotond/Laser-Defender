using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 20f;
    public float padding = 1.0f;
    public float projectileSpeed = 5f;
    public float firingRate = 0.2f;
    public float health = 300f;
    public int hitPoints = 3;

    public AudioClip fireSound;

    private HealthCheck healthCheck;

    public GameObject projectile;

    float xMin = -7.5f;
    float xMax = 7f;

	// Use this for initialization
	void Start ()
    {
        healthCheck = GameObject.Find("Health").GetComponent<HealthCheck>();
        hitPoints = (int)health / 100;

        float distance = transform.position.z - Camera.main.transform.position.z;

        Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, distance));
        Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, distance));

        xMin = leftMost.x + padding;
        xMax = rightMost.x - padding;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            InvokeRepeating("Fire", 0.0001f, firingRate);
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke("Fire");
        }


		if(Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }

        float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
	}

    void Fire()
    {
        Vector3 startPosition = transform.position + new Vector3(0f, 1f, 0);
        GameObject beam = Instantiate(projectile, startPosition, Quaternion.identity);
        beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("Hit " + collider);
        Projectile missile = collider.gameObject.GetComponent<Projectile>();
        if (missile)
        {
            health -= missile.GetDamage();
            hitPoints = (int)health / 100;
            missile.Hit();
            healthCheck.UpdateHealth(hitPoints);
            if (health <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        LevelManager lvlMgr = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        lvlMgr.LoadLevel("Win Screen");
        HealthCheck.ResetHealth();
        Destroy(gameObject);
    }
}
