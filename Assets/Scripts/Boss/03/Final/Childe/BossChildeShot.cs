using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChildeShot : MonoBehaviour
{
    public GameObject bullet;
    [Range(1.0f, 15.0f)]
    public float shotSpeed;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!Boss03Manager.GetDeadBefor())
        {
            #region ロックオン
            var vec = (player.transform.position - transform.position).normalized;
            var angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) - 90.0f;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
            #endregion
            if (FindObjectOfType<BossFinal>().oneShotFlg)
            {
                SoundManager.GetInstance.PlaySE("EnemyShot");
                GameObject runcherBullet = GameObject.Instantiate(bullet) as GameObject;
                runcherBullet.GetComponent<Rigidbody2D>().velocity = transform.up.normalized * shotSpeed;
                runcherBullet.transform.position = transform.position;
            }
        }
    }
}
