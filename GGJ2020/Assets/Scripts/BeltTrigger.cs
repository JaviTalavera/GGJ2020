using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltTrigger : MonoBehaviour
{
    public GameObject initPos;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Belt"))
        {
            if (other.TryGetComponent<Belt>(out var belt))
            {
                belt.transform.position = new Vector3( belt._otherBelt.transform.position.x - 20, belt.transform.position.y, belt.transform.position.z);
            }
            other.gameObject.GetComponent<Belt>().Refresh();
        }
    }

}
