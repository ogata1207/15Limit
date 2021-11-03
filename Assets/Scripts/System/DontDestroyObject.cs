using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObject : MonoBehaviour {
    [Header("保持し続けたい場合はチェック")]
    public bool isActive = true;
    private static bool isInstance = false;
    
    void Awake()
    {
        if (isActive)
        {
            //既に保持しているFade用のCanvasがある場合2つ目のFadeCanvasを消す
            if (isInstance) Destroy(gameObject);
            else
            {
                isInstance = true;
                //FadeCanvasを保持する
                DontDestroyOnLoad(gameObject);

            }
        }
    }
}
