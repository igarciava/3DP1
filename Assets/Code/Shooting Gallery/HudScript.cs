using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudScript : MonoBehaviour
{
    [Header("Points")]
    public Text PointsText;
    public int PointCounter = 0;
    public bool HasEntered;

    [Header("Ammo")]
    public Text AmmoText;
    int CurrentAmmo;
    int MaxAmmo;

    // Start is called before the first frame update
    void Start()
    {
        CurrentAmmo = GameObject.FindGameObjectWithTag("Player").GetComponent<FPPlayerController>().m_CurrentAmmo;
        MaxAmmo = GameObject.FindGameObjectWithTag("Player").GetComponent<FPPlayerController>().m_MaxAmmo;
        ShowAmmo();
    }

    // Update is called once per frame
    void Update()
    {
        CurrentAmmo = GameObject.FindGameObjectWithTag("Player").GetComponent<FPPlayerController>().m_CurrentAmmo;
        MaxAmmo = GameObject.FindGameObjectWithTag("Player").GetComponent<FPPlayerController>().m_MaxAmmo;
        if (HasEntered)
        {
            ActivatePoints();
        }
        else
        {
            DeactivatePoints();
            PointCounter = 0;
        }
        ShowAmmo();
    }

    public void AddPoint(int extrapoint)
    {
        PointCounter += extrapoint;
    }
    

    public void ActivatePoints()
    {
        PointsText.text = "Points: " + PointCounter;
    }

    public void DeactivatePoints()
    {
        PointsText.text = "";
    }

    public void ShowAmmo()
    {
        AmmoText.text = CurrentAmmo + "/" + MaxAmmo;
    }
}
