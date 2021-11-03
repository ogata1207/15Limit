using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazleFlashScript : MonoBehaviour
{
    private GameObject shotPos;
    // Start is called before the first frame update
    void Start()
    {
        shotPos = GameObject.Find("PlayerShotPos");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = shotPos.transform.position;
        Destroy(gameObject, 0.1f);
    }
}
