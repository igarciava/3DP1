using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    FPPlayerController m_Player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Trap")
        {
            m_Player.RestLife();
        }
    }
}