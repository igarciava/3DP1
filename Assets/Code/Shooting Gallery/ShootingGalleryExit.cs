using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingGalleryExit : MonoBehaviour
{
    public HudScript PointsScript;
    int PointsNeeded = 500;
    public MeshRenderer TheMesh;
    public Collider TheCollider;

    private void Start()
    {
        TheMesh.enabled = true;
        TheCollider.isTrigger = false;
    }

    private void Update()
    {
        if(HasScored())
        {
            TheMesh.enabled = false;
            TheCollider.isTrigger = true;
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(EraseHudPoints());
        }
    }
    IEnumerator EraseHudPoints()
    {
        yield return new WaitForSeconds(3.0f);
        PointsScript.HasEntered = false;
    }

    bool HasScored()
    {
        return PointsScript.PointCounter >= PointsNeeded;
    }


    public void Restart()
    {
        TheMesh.enabled = true;
        TheCollider.isTrigger = false;
    }
}
