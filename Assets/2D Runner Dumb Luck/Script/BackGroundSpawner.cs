using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundSpawner : MonoBehaviour
{

    [SerializeField] GameObject background;
    [SerializeField] GameObject currentBackground;
    [SerializeField] GameObject player;

    void Update()
    {
        if (player.transform.position.x >= currentBackground.transform.position.x)
        {
            SpriteRenderer rend = currentBackground.GetComponentInChildren<SpriteRenderer>();

            GameObject newSpawnBackground = Instantiate<GameObject>(background);
            //place it
            newSpawnBackground.transform.position = new Vector2(rend.bounds.max.x + rend.bounds.extents.x, currentBackground.transform.position.y);

            currentBackground = newSpawnBackground;
        }
    }
}
