using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelePit : MonoBehaviour
{
    public Transform destination;
    public GameObject Player;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("PlayerRB"))
            Player.transform.position = destination.position;
    }
}
