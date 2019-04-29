using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float m_Health;
    [SerializeField] protected float m_Speed;
    [SerializeField] protected float m_ShotDelay;
    [SerializeField] protected float m_ShotDamage;
    [SerializeField] protected float m_AimDistance;
    [SerializeField] protected float m_HealAmount;
    protected Transform m_Target;
    protected bool m_IsShooting;
    protected BulletPooler m_Bullets;
    protected NavMeshAgent agent;
    public AudioClip hitSound;
    public AudioClip dieSound;
    protected AudioSource source;
    // Start is called before the first frame update
    protected void Start()
    {
        m_Target = GameObject.FindGameObjectWithTag("Player").transform;
        m_IsShooting = false;
        m_Bullets = GameObject.Find("GameController").GetComponent<BulletPooler>();
        agent = GetComponent<NavMeshAgent>();
        agent.destination = m_Target.position;
        agent.speed = m_Speed;
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (m_Health <= 0)
        {
            source.PlayOneShot(dieSound, 0.6f);
            m_Target.gameObject.GetComponent<PlayerController>().Heal(m_HealAmount);
            Destroy(this.gameObject);
        }
    }

    protected void Move()
    {
        //Vector3 moveDir = m_Target.transform.position - this.transform.position;
        //moveDir = moveDir.normalized;
        //transform.Translate(moveDir * Time.deltaTime * m_Speed, Space.World);
        agent.destination = m_Target.position;
        float distFromTarg = Vector3.Distance(transform.position, m_Target.position);
        if (distFromTarg < m_AimDistance)
        {
            agent.destination = (transform.position - m_Target.position).normalized * distFromTarg + transform.position;
        }
        transform.LookAt(m_Target);
    }


    public void Damage(float dmg)
    {
        source.PlayOneShot(hitSound, 0.5f);
        m_Health -= dmg;
    }

    public void Knockback(Vector3 dir, int amount)
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.velocity = Vector3.Scale(new Vector3(amount, 1, amount), dir.normalized);
        agent.velocity = this.GetComponent<Rigidbody>().velocity;
    }
}
