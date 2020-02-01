using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    static int points;
    static float time = 0;
    GameObject piece; //No idea what this is

    //Colors 
    static int _NCOLORS = 5;
    static Color []_colors = { Color.red, Color.green, Color.blue, Color.white, Color.black};

    public enum pieceType { ARM_L, ARM_R, LEG_L, LEG_R, HEAD};


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }

    public static Color GetColor()
    {
        return _colors[Random.Range(0, _NCOLORS)];
    }
}
