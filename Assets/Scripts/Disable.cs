using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("DisableItem"))
        {
            other.transform.parent.gameObject.SetActive(false);
        }
    }
}
