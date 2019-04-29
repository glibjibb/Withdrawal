using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunemy : Enemy
{
    [SerializeField] private Transform m_Muzzle;
    [SerializeField] private int m_Shots;
    private float timer;
    private float shotTimer;
    public AudioClip shootSound;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        timer = 0;
        shotTimer = 0;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        if (m_Target == null)
            return;
        timer += Time.deltaTime;
        if (timer > m_ShotDelay)
        {
            m_IsShooting = !m_IsShooting;
            timer = 0;
        }
        if (m_IsShooting)
        {
            AimAndShoot();
        }
        else
        {
            agent.speed = m_Speed;
            Move();
        }
    }

    private void AimAndShoot()
    {
        shotTimer += Time.deltaTime;

        agent.speed = m_Speed / 2;
        Move();

        if(shotTimer > m_ShotDelay / m_Shots)
        {
            if(Vector3.Distance(m_Target.position, this.transform.position) < 20)
                Shoot();
            shotTimer = 0;
        }

    }

    private void Shoot()
    {
        source.PlayOneShot(shootSound, 0.2f);
        GameObject bullet = m_Bullets.GetEnemyBullet();
        bullet.SetActive(true);
        bullet.transform.position = m_Muzzle.transform.TransformPoint(m_Muzzle.transform.localPosition);
        bullet.transform.rotation = this.transform.rotation;
        bullet.GetComponent<Bullet>().SetDamage(m_ShotDamage);
    }
}
