using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shape : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject squareShapeImage;
    public Vector3 shapeSelectedScale;
    public Vector2 offset = new Vector2(0, 500);

    [HideInInspector]
    public ShapeData currentShapeData;

    private List<GameObject> _currentSquareShape = new List<GameObject>();
    private Vector3 _shapeStartScale;
    private bool _shapeDraggable = true;
    private RectTransform _transform;
    private Canvas _canvas;

    void Awake()
    {
        _shapeStartScale = this.GetComponent<RectTransform>().localScale;
        _shapeDraggable = true;
        _transform = this.GetComponent<RectTransform>();
        _canvas = this.GetComponentInParent<Canvas>();
    }

    public void RequestNewShape(ShapeData shapeData)
    {
        CreateShape(shapeData);
    }

    private void CreateShape(ShapeData shapeData)
    {
        currentShapeData = shapeData;
        var totalSquareImage = GetNumberofSquare(shapeData);

        while (_currentSquareShape.Count < totalSquareImage)
        {
            _currentSquareShape.Add(Instantiate(squareShapeImage, transform) as GameObject);
        }
        foreach (var square in _currentSquareShape)
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
                    _currentSquareShape[currentIndexInList].SetActive(true);
                    _currentSquareShape[currentIndexInList].GetComponent<RectTransform>().localPosition =
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

    public void OnEndDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = _shapeStartScale;
        GameEvent.checkIfShapeCanBePlace();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _transform.anchorMin = new Vector2(0, 0);
        _transform.anchorMax = new Vector2(0, 0);
        _transform.pivot = new Vector2(0, 0);

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, eventData.position, Camera.main, out pos);

        _transform.localPosition = pos + offset;

        GameEvent.validBlockToPlaceShape();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = shapeSelectedScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }
}
