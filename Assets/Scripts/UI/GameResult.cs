using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  仮
/// </summary>
public class GameResult : OGT_Utility.Singleton<GameResult>
{
    public enum ResultStatus
    {
        GameClear,
        GameOver
    }
    public Sprite gameClearSprite;
    public GameObject clearObj;
    public bool clearFlg;

    public Sprite gameOverSprite;

    //private bool overFlg;
    //public GameObject gameOverObj;

    private SpriteRenderer spriteRenderer;

    private bool spriteEnabled
    {
        get
        {
            return spriteRenderer.enabled;
        }
        set
        {
            spriteRenderer.enabled = value; 
        }
    }
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = null;
        clearFlg = false;
        //overFlg = false;
    }
    public void ChangeSprite(ResultStatus state)
    {
        switch (state)
        {
            case ResultStatus.GameClear:
                if(!clearFlg) Instantiate(clearObj);
                clearFlg = true;
                //spriteRenderer.sprite = gameClearSprite;
                break;
            case ResultStatus.GameOver:
                //if(!overFlg)Instantiate(gameOverObj);
                //overFlg = true;
                spriteRenderer.sprite = gameOverSprite;
                break;
        }
    }
}
