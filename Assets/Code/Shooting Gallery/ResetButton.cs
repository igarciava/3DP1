using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour
{
    public bool Reset = false;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Reset = true;
        }
        else
        {
            Reset = false;
        }
    }
}
