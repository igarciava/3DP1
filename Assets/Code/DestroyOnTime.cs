using System.Collections;
using UnityEngine;
public class DestroyOnTime : MonoBehaviour
{
    public float m_DestroyOnTime = 3.0f;

    private void Start()
    {
        StartCoroutine(DestroyOnTimeFn());
    }
    IEnumerator DestroyOnTimeFn()
    {
        yield return new WaitForSeconds(m_DestroyOnTime);
        GameObject.Destroy(gameObject);
    }
}
