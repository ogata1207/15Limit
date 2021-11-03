using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceScene : MonoBehaviour
{
    enum EnhanceStatus
    {
        Critical,
        AttackPower,
        AttackSpeed,
        None = -1
    }

    static public int[] enhanceProgress = new int[sizeof(EnhanceStatus) - 1];

    [Header("(残りの時間 / enhanceRate) = 強化回数")]
    public int enhanceRate;

    [Space]
    public GameObject[] panel;
    [Header("Size3 [0.クリティカル 1.攻撃 2.攻撃速度]")]
    public GameObject[] maxPanel;
    public GameObject cursor;
    public float cursorMoveInterval;

    public int maxProgress;
    public Slider[] slider = new Slider[3];
    public Text text;

    public float spriteSizeRate;

    private int enhanceCount;
    private int targetPanel;
    private float moveInterval;


    private TimeManager timeManager;
    private PlayerStatus playerStatus;
    private SoundManager soundManager;
    private PlayerLevelUp playerLevelUp;

    public int GetenhanceCount() { return enhanceCount; }

    static public void Reset()
    {
        enhanceProgress = new int[sizeof(EnhanceStatus) - 1];
    }

    // Start is called before the first frame update
    void Start()
    {
        soundManager = SoundManager.GetInstance;
        playerStatus = PlayerStatus.GetInstance;
        timeManager = TimeManager.GetInstance;

        var time = timeManager.GetCountDownTimer();
        enhanceCount = Mathf.CeilToInt(Mathf.CeilToInt(timeManager.GetCountDownTimer()) / enhanceRate);
        text.text = enhanceCount.ToString();
        foreach (var p in maxPanel)
        {
            p.SetActive(false);
        }
        var size = sizeof(EnhanceStatus) - 1;
        for (int i = 0; i < size; i++)
        {
            slider[i].maxValue = maxProgress;
            slider[i].value = enhanceProgress[i];
        }


        timeManager.timeScale = 1.0f;

        playerLevelUp = FindObjectOfType<PlayerLevelUp>();
    }

    void Update()
    {

        //遷移
        if (playerStatus.CheckIfLevelIsAtMaximum())
        {
            StartCoroutine(RequestTransitionWithWait(SceneName.Game, 3.0f));
        }

        if (!CheckIfLevelIsMaximum(EnhanceStatus.Critical)) maxPanel[0].SetActive(true);
        if (!CheckIfLevelIsMaximum(EnhanceStatus.AttackPower)) maxPanel[1].SetActive(true);
        if (!CheckIfLevelIsMaximum(EnhanceStatus.AttackSpeed)) maxPanel[2].SetActive(true);

        if (enhanceCount <= 0)
        {
            SceneStateManager.GetInstance.RequestNextScene(SceneName.Game);
        }

        {   //ここ全部消す
            text.text = enhanceCount.ToString();
        }


        //指定されているステータスを強化する
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.C)) enhanceCount = 999;
        if (Input.GetButtonDown("Fire2")) Enhance(targetPanel);
#else
        if (Input.GetButtonDown("Fire2")) Enhance(targetPanel);
#endif

        //カーソル移動
        cursor.transform.position = Vector3.Lerp(cursor.transform.position, panel[targetPanel].transform.position, 0.5f);

        //連続で移動するのを防ぐ
        moveInterval -= Time.deltaTime;

        if (moveInterval > 0) return;

        //傾きに応じてカーソルを移動させる
        var y = Input.GetAxisRaw("Vertical");
        if (y > 0.3f)
        {
            targetPanel = Mathf.Clamp(targetPanel - 1, 0, 2);
            moveInterval = cursorMoveInterval;
            soundManager.PlaySE("SE_CursorMove");
        }
        else if (y < -0.3f)
        {
            targetPanel = Mathf.Clamp(targetPanel + 1, 0, 2);
            moveInterval = cursorMoveInterval;
            soundManager.PlaySE("SE_CursorMove");
        }

    }

    void Enhance(EnhanceStatus target)
    {
        //強化回数が0の場合ここで弾く
        if (enhanceCount <= 0) return;
        if (!CheckIfLevelIsMaximum(target)) return;
        soundManager.PlaySE("PowerUp");
        playerLevelUp.LevelUp();

        enhanceProgress[(int)target]++;
        slider[(int)target].value = enhanceProgress[(int)target];

        if (slider[(int)target].value == slider[(int)target].maxValue)
        {
            LevelUp(target);
        }

        enhanceCount--;
    }

    void Enhance(int number)
    {
        EnhanceStatus target = EnhanceStatus.None;

        switch (number)
        {
            case 0:
                target = EnhanceStatus.Critical;
                break;
            case 1:
                target = EnhanceStatus.AttackPower;
                break;
            case 2:
                target = EnhanceStatus.AttackSpeed;
                break;
        }

        Enhance(target);
    }
    void LevelUp(EnhanceStatus target)
    {
        switch (target)
        {
            case EnhanceStatus.Critical:
                playerStatus.currentCriticalLevel++;

                break;
            case EnhanceStatus.AttackPower:
                playerStatus.currentAttackPowerLevel++;
                break;
            case EnhanceStatus.AttackSpeed:
                playerStatus.currentAttackSpeedLevel++;
                break;
        }

        enhanceProgress[(int)target] = 0;
        slider[(int)target].value = 0.0f;

    }

    bool CheckIfLevelIsMaximum(EnhanceStatus target)
    {
        var retval = false;

        switch (target)
        {
            case EnhanceStatus.AttackPower:
                retval = (playerStatus.currentAttackPowerLevel < playerStatus.MaxAttackPowerLevel) ? true : false;
                break;
            case EnhanceStatus.AttackSpeed:
                retval = (playerStatus.currentAttackSpeedLevel < playerStatus.MaxAttackSpeedLevel) ? true : false;
                break;
            case EnhanceStatus.Critical:
                retval = (playerStatus.currentCriticalLevel < playerStatus.MaxCriticalLevel) ? true : false;
                break;
        }

        return retval;
    }

    public IEnumerator RequestTransitionWithWait(SceneName nextScene, float time)
    {
        timeManager.timeScale = 1.0f;
        var wait = new WaitForSeconds(time);
        yield return wait;
        SceneStateManager.GetInstance.RequestNextScene(nextScene);

    }
}
