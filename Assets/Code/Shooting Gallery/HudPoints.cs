using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudPoints : MonoBehaviour
{
    public int PointCounter = 0;
    public Text Text;
    public bool HasEntered;

    // Start is called before the first frame update
    void Start()
    {
                
    }

    // Update is called once per frame
    void Update()
    {
        if(HasEntered)
        {
            ActivatePoints();
        }
        else
        {
            DeactivatePoints();
            PointCounter = 0;
        }
    }

    public void AddPoint(int extrapoint)
    {
        PointCounter += extrapoint;
    }
    

    public void ActivatePoints()
    {
        Text.text = "Points: " + PointCounter;
    }

    public void DeactivatePoints()
    {
        Text.text = "";
    }
}
