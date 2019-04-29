using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameflow : MonoBehaviour
{
    public int m_Wave;
    private PlayerController m_PlayerController;
    private int m_EnemiesLeft;
    private bool m_DoneSpawning;
    private bool m_ShopPhase;
    [SerializeField] private GameObject m_StatsPanel;
    private Text m_StatsText;
    [SerializeField] private Text m_WaveText;
    [SerializeField] private GameObject m_UpgradeSpawns;
    [SerializeField] private GameObject m_UpgradePrefab;
    [SerializeField] private GameObject m_NextWave;

    // Start is called before the first frame update
    void Start()
    {
        m_Wave = 1;
        m_StatsText = m_StatsPanel.GetComponentInChildren<Text>();
        m_StatsPanel.SetActive(false);
        m_PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        if(m_DoneSpawning)
        {
            m_EnemiesLeft = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if(m_EnemiesLeft <= 0 && !m_ShopPhase)
            {
                ShopPhase();
                m_DoneSpawning = false;
            }
        }
        if (m_ShopPhase)
        {
            m_WaveText.text = "Shop phase";
            m_StatsText.text = m_PlayerController.GetStats();
        }
        else
            m_WaveText.text = "Wave " + m_Wave;
    }

    public void DoneSpawning()
    {
        m_DoneSpawning = true;
    }

    public void NextWave()
    {
        m_ShopPhase = false;
        m_DoneSpawning = false;
        m_Wave++;
        this.GetComponent<EnemySpawner>().NextWave();
        foreach (GameObject upg in GameObject.FindGameObjectsWithTag("Upgrade"))
        {
            GameObject.Destroy(upg);
        }
        m_StatsPanel.SetActive(false);
        m_NextWave.SetActive(false);
    }

    private void ShopPhase()
    {
        m_ShopPhase = true;
        m_StatsPanel.SetActive(true);
        SpawnUpgrades();
        m_NextWave.SetActive(true);
    }

    private void SpawnUpgrades()
    {
        foreach(Transform t in m_UpgradeSpawns.transform)
        {
            float minAmt = 0;
            float maxAmt = 0;
            Upgrade.upgradeType randomType = (Upgrade.upgradeType)UnityEngine.Random.Range(0, 6);
            switch (randomType)
            {
                case Upgrade.upgradeType.Health:
                    minAmt = 10;
                    maxAmt = 50;
                    break;
                case Upgrade.upgradeType.Healing:
                    minAmt = 2;
                    maxAmt = 3;
                    break;
                case Upgrade.upgradeType.Damage:
                    minAmt = 1;
                    maxAmt = 5;
                    break;
                case Upgrade.upgradeType.Speed:
                    minAmt = 0.1f;
                    maxAmt = 1f;
                    break;
                case Upgrade.upgradeType.Accuracy:
                    minAmt = 0.01f;
                    maxAmt = 0.1f;
                    break;
                case Upgrade.upgradeType.BulletSpeed:
                    minAmt = 0.1f;
                    maxAmt = 1f;
                    break;
                case Upgrade.upgradeType.ShotDelay:
                    minAmt = 0.01f;
                    maxAmt = 0.05f;
                    break;
                default:
                    return;
            }
            float amount = UnityEngine.Random.Range(minAmt, maxAmt);
            amount = (float)Math.Round(amount, 3) * (1 + m_Wave/10);
            float cost = (float) Math.Round(25 * (1 + m_Wave / 10) * (amount / maxAmt), 3);
            GameObject upg = GameObject.Instantiate(m_UpgradePrefab);
            Upgrade upgradeScript = upg.GetComponent<Upgrade>();
            upgradeScript.upgrade = randomType;
            upgradeScript.amount = amount;
            upgradeScript.cost = cost;
            upg.transform.position = t.position;
        }
    }
}
