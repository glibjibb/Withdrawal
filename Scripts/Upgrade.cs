using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public enum upgradeType
    {
        Health,
        Healing,
        Accuracy,
        Speed,
        BulletSpeed,
        Damage,
        ShotDelay
    };
    public upgradeType upgrade;
    public float amount;
    public float cost;
    private PlayerController player;
    [SerializeField] private TextMesh infoText;
    public AudioClip upgradeSound;
    public AudioClip failSound;
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        infoText.text = "";
        source = player.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowInfo()
    {
        switch (upgrade)
        {
            case upgradeType.Health:
                infoText.text = "Max Health\n Increase your max health";
                infoText.text += "\nAmount: +" + amount + "\nCost: -" + cost + " HP";
                break;
            case upgradeType.Healing:
                infoText.text = "Healing Factor\n Increase amount healed on kills";
                infoText.text += "\nAmount: " + amount + "\nCost: -" + cost + " HP";
                break;
            case upgradeType.Damage:
                infoText.text = "Damage\n Increase bullet damage";
                infoText.text += "\nAmount: +" + amount + "\nCost: -" + cost + " HP";
                break;
            case upgradeType.Speed:
                infoText.text = "Speed\n Increase move speed";
                infoText.text += "\nAmount: +" + amount + "\nCost: -" + cost + " HP";
                break;
            case upgradeType.Accuracy:
                infoText.text = "Accuracy\n Increase Accuracy";
                infoText.text += "\nAmount: +" + amount * 100 + "%\nCost: -" + cost + " HP";
                break;
            case upgradeType.BulletSpeed:
                infoText.text = "Bullet speed\n Increase bullet speed";
                infoText.text += "\nAmount: +" + amount + "\nCost: -" + cost + " HP";
                break;
            case upgradeType.ShotDelay:
                infoText.text = "Shot delay\n Decrease shot delay";
                infoText.text += "\nAmount: -" + amount + "s\nCost: -" + cost + " HP"; 
                break;
            default:
                return;
        }
    }

    public void HideInfo()
    {
        infoText.text = "";
    }

    public void BuyUpgrade()
    {
        if (cost > player.m_CurHealth)
        {
            source.PlayOneShot(failSound, 0.6f);
            return;
        }
        source.PlayOneShot(upgradeSound, 0.6f);
        player.Damage(cost);
        switch(upgrade)
        {
            case upgradeType.Health:
                player.UpgradeHealth(amount);
                break;
            case upgradeType.Healing:
                player.UpgradeHealing((int)amount);
                break;
            case upgradeType.Damage:
                player.UpgradeDamage((int)amount);
                break;
            case upgradeType.Speed:
                player.UpgradeSpeed((int)amount);
                break;
            case upgradeType.Accuracy:
                player.UpgradeAccuracy(amount);
                break;
            case upgradeType.BulletSpeed:
                player.UpgradeBulletSpeed((int)amount);
                break;
            case upgradeType.ShotDelay:
                player.UpgradeShotDelay(amount);
                break;
            default:
                return;
        }
        GameObject.Destroy(this.gameObject);
    }
}
