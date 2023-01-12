using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int columns = 0;
    public int rows = 0;
    public GameObject gridSquare;
    public Vector2 startPosition = new Vector2(0, 0);
    public float squareScale = 0.5f;
    public float everySquareOffset = 0.0f;

    private Vector2 offset = new Vector2(0, 0);
    private List<GameObject> _gridSquares = new List<GameObject>();


    void Start()
    {
        SpawnGridSquare();
        SetGridSquarePosition();
    }

    private void SpawnGridSquare()
    {
        int square_index = 0;

        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < columns; col++)
            {
                _gridSquares.Add(Instantiate(gridSquare) as GameObject);
                _gridSquares[_gridSquares.Count - 1].transform.SetParent(this.transform);
                _gridSquares[_gridSquares.Count - 1].transform.transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().setImage(square_index % 2 == 0);
                square_index++; 
            }
        }
    }

    private void SetGridSquarePosition()
    {
        int column_number = 0;
        int row_number = 0;

        var square_rect = _gridSquares[0].GetComponent<RectTransform>();
        offset.x = square_rect.rect.width * square_rect.transform.localScale.x + everySquareOffset;
        offset.y = square_rect.rect.height * square_rect.transform.localScale.y + everySquareOffset;

        foreach (var square in _gridSquares) 
        {
            if(column_number+1 > columns)
            {
                column_number = 0;
                row_number++;
            }

            var pos_offset_x = offset.x * column_number;
            var pos_offset_y = offset.y * row_number;

            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition.x + pos_offset_x, startPosition.y - pos_offset_y);
            square.GetComponent<RectTransform>().transform.localPosition = new Vector3(startPosition.x + pos_offset_x, startPosition.y - pos_offset_y, 0.0f);

            column_number++;
        }
    }
}
