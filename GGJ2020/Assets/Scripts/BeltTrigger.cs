using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltTrigger : MonoBehaviour
{
    public GameObject initPos;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.TryGetComponent<Belt>(out var belt))
        {
            belt.transform.position = new Vector3( 
                belt._otherBelt.transform.position.x - 20, 
                belt.transform.position.y, 
                belt.transform.position.z);
            belt.Refresh();
        }
    }

}
