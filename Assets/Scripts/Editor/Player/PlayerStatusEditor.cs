using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerStatusEditor : EditorWindow
{
    static public Vector2 windowSize = new Vector2(450, 500);
    public PlayerStatus status;

    public int toolBar = 0;

    private Vector2 scrollPosition;

    private SerializedObject serializeObject;
    private SerializedProperty weaponProperty;
    private int currentIndex;

    [MenuItem(itemName:"OGT設定/プレイヤー")]
    static void Create()
    {
        var window = GetWindow<PlayerStatusEditor>("プレイヤー設定");
        window.minSize = windowSize;
        window.maxSize = windowSize;
    }

    private void OnGUI()
    {
        if(status == null)
        {
            status = PlayerStatus.GetInstance;

            serializeObject = new SerializedObject(status);
            weaponProperty = serializeObject.FindProperty("weaponStatus");
        }

        toolBar = GUILayout.Toolbar(toolBar, new string[] { "基本ステータス", "レベルアップ毎の上昇値", "武器設定" });
        switch(toolBar)
        {
            case 0:
                Status();
                break;
            case 1:
                Rate();
                break;
            case 2:
                Weapons();
                break;
        }
         
        SaveButton();
    }

    private void Status()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUI.skin.box);
            {
                //*******************************************************************************************

                EditorGUILayout.LabelField("[基本ステータス]");
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.LabelField("移動スピード");
                    status.moveSpeed = EditorGUILayout.FloatField("", status.moveSpeed);
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("攻撃力");
                    status.attackPower = EditorGUILayout.FloatField("", status.attackPower);
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("攻撃速度");
                    status.attackSpeed = EditorGUILayout.FloatField("", status.attackSpeed);
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("コンテニュー回数");
                    status.continueCount = EditorGUILayout.IntField("", status.continueCount);
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("ダッシュチャージ時間");
                    status.dashChargeTime = EditorGUILayout.FloatField("", status.dashChargeTime);
                    EditorGUILayout.Space();

                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();

                //*******************************************************************************************

                EditorGUILayout.LabelField("[クリティカル]");
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.LabelField("クリティカル率");
                    status.critical = EditorGUILayout.FloatField("", status.critical);
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("クリティカル判定時の攻撃力[倍率]");
                    status.criticalDamageRate = EditorGUILayout.FloatField("", status.criticalDamageRate);
                    EditorGUILayout.Space();
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();

                //*******************************************************************************************

                EditorGUILayout.LabelField("[被ダメージ]");
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.LabelField("スタン時間");
                    status.stunTime = EditorGUILayout.FloatField("", status.stunTime);
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("無敵時間");
                    status.invincibleTime = EditorGUILayout.FloatField("", status.invincibleTime);
                    EditorGUILayout.Space();
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();

                //*******************************************************************************************

                EditorGUILayout.LabelField("[ショット]");
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.LabelField("ショットの間隔");
                    status.shotInterval = EditorGUILayout.FloatField("", status.shotInterval);
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("弾のスピード");
                    status.shotSpeed = EditorGUILayout.FloatField("", status.shotSpeed);
                    EditorGUILayout.Space();
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();

                //*******************************************************************************************
            }
            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndVertical();
    }

    private void Rate()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            EditorGUILayout.LabelField("1レベル毎の攻撃力の上昇値[倍率]");
            status.attackLevelRate = EditorGUILayout.FloatField("", status.attackLevelRate);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("1レベル毎の攻撃速度の上昇値[倍率]");
            status.attackSpeedLevelRate = EditorGUILayout.FloatField("", status.attackSpeedLevelRate);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("1レベル毎のクリティカル率の上昇値[倍率]");
            status.criticalLevelRate = EditorGUILayout.FloatField("", status.criticalLevelRate);
            EditorGUILayout.Space();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            EditorGUILayout.LabelField("攻撃力の最大レベル");
            status.MaxAttackPowerLevel = EditorGUILayout.IntField("", status.MaxAttackPowerLevel);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("攻撃速度の最大レベル");
            status.MaxAttackSpeedLevel = EditorGUILayout.IntField("", status.MaxAttackSpeedLevel);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("クリティカルの最大レベル");
            status.MaxCriticalLevel = EditorGUILayout.IntField("", status.MaxCriticalLevel);
            EditorGUILayout.Space();
        }
        EditorGUILayout.EndVertical();

    }

    private void Weapons()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            weaponProperty.arraySize = EditorGUILayout.IntField("MaxSize", weaponProperty.arraySize);

            //スライドでインデックスを決定
            currentIndex = EditorGUILayout.IntSlider(currentIndex, 0, weaponProperty.arraySize - 1);

            //スライドで指定した配列の中身を抽出
            SerializedProperty selectTileStatus = weaponProperty.GetArrayElementAtIndex(currentIndex);

            //抽出した配列の中身を表示
            EditorGUILayout.PropertyField(selectTileStatus, true);

            //プロパティの更新
            serializeObject.ApplyModifiedProperties();
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
                EditorUtility.SetDirty(status);
                AssetDatabase.SaveAssets();
            }
        }
        EditorGUILayout.EndVertical();
    }
}
