using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_EnemyPrefabs;
    [SerializeField] private float[,] m_EnemyChances;
    [SerializeField] private float m_SpawnDelay;
    public int m_NumSpawn;
    private GameObject[] m_Spawns;
    public int m_Wave;
    private int m_MaxPerWave;
    public bool m_Spawning;
    private float SpawnTimer;
    private float timer;
    private int numSpawned;

    // Start is called before the first frame update
    void Start()
    {
        m_Spawns = GameObject.FindGameObjectsWithTag("Spawner");
        int max = 20 * m_Wave;
        int min = max / 2;
        m_MaxPerWave = Random.Range(min, max);
        m_Spawning = true;
        InitializeWeights();
        Random.seed = (int)System.DateTime.Now.Ticks;
    }

    private void InitializeWeights()
    {
        m_EnemyChances = new float[,]{
            { 0.7f, 0.2f, 0.05f, 0.05f},    // wave 1
            { 0.55f, 0.25f, 0.1f, 0.1f },   // 2
            { 0.35f, 0.25f, 0.2f, 0.2f },   // 3
            { 0.2f, 0.3f, 0.3f, 0.2f },   // 4
            { 0.1f , 0.3f, 0.3f, 0.3f },   // 5   
            { 0.35f, 0.25f, 0.2f, 0.2f },   // 6
            { 0, 0.1f, 0.5f, 0.4f },   // 7
            { 0.1f, 0.2f, 0.2f, 0.6f },   // 8
            { 0.1f, 0.3f, 0.3f, 0.3f },   // 9
            { 0.1f, 0,0,0},   // 10
        };
    }

    // Update is called once per frame
    void Update()
    {
        int max = 20 * m_Wave;
        int min = max / 2;
        m_MaxPerWave = Random.Range(min, max);
        SpawnTimer += Time.deltaTime;
        if (SpawnTimer > m_SpawnDelay)
        {
            m_Spawning = !m_Spawning;
            SpawnTimer = 0;
        }
        if (m_Spawning && numSpawned < m_MaxPerWave)
        {
            Spawn();
        }
        else if (numSpawned >= m_MaxPerWave)
        {
            this.GetComponent<Gameflow>().DoneSpawning();
        }
    }

    private void Spawn()
    {
        timer += Time.deltaTime;
        if(timer > m_SpawnDelay / m_NumSpawn)
        {
            timer = 0;
            int spawnPt = Random.Range(0, m_Spawns.Length - 1);
            float randWeight = Random.value;
            float min = 0;
            float max = m_EnemyChances[m_Wave - 1, 0];
            for(int i = 0; i < m_EnemyChances.GetLength(1); i++)
            {
                if(randWeight > min && randWeight < max && numSpawned < m_MaxPerWave)
                {
                    GameObject enemy = GameObject.Instantiate(m_EnemyPrefabs[i]);
                    enemy.transform.position = m_Spawns[spawnPt].transform.position;
                    numSpawned++;
                    break;
                }
                else
                {
                    min = max;
                    max = (i + 1 == m_EnemyChances.GetLength(1) - 1 ? 1 : min + m_EnemyChances[m_Wave - 1, i + 1]);
                }
            }
        }
    }

    public void NextWave()
    {
        m_Wave++;
        numSpawned = 0;
    }
}
