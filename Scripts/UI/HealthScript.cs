using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    private PlayerController pc;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Text hpText;
    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        hpSlider.value = pc.m_CurHealth;
        hpSlider.maxValue = pc.m_Health;
        hpText.text = System.Math.Truncate( pc.m_CurHealth ) + "/" + System.Math.Truncate(pc.m_Health);
    }
}
