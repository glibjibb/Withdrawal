using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int m_Speed;
    public float m_Damage;
    public AudioClip hitSound;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector3 localForward = transform.worldToLocalMatrix.MultiplyVector(transform.forward);
        this.transform.Translate(localForward * Time.deltaTime * m_Speed);
    }

    public void SetSpeed(int speed)
    {
        m_Speed = speed;
    }
    public void SetDamage(float dmg)
    {
        m_Damage = dmg;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(this.tag == "PlayerBullet" && collision.gameObject.tag == "Enemy")
        {
            source.PlayOneShot(hitSound, 0.5f);
            collision.gameObject.GetComponent<Enemy>().Damage(m_Damage);
            collision.gameObject.GetComponent<Enemy>().Knockback(collision.gameObject.transform.position - transform.position, 1);
        }
        else if (this.tag == "EnemyBullet" && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().Damage(m_Damage);
            collision.gameObject.GetComponent<PlayerController>().Knockback(collision.gameObject.transform.position - transform.position, 2);
        }

        this.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Environment")
        {
            this.gameObject.SetActive(false);
        }
    }

}
