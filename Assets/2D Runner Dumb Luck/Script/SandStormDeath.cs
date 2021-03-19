using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandStormDeath : MonoBehaviour
{

    public GameObject player;
    public bool touch = false;
    
  
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        

    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            Debug.Log("Enter Trigger");

            touch = true;

            other.gameObject.SetActive(false);
        }


    }

    void OnTriggerExit2D(Collider2D other)
    {

        touch = false;
        //Debug.Log("Collission does not exist");

    }

    void OnTriggerStay2D(Collider2D other)
    {

        // If the objects has this tag it will be looking for this function
        if (gameObject.tag == "SandStorm")
        {
            touch = true;

        }
    }

}
