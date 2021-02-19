using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerController player;

    public bool isFollowing = true;

    public float xOffset;
    public float yOffset;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowing)
        {
            transform.position = new Vector3(player.transform.position.x + xOffset, 0, transform.position.z);
        }
    }
}