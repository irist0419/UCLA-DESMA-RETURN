using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class inspect : MonoBehaviour
{
    public GameObject UiGameObject;
    public CinemachineFreeLook freeLook;
    
    
    // Start is called before the first frame update
    void Start()
    {
        UiGameObject.SetActive(false);
    }
    // Update is called once per frame
    void OnTriggerEnter(Collider other) {
        Debug.Log("Collision True");
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player True");
            UiGameObject.SetActive(true);
            freeLook.enabled = false;
     
        }
    }
 
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            UiGameObject.SetActive(false);
            freeLook.enabled = true;
        }
    }
    
}
