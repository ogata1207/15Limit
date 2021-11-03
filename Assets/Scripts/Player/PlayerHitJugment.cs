using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitJugment : MonoBehaviour
{
    public GameObject hitEffect;
    public GameObject itemGet;
    private PlayerController player;


    void Start()
    {
        player = GetComponent<PlayerController>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            player.currentStunTime = PlayerStatus.GetInstance.stunTime;
            GameObject ef = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(ef, 0.8f);
        }
        if (collision.gameObject.tag == "Laser")
        {
            player.currentStunTime = PlayerStatus.GetInstance.stunTime;
            GameObject ef = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(ef, 0.8f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            player.currentStunTime = PlayerStatus.GetInstance.stunTime;
            GameObject ef = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(ef, 0.8f);
        }
        if (collision.tag == "Laser")
        {
            player.currentStunTime = PlayerStatus.GetInstance.stunTime;
            GameObject ef = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(ef, 0.8f);
        }
        if (collision.tag == "Item")
        {

            GameObject ef = Instantiate(itemGet, transform.position, Quaternion.identity) as GameObject;
            Destroy(ef, 0.5f);
        }
        if (collision.tag == "Boss")
        {
            player.currentStunTime = PlayerStatus.GetInstance.stunTime;
            GameObject ef = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(ef, 0.8f);
        }
        if (collision.tag == "BossBody")
        {
            player.currentStunTime = PlayerStatus.GetInstance.stunTime;
            GameObject ef = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(ef, 0.8f);
        }
    }
}

