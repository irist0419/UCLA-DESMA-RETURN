using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;
using TMPro;
using UnityEngine;

public class NPCcontrol : MonoBehaviour
{
    [SerializeField] private string chatName;

    private bool canTalk = false;

    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        canTalk = true;
        Say();
    }
    
    private void OnTriggerExit(Collider other)
    {
        canTalk = false;
    }


    void Say()
    {
        if (canTalk)
        {
            Flowchart flowChart = GameObject.Find("Flowchart").GetComponent<Flowchart>();
            if (flowChart.HasBlock(chatName))
            {
                flowChart.ExecuteBlock(chatName);
            }
                
        }
    }    
}