using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Piece : MonoBehaviour
{
    public enum pieceType { leg_l, leg_r, arm_l, arm_r, head };
    public Color[] colors = { Color.red, Color.green, Color.blue, Color.white, Color.black };

    public Color[] _colors;
    public int _numberConectors;
    public pieceType _type;
    
    
    Piece(int numberConectors)
    {
        //random colors
        for(int i=0;i< _numberConectors; i++)
        {
            _colors[i] = colors[Random.Range(0,colors.Length)];
        }

        //random piece type
        switch (Random.Range(0, 5))
        {
            case 0:
                _type = pieceType.leg_l;
                break;

            case 1:
                _type = pieceType.leg_r;
                break;

            case 2:
                _type = pieceType.arm_l;
                break;

            case 3:
                _type = pieceType.arm_r;
                break;

            case 4:
                _type = pieceType.head;
                break;
        }
    }


    
}
