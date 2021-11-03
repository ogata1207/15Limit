using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RewardGroup
{
    public RewardArray[] reward = new RewardArray[3];
}

public enum BossReward
{
    Reward1,
    Reward2,
    Reward3
}

[System.Serializable]
public class RewardArray
{
    public GameObject prefab;
    public string manual;
}
public class Passive : MonoBehaviour
{
    public static BossReward bossReward;

    public float cursorMoveInterval;

    [SerializeField, Header("報酬の設定")]
    public RewardGroup[] rewards;

    public Text manualText;

    private SoundManager soundManager;
    private float moveInterval;
    private  GameObject[] frameArray;

    public int currentIndex;                //選択しているアイコン
    private SpriteRenderer[] frameSprite;   //
    private bool isSelect;

    // Start is called before the first frame update
    void Start()
    {
        soundManager = SoundManager.GetInstance;

        frameArray = new GameObject[3];

        for(int index = 0; index < rewards[(int)bossReward].reward.Length; index++)
        {
            frameArray[index] = Instantiate(rewards[(int)bossReward].reward[index].prefab);
        }

        frameSprite = new SpriteRenderer[frameArray.Length];
        for(int index = 0; index < frameArray.Length; index++)
        {
            frameSprite[index] = frameArray[index].GetComponent<SpriteRenderer>();
        }
        currentIndex = 2;
        ChangeTarget(0);
    }

    // Update is called once per frame
    void Update()
    {
       //カーソルの移動制限
        moveInterval -= Time.deltaTime;
        if (moveInterval > 0) return;

        //すでに選んでいる状態なら無視する
        if (isSelect) return;
        

        if (Input.GetButtonDown("Transition")) SelectPassiveItem();

        //傾きに応じてカーソルを移動させる
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

    private void ChangeTarget(int index)
    {
        //配列内に収める
        index = Mathf.Clamp(index, 0, frameSprite.Length - 1);
        
        if (index != currentIndex)
        {
            //選択されていたフレームの色を白に戻す
            frameSprite[currentIndex].color = Color.white;

            //選択されたフレームを赤にする
            frameSprite[index].color = Color.red;

            //説明文の更新
            manualText.text = rewards[(int)bossReward].reward[index].manual;

            //更新
            currentIndex = index;
        }
    }

    private void SelectPassiveItem()
    {
        isSelect = true;
        var passiveItem = frameSprite[currentIndex].GetComponent<PassiveItem>();
        passiveItem.RegisterPassive();
        bossReward++;
        SceneStateManager.GetInstance.RequestNextScene(SceneName.Enhance);
    }
}
