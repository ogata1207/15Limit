using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeadCamera : MonoBehaviour
{
    private Vector3 pos;
    private Vector3 save;
    private float saveSize;
    public Camera _camera;
    private GameObject boss;
    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.Find("BossFinal");
        save = _camera.transform.position;
        saveSize = _camera.orthographicSize;
        pos = _camera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeManager.GetInstance.timeScale >= 1.0f)
        {
            if (Boss03Manager.GetRealDead() && !Boss03Manager.GetRealDeadEnd())
            {
                pos.x = boss.transform.position.x;
                pos.y = boss.transform.position.y;
                _camera.orthographicSize = 3.0f;
            }
            _camera.transform.position = pos;
        }
    }
}
