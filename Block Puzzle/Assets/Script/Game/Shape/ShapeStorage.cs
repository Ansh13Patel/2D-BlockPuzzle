using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeStorage : MonoBehaviour
{
    public List<ShapeData> shapeData;
    public List<Shape> shapes;

    void Start()
    {
        foreach(var shape in shapes)
        {
            int randomIndex = Random.Range(0, shapeData.Count);
            shape.RequestNewShape(shapeData[randomIndex]);
        }
    }
}
