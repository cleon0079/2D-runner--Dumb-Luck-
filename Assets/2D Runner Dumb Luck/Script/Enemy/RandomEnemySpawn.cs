using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemySpawn : MonoBehaviour
{
    public GameObject enemy;
    public float spawnTime = 2f;
    private Vector2 spawnPosition;
    // Use this for initialization
    void Start()
    {

        // BAD BAD BAD BAD NEVER USE THIS 
        //InvokeRepeating("Spawn", spawnTime, spawnTime);
        StartCoroutine(Spawn());
        spawnPosition.y = transform.position.y;

    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);

            spawnPosition.x = Random.Range(-5 ,5) + transform.position.x;

            Instantiate(enemy, spawnPosition, Quaternion.identity );

        }
    }

}
