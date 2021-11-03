using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> objectList;
    private GameObject poolObj;

    public void CreatePool(string key, GameObject obj, int size)
    {
        objectList = new List<GameObject>();

        //指定された数オブジェクトをプールする
        for (int i = 0; i < size; i++)
        {
            ;

            var newObj = CreateObject();
            //リストに入れる
            objectList.Add(newObj);

        }
    }

    public GameObject Use(string key)
    {
        GameObject retObj;

        //使用していないオブジェクトを取得して返す
        foreach(var obj in objectList)
        {
            if (obj.activeSelf == false) return obj; 
        }

        //使用できるオブジェクトがなければ新しく追加する
        var newObj = CreateObject();
        return newObj;

    }

    public GameObject CreateObject()
    {
        //インスタンス生成
        var newObj = Instantiate(poolObj);
        newObj.name = poolObj.name + objectList.Count;

        //まとめる
        newObj.transform.parent = transform;

        //非アクティブにする
        newObj.SetActive(false);

        return newObj;
    }



}
