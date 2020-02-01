using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Piece : MonoBehaviour
{
    //General Piece information
    public enum pieceType { leg_l, leg_r, arm_l, arm_r, head };
    private Color[] colors = { Color.red, Color.green, Color.blue, Color.white, Color.black };

    //Piece's info
    public pieceType _type;

    //Conectors' info
    private int _numberConectors;
    private int _maxConectors = 3;
    public Color[] _colors;
    
    //Conector reference to intantiate conector objects (the things that came out of the piece)
    [SerializeField] public GameObject _conectorPrefab;

    void Start()
    {
        //No action is required here.
    }

    
    //Method to initialize Robot's pieces
    public void Initialize(int type)
    {
        //Initialize piece type
        switch (type)
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

        //Initialize conectors data
        _numberConectors = Random.Range(1,_maxConectors+1);
        _colors = new Color[_numberConectors];

        //Instantiate the conectors
        for (int i = 0; i < _numberConectors; i++)
        {
            Instantiate(_conectorPrefab, transform.GetChild(i + 1));        //The i+1 is because the first child in the prefab is the sprite.
            _colors[i] = colors[Random.Range(0, colors.Length)];            //Pick random color.
            if (transform.GetChild(i + 1).TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
            {
                transform.GetChild(i + 1).gameObject.SetActive(true);
                spriteRenderer.color = _colors[i];      //Asign color to component's spriteRenderer. 
                                                        //(Must be a sprite. If not, pick the right component; not SpriteRenderer)
            };
        }

        //Disable the Conectors we won't use. The prefab has 3 conectors, so we hide the ones we won't use.
        for (int i = _numberConectors; i < _maxConectors; i++)
        {
            transform.GetChild(i + 1).gameObject.SetActive(false);
        }
    }

    //Method to initialize random pieces
    public void Initialize()
    {
        //Initialize piece type
        switch (Random.Range(0,5))
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

        //Initialize conectors data
        _numberConectors = Random.Range(1, _maxConectors + 1);
        _colors = new Color[_numberConectors];

        //Instantiate the conectors
        for (int i = 0; i < _numberConectors; i++)
        {
            Instantiate(_conectorPrefab, transform.GetChild(i + 1));        //The i+1 is because the first child in the prefab is the sprite.
            _colors[i] = colors[Random.Range(0, colors.Length)];            //Pick random color.
            if (transform.GetChild(i + 1).TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
            {
                transform.GetChild(i + 1).gameObject.SetActive(true);
                spriteRenderer.color = _colors[i];      //Asign color to component's spriteRenderer. 
                                                        //(Must be a sprite. If not, pick the right component; not SpriteRenderer)
            };
        }

        //Disable the Conectors we won't use. The prefab has 3 conectors, so we hide the ones we won't use.
        for (int i = _numberConectors; i < _maxConectors; i++)
        {
            transform.GetChild(i + 1).gameObject.SetActive(false);
        }
    }

}
