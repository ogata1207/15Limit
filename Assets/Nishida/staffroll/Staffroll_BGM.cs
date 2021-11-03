using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staffroll_BGM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.GetInstance.PlayBGM("sutaffBGM");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
