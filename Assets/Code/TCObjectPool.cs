using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCObjectPool
{
    List<GameObject> mObjectsPool;
    int mCurrentElementID = 0;
    public TCObjectPool(int ElementsCount, GameObject Element)
    {
        mObjectsPool = new List<GameObject>();
        for(int i = 0; i < ElementsCount; i++)
        {
            GameObject lElement = GameObject.Instantiate(Element);
            lElement.SetActive(false);
            mObjectsPool.Add(lElement);
        }
        mCurrentElementID = 0;
    }
    public GameObject GetNextElement()
    {
        GameObject lElement = mObjectsPool[mCurrentElementID];
        ++mCurrentElementID;
        if (mCurrentElementID >= mObjectsPool.Count)
            mCurrentElementID = 0;
        return lElement;
    }
}
