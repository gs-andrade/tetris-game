using UnityEngine;

public class Node
{
    public readonly int KeyX;
    public readonly int KeyY;
    public readonly Vector2 WorldPosition;
    public GameObject Block;
    public Node(int keyX, int keyY, Vector2 worldPosition)
    {
        KeyX = keyX;
        KeyY = keyY;
        WorldPosition = worldPosition;
    }

    public void DestroyBlock()
    {
        Block = null;
    }

    public void DisableBlock()
    {
        if (Block != null)
            Block.SetActive(false);

        Block = null;
    }
}

