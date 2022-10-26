using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    GameObject Player;
    float DoorSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(Player.GetComponent<FPPlayerController>().HasAKey)
        {
            gameObject.transform.position += Vector3.right * DoorSpeed * Time.deltaTime;
            StartCoroutine(RetrieveKey());
        }
    }

    IEnumerator RetrieveKey()
    {
        yield return new WaitForSeconds(1.0f);
        Player.GetComponent<FPPlayerController>().HasAKey = false;
    }
}
