using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGallery : MonoBehaviour
{
    public ShotingGalleryEntry GalleryEntryScript;
    public ShootingGalleryExit GalleryExitScript;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GalleryEntryScript.Restart();
            GalleryExitScript.Restart();
        }
    }
}
