using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShapeData), false)]
[CanEditMultipleObjects]
[System.Serializable]   
public class ShapeDataDrawer : Editor
{
    public ShapeData ShapeDataInstance => target as ShapeData;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

    }
}
