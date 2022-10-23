using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGallery : MonoBehaviour
{
    public GameObject TheGalleryPrefab;
    public ResetButton ResetButton;
    Transform GalleryPosition;

    // Start is called before the first frame update
    void Start()
    {
        GalleryPosition = TheGalleryPrefab.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsReset())
        {
            Destroy(TheGalleryPrefab);
            Instantiate(TheGalleryPrefab, GalleryPosition);
        }
    }

    bool IsReset()
    {
        return ResetButton.Reset;
    }

}
