using UnityEngine;
using System.Collections.Generic;
using System;


namespace OGT_Utility
{
    /// <summary>
    /// 起動時のアプリケーション設定
    /// </summary>
    namespace RuntimeInitialize
    {

        public class ApplicationManager : MonoBehaviour
        {
            /// <summary>
            /// 起動時に実行されるメソッド
            /// </summary>
            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
            static private void Init()
            {
                //マネージャー系の生成
                var obj = (GameObject)Resources.Load("Prefabs/Manager");
                var manager = Instantiate(obj);
                manager.name = "Manager";

                //プレイヤーの初期化
                PlayerStatus.GetInstance.ResetLevel();
            }

        }
    }

    //拡張メソッド
    static public class Extentions
    {
        #region 2次元配列用のループ
        //WithIndex用の構造体
        public struct WithIndexStruct<T>
        {
            //配列の中身
            public T element { get; }
            //縦
            public int x { get; }
            //横
            public int y { get; }

            //同アセンブリ内専用のコンストラクタ(このスクリプトのみアクセス可能)
            internal WithIndexStruct(T element, int x, int y)
            {
                this.element = element;
                this.x = x;
                this.y = y;
            }
        }

        //2次元配列用拡張メソッド(ループ用)
        public static IEnumerable<WithIndexStruct<T>> WithIndex<T>(this T[,] self)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            //Xの要素の数だけループ
            for (int x = 0; x < self.GetLength(0); x++)
            {
                //Yの要素の数だけループ
                for (int y = 0; y < self.GetLength(1); y++)
                {
                    yield return new WithIndexStruct<T>(self[x, y], x, y);
                }
            }
        }
        #endregion
    }


    public class RealtimeWaitForSeconds : CustomYieldInstruction
    {
        private float waitTime;

        public override bool keepWaiting
        {
            get
            {
                return (Time.realtimeSinceStartup < waitTime);
            }
        }

        public RealtimeWaitForSeconds(float time)
        {
            waitTime = Time.realtimeSinceStartup + time;
        }


    }
    //SingletonBehavior
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
   {
        private void Awake()
        {
            //既に別のインスタンスが存在する場合削除する
            if (_instance != null) Destroy(gameObject);
            _instance = FindObjectOfType<T>();
        }

        static volatile T _instance;

        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <returns></returns>
        /// 参考
        /// https://qiita.com/kajitack/items/4b0175755b0cc47d4f6e
        /// https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/keywords/volatile
        static public T GetInstance
        {
            get
            {
                //if (_instance == null)
                //{
                //    //Tが存在するか探す
                //    var instance = FindObjectOfType<T>();

                //    //Tが存在していない
                //    if (instance == null)
                //    {
                //        Debug.LogError("存在していない");
                //    }
                //    else
                //    {
                //        _instance = instance;
                //    }

                //}
                return _instance;
            }
        }

        //次のシーンに引き継ぐ時はこれを呼ぶ
        protected void TakeOverToTheNextScene()
        {
            DontDestroyOnLoad(gameObject);
        }
   }
}