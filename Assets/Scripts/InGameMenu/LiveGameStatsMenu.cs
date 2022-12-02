using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LiveGameStatsMenu : MonoBehaviour
{

    public GameObject liveGameStatsMenu;
    public TextMeshProUGUI heartsText;

    
    void Start()
    {
        for (int i = 0; i < 10; i++) {
            heartsText.text += "<sprite=1>";
        }    
    }

    void Update()
    {
        
    }



}
