using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandStorm : MonoBehaviour
{
    public float speed = 6f;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}
