using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public GameObject player;
    PlayerController playerCon;


    private void Start()
    {
        playerCon = player.GetComponent<PlayerController>();
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerCon.playerIsDead = true;

        }       
    }
}
