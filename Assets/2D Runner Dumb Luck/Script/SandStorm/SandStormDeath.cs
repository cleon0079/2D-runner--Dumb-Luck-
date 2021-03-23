using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandStormDeath : MonoBehaviour
{
    public GameObject player;
    PlayerController playerCon;
    public GameObject gamePlayUI;
    public GameObject DeadMenu;

    private void Start()
    {
        playerCon = player.GetComponent<PlayerController>();
    }



    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            playerCon.playerIsDead = true;
            gamePlayUI.SetActive(false);
            DeadMenu.SetActive(true);
        }
    }
}
