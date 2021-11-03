using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelUp : MonoBehaviour
{
    public GameObject levelUp;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.A))
        //{
        //    var shakeObj = Camera.main.GetComponent<ShakeObject>();
        //    shakeObj.PlayShake(0.1f, 0.1f);
        //    GameObject ef = Instantiate(levelUp, transform.position, Quaternion.identity) as GameObject;
        //    Destroy(ef, 0.6f);
        //}
    }

    public void LevelUp()
    {
        var shakeObj = Camera.main.GetComponent<ShakeObject>();
        shakeObj.PlayShake(0.1f, 0.1f);
        GameObject ef = Instantiate(levelUp, transform.position, Quaternion.identity) as GameObject;
        Destroy(ef, 0.6f);
    }
}
