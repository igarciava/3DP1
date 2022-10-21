using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDummy : MonoBehaviour
{
    public int TimesShooted = 0;

    // Start is called before the first frame update
    void Start()
    {
        TimesShooted = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimesShooted >= 2)
            Destroy(this.gameObject);
    }

    public void HeadShot()
    {
        TimesShooted += 2;
    }
    public void NormalShot()
    {
        TimesShooted++;
    }
}
