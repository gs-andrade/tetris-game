using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public float NodeSize { get { return 1; } private set { } }
    public int LineAmmount { get { return 10; } private set { } }
    public int ColumnAmmount { get { return 20; } private set { } }

    private Node[,] nodes;

    private bool initialized = false;
    public void Setup()
    {
        if (!initialized)
            CreateGridAndNodes();

    }

    private void CreateGridAndNodes()
    {
        initialized = true;

        nodes = new Node[LineAmmount, ColumnAmmount];

        for (int i = 0; i < LineAmmount; i++)
        {
            for (int j = 0; j < ColumnAmmount; j++)
            {
                Vector2 pos = new Vector2(i * NodeSize, j * NodeSize);
                var node = new Node(i, j, pos);
                nodes[i, j] = node;
            }
        }
    }

    public void AddObjectToNode(int keyX, int keyY, GameObject obj)
    {
        nodes[keyX, keyY].Block = obj;
    }

    public bool CheckIfNodeHasABlock(int keyX, int keyY)
    {
        if (keyX < 0 || keyY < 0 || keyX >= nodes.GetLength(0) || keyY >= nodes.GetLength(1))
        {
            return false;
        }

        return nodes[keyX, keyY].Block != null;
    }

    public int CheckIfLineIsFullAndDestroy()
    {
        int linesDestroyed = 0;

        for (int columnIndex = 0; columnIndex < ColumnAmmount - 1; columnIndex++)
        {
            if (InnerCheckIfLineIsFull(columnIndex))
            {
                DeleteFullLine(columnIndex);
                ReOrganizeGrid(columnIndex);
                columnIndex--;
                linesDestroyed++;
            }
        }

        return linesDestroyed;
    }

    private bool InnerCheckIfLineIsFull(int columnIndex)
    {
        for (int i = 0; i < LineAmmount; i++)
        {
            if (!CheckIfNodeHasABlock(i, columnIndex))
            {
                return false;
            }
        }

        return true;
    }

    public void ResetGrid()
    {
        for(int i = 0; i < ColumnAmmount; i++)
        {
            DeleteFullLine(i);
        }
    }

    private void DeleteFullLine(int columnIndex)
    {
        for (int i = 0; i < LineAmmount; i++)
        {
            nodes[i, columnIndex].DisableBlock();
        }
    }

    private void ReOrganizeGrid(int startPoint)
    {
        for (int columnIndex = startPoint; columnIndex < ColumnAmmount; columnIndex++)
        {
            for (int lineIndex = 0; lineIndex < LineAmmount; lineIndex++)
            {
                if (CheckIfNodeHasABlock(lineIndex, columnIndex))
                {
                    nodes[lineIndex, columnIndex - 1].Block = nodes[lineIndex, columnIndex].Block;
                    nodes[lineIndex, columnIndex].DestroyBlock();
                    nodes[lineIndex, columnIndex - 1].Block.transform.position += new Vector3(0, -NodeSize, 0);
                }
            }
        }
    }



}