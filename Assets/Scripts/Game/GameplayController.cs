using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public TetrisBlock[] BlockPrefabs;
    public float BlockDelayToMoveBlockDanwards = 0.75f;
    public int LinesDestroyed { private set; get; }

    private Grid grid;
    private TetrisBlock blockCurrent;
    private float delayToMoveBlock;

    private Dictionary<int,Pool<TetrisBlock>>blockPool;

    private List<TetrisBlock> activeBlocks;

    private float nodeSize;
    private int blocksAmmount;

    public void Setup()
    {
        grid = GetComponent<Grid>();
        grid.Setup();

        nodeSize = grid.NodeSize;

        blocksAmmount = BlockPrefabs.Length;

        blockPool = new Dictionary<int, Pool<TetrisBlock>>();

        activeBlocks = new List<TetrisBlock>();

        for (int i = 0; i < blocksAmmount; i++)
        {
            var newPool = new Pool<TetrisBlock>(10, BlockPrefabs[i], transform);
            blockPool.Add(i, newPool);
        }

    }

    public void ResetGame()
    {
        grid.ResetGrid();

        for (int i = 0; i < activeBlocks.Count; i++)
        {
            ReturnBlockToPool(activeBlocks[i]);
        }

        if(blockCurrent != null)
        {
            blockCurrent.DisableAllChildren();
            ReturnBlockToPool(blockCurrent);
        }
            

        CreateNewBlock();
    }

    public void TryAddDestroyedBlockToPool()
    {
        for (int i = 0; i < activeBlocks.Count; i++)
        {
            var block = activeBlocks[i];

            if (block.CanReturnToPool()) 
                ReturnBlockToPool(block);   
        }
    }

    private void ReturnBlockToPool(TetrisBlock block)
    {
        blockPool[block.PoolIndex].AddToPool(block);
    }

    public GameState UpdateGame()
    {
        return InnerUpdateGame();
    }

    private void CreateNewBlock()
    {
        delayToMoveBlock = BlockDelayToMoveBlockDanwards;
        int poolIndex = UnityEngine.Random.Range(0, blocksAmmount);

        var block = blockPool[poolIndex].GetInstance();
        block.Setup(poolIndex);
        block.SetPosition(new Vector3(grid.LineAmmount / 2, grid.ColumnAmmount - nodeSize, 0));

        blockCurrent = block;
    }

    private GameState InnerUpdateGame()
    {
        var movement = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            movement = new Vector3(-nodeSize, 0, 0);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            movement = new Vector3(nodeSize, 0, 0);

        if (movement != Vector3.zero)
        {
            blockCurrent.AddPosition(movement);
            if (!SimulateBlockMovementAndTryToMoveBlock())
                blockCurrent.AddPosition(-movement);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            blockCurrent.CachedTranform.RotateAround(blockCurrent.CachedTranform.TransformPoint(blockCurrent.RotationPoint), new Vector3(0, 0, 1), 90);
            if (!SimulateBlockMovementAndTryToMoveBlock())
                blockCurrent.CachedTranform.RotateAround(blockCurrent.CachedTranform.TransformPoint(blockCurrent.RotationPoint), new Vector3(0, 0, 1), -90);
        }

        delayToMoveBlock -= Time.deltaTime;

        if (Input.GetKey(KeyCode.DownArrow))
        {
            delayToMoveBlock -= delayToMoveBlock * 0.5f;
        }

        if (delayToMoveBlock <= 0)
        {
            blockCurrent.CachedTranform.position += new Vector3(0, -nodeSize, 0);
            if (!SimulateBlockMovementAndTryToMoveBlock())
            {
                blockCurrent.CachedTranform.position += new Vector3(0, nodeSize, 0);

                if (!IsGameOver())
                {
                    AddBlockToGrid();
                    LinesDestroyed = grid.CheckIfLineIsFullAndDestroy();
                    CreateNewBlock();

                    SoundController.instance.PlayAudioEffect("BlockHit");

                    if (LinesDestroyed > 0)
                        return GameState.IncreaseScore;

                }
                else
                    return GameState.GameOver;
            }
            delayToMoveBlock = BlockDelayToMoveBlockDanwards;
        }

        return GameState.Gameplay;

    }

    private bool IsGameOver()
    {
        var childs = blockCurrent.CachedChilds;

        for (int i = 0; i < childs.Length; i++)
        {
            var child = childs[i];

            var nodeSize = grid.NodeSize;

            var keyY = Mathf.RoundToInt(child.transform.position.y / nodeSize);

            if (keyY >= grid.ColumnAmmount)
                return true;

        }

        return false;
    }


    private bool SimulateBlockMovementAndTryToMoveBlock()
    {
        var childs = blockCurrent.CachedChilds;

        for (int i = 0; i < childs.Length; i++)
        {
            var child = childs[i];

            var nodeSize = grid.NodeSize;

            var keyX = Mathf.RoundToInt(child.transform.position.x / nodeSize);
            var keyY = Mathf.RoundToInt(child.transform.position.y / nodeSize);

            if (keyX < 0 || keyX >= grid.LineAmmount | keyY < 0)
                return false;

            if (grid.CheckIfNodeHasABlock(keyX, keyY))
                return false;

        }

        return true;
    }

    private void AddBlockToGrid()
    {
        var childs = blockCurrent.CachedChilds;
        for (int i = 0; i < childs.Length; i++)
        {
            var child = childs[i];

            var nodeSize = grid.NodeSize;

            var keyX = Mathf.RoundToInt(child.transform.position.x / nodeSize);
            var keyY = Mathf.RoundToInt(child.transform.position.y / nodeSize);

            grid.AddObjectToNode(keyX, keyY, child.gameObject);
        }
    }

   

}
