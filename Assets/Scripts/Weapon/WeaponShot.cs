using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponShot : MonoBehaviour, IWeapons
{
    public GameObject item;

    //KBTIT
    private GameObject shotPos;
    public GameObject mazleFlash;
    //KBTIT

    public Vector3 shakePower;
    public float shakeTime = 0.01f;

    [SerializeField]
    private GameObject bullet;
    private List<GameObject> bulletList;
    private PlayerStatus status;
    private float interval;
    private ShakeObject shake;

    // Start is called before the first frame update
    void Start()
    {
        bulletList = new List<GameObject>();
        status = PlayerStatus.GetInstance;

        //プレイヤーに装備する
        var player = FindObjectOfType<PlayerController>();
        player.WeaponChange(Idle, Attack);
        status.currentWeapons = Weapons.Shot;
        shake = Camera.main.gameObject.GetComponent<ShakeObject>();

        shotPos = GameObject.Find("PlayerShotPos");
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void Attack(PlayerInfo player)
    {
        interval += Time.deltaTime;
        Debug.Log("AttackSpeed : " + (status.shotInterval - ((status.attackSpeedLevelRate * status.currentAttackSpeedLevel) * 0.1f)));
        if (interval > (status.shotInterval - ((status.attackSpeedLevelRate * status.currentAttackSpeedLevel) * 0.1f)))
        {
            shake.PlayShake(shakePower, shakeTime);
            SoundManager.GetInstance.PlaySE("EnemyShot");

            //弾生成
            var newBullet = Instantiate(bullet, shotPos.transform.position, transform.rotation);
            newBullet.name = "Player Bullet";

            var mazle = Instantiate(mazleFlash, shotPos.transform.position, transform.rotation);
            //Destroy(mazleFlash, 0.1f);

            //リストに追加
            bulletList.Add(newBullet);

            //リセット
            interval = 0.0f;
        }
    }

    public void Idle(PlayerInfo player)
    {

    }

    public void DestroyObject()
    {
        var obj = Instantiate(item, transform.position, Quaternion.identity);

        Debug.Log("Destroy Shot");
        Destroy(gameObject);
    }

}
