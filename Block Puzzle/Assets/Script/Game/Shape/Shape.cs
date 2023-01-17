using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public GameObject squareShapeImage;

    //[HideInInspector]
    public ShapeData currentShapeData;

    private List<GameObject> _currentShape = new List<GameObject>();

    void Start()
    {
        RequestNewShape(currentShapeData);
    }

    public void RequestNewShape(ShapeData shapeData)
    {
        CreateShape(shapeData);
    }

    public void CreateShape(ShapeData shapeData)
    {
        currentShapeData = shapeData;
        var totalSquareImage = GetNumberofSquare(shapeData);

        while (_currentShape.Count <= totalSquareImage)
        {
            _currentShape.Add(Instantiate(squareShapeImage, transform) as GameObject);
        }
        foreach (var square in _currentShape)
        {
            square.gameObject.transform.position = Vector3.zero;
            square.gameObject.SetActive(false);
        }

        var squareRect = squareShapeImage.GetComponent<RectTransform>();
        var moveDistance = new Vector2(squareRect.rect.width * squareRect.localScale.x,
            squareRect.rect.height * squareRect.localScale.y);

        int currentIndexInList = 0;

        for (var row = 0; row < shapeData.rows; row++)
        {
            for (var column = 0; column < shapeData.columns; column++)
            {
                if (shapeData.board[row].column[column])
                {
                    _currentShape[currentIndexInList].SetActive(true);
                    _currentShape[currentIndexInList].GetComponent<RectTransform>().localPosition =
                        new Vector2(GetXPositionForSquareShape(shapeData, column, moveDistance),
                        GetYPositionSquareShape(shapeData, row, moveDistance));

                    currentIndexInList++;
                }
            }
        }
    }

    private float GetYPositionSquareShape(ShapeData shapeData, int row, Vector2 moveDistance)
    {
        float yShift = 0f;

        if (shapeData.rows > 1)
        {
            if (shapeData.rows % 2 != 0)
            {
                var middleSquareIndex = (shapeData.rows - 1) / 2;
                var multipler = (shapeData.rows - 1) / 2;
                if (row < middleSquareIndex)
                {
                    yShift = moveDistance.y * 1;
                    yShift *= multipler;
                }
                else if (row > middleSquareIndex)
                {
                    yShift = moveDistance.y * -1;
                    yShift *= multipler;
                }
            }
            else
            {
                var middleSquareIndex2 = (shapeData.rows == 2) ? 1 : (shapeData.rows / 2);
                var middleSquareIndex1 = (shapeData.rows == 2) ? 0 : (shapeData.rows - 1);
                var multipler = shapeData.rows / 2;

                if (row == middleSquareIndex1 || row == middleSquareIndex2)
                {
                    if (row == middleSquareIndex2)
                        yShift = (moveDistance.y / 2) * -1;
                    else
                        yShift = (moveDistance.y / 2) * 1;
                }

                if (row < middleSquareIndex1 && row < middleSquareIndex2)
                {
                    yShift = moveDistance.y * 1;
                    yShift *= multipler;
                }
                else if (row > middleSquareIndex1 && row > middleSquareIndex2)
                {
                    yShift = moveDistance.y * -1;
                    yShift *= multipler;
                }
            }
        }
        return yShift;
    }

    private float GetXPositionForSquareShape(ShapeData shapeData, int column, Vector2 moveDistance)
    {
        float xShift = 0f;

        if (shapeData.columns > 1)
        {
            if (shapeData.columns % 2 != 0)
            {
                var middleSqaureIndex = (shapeData.columns - 1) / 2;
                var multipler = (shapeData.columns - 1) / 2;
                if (column < middleSqaureIndex)
                {
                    xShift = moveDistance.x * -1;
                    xShift *= multipler;
                }
                else if (column > middleSqaureIndex)
                {
                    xShift = moveDistance.x * 1;
                    xShift *= multipler;
                }
            }
            else
            {
                var middleSquareIndex2 = (shapeData.columns == 2) ? 1 : (shapeData.columns / 2);
                var middleSquareIndex1 = (shapeData.columns == 2) ? 0 : (shapeData.columns - 1);
                var multipler = shapeData.columns / 2;

                if (column == middleSquareIndex1 || column == middleSquareIndex2)
                {
                    if (column == middleSquareIndex2)
                        xShift = moveDistance.x / 2;
                    else
                        xShift = (moveDistance.x / 2) * -1;
                }

                if (column < middleSquareIndex1 && column < middleSquareIndex2)
                {
                    xShift = moveDistance.x * -1;
                    xShift *= multipler;
                }
                else if (column > middleSquareIndex1 && column > middleSquareIndex2)
                {
                    xShift = moveDistance.x * 1;
                    xShift *= multipler;
                }
            }
        }
        return xShift;
    }

    private int GetNumberofSquare(ShapeData shapeData)
    {
        int numberOfSquare = 0;
        foreach (var rowData in shapeData.board)
        {
            foreach (var active in rowData.column)
            {
                if (active)
                    numberOfSquare++;
            }
        }

        return numberOfSquare;
    }
}
