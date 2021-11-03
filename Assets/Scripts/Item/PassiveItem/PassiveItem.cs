using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPassiveSkill
{
    void UniqueEffect();
    void PassiveStun();
    void PassiveDashCharge();
    void PassiveDash();
}



public class PassiveItem : MonoBehaviour, IPassiveSkill
{
    public PassiveType type;
    public virtual void UniqueEffect() { type = PassiveType.None; }
    public virtual void PassiveStun() { type = PassiveType.Stun; }
    public virtual void PassiveDashCharge() { type = PassiveType.DashCharge; }
    public virtual void PassiveDash() { type = PassiveType.Dash; }

    public enum PassiveType
    {
        Stun,
        Dash,
        DashCharge,
        None
    }

    public void RegisterPassive()
    {
        var player = PlayerStatus.GetInstance;

        switch(type)
        {
            case PassiveType.Stun:
                player.passiveStun += PassiveStun;
                Debug.Log("スタンパッシブに追加");
                break;
            case PassiveType.Dash:
                player.passiveDashCharge += PassiveDashCharge;
                break;
            case PassiveType.DashCharge:
                player.passiveDash += PassiveDash;
                break;
        }
        UniqueEffect();
    }

    //void OnTriggerEnter2D(Collider2D col)
    //{
    //    var player = PlayerStatus.GetInstance;

    //    player.passiveStun       += PassiveStun;
    //    player.passiveDashCharge += PassiveDashCharge;
    //    player.passiveDash       += PassiveDash;

    //    Destroy(gameObject);
    //}


}
