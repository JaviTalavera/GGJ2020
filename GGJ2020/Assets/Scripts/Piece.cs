using UnityEngine;


public class Piece : MonoBehaviour
{
    public AudioClip _audioError;
    //General Piece information
    public enum pieceType { leg_l, leg_r, arm_l, arm_r };
    private Color[] colors = { Color.red, Color.green, Color.blue, Color.white, Color.black };

    //Piece's info
    public pieceType _type;

    //Conectors' info
    public int _numberConectors;
    private int _maxConectors = 3;
    public Color[] _colors;

    //Mouse interaction variables
    private Robot _mouseOverRobot;
    private float _returnTime = 1;
    bool used = true;           //Initialized from robot by deafult    
    private GameObject _aura;
    bool clicked = false;

    //References to intantiate objects
    [SerializeField] public GameObject _conectorPrefab;
    [SerializeField] public GameObject _auraPrefab;


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
        }

        //Initialize conectors data
        _numberConectors = Random.Range(1, _maxConectors + 1);
        _colors = new Color[_numberConectors];

        //Instantiate the conectors
        for (int i = 0; i < _numberConectors; i++)
        {
            //Instantiate(_conectorPrefab, transform.GetChild(i + 1));        //The i+1 is because the first child in the prefab is the sprite.
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

        //Initialize it's aura
        _aura = Instantiate(_auraPrefab, transform);
        _aura.SetActive(false);
        this.gameObject.SetActive(false);
        used = false;
    }

    //Method to initialize random pieces
    public void Initialize()
    {
        //Initialize piece type
        Initialize(Random.Range(0, 4));

    }

    //Getters and setters
    public int GetNConectors()
    {
        return _numberConectors;
    }

    public Color[] GetColors()
    {
        return _colors;
    }

    public int GetMaxConnectors()
    {
        return _maxConectors;
    }

    public bool HasBeenUsed()
    {
        return used;
    }

    public void SetUsed(bool value)
    {
        used = value;
    }

    //Mouse interaction
    public void OnMouseOver()
    {
        if (GameManager.IsPause) return;
        _aura.transform.position = new Vector3(
                _aura.transform.position.x,
                _aura.transform.position.y,
                1f);
        if (!clicked)
            _aura.SetActive(true);
    }

    public void OnMouseExit()
    {
        if (GameManager.IsPause) return;
        _aura.SetActive(false);
    }

    public void OnMouseDown()
    {
        if (GameManager.IsPause) return;
        //Remove the parent link
        transform.SetParent(null);
        clicked = true;
        _aura.SetActive(false);
    }

    public void OnMouseDrag()
    {
        if (GameManager.IsPause) return;
        if (clicked)
        {
            //Drag
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            transform.position = new Vector3(transform.position.x,
                                        transform.position.y,
                                        -1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Robot>(out Robot r))
        {
            //Store robot reference
            _mouseOverRobot = r;
            _aura.SetActive(true);
        }
        else {
            var rob = collision.gameObject.GetComponentInParent<Robot>();
            if (rob)
            {
                _mouseOverRobot = rob;
                _aura.SetActive(true);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        _mouseOverRobot = null;
        _aura.SetActive(false);
    }

    public void OnMouseUp()
    {
        _aura.SetActive(false);
        if (_mouseOverRobot != null && _mouseOverRobot.checkCorrectPiece(this))
        {
            //Attach piece to robot
            this.used = true;
            GameObject.FindWithTag("Audio").GetComponent<PassAudioToNextScene>().Play(1);
            _mouseOverRobot.Repair(this);
            gameObject.SetActive(false);

        }
        else
        {
            //Lerp the piece to the shadowPiece's transform.position
            this.gameObject.SetActive(false);
            GameObject.FindWithTag("Audio").GetComponent<PassAudioToNextScene>().Play(0);
            GameObject.FindWithTag("LevelManager").GetComponent<Level>()?.GetPiecesQueue().Enqueue(this);
            GameObject.FindWithTag("GameManager").GetComponent<GameManager>()?.SubtractTime(5.0f);
        }
    }

    public static bool operator ==(Piece pOne, Piece pTwo)
    {
        if (pOne._type != pTwo._type) return false;

        if (pOne._numberConectors == pTwo._numberConectors)
        {
            for (int i = 0; i < pOne._numberConectors; i++)
            {
                if (!pOne._colors[i].Equals(pTwo._colors[i])) return false;
            }
        }
        else { return false; }

        return true;
    }

    public static bool operator !=(Piece pOne, Piece pTwo) => !pOne == pTwo;
}