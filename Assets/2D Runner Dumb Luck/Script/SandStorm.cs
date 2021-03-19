using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandStorm : MonoBehaviour
{
    // SandStorm moving speed
    public float speed = 6f;

    // Update is called once per frame
    void Update()
    {
        // SandStorm moving to the right
        gameObject.transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}
