using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class ShapeData : ScriptableObject
{
    [System.Serializable]
    public class Row
    {
        public int size = 0;
        public bool[] column;

        public Row() { }

        public Row(int size)
        {
            CreateRow(size);
        }

        public void CreateRow(int size)
        {
            this.size = size;
            column = new bool[size];
            ClearRow();
        }

        public void ClearRow()
        {
            for (int i = 0; i < size; i++)
            {
                column[i] = false;
            }
        }
    }

    public int rows = 0;
    public int columns = 0;
    public Row[] board;

    public void ClearBoard()
    {
        for (int i = 0; i < rows; i++)
        {
            board[i].ClearRow();
        }
    }

    public void CreateBoard()
    {
        board = new Row[rows];
        for (int i = 0; i < rows; i++)
        {
            board[i] = new Row(columns);
        }
    }
}
