using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LiveGameStatsMenu : MonoBehaviour
{

    public GameObject liveGameStatsMenu;
    public TextMeshProUGUI heartsText;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++) {
            heartsText.text += "<sprite=1>";
        }    
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
