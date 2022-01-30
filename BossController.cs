using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class BossController : MonoBehaviour
{

    public GameObject enemyPrefab;
    public float width = 10f;
    public float height = 5f;
    public float speed = 5f;
    public int timer = 20;

    private bool movingRight = false;
    private bool canSpawn = false;
    private float xMax;
    private float xMin;

    bool canBeDefeated = false;


    //Time.timeScale = 1.0f;


    // Use this for initialization
    void Start()
    {
        //useful for 3d games
        float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera));
        Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceToCamera));

        xMin = leftBoundary.x;
        xMax = rightBoundary.x;

        StartCoroutine("LoseTime");
        Time.timeScale = 1;
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
    }

    void SpawnEnemies()
    {
        foreach (Transform child in transform)
        {
            GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = child;
        }
    }

    void SpawnUntilFull()
    {
        Transform freePosition = NextFreePosition();
        if (freePosition)
        {
            GameObject enemy = Instantiate(enemyPrefab, freePosition.transform.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = freePosition;
        }
        if (NextFreePosition())
        {
            Invoke("SpawnUntilFull", 0);
        }
    }

    bool AllMembersDead()
    {
        foreach (Transform childPositionGameObject in transform)
        {
            if (childPositionGameObject.childCount > 0)
            {
                return false;
            }
        }
        //canSpawn = false;
        return true;
    }

    Transform NextFreePosition()
    {
        foreach (Transform childPositionGameObject in transform)
        {
            if (childPositionGameObject.childCount == 0)
            {
                return childPositionGameObject;
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (movingRight)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f);
        }
        else
        {
            transform.position += new Vector3(-speed * Time.deltaTime, 0f, 0f);
        }

        float rightEdgeOfFormation = transform.position.x + (0.5f * width);
        float leftEdgeOfFormation = transform.position.x - (0.5f * width);

        if (leftEdgeOfFormation < xMin)
        {
            movingRight = true;
        }
        else if (rightEdgeOfFormation > xMax)
        {
            movingRight = false;
        }

        if (canSpawn && !canBeDefeated)
        {
            //Debug.Log("Spawnign");
            StopCoroutine("LoseTime");
            timer = 20;
            SpawnUntilFull();
            canBeDefeated = true;
        }

        else if (AllMembersDead() && canBeDefeated)
        {
            //Debug.Log("AllMembersDead() && canBeDefeated");
            StartCoroutine("LoseTime");
            canSpawn = false;
            canBeDefeated = false;
        }

        else if (timer == 0 && !canSpawn)
        {
            //Debug.Log("timer == 0 && !canSpawn");
            StopCoroutine("LoseTime");
            canSpawn = true;
            canBeDefeated = false;
        }
    }

    IEnumerator LoseTime()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            timer--;
            //Debug.Log("Time: " + timer);
        }
    }
}
