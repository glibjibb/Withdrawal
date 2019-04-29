using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooler : MonoBehaviour
{
    [SerializeField] private GameObject PlayerBullet;
    [SerializeField] private GameObject EnemyBullet;
    [SerializeField] private int NumPlayerBullets;
    [SerializeField] private int NumEnemyBullets;
    public List<GameObject> PBulletPool;
    public List<GameObject> EBulletPool;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < NumPlayerBullets; i++)
        {
            GameObject pb = GameObject.Instantiate(PlayerBullet);
            pb.SetActive(false);
            PBulletPool.Add(pb);
        }
        for (int i = 0; i < NumEnemyBullets; i++)
        {
            GameObject eb = GameObject.Instantiate(EnemyBullet);
            eb.SetActive(false);
            EBulletPool.Add(eb);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetPlayerBullet()
    {
        for(int i = 0; i < NumPlayerBullets; i++)
        {
            if (!PBulletPool[i].activeInHierarchy)
                return PBulletPool[i];
        }
        return null;
    }
    public GameObject GetEnemyBullet()
    {
        for (int i = 0; i < NumEnemyBullets; i++)
        {
            if (!EBulletPool[i].activeInHierarchy)
                return EBulletPool[i];
        }
        return null;
    }

}
