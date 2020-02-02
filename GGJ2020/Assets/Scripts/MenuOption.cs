using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOption : MonoBehaviour
{
    public GameObject joint;
    public float force;
    public int id; //1. Jugar 2. Musica 3. Creditos 4. Salir

    private void OnMouseDown()
    {
        if(id==1 || id==4) joint.transform.position += Vector3.up * force * Time.deltaTime;
        if(id==2 || id==3) GetComponent<Rigidbody2D>().AddForce(Vector3.down * 50, ForceMode2D.Impulse);
        else GetComponent<Rigidbody2D>().AddForce(Vector3.down * 200, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (id){
            case 1:
                //Load scene jugar
                break;
            case 2:
                //Cambiar musica
                break;
            case 3:
                //Mostrar créditos
                break;
            default:
                Application.Quit();
                break;
        }
    }
}
