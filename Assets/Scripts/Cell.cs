using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Vector3 worldPosition;
    public Vector2Int gridIndex;
    public byte cost;
    public ushort bestCost;
    public GridDirection bestDirection;
    public Cell(Vector3 worldPosition, Vector2Int gridIndex)
    {
        this.worldPosition = worldPosition;
        this.gridIndex = gridIndex;
        this.cost = 1;
        bestCost = ushort.MaxValue;
        bestDirection = GridDirection.None;
    }
    public void IncreaseCost(int amount)
    {
        if (cost < byte.MaxValue)
        {
            int costCaculate = cost + amount;
            if (costCaculate >= 255)
            {
                cost = byte.MaxValue;
            }
            else
            {
                cost = (byte)costCaculate;
            }
        }
    }
}
