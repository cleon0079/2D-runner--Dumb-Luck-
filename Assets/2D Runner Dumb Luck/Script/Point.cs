using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Point : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] int point;
    [SerializeField] int earnPoint = 1;
    float timer;
    float gameTime;

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<Text>();
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
        }
        if(gameTime >= 30f && earnPoint < 4)
        {
            earnPoint = 4; 
        }
        if(gameTime >= 60f && earnPoint < 8)
        {
            earnPoint = 8;
        }
        if(gameTime >= 180f && earnPoint < 16)
        {
            earnPoint = 16;
        }
        text.text = "Point : " + point;
    }
}
