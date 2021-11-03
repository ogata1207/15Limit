using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spar_test : MonoBehaviour
{
    public GameObject testP1;
    public GameObject testP2;
    public int speed;



    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,speed, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            Instantiate(testP1, transform.position, transform.rotation);
            Instantiate(testP2, transform.position, transform.rotation);
        }
    }



 
}
