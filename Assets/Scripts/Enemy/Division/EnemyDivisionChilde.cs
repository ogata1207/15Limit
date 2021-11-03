using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDivisionChilde : MonoBehaviour
{
    private Vector3 pos;
    private Vector3 vec;
    private GameObject player;
    public float speed;

    public GameObject DeadEffet;
    // Start is called before the first frame update
    void Start()
    {
        var enemyGroup = GameObject.FindWithTag("EnemyGroup");
        transform.parent = enemyGroup.transform;
        pos = transform.position;
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        var myHp = this.GetComponent<EnemyHitJudgment>().hp;
        if (myHp <= 0.0f)
        {
            GameObject ef = Instantiate(DeadEffet, transform.position, Quaternion.identity) as GameObject;
            Destroy(ef, 0.3f);
            Destroy(gameObject);
        }
        Tracking();
        DirFixid();
    }
    void Tracking()
    {
        Vector3 velocity = transform.rotation * new Vector3(0, speed, 0);
        transform.position += velocity * Time.deltaTime;
    }

    void DirFixid()
    {
        var vec = (player.transform.position - transform.position).normalized;
        var angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) - 90.0f;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }
}
