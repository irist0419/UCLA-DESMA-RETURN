using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;
using TMPro;
using UnityEngine;

public class NPCcontrol : MonoBehaviour
{
    [SerializeField] private string chatName;
    [SerializeField] private GameObject VCam;

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
        VCam.SetActive(true);
        
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