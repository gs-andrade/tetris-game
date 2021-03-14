using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    private float previousTime;
    public Vector3 RotationPoint;

    public int PoolIndex { get; private set; }
    public  Transform CachedTranform { get; private set; }
    public GameObject CachedGameObject { get; private set; }
    public GameObject[] CachedChilds { get; private set; }

    private int childAmmount;
    public void Setup(int poolIndex)
    {
        if (CachedTranform == null)
            CachedTranform = transform;

        if (CachedGameObject == null)
            CachedGameObject = gameObject;

        if (CachedChilds == null)
        {
            childAmmount = transform.childCount;
            CachedChilds = new GameObject[childAmmount];

            for (int i = 0; i < childAmmount; i++)
                CachedChilds[i] = transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < childAmmount; i++)
            CachedChilds[i].SetActive(true);

        PoolIndex = poolIndex;

    }

    public void SetPosition(Vector3 position)
    {
        CachedTranform.position = position;
    }

    public void AddPosition(Vector3 position)
    {
        CachedTranform.position += position;
    }

    public bool CanReturnToPool()
    {
        for (int i = 0; i < CachedChilds.Length; i++)
        {
            if (CachedChilds[i].gameObject.activeInHierarchy)
                return false;
        }

        return true;
    }

    public void DisableAllChildren()
    {
        for (int i = 0; i < childAmmount; i++)
            CachedChilds[i].SetActive(false);
    }

}
