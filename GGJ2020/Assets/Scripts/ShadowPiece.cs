using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPiece : MonoBehaviour
{
    private Piece _piece;

    private Color[] _colors;
    private int _numberConectors;
    private int _maxConectors;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(Piece piece)
    {
        //Pick piece reference
        _piece = piece;

        //Pick piece's sprite
        if(_piece.TryGetComponent<SpriteRenderer>(out SpriteRenderer pieceSpriteRenderer) 
            && TryGetComponent<SpriteRenderer>(out SpriteRenderer shadowPieceSpriteRenderer))
        {
            shadowPieceSpriteRenderer.sprite = pieceSpriteRenderer.sprite;

            shadowPieceSpriteRenderer.color = new Color(
                    shadowPieceSpriteRenderer.color.r,
                    shadowPieceSpriteRenderer.color.g,
                    shadowPieceSpriteRenderer.color.b,
                    0.5f);
        }

        //Initialize conectors data
        _numberConectors = piece.GetNConectors();
        _colors = piece.GetColors();

        //Instantiate the conectors
        for (int i = 0; i < _numberConectors; i++)
        {
            if (transform.GetChild(i + 1).TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
            {
                transform.GetChild(i + 1).gameObject.SetActive(true);
                spriteRenderer.color = _colors[i];      //Asign color to component's spriteRenderer. 
                                                        //(Must be a sprite. If not, pick the right component; not SpriteRenderer)
            };
        }

        //Disable the Conectors we won't use. The prefab has 3 conectors, so we hide the ones we won't use.
        for (int i = _numberConectors; i < piece.GetMaxConnectors(); i++)
        {
            transform.GetChild(i + 1).gameObject.SetActive(false);
        }
    }

    //Collision with border
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BeltBorder"))
        {
            _piece.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
