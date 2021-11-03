using System;
using UnityEngine;


public class StateManager
{
    Action<bool> nextState;
    Action<bool> currentState;
    
    public float startTime;         //ステートに移った時点の時間
    public float elapsedTime;       //現在のステートの経過時間

    public void RequestState(Action<bool> next)
    {
        nextState = next;
    }

    public void Update()
    {
        if( nextState != null )
        {
            //現在のステートを破棄する
            currentState = null;

            //リクエストされたステートに移行
            currentState = nextState;

            //ステイトの初期化
            currentState(true);

            //経過時間の初期化
            startTime = Time.time;

            //リクエストを破棄する
            nextState = null;
        }
        else
        {
            //更新

            elapsedTime = Time.time - startTime;
            currentState(false);
        }
    }
}
    