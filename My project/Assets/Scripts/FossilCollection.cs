using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FossilCollection : MonoBehaviour
{
    public TMP_Text scoreText;
    int fossilCount = 0;

    public void FossilCounter()
    {
        fossilCount++;
        scoreText.text = fossilCount.ToString();
    }
}
    