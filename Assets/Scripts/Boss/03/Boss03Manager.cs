using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Boss03Manager
{
    static float hp;

    static int form;         //1:1形態 2:2形態
    static int pattern;      //0:A 1:B 2:C 3:D

    //1:1回タックル 2:2回タックル 3:3回タックル 4:1回タックル(強)
    //5:3回タックル(強) 6:4方向ショット1回 7:4方向ショット2回
    //8:円形ショット 

    //9:8方向ショット 10:タックルしながらショット 
    static int mode;

    static bool tackleNow;
    static int tackleCount;
    static int tacklePos;   //1:左上 2:右上 3:右下 4:左下
    static bool tackleLine;
    static float speed;
    static bool overlap; //子供を重ねるFlg
    static bool centerCol;

    static bool deadPerformance;
    static bool deadPerformanceEnd;

    static bool deadBefor;
    //デンジャーの位置 1:下 2:上 3真ん中
    static int denPos;

    static bool childeDead;

    static bool childeDead01;
    static bool childeDead02;
    static bool childeDead03;
    static bool childeDead04;

    static bool realDead;
    static bool realDeadEnd;

    // Start is called before the first frame update
    static public void Start()
    {
        form = 0;
        mode = 0;
        tackleNow = false;
        tackleCount = 0;
        tacklePos = 0;
        tackleLine = false;
        overlap = true;
        centerCol = false;

        deadPerformance = false;
        deadPerformanceEnd = false;
        denPos = 0;

        deadBefor = false;
        childeDead = false;

        childeDead01 =false;
        childeDead02 =false;
        childeDead03 =false;
        childeDead04 = false;

        realDead = false;
        realDeadEnd = false;
    }
    static public void Init()
    {
        tackleNow = false;
        tackleCount = 0;
        tacklePos = 0;
        tackleLine = false;
        centerCol = false;
    }
    //static public float GetHp() { return hp; }
    //static public void SetHp(float num) { hp = num; }

    static public int GetForm() { return form; }
    static public void SetForm(int state) { form = state; }

    static public int GetPattern() { return pattern; }
    static public void SetPattern(int state) { pattern = state; }

    static public int GetMode() { return mode; }
    static public void SetMode(int state) { mode = state; }

    static public bool GetTackleNow() {return tackleNow; }
    static public void SetTackleNow(bool f) { tackleNow = f; }

    static public int GetTackleCount() { return tackleCount; }
    static public void SetTackleCount(int count) { tackleCount = count; }

    static public int GetTacklePos() { return tacklePos; }
    static public void SetTacklePos(int pos) { tacklePos = pos; }

    static public bool GetTackleLine() { return tackleLine; }
    static public void SetTackleLine(bool f) { tackleLine = f; }

    static public float GetSpeed() { return speed; }
    static public void SetSpeed(float num) { speed = num; }

    static public bool GetOverlap() { return overlap; }
    static public void SetOverlap(bool f) { overlap = f; }

    static public bool GetCenterCol() { return centerCol; }
    static public void SetCenterCol(bool f) { centerCol = f; }

    static public bool GetDeadPerformance() { return deadPerformance; }
    static public void SetDeadPerformance(bool f) { deadPerformance = f; }

    static public int GetDenPos() { return denPos; }
    static public void SetDenPos(int state) { denPos = state; }

    static public bool GetDeadPerformanceEnd() { return deadPerformanceEnd; }
    static public void SetDeadPerformanceEnd(bool f) { deadPerformanceEnd = f; }

    static public bool GetDeadBefor() { return deadBefor; }
    static public void SetDeadBefor(bool f) { deadBefor = f; }

    static public bool GetChildeDead() { return childeDead; }
    static public void SetChildeDead(bool f) { childeDead = f; }

    static public bool GetChildeDead01() { return childeDead01; }
    static public void SetChildeDead01(bool f) { childeDead01 = f; }
    static public bool GetChildeDead02() { return childeDead02; }
    static public void SetChildeDead02(bool f) { childeDead02 = f; }
    static public bool GetChildeDead03() { return childeDead03; }
    static public void SetChildeDead03(bool f) { childeDead03 = f; }
    static public bool GetChildeDead04() { return childeDead04; }
    static public void SetChildeDead04(bool f) { childeDead04 = f; }

    static public bool GetRealDead() { return realDead; }
    static public void SetRealDead(bool f) { realDead = f; }

    static public bool GetRealDeadEnd() { return realDeadEnd; }
    static public void SetRealDeadEnd(bool f) { realDeadEnd = f; }
}
