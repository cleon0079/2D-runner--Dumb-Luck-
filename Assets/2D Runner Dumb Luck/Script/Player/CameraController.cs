using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerController player;
    public float xOffset = 7f;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Camera follows the player
        transform.position = new Vector3(player.transform.position.x + xOffset, 0, transform.position.z); 
    }
}