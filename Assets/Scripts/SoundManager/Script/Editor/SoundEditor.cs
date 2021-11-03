using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SoundEditor : EditorWindow {

    static Vector2 windowSize = new Vector2(450, 500);

    public SoundStatus status;
    public SerializedObject statusObject;
    public SerializedProperty soundList;
    public int toolBar = 0;

    public int size = 100;
    public Vector2 listScrollPosition = Vector2.zero;

    [MenuItem(itemName: "OGT設定/サウンド")]
    static void Create()
    {

        var window = GetWindow<SoundEditor>("サウンドの設定");

        //サイズ固定
        window.minSize = windowSize;
        window.maxSize = windowSize;
    }

    private void OnGUI()
    {
        //初期化
        if(status == null)
        {
            status = SoundStatus.GetInstance;
            statusObject = new SerializedObject(status);

            //BGMとSEのリストを更新する
            status.LoadFile();
             
        }

        toolBar = GUILayout.Toolbar(toolBar, new string[] { "SEリスト", "BGMリスト", "初期音量" });
        switch (toolBar)
        {
            case 0:
                ViewSoundList(status.seList);
                break;
            case 1:
                ViewSoundList(status.bgmList);
                break;
            case 2:
                SoundVolume();
                break;
        }


    }

    private void SoundVolume()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            EditorGUILayout.LabelField("マスター音量");
            status.masterVolume = EditorGUILayout.FloatField("音量:", status.masterVolume);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("BGM音量");
            status.bgmVolume = EditorGUILayout.FloatField("音量:", status.bgmVolume);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("SE音量");
            status.seVolume = EditorGUILayout.FloatField("音量:", status.seVolume);

        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            //TileStatuTableを保存する
            if (GUILayout.Button("保存"))
            {
                //セーブ
                EditorUtility.SetDirty(status);
                AssetDatabase.SaveAssets();
            }
        }
        EditorGUILayout.EndVertical();
    }
    private void ViewSoundList(Dictionary<string,AudioClip> list)
    {
        EditorGUILayout.BeginVertical();
        {
            //スクロール開始
            listScrollPosition = EditorGUILayout.BeginScrollView(listScrollPosition, GUI.skin.box);
            {
                // スクロール範囲
                foreach(KeyValuePair<string, AudioClip>kvp in list)
                {
                    EditorGUILayout.SelectableLabel(kvp.Key);
                }
            }

            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndVertical();
    }


}
