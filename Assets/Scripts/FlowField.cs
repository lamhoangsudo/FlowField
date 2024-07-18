using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowField
{
    public Cell[,] cells { get; private set; }
    public Vector2Int gridSize { get; private set; }
    public float cellRadius { get; private set; }
    private float cellDiameter;
    public Cell cellDestination;

    public FlowField(Vector2Int gridSize, float cellRadius)
    {
        this.gridSize = gridSize;
        this.cellRadius = cellRadius;
        cellDiameter = cellRadius * 2;
    }
    public void CreateGrid()
    {
        cells = new Cell[gridSize.x, gridSize.y];
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 worldPositionCell = new(cellDiameter * x + cellRadius, cellDiameter * y + cellRadius, 0);
                cells[x, y] = new Cell(worldPositionCell, new Vector2Int(x, y));
            }
        }
    }
    public void CreateCostField()
    {
        Vector3 cellHalfExtend = Vector3.one * cellRadius;
        int layerMash = LayerMask.GetMask("Impasssible", "RoughTerrain");
        foreach (Cell cell in cells)
        {
            Collider[] obstacles = Physics.OverlapBox(cell.worldPosition, cellHalfExtend, Quaternion.identity, layerMash);
            bool hasIncreasedCost = false;
            foreach (Collider obstacle in obstacles)
            {
                if (obstacle.gameObject.layer == 6)
                {
                    cell.IncreaseCost(255);
                    continue;
                }
                else if (obstacle.gameObject.layer == 7 && hasIncreasedCost == false)
                {
                    cell.IncreaseCost(3);
                    hasIncreasedCost = true;
                }
            }
        }
    }
    public void CreateIntergrationFileld(Cell _cellDestination)
    {
        cellDestination = _cellDestination;
        cellDestination.cost = 0;
        cellDestination.bestCost = 0;
        Queue<Cell> cellsToCheck = new Queue<Cell>();
        cellsToCheck.Enqueue(cellDestination);
        while (cellsToCheck.Count > 0)
        {
            Cell curCell = cellsToCheck.Dequeue();
            List<Cell> neighborCells = GetNeighborCells(curCell.gridIndex, GridDirection.CardinalDirections);
            foreach (Cell curNeighbor in neighborCells)
            {
                if (curNeighbor.cost == byte.MaxValue)
                {
                    continue;
                }
                if (curNeighbor.cost + curCell.bestCost < curNeighbor.bestCost)
                {
                    curNeighbor.bestCost = (ushort)(curNeighbor.cost + curCell.bestCost);
                    cellsToCheck.Enqueue(curNeighbor);
                }
            }
        }
    }
    public List<Cell> GetNeighborCells(Vector2Int nodeIndex, List<GridDirection> directions)
    {
        List<Cell> neighborCells = new();
        foreach (Vector2Int currentDirections in directions)
        {
            Cell newNeighbor = GetCellAtRelativePos(nodeIndex, currentDirections);
            if (newNeighbor != null)
            {
                neighborCells.Add(newNeighbor);
            }
        }
        return neighborCells;
    }
    public Cell GetCellAtRelativePos(Vector2Int orignPos, Vector2Int relativePos)
    {
        Vector2Int finalPos = orignPos + relativePos;
        if (finalPos.x < 0 || finalPos.x >= gridSize.x
        || finalPos.y < 0 || finalPos.y >= gridSize.y)
        {
            return null;
        }
        return cells[finalPos.x, finalPos.y];
    }
    public Cell GetCellFromWorldPos(Vector3 worldPos)
    {
        float percentX = worldPos.x / (gridSize.x * cellDiameter);
        float percentY = worldPos.y / (gridSize.y * cellDiameter);
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x = Mathf.Clamp(Mathf.FloorToInt((gridSize.x) * percentX), 0, gridSize.x - 1);
        int y = Mathf.Clamp(Mathf.FloorToInt((gridSize.y) * percentY), 0, gridSize.y - 1);
        return cells[x, y];
    }
    public void CreateFlowField()
    {
        foreach(Cell curCell in cells)
        {
            List<Cell> curNeighbors = GetNeighborCells(curCell.gridIndex, GridDirection.AllDirections);
            int bestCost = curCell.bestCost;
            foreach(Cell curNeighbor in curNeighbors)
            {
                if(curNeighbor.bestCost < bestCost)
                {
                    bestCost = curNeighbor.bestCost;
                    curCell.bestDirection = GridDirection.GetDirectionFromV2I(curNeighbor.gridIndex - curCell.gridIndex);
                }
            }
        }
    }
}
