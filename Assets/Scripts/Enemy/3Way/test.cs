using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Aim(this.gameObject, player, -90);
    }
    Vector2 Aim(GameObject rotationObject, GameObject targetObject, float angleOffset)
    {
        Vector3 posDif = targetObject.transform.position - rotationObject.transform.position;
        float angle = Mathf.Atan2(posDif.y, posDif.x) * Mathf.Rad2Deg;
        Vector3 euler = new Vector3(0, 0, angle + angleOffset);

        rotationObject.transform.rotation = Quaternion.Euler(euler);

        return posDif.normalized;
    }
}
