using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector2Int gridSize;
    public float cellRadius = 0.5f;
    public FlowField currentFlowField;
    public GridDebug gridDebug;
    public void CreateFlowField()
    {
        currentFlowField = new FlowField(gridSize, cellRadius);
        currentFlowField.CreateGrid();
        currentFlowField.CreateCostField();
        gridDebug.SetFlowField(currentFlowField);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateFlowField();
            Vector3 mousePos = new(Input.mousePosition.x, Input.mousePosition.y, 10f);
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
            Cell destinationCell = currentFlowField.GetCellFromWorldPos(worldMousePos);
            currentFlowField.CreateIntergrationFileld(destinationCell);
            currentFlowField.CreateFlowField();
            gridDebug.DrawFlowField();
        }
    }
}
