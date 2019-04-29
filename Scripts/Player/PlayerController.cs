using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int moveSpeed;
    [SerializeField] private float shotDelay;
    [SerializeField] private Transform m_Muzzle;
    [SerializeField] public float m_Health;
    [SerializeField][Range(0.85f,1)] private float m_Accuracy;
    [SerializeField] private int m_BulletSpeed;
    [SerializeField] private int m_BulletDamage;
    [SerializeField] private int m_HealingFactor;
    [SerializeField] private GameObject m_PauseMenu;
    private Transform m_Player;
    private Rigidbody m_PlayerRB;
    private Vector3 m_Move;
    private BulletPooler m_Bullets;
    public float m_CurHealth;
    private bool m_Grounded;
    private bool hasDash;
    private bool hasMG;
    private bool hasSG;
    private bool hasGrenade;
    private bool isPaused;

    private float lastShot;

    public AudioClip shootSound;
    public AudioClip hitSound;
    public AudioClip dieSound;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        m_Player = this.transform;
        m_PlayerRB = this.GetComponent<Rigidbody>();
        m_Move = Vector3.zero;
        m_Health = 100;
        m_CurHealth = m_Health;
        m_Bullets = GameObject.Find("GameController").GetComponent<BulletPooler>();
        m_Grounded = true;
        Random.seed = (int)System.DateTime.Now.Ticks;
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    { 
        if(m_CurHealth <= 0)
        {
            source.PlayOneShot(dieSound, 1);
            Destroy(this.gameObject);
        }

    }

    private void FixedUpdate()
    {
        // read inputs
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");


        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit camRayHit;
        if (Physics.Raycast(camRay, out camRayHit, 30, 1 << LayerMask.NameToLayer("Floor")))
        {
            Vector3 targetPosition = new Vector3(camRayHit.point.x, transform.position.y, camRayHit.point.z);
            transform.LookAt(targetPosition);
        }

        m_Move = v * Vector3.forward + h * Vector3.right;
        Move();

        if(Input.GetMouseButtonDown(0) && Time.time - lastShot > shotDelay)
        {
            Shoot();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                Pause();
            else
                Unpause();
        }
    }

    private void Pause()
    {
        m_PauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void Unpause()
    {
        m_PauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    public void Quit()
    {
        Application.Quit();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            m_Grounded = true;
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            this.gameObject.layer = 0;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            source.PlayOneShot(hitSound, 0.5f);
            Damage(10);
            Knockback(transform.position - collision.gameObject.transform.position,5);
            collision.gameObject.GetComponent<Enemy>().Knockback(collision.gameObject.transform.position - transform.position, 2);
        }
    }

    private void Move()
    {
        if(m_Grounded)
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            m_Player.transform.Translate(m_Move * Time.deltaTime * moveSpeed, Space.World);
        }
    }

    private void Shoot()
    {
        GameObject bullet = m_Bullets.GetPlayerBullet();
        bullet.transform.position = m_Muzzle.transform.TransformPoint(m_Muzzle.transform.localPosition);
        bullet.transform.rotation = this.transform.rotation;
        float displace = Random.Range(-1 * (1 - m_Accuracy), (1 - m_Accuracy)) * 90;
        bullet.transform.Rotate(0, displace, 0);
        bullet.GetComponent<Bullet>().SetSpeed(m_BulletSpeed);
        bullet.GetComponent<Bullet>().SetDamage(m_BulletDamage);
        bullet.SetActive(true);
        lastShot = Time.time;
        source.PlayOneShot(shootSound, 0.3f);
    }

    public void Damage(float dmg)
    {
        m_CurHealth -= dmg;
    }

    public void Heal(float hp)
    {
        if(m_CurHealth < m_Health)
        {
            if (m_CurHealth + (hp * m_HealingFactor) > m_Health)
                m_CurHealth = m_Health;
            else
                m_CurHealth += (hp * m_HealingFactor);
        }
    }

    public void Knockback(Vector3 dir, int amt)
    {
        m_Grounded = false;
        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.velocity = Vector3.Scale(new Vector3(amt,1,amt), (dir.normalized + new Vector3(0, amt, 0)));
        this.gameObject.layer = 9;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Upgrade")
        {
            other.gameObject.GetComponent<Upgrade>().ShowInfo();
            if (Input.GetKeyDown(KeyCode.E))
            {
                other.gameObject.GetComponent<Upgrade>().BuyUpgrade();
            }
        }
        if(other.gameObject.tag == "NextWave")
        {
            other.gameObject.GetComponent<NextWave>().ShowInfo();
            if (Input.GetKeyDown(KeyCode.E))
            {
                other.gameObject.GetComponent<NextWave>().Activate();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Upgrade")
        {
            other.gameObject.GetComponent<Upgrade>().HideInfo();
        }
        if (other.gameObject.tag == "NextWave")
        {
            other.gameObject.GetComponent<NextWave>().HideInfo();
        }
    }

    public void UpgradeHealth(float amt)
    {
        m_Health += amt;
    }
    public void UpgradeHealing(int amt)
    {
        m_HealingFactor += amt;
    }
    public void UpgradeAccuracy(float amt)
    {
        if(m_Accuracy < 1)
        {
            if (m_Accuracy + amt > 1)
                m_Accuracy = 1;
            else
                m_Accuracy += amt;
        }
    }
    public void UpgradeDamage(int amt)
    {
        m_BulletDamage += amt;
    }
    public void UpgradeSpeed(int amt)
    {
        moveSpeed += amt;
    }
    public void UpgradeBulletSpeed(int amt)
    {
        m_BulletSpeed += amt;
    }
    public void UpgradeShotDelay(float amt)
    {
        if (shotDelay > 0)
        {
            if (shotDelay - amt < 0)
                shotDelay = 0;
            else
                shotDelay -= amt;
        }
    }

    public string GetStats()
    {
        string result = "Player Stats\n";
        result += "Max health: " + m_Health;
        result += "\nHealing Factor: " + m_HealingFactor;
        result += "\nSpeed: " + moveSpeed;
        result += "\nAccuracy: " + m_Accuracy;
        result += "\nBullet damage: " + m_BulletDamage;
        result += "\nBullet speed: " + m_BulletSpeed;
        result += "\nShot delay: " + shotDelay;
        return result;
    }
}
