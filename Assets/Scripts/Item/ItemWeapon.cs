using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWeapon : MonoBehaviour
{
    public GameObject weapon;
    IEnumerator Start()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        var collider = GetComponent<BoxCollider2D>();
        var wait = new OGT_Utility.RealtimeWaitForSeconds(1.0f);
        var startTime = Time.unscaledTime;
        var elapsedTime = Time.unscaledTime;

        collider.enabled = false;
        yield return wait;
        collider.enabled = true;
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag != "Player") return;

        //前回の武器を消す
        FindObjectOfType<PlayerController>().DestroyWeapon();

        //武器の生成
        var obj = Instantiate(weapon, col.transform);
        obj.transform.parent = col.transform;


        //拾ったときのSE
        SoundManager.GetInstance.PlaySE("WeaponChange");

        //コライダーを消す
        Destroy(gameObject);
    }
}
