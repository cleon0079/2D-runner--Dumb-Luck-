using UnityEngine;
using UnityEngine.UI;

public class Point : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] GameObject player;
    [SerializeField] GameObject sandStorm;
    public int point;
    public int earnPoint = 1;
    public float timer;
    public float gameTime;
    PlayerController playerController;
    SandStorm sand;

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<Text>();
        playerController = player.GetComponent<PlayerController>();
        sand = sandStorm.GetComponent<SandStorm>();
    }

    // Update is called once per frame
    void Update()
    {
        // A timer for earnpoint per second
        timer += Time.deltaTime;

        // When the player is dead, stop earning point
        if (playerController.playerIsDead == false)
        {
            // Record the gaming time
            gameTime += Time.deltaTime;
            if (timer >= 1.0f)
            {
                point += earnPoint;
                timer -= 1.0f;
            }
        }
        else
        {
            if (timer >= 1.0f)
            {
                timer -= 1.0f;
            }
        }

        // Adding earnpoint and player speed and sandstorm speed every level
        if(gameTime >= 10f && earnPoint < 2)
        {
            earnPoint = 2;
            playerController.walkSpeed = playerController.walkSpeed + 4;
            sand.speed = sand.speed + 4;
        }
        if(gameTime >= 30f && earnPoint < 4)
        {
            earnPoint = 4;
            playerController.walkSpeed = playerController.walkSpeed + 8;
            sand.speed = sand.speed + 8;
        }
        if (gameTime >= 60f && earnPoint < 8)
        {
            earnPoint = 8;
            playerController.walkSpeed = playerController.walkSpeed + 10;
            sand.speed = sand.speed + 10;
        }
        if (gameTime >= 180f && earnPoint < 16)
        {
            earnPoint = 16;
            playerController.walkSpeed = playerController.walkSpeed + 20;
            sand.speed = sand.speed + 20;
        }
        text.text = "Point : " + point;
    }
}
