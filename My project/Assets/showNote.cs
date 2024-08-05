using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script : MonoBehaviour
{
    public GameObject UiGameObject;

    // Start is called before the first frame update
    void Start()
    {
        UiGameObject.SetActive(false);
    }
    // Update is called once per frame
    void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            UiGameObject.SetActive(true);

        }
    }

    void OnTriggerExit(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            UiGameObject.SetActive(false);

        }
    }
}