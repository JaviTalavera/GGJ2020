using UnityEngine;

public class Scale : MonoBehaviour
{
    private Vector3 _originalScale;

    private void Start()
    {
        _originalScale = transform.localScale;
    }

    public void OnMouseEnter()
    {
        transform.localScale += new Vector3(1.001F, 1.001f, 1.001f); //adjust these values as you see fit
    }


    public void OnMouseExit()
    {
        transform.localScale = _originalScale;  // assuming you want it to return to its original size when your mouse leaves it.
    }

}