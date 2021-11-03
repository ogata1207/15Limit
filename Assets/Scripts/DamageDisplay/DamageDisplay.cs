using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageDisplay : MonoBehaviour
{
    public enum DrawType
    {
        Normal,
        TEST01,
        TEST02,
    }

    public Text textObj;
    [Header("ダメージ表示の種類")]
    public DrawType type;
    [Space(1), Header("共通")]
    public float moveSpeed;
    public float DrawTime;

    [Space(2), Header("縮小")]
    public float scalingSpeed;
    public float fontScaleRate;

    // Start is called before the first frame update
    void Start()
    {
       
    }
    public void DrawDamage(Vector3 position, string text)
    {
        transform.position = position;
        textObj.text = text;

        StartCoroutine(Draw());
    }

    IEnumerator Draw()
    {
        var startTime = Time.time;
        var endTime = DrawTime;

        var originalScale = textObj.rectTransform.localScale;

        switch (type)
        {
            case DrawType.Normal:
                while (endTime > Time.time - startTime)
                {
                    textObj.rectTransform.position += new Vector3(0, moveSpeed);
                    yield return null;
                }
                break;

            case DrawType.TEST01:
                textObj.rectTransform.localScale *= fontScaleRate;
                while (endTime > Time.time - startTime)
                {
                    //縮小
                    var x = Mathf.Clamp(textObj.rectTransform.localScale.x - scalingSpeed, originalScale.x, float.MaxValue);
                    var y = Mathf.Clamp(textObj.rectTransform.localScale.y - scalingSpeed, originalScale.y, float.MaxValue);
                    textObj.rectTransform.localScale = new Vector3(x, y);

                    //移動
                    textObj.rectTransform.position += new Vector3(0, moveSpeed);

                    yield return null;
                }
                break;

            case DrawType.TEST02:
                break;
        }
        Destroy(gameObject);
        
    }

}
