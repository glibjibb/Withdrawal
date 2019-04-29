using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextWave : MonoBehaviour
{
    [SerializeField] private TextMesh infoText;
    // Start is called before the first frame update
    void Start()
    {
        infoText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowInfo()
    {
        infoText.text = "Begin next wave";
    }

    public void HideInfo()
    {
        infoText.text = "";
    }

    public void Activate()
    {
        GameObject.Find("GameController").GetComponent<Gameflow>().NextWave();
    }
}
