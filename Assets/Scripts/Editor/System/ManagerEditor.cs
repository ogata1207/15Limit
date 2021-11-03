using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ManagerEditor : EditorWindow
{
    static public Vector2 windowSize = new Vector2(450, 500);

    public TimeManagerStatus timeStatus;
    public StageStatus stageStatus;

    public int toolBar = 0;
    public int currentIndex = 0;

    public SerializedProperty stages;
    public SerializedObject stageObject;

    private Vector2 scrollPosition;

    [MenuItem(itemName: "OGT設定/ゲーム関連")]
    static void Create()
    {
        var window = GetWindow<ManagerEditor>("ゲーム設定");
        window.minSize = windowSize;
        window.maxSize = windowSize;
    }

    private void OnGUI()
    {
        if (timeStatus == null)
        {
            timeStatus = TimeManagerStatus.GetInstance;
            stageStatus = StageStatus.GetInstance;

            stageObject = new SerializedObject(stageStatus);
            stages = stageObject.FindProperty("stages");
        }

        toolBar = GUILayout.Toolbar(toolBar, new string[] { "ステージ管理", "時間設定", "武器設定" });
        switch (toolBar)
        {
            case 0:
                StageSetting();
                break;
            case 1:
                TimerSetting();
                break;
            case 2:
                break;
        }

        SaveButton();
    }

    private void StageSetting()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            EditorGUILayout.LabelField("ステージの管理");

            //    //構造体の最大数の設定
            stages.arraySize = EditorGUILayout.IntField("MaxSize", stages.arraySize);
            if (stages.arraySize != 0)
            {
#if false
                //スライドでインデックスを決定
                currentIndex = EditorGUILayout.IntSlider(currentIndex, 0, stages.arraySize - 1);

                //スライドで指定した配列の中身を抽出
                SerializedProperty selectTileStatus = stages.GetArrayElementAtIndex(currentIndex);

                //抽出した配列の中身を表示
                EditorGUILayout.PropertyField(selectTileStatus, true);

                //プロパティの更新
                stageObject.ApplyModifiedProperties();
#else
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUI.skin.box);
                {
                    for (int i = 0; i < stages.arraySize; i++)
                    {
                        EditorGUILayout.BeginVertical(GUI.skin.box);
                        {
                            EditorGUILayout.LabelField("[" + "ステージ" + (i + 1) + "]");
                            //スライドで指定した配列の中身を抽出
                            SerializedProperty selectTileStatus = stages.GetArrayElementAtIndex(i);

                            //抽出した配列の中身を表示
                            EditorGUILayout.PropertyField(selectTileStatus, true);
                        }
                        EditorGUILayout.EndVertical();
                    }
                }
                EditorGUILayout.EndScrollView();
#endif
            }

            //プロパティの更新
            stageObject.ApplyModifiedProperties();


        }
        EditorGUILayout.EndVertical();
    }

    private void TimerSetting()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            EditorGUILayout.LabelField("遷移後からゲームスタートまでの間の時間");
            timeStatus.mainWaitTime = EditorGUILayout.FloatField("[秒]", timeStatus.mainWaitTime);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("ゲームオーバーまでのタイムリミット(一応)");
            timeStatus.mainTimeLimit = EditorGUILayout.FloatField("[秒]", timeStatus.mainTimeLimit);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("フェードイン");
            timeStatus.fadeInTime = EditorGUILayout.FloatField("[秒]", timeStatus.fadeInTime);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("フェードアウト");
            timeStatus.fadeOuntTime = EditorGUILayout.FloatField("[秒]", timeStatus.fadeOuntTime);
            EditorGUILayout.Space();

        }
        EditorGUILayout.EndVertical();
    }
    private void SaveButton()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            //TileStatuTableを保存する
            if (GUILayout.Button("保存"))
            {
                //セーブ
                EditorUtility.SetDirty(timeStatus);
                AssetDatabase.SaveAssets();
            }
        }
        EditorGUILayout.EndVertical();
    }
}
