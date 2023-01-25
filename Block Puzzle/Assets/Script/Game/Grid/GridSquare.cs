using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    public Image HooverImage;
    public Image ActiveImage;
    public Image normalImage;
    public List<Sprite> normalImages;

    public bool selected { get; set; }
    public int squareIndex { get; set; }
    public bool squareOccupied { get; set; }

    private bool _isvalid;

    private void Start()
    {
        selected = false;
        squareOccupied = false;
        _isvalid = true;
    }

    public bool shapeCanBePlace()
    {
        return HooverImage.gameObject.activeSelf;
    }

    public void ActivateImage()
    {
        HooverImage.gameObject.SetActive(false);
        ActiveImage.gameObject.SetActive(true);
        squareOccupied = true;
    }

    public void DeselectSquare()
    {
        HooverImage.gameObject.SetActive(false);
        _isvalid = false;
    }

    public void setImage(bool setFirstImage)
    {
        normalImage.GetComponent<Image>().sprite = setFirstImage ? normalImages[1] : normalImages[0]; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isvalid)
            return; 
        HooverImage.gameObject.SetActive(true);
        selected = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!_isvalid)
            return;
        HooverImage.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        HooverImage.gameObject.SetActive(false);
        selected = false;
        _isvalid = true;
    }
}
