using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    public GameObject missilePrefab;
    public float missileSpeed;
    void Update()
    {
        GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity) as GameObject;
        Rigidbody2D missileRb = missile.GetComponent<Rigidbody2D>();
        missileRb.AddForce(transform.forward * missileSpeed);
        Destroy(gameObject, 1.0f);
    }
}
