using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<FossilCollection>() != null)
        {
            other.GetComponent<FossilCollection>().FossilCounter();
        }

        gameObject.SetActive(false);

    }
}

