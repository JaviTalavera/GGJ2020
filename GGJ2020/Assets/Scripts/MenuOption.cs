using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOption : MonoBehaviour
{
    public GameObject joint;
    public GameObject credits;
    public float force;
    public int id; //1. Jugar 2. Musica 3. Creditos 4. Salir
    public AudioSource _audioSource;

    private void OnMouseDown()
    {
        if(id==1 || id==4) joint.transform.position += Vector3.up * force * Time.deltaTime;
        if(id==2 || id==3) GetComponent<Rigidbody2D>().AddForce(Vector3.down * 50, ForceMode2D.Impulse);
        else GetComponent<Rigidbody2D>().AddForce(Vector3.down * 200, ForceMode2D.Impulse);

        if(id==2)
        {
            //Musica ===> Julen, te toca a ti!
            _audioSource.mute = !_audioSource.mute;
        }

        if (id ==3)
        {
            //Mostramos canvas
            credits.SetActive(!credits.activeSelf);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (id){
            case 1:
                SceneManager.LoadScene("1_Game");
                print("Hola");
                break;
            default:
                Application.Quit();
                break;
        }
    }
}
