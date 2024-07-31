using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyPad : MonoBehaviour
{
    public GameObject keypadOB;
    public GameObject animateOB;
    public Animator animator;
    public TextMeshProUGUI text;
    public string answer = "1440";

    public AudioSource button;
    public AudioSource correct;
    public AudioSource wrong;

    public bool animate;
    void Start()
    {
        
    }
    
    public void Number(string input)
    {
        text.text += input;
        button.Play();
    }
    
    public void Execute()
    {
        if (text.text == answer)
        {
            correct.Play();
            text.text = "Correct";
            animate = true;
        }
        else
        {
            wrong.Play();
            text.text = "Wrong";
        }
    }

    public void Clear()
    {
        text.text = "";
        button.Play();
    }

    public void Exit()
    {
        keypadOB.SetActive(false);
    }
    void Update()
    {
        if (text.text == "Correct" && animate)
        {
            animator.SetBool("isOpen", true);
            Debug.Log("its open");
        }

        if (keypadOB.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
