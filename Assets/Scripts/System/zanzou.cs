using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class zanzou : MonoBehaviour {

    //設定
    public float 残像の間隔 = 0.2f;
    public float 消えるまでの時間 = 0.2f;
    public float 透明度 = 0.5f;
    public Color color;

    //残像リスト
    List<GameObject> trailParts = new List<GameObject>();
    private GameObject trailParent;
    void Start()
    {
        trailParent = new GameObject();
        trailParent.name = "TrailParent";
        InvokeRepeating("SpawnTrailPart", 0, 0.001f); // replace 0.2f with needed repeatRate
    }

    void SpawnTrailPart()
    {
       
        GameObject trailPart = new GameObject();
        SpriteRenderer trailPartRenderer = trailPart.AddComponent<SpriteRenderer>();
        trailPart.transform.parent =trailParent. transform;
        trailPartRenderer.sprite = GetComponent<SpriteRenderer>().sprite;
        trailPartRenderer.color = color;
        trailPart.transform.position =transform.position;
        trailPart.transform.localScale = transform.localScale;
        trailParts.Add(trailPart);

        StartCoroutine(FadeTrailPart(trailPartRenderer));
        StartCoroutine(DestroyTrailPart(trailPart, 消えるまでの時間)); // replace 0.5f with needed lifeTime
        
    }

    IEnumerator FadeTrailPart(SpriteRenderer trailPartRenderer)
    {
        Color color = trailPartRenderer.color;
        color.a -= 透明度; // replace 0.5f with needed alpha decrement
        trailPartRenderer.color = color;

        yield return new WaitForEndOfFrame();
    }

    IEnumerator DestroyTrailPart(GameObject trailPart, float delay)
    {
        yield return new WaitForSeconds(delay);

        trailParts.Remove(trailPart);
        Destroy(trailPart);
    }

    void Flip()
    {
        //facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        FlipTrail();
    }

    void FlipTrail()
    {
        foreach (GameObject trailPart in trailParts)
        {
            Vector3 trailPartLocalScale = trailPart.transform.localScale;
            trailPartLocalScale.x *= -1;
            trailPart.transform.localScale = trailPartLocalScale;
        }
    }
}
