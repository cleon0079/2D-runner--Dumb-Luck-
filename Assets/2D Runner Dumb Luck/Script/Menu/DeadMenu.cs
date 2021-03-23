using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadMenu : MonoBehaviour
{
    public GameObject player;
    public GameObject sandStorm;
    public GameObject point;
    public GameObject backgroundSpawner;
    private Vector2 playerSpawnPosition;
    private Vector2 sandStormSpawnPosition;
    PlayerController playerController;
    Point point1;
    SandStorm sandStorm1;
    BackGroundSpawner backgroundSpawn;
    GameObject[] enemy;
    GameObject[] backGround;

    private void Start()
    {
        // Get the player and sandstorm original position 
        playerSpawnPosition = new Vector2(player.transform.position.x, player.transform.position.y);
        sandStormSpawnPosition = new Vector2(sandStorm.transform.position.x, sandStorm.transform.position.y);
        playerController = player.GetComponent<PlayerController>();
        point1 = point.GetComponent<Point>();
        sandStorm1 = sandStorm.GetComponent<SandStorm>();
        backgroundSpawn = backgroundSpawner.GetComponent<BackGroundSpawner>();
    }

    private void Update()
    {
        // Get the gameobject that we spawn while playing the game
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        backGround = GameObject.FindGameObjectsWithTag("BackGround");

    }



    public void Back()
    {
        // Back to MainMenu
        SceneManager.LoadScene("MainMenu");
    }

    public void Resume()
    {
        // Reset all the change we make while playing back to defalut
        point1.gameTime = 0f;
        point1.timer = 0f;
        point1.earnPoint = 1;
        point1.point = 0;
        playerController.walkSpeed = 8f;
        sandStorm1.speed = 6f;
        player.transform.position = playerSpawnPosition;
        sandStorm.transform.position = sandStormSpawnPosition;
        backgroundSpawn.currentBackground = backgroundSpawn.fristBackground;
        Destroy();
        playerController.playerIsDead = false;
    }

    public void Destroy()
    {
        foreach (GameObject gameObject in enemy)
        {
            Destroy(gameObject);
        }
        foreach (GameObject gameObject in backGround)
        {
            Destroy(gameObject);
        }
    }
}
