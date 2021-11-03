using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss03 : MonoBehaviour
{
    CircleCollider2D cc;
    private float timer;
    private float spedd;
    public Transform deleteTarget;
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(DoShake(duration, magnitude));
    }
    private IEnumerator DoShake(float duration, float magnitude)
    {
        var pos = transform.localPosition;

        var elapsed = 0f;

        while (elapsed < duration)
        {
            var x = pos.x + Random.Range(-1f, 1f) * magnitude;
            var y = pos.y + Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, pos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = pos;
    }

    void TackleMove(Transform target, float speed)
    {
        var vec = target.position - transform.position;
        vec = vec.normalized;
        transform.position += new Vector3(vec.x * speed, vec.y * speed) * Time.deltaTime;
    }

    void Start()
    {
        spedd = 50.0f;
        cc = GetComponent<CircleCollider2D>();
        timer = 0;
    }

    void Update()
    {
        if (Boss03Manager.GetDeadPerformance())
        {
            TimeManager.GetInstance.stopFlg = true;
            Boss03Manager.SetOverlap(true);

            Boss03Manager.SetSpeed(spedd);
            cc.enabled = false;
            timer += Time.deltaTime;
            if (!Boss03Manager.GetDeadPerformanceEnd()) Shake(0.5f, 0.5f);
        }

        if (timer >= 2)
        {
            Boss03Manager.SetOverlap(true);

            Boss03Manager.SetSpeed(spedd);

            Boss03Manager.SetDeadPerformanceEnd(true);
        }
        if (timer >= 3.1f)
        {
            Boss03Manager.SetOverlap(true);
            Boss03Manager.SetSpeed(spedd);
            TackleMove(deleteTarget.transform, spedd);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DeletePos")
        {
            Destroy(gameObject);
        }
    }

}
