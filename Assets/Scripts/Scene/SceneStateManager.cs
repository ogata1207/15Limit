using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum SceneName
{
    Title,
    Game,
    Enhance,
    PassiveItem,
    GameClear
}

public class SceneStateManager : OGT_Utility.Singleton<SceneStateManager>
{

    //シーン情報
    public SceneName currentSceneName;

    [Header("自動遷移までの時間")]
    public float waitTime;
    //ゴリ押し
    public GameObject playerPrefab;
    public GameObject alarmPrefab;

    public int currentContinueCount;        //コンテニューできる回数
    public GameObject continuePanel;
    //遷移先の管理
    private StateManager stateManager;

    //フェード
    private FadeController fadeController;

    //サウンド関連
    private SoundManager soundManager;

    private EnemyEmitter enemyEmitter;
    private GameObject continueObject;
    //シーン遷移中か
    private bool isLoad;
    private bool isAlarm;

    private Coroutine sceneChange;
    private Coroutine sceneChangeWithWait;

    //時間関連
    private TimeManager timeManager;
    private TimeManagerStatus timeManagerStatus;

    //現在のステージ
    [SerializeField]
    private int nextStageNumber;

    //KBTIT
    public void SetStageNum(int num) { nextStageNumber = num; }
    public int GetStageNum() { return nextStageNumber; }
    private Vector3 pos;
    //KBTIT

    /*****************************************/
    /***/           /*WARNING*/           /***/
                                          /***/
    private bool tokkanKouji;    /***/
    /*****************************************/

    //********************************************************************************************************************
    //
    //
    //********************************************************************************************************************
    // Use this for initialization
    void Start()
    {
        stateManager = new StateManager();
        fadeController = FindObjectOfType<FadeController>();

        //サウンド関連
        soundManager = SoundManager.GetInstance;

        //時間関連
        timeManager = TimeManager.GetInstance;
        timeManagerStatus = TimeManagerStatus.GetInstance;

        enemyEmitter = EnemyEmitter.GetInstance;

        //初期のコンテニュー回数をセット
        currentContinueCount = PlayerStatus.GetInstance.continueCount;

        //シーン遷移後も保持する
        TakeOverToTheNextScene();

        //初期のステート
        //RequestNextScene(sceneName);

        stateManager.RequestState(TitleScene);

        //KBTIT
        pos.x = 0.15f;
        pos.y = 0.68f;
        //KBTIT
    }

    void Update()
    {
        stateManager.Update();
    }

    //********************************************************************************************************************
    //
    //
    //********************************************************************************************************************


    void Initialize()
    {
        if (tokkanKouji) return;
        tokkanKouji = true;
        currentSceneName = SceneName.Game;

        if (nextStageNumber >= 0 && nextStageNumber <= 8)
        {
            soundManager.PlayBGM("MAIN_BGM");
        }
        if (nextStageNumber == 9)
        {
            soundManager.PlayBGM("boss30");
        }
        if (nextStageNumber >= 10 && nextStageNumber <= 18)
        {
            soundManager.PlayBGM("10_19");
        }
        if (nextStageNumber == 19)
        {
            soundManager.PlayBGM("boss30");
        }
        if (nextStageNumber >= 20 && nextStageNumber <= 28)
        {
            soundManager.PlayBGM("21_29");
        }
        if (nextStageNumber == 29)
        {
            soundManager.PlayBGM("boss30");
        }
        if (nextStageNumber == 30)
        {
            soundManager.PlayBGM("boss31");
        }
        //------------------KBTIT---------------------------
        //プレイヤー生成
        //Boss02の時
        //x0.15y0.68
        SetStageNum(nextStageNumber);
        if (nextStageNumber == 19)
        {
            var obj02 = Instantiate(playerPrefab, pos, Quaternion.identity);
        }
        else
        {
            var obj01 = Instantiate(playerPrefab);
        }
        //------------------KBTIT---------------------------

        //敵生成
        enemyEmitter.SpawnEnabled = true;
        enemyEmitter.SpawnEnemy(nextStageNumber);
        Debug.Log("SceneMain Load");

        //タイムセット
        timeManager.SetCountDownTimer(timeManagerStatus.mainTimeLimit);
        timeManager.SetUnscaledTimerEnabled(true);
        timeManager.SetUnscaledTimer(timeManagerStatus.mainWaitTime);

        //
        Destroy(continueObject);
        continueObject = null;

        //ロード完了
        isLoad = false;
    }


    /// <summary>
    /// 各シーンの遷移させる
    /// </summary>
    /// <param name="name">遷移先のシーン名</param>
	public void RequestNextScene(SceneName name)
    {
        //遷移中は終了まで次の遷移を弾く
        if (sceneChange != null) return;

        switch (name)
        {
            case SceneName.Title:
                sceneChange = StartCoroutine(SceneChange("SceneTitle", TitleScene));
                break;
            case SceneName.Game:
                sceneChange = StartCoroutine(SceneChange("SceneMain", GameScene));
                break;
            case SceneName.Enhance:
                sceneChange = StartCoroutine(SceneChange("EnhanceScene", EnhanceScenee));
                break;
            case SceneName.PassiveItem:
                sceneChange = StartCoroutine(SceneChange("PassiveItemScene", PassiveItemScene));
                break;
            case SceneName.GameClear:
                sceneChange = StartCoroutine(SceneChange("GameClear", GameClearScene));
                break;
        }
    }

    //シーンを切り替える
    IEnumerator SceneChange(string nextScene, System.Action<bool> next)
    {
        timeManager.timeScale = 1.0f;
        var wait = fadeController.FadeOut();
        yield return wait;

        //Stateを次のシーンに遷移させる
        stateManager.RequestState(next);
        LoadScene.RequestScene(nextScene);

        while (SceneManager.GetActiveScene().name != nextScene)
        {
            yield return null;
        }
        isLoad = true;
        sceneChange = null;


        isUsing = false;
        sceneChangeWithWait = null;

        //フェードイン開始
        StartCoroutine(fadeController.FadeIn());
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void GameReset()
    {
        //ステージリセット
        nextStageNumber = 0;
        PlayerStatus.GetInstance.ResetLevel();

        //初期のコンテニュー回数をセット
        currentContinueCount = PlayerStatus.GetInstance.continueCount;
        PlayerController.isKnockBack = false;
        PlayerController.currentInvincibleTime = PlayerStatus.GetInstance.invincibleTime;
        if (PlayerController.knockbackObject)
        {
            Destroy(PlayerController.knockbackObject.gameObject);
            PlayerController.knockbackObject = null;

        }
        //強化画面の初期化
        EnhanceScene.Reset();

        //パッシブ獲得画面の初期化
        Passive.bossReward = BossReward.Reward1;



    }

    //********************************************************************************************************************
    //
    //
    //********************************************************************************************************************

    /// <summary>
    /// タイトル
    /// </summary>
    /// <param name="sceneInitialize"></param>
    void TitleScene(bool sceneInitialize)
    {
        if (sceneInitialize)
        {
            //ロード完了
            isLoad = false;

            currentSceneName = SceneName.Title;

            soundManager.StopSubBGM();
            //タイトルBGMを鳴らす
            soundManager.PlayBGM("TITLE_BGM02");

            //初期化
            GameReset();
        }

        //シーン遷移
        if (Input.GetButtonDown("Transition"))
        {
            RequestNextScene(SceneName.Game);
        }
    }

    /// <summary>
    /// メイン
    /// </summary>
    /// <param name="sceneInitialize"></param>
    void GameScene(bool sceneInitialize)
    {

        //まだロード画面
        if (sceneInitialize)
        {
            //BGM再生
            soundManager.PlayBGM("MAIN_BGM");
            isAlarm = false;

            tokkanKouji = false;
        }


        if (SceneManager.GetActiveScene().name == "SceneMain")
        {

            //ステージ準備
            if (isLoad) Initialize();

            //ゲーム開始までの待機時間
            if (timeManager.isCountDown)
            {
                timeManager.timeScale = 0.0f;

                //タイマーが0になるとゲームスタート
                if (timeManager.GetUnscaledTimer() <= 0.0f)
                {

                    //
                    soundManager.PlaySubBGM("Alarm9");

                    //メインのタイマーを開始する
                    timeManager.SetUnscaledTimerEnabled(false);
                    timeManager.StartCountDown();
                    timeManager.timeScale = 1.0f;
                }

                return;
            }
            else if (tokkanKouji && timeManager.GetCountDownTimer() <= 0.0f)
            {//カウントが0になった時
                GameResult.GetInstance.ChangeSprite(GameResult.ResultStatus.GameOver);

                //画面停止
                //timeManager.timeScale = 0.0f;
                timeManager.StopCountDown();


                if (currentContinueCount > 0)
                {
                    if (continueObject == null)
                        continueObject = Instantiate(continuePanel);
                    timeManager.timeScale = 0.0f;
                    //PlayerStatus.GetInstance.continueCount--;
                    //RequestNextScene(SceneName.Game);
                }
                else
                    if (sceneChangeWithWait == null)
                    sceneChangeWithWait = StartCoroutine(RequestTransitionWithWait(SceneName.Title, waitTime));
                ////エンターでタイトルへ戻る
                //if (Input.GetButtonDown("Transition"))
                //{
                //    soundManager.StopSubBGM();
                //    timeManager.timeScale = 1.0f;
                //    RequestNextScene(SceneName.Title);
                //}
            }
            else if (enemyEmitter.isFinish && enemyEmitter.SpawnEnabled == false)
            { //ステージクリアー判定
                if (sceneChangeWithWait != null) return;
                soundManager.StopSubBGM();
                //クリアした時ゲームを停止させる
                //timeManager.timeScale = 0.0f;
                timeManager.StopCountDown();


                var currentStageNumber = nextStageNumber;
                if (sceneChangeWithWait == null)
                {
                    switch (currentStageNumber)
                    {
                        case 9:     //1回目のパッシブ獲得画面
                            sceneChangeWithWait = StartCoroutine(RequestTransitionWithWait(SceneName.PassiveItem, waitTime));
                            //ステージクリアの画像を表示
                            GameResult.GetInstance.ChangeSprite(GameResult.ResultStatus.GameClear);
                            break;

                        case 19:    //2回目のパッシブ獲得画面
                            sceneChangeWithWait = StartCoroutine(RequestTransitionWithWait(SceneName.PassiveItem, waitTime));
                            //ステージクリアの画像を表示
                            GameResult.GetInstance.ChangeSprite(GameResult.ResultStatus.GameClear);
                            break;

                        //case 29:    //ボス第一形態討伐
                        //    sceneChangeWithWait = StartCoroutine(RequestTransitionWithWait(SceneName.Enhance, waitTime));
                        //    break;

                        case 30:    //ボス最終形態討伐

                            sceneChangeWithWait = StartCoroutine(RequestTransitionWithWait(SceneName.GameClear, waitTime));
                            //ステージクリアの画像を表示
                            GameResult.GetInstance.ChangeSprite(GameResult.ResultStatus.GameClear);
                            break;

                        default:    //その他のステージクリア類
                            sceneChangeWithWait = StartCoroutine(RequestTransitionWithWait(SceneName.Enhance, waitTime));
                            //ステージクリアの画像を表示
                            GameResult.GetInstance.ChangeSprite(GameResult.ResultStatus.GameClear);

                            break;
                    }
                }
            }

            if (!isAlarm && timeManager.GetCountDownTimer() <= 3.0f)
            {
                isAlarm = true;
                var obj = Instantiate(alarmPrefab);
            }


        }
    }

    /// <summary>
    /// 強化シーン
    /// </summary>
    /// <param name="sceneInitialize"></param>
    void EnhanceScenee(bool sceneInitialize)
    {
        //soundManager.StopBGM();
        if (sceneInitialize)
        {
            soundManager.PlayBGM("LVUP_BGM");
            isLoad = false;
        }
    }

    void PassiveItemScene(bool sceneInitialize)
    {
        if (sceneInitialize)
        {
            soundManager.PlayBGM("passibu");
            isLoad = false;
        }
    }
    void GameClearScene(bool sceneInitialize)
    {
        //************************************************************************
        // サンプルコード
        //************************************************************************
        // 通常の遷移
        //if (Input.GetButtonDown("Transition")) RequestNextScene(SceneName.Title);
        //自動遷移(シーン名、待機時間)
        if (sceneInitialize)
        {
            soundManager.PlayBGM("sutaffBGM");
            isLoad = false;
        }
        sceneChangeWithWait = StartCoroutine(RequestTransitionWithWait(SceneName.Title, 60.0f));
    }


    //↓気にするな....
    bool isUsing;

    public IEnumerator RequestTransitionWithWait(SceneName nextScene, float time)
    {
        if (isUsing == true) yield break;
        isUsing = true;

        timeManager.timeScale = 1.0f;
        var wait = new WaitForSeconds(time);
        yield return wait;

        if (nextScene == SceneName.Enhance || nextScene == SceneName.PassiveItem)
            nextStageNumber++;

        RequestNextScene(nextScene);


    }

}
