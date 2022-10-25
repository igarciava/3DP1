using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotingGalleryEntry : MonoBehaviour
{
    public HudScript PointsScript;
    public Collider Collider;
    public MeshRenderer TheMesh;

    private void Start()
    {
        TheMesh.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PointsScript.HasEntered = true;
            StartCoroutine(QuitTrigger());
        }
    }

    IEnumerator QuitTrigger()
    {
        yield return new WaitForSeconds(2.0f);
        Collider.isTrigger = false;
        TheMesh.enabled = true;
    }

    public void Restart()
    {
        TheMesh.enabled = false;
        Collider.isTrigger = true;
    }
}
