using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sun : MonoBehaviour
{
    List<Image> sun;
    float gameTime;

    private void Start()
    {
        gameTime = Time.deltaTime;

    }

    // Update is called once per frame
    void Update()
    {
        if(gameTime >= 1)
        {

        }
    }
}
