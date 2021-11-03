using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EffectPlayer : MonoBehaviour
{
    public List<Sprite> sprites;
    public float animationWait;

    private SpriteRenderer spriteRenderer;
    private IEnumerator animation;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
        StartAnimation(true);
    }

    public void StartAnimation(bool isLoop)
    {
        //アニメーションを停止させる
        Stop();

        //アニメーション開始
        animation = AnimationLoop(isLoop);
        Debug.Log("animation : " + animation);
        StartCoroutine(animation);

    }

    public void Stop()
    {
        if (animation != null) 
        StopCoroutine(animation);
    }

    IEnumerator AnimationLoop(bool isLoop)
    {
        //1コマ当たりの感覚(アニメーションスピード)
        var wait = new WaitForSeconds(animationWait);
        //コマの番号
        int index = 0;
        //コマの最後
        int last = sprites.Count-1;
        Debug.Log("Anim Count :" + last);
        while (isLoop)
        {
            //次のスプライトの番号
            int next = index + 1;
            
            //一番後ろまでいったら0に戻す
            index = (next > last) ? 0 : next;

            //スプライトの変更
            spriteRenderer.sprite = sprites[index];

            yield return wait;
        }
    }
}
