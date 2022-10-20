using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingGalleryExit : MonoBehaviour
{
    public HudPoints PointsScript;
    int PointsNeeded = 500;
    public MeshRenderer TheMesh;
    public Collider TheCollider;

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

    bool HasScored()
    {
        return PointsScript.PointCounter >= PointsNeeded;
    }

    IEnumerator EraseHudPoints()
    {
        yield return new WaitForSeconds(3.0f);
        PointsScript.HasEntered = false;
    }
}
