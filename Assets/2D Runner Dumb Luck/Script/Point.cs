using UnityEngine;
using UnityEngine.UI;

public class Point : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] int point;
    [SerializeField] int earnPoint = 1;
    [SerializeField] GameObject player;
    [SerializeField] GameObject sandStorm;
    float timer;
    float gameTime;
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
        gameTime += Time.deltaTime;
        timer += Time.deltaTime;
        if(timer >= 1.0f)
        {
            point += earnPoint;
            timer -= 1.0f;
        }
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
