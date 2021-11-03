using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public class WeaponStatus
{
    public string name;
    public float power;
    public float attackSpeed;
}

public enum Weapons
{
    Hammer = 0,
    Shot = 1
}

[CreateAssetMenu(fileName = "PlayerStatus", menuName = "OGT/Status Table/PlayerStatus")]
public class PlayerStatus : ScriptableObject
{
    [SerializeField, Header("Resourcesフォルダに置く")]
    public static string FILE_PATH = "Status/PlayerStatus";

    #region Instance
    private static PlayerStatus instance;
    public static PlayerStatus GetInstance
    {
        get
        {
            if (instance == null)
            {
                var table = (PlayerStatus)Resources.Load(FILE_PATH);
                if (table == null) Debug.LogError("指定のパスにTileStatusTableが存在ません");
                else instance = table;
            }
            return instance;
        }
    }

    #endregion

    /*
        基本ステータス
    */
    public float moveSpeed;             //移動速度
    public float attackPower;           //攻撃力
    public float attackSpeed;           //攻撃速度
    public float critical;              //攻撃力
    public float criticalDamageRate;    //クリティカル判定が出たときのダメージの倍率
    public float stunTime;              //巣短時間
    public float invincibleTime;        //無敵時間
    public float shotSpeed;             //ショットのスピード
    public float shotInterval;
    public float dashChargeTime;

    /*
        現在のレベル
    */
    public int currentAttackPowerLevel = 1;
    public int currentAttackSpeedLevel = 1;
    public int currentCriticalLevel = 1;

    /*
        最大レベル ( TODO )
    */
    public int MaxAttackPowerLevel;
    public int MaxAttackSpeedLevel;
    public int MaxCriticalLevel;

    /*
        1レベルあたりの倍率
    */
    public float attackLevelRate;
    public float attackSpeedLevelRate;
    public float criticalLevelRate;

    /*
        パッシブ関連
    */
    public Action passiveStun;
    public Action passiveDashCharge;
    public Action passiveDash;

    /*
        武器のステータス
    */
    public Weapons currentWeapons; 
    [SerializeField]
    public WeaponStatus[] weaponStatus;


    /*
        その他
    */
    public int continueCount;   //コンテニューができる回数

    //-------------------KBTIT---------------------
    //クリティカル率
    public bool CriticalRate(float percent)
    {
        var probabilityRate = UnityEngine.Random.value * 100.0f;
        return (probabilityRate < percent) ? true : false;
        //else return false;
    }
    //使用方法
    //:例
    //50%の確率でクリティカル
    //if(CriticalRate(50)){攻撃力*=1.5f}
    //--------------------KBTIT---------------------

    public float AttackDamage()
    {
        /* 2019/12/05 編集 */
 
        //クリティカル率計算
        var critRate = critical + (criticalLevelRate * currentCriticalLevel);

        //クリティカル判定
        var rate = (CriticalRate(critRate)) ? criticalDamageRate : 1.0f;

        //攻撃力の計算
        //基本攻撃力
        var baseAttackPower = attackPower * weaponStatus[(int)currentWeapons].power;

        //レベルに比例した倍率
        var lvlRate = (currentAttackPowerLevel > 0) ? currentAttackPowerLevel * attackLevelRate : 1 ;

        //最終結果
        var damage = baseAttackPower * lvlRate * rate;
        Debug.Log("Weapon[" +weaponStatus[(int)currentWeapons].name +  "] BaseAttackPower[" + baseAttackPower + "] LevelRate[" + lvlRate + "]");
        return damage;
    }

    public void ResetLevel()
    {
        currentAttackPowerLevel   = 0;
        currentAttackSpeedLevel   = 0;
        currentCriticalLevel      = 0;

        passiveStun = null;
        passiveDash = null;
        passiveDashCharge = null;
    }

    /// <summary>
    /// レベルが最大かどうかを確認する
    /// </summary>
    public bool CheckIfLevelIsAtMaximum()
    {
        if (MaxAttackPowerLevel != currentAttackPowerLevel) return false;
        if (MaxAttackSpeedLevel != currentAttackSpeedLevel) return false;
        if (MaxCriticalLevel != currentCriticalLevel) return false;

        return true;
    }
}
