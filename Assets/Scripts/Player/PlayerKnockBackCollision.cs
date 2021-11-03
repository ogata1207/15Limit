using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockBackCollision : MonoBehaviour
{
    public float power;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            var pos = transform.position;
            var enemy = collision.GetComponent<EnemyHitJudgment>();

            var dir = pos - collision.transform.position;

            enemy.SetKnockBack(dir, power);
        }
    }
}
