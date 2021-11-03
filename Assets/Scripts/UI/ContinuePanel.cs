using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinuePanel : MonoBehaviour
{
    public Text[] text;
    public Text continueCount;
    public float cursorMoveInterval;
    private int currentIndex;
    private float moveInterval;
    private SoundManager soundManager;
    // Start is called before the first frame update
    void Start()
    {
        continueCount.text = ""+SceneStateManager.GetInstance.currentContinueCount;
        soundManager = SoundManager.GetInstance;
    }

    // Update is called once per frame
    void Update()
    {
        moveInterval -= Time.deltaTime;

        if (Input.GetButtonDown("Transition")) Select();

        if (moveInterval > 0) return;

        var x = Input.GetAxisRaw("Horizontal");

        if (x > 0.3f)
        {
            ChangeTarget(currentIndex + 1);
            moveInterval = cursorMoveInterval;
            soundManager.PlaySE("SE_CursorMove");
        }
        else if (x < -0.3f)
        {
            ChangeTarget(currentIndex - 1);
            moveInterval = cursorMoveInterval;
            soundManager.PlaySE("SE_CursorMove");
        }
    }
    private void Select()
    {
        if(currentIndex == 1)
        {
            SceneStateManager.GetInstance.RequestNextScene(SceneName.Title);
            return;
        }

        SceneStateManager.GetInstance.currentContinueCount--;
        SceneStateManager.GetInstance.RequestNextScene(SceneName.Game);
    }
    private void ChangeTarget(int index)
    {
        

        //配列内に収める
        index = Mathf.Clamp(index, 0, text.Length - 1);

        if (index != currentIndex)
        {
            text[currentIndex].color = Color.white;
            text[index].color = Color.red;

            //更新
            currentIndex = index;
        }
    }
}
