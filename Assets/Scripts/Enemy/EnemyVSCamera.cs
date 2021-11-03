using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVSCamera : MonoBehaviour
{
    private Vector3 me;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // 画面左下のワールド座標をビューポートから取得 get lower left (in world coordinate) from ViewPort
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0f));

        // 画面右上のワールド座標をビューポートから取得 get upper right (in world coordinate) from ViewPort
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        me.x = Mathf.Clamp(me.x, min.x, max.x);
        me.y = Mathf.Clamp(me.y, min.y, max.y);

        transform.position = me;

    }
}
