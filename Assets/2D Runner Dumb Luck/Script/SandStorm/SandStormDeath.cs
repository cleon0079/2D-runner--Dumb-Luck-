using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandStormDeath : MonoBehaviour
{
    PlayerController playerCon;
    public GameObject gamePlayUI;
    public GameObject DeadMenu;

    private void Start()
    {
        playerCon = GetComponent<PlayerController>();
    }



    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Enemy" || other.tag == "SandStorm")
        {
            playerCon.playerIsDead = true;
            gamePlayUI.SetActive(false);
            DeadMenu.SetActive(true);
        }
    }
}
