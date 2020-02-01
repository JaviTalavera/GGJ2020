using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belt : MonoBehaviour
{
    public static float speed;
    public GameObject[] spawners;
    
    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * speed;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Change pieces
        // push de las piezas que tenemos
        // pop de las nuevas piezas
        //Spawn -> Setear la posicion de las piezas
    }
}
