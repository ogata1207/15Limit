using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRealDeadCamera : MonoBehaviour
{
    private Vector3 save;
    private float saveSize;
    public Camera _camera;
    // Start is called before the first frame update
    void Start()
    {
        save = _camera.transform.position;
        saveSize = _camera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeManager.GetInstance.timeScale >= 1.0f)
        {
            if (Boss03Manager.GetRealDeadEnd())
            {
                _camera.transform.transform.position = save;
                _camera.orthographicSize = saveSize;
            }
            _camera.transform.position = save;
        }
    }
}
