using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss02CameraSize : MonoBehaviour
{
    public Camera _camera;
    // Update is called once per frame
    void Update()
    {
        if (SceneStateManager.GetInstance.GetStageNum() == 19)
        {
            _camera.orthographicSize = 6.5f;
        }
        else
        {
            _camera.orthographicSize = 5.0f;
        }
    }
}
