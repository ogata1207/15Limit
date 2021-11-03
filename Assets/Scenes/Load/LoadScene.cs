using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneList
{
    Title,
    SampleScene
}

public class LoadScene : MonoBehaviour {

    
    public Slider slider;
    static private bool isUse;

    static private string nextScene;
	// Use this for initialization
	IEnumerator Start () {

        if (isUse) yield break;
        isUse = true;

        var async = SceneManager.LoadSceneAsync(nextScene);
        async.allowSceneActivation = false;

        //ロードが完了するまで待機
        while (true)
        {
            //なんか0.9fまでしかいきません
            slider.value = async.progress;
            if(async.progress >= 0.9f)
            {
                isUse = false;
                async.allowSceneActivation = true;
            }
            yield return null;
        }
        
        
        
    
    }
    static public void RequestScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadScene");
    }

}
