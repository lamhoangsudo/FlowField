using UnityEditor;
using UnityEngine;


public enum FlowFieldDisplayType { None, AllIcons, DestinationIcon, CostField, IntegrationField };

public class GridDebug : MonoBehaviour
{
	public GridController gridController;
	public bool displayGrid;

	public FlowFieldDisplayType curDisplayType;

	private Vector2Int gridSize;
	private float cellRadius;
	private FlowField curFlowField;

	private Sprite[] ffIcons;

	private void Start()
	{
		ffIcons = Resources.LoadAll<Sprite>("Sprites/FFicons");
	}

	public void SetFlowField(FlowField newFlowField)
	{
		curFlowField = newFlowField;
		cellRadius = newFlowField.cellRadius;
		gridSize = newFlowField.gridSize;
	}
	public void DrawFlowField()
	{
		ClearCellDisplay();

		switch (curDisplayType)
		{
			case FlowFieldDisplayType.AllIcons:
				DisplayAllCells();
				break;

			case FlowFieldDisplayType.DestinationIcon:
				DisplayDestinationCell();
				break;

			default:
				break;
		}
	}
	private void DisplayAllCells()
	{
		if (curFlowField == null) { return; }
		foreach (Cell curCell in curFlowField.cells)
		{
			DisplayCell(curCell);
		}
	}

	private void DisplayDestinationCell()
	{
		if (curFlowField == null) { return; }
		DisplayCell(curFlowField.cellDestination);
	}

	private void DisplayCell(Cell cell)
	{
		GameObject iconGO = new GameObject();
		SpriteRenderer iconSR = iconGO.AddComponent<SpriteRenderer>();
		iconGO.transform.parent = transform;
		iconGO.transform.position = cell.worldPosition;
        iconGO.transform.position = cell.worldPosition + new Vector3(0, 0, -0.2f);
        iconGO.transform.localScale = new Vector3(cellRadius * 2, cellRadius * 2, cellRadius * 2);
        if (cell.cost == 0)
		{
			iconSR.sprite = ffIcons[3];
			Quaternion newRot = Quaternion.Euler(0, 0, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (cell.cost == byte.MaxValue)
		{
			iconSR.sprite = ffIcons[2];
			Quaternion newRot = Quaternion.Euler(0, 0, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (cell.bestDirection == GridDirection.North)
		{
			iconSR.sprite = ffIcons[0];
			Quaternion newRot = Quaternion.Euler(0, 0, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (cell.bestDirection == GridDirection.South)
		{
			iconSR.sprite = ffIcons[0];
			Quaternion newRot = Quaternion.Euler(0, 0, -180);
			iconGO.transform.rotation = newRot;
		}
		else if (cell.bestDirection == GridDirection.East)
		{
			iconSR.sprite = ffIcons[0];
			Quaternion newRot = Quaternion.Euler(0, 0, -90);
			iconGO.transform.rotation = newRot;
		}
		else if (cell.bestDirection == GridDirection.West)
		{
			iconSR.sprite = ffIcons[0];
			Quaternion newRot = Quaternion.Euler(0, 0, 90);
			iconGO.transform.rotation = newRot;
		}
		else if (cell.bestDirection == GridDirection.NorthEast)
		{
			iconSR.sprite = ffIcons[1];
			Quaternion newRot = Quaternion.Euler(0, 0, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (cell.bestDirection == GridDirection.NorthWest)
		{
			iconSR.sprite = ffIcons[1];
			Quaternion newRot = Quaternion.Euler(0, 0, 90);
			iconGO.transform.rotation = newRot;
		}
		else if (cell.bestDirection == GridDirection.SouthEast)
		{
			iconSR.sprite = ffIcons[1];
			Quaternion newRot = Quaternion.Euler(0, 0, -90);
			iconGO.transform.rotation = newRot;
		}
		else if (cell.bestDirection == GridDirection.SouthWest)
		{
			iconSR.sprite = ffIcons[1];
			Quaternion newRot = Quaternion.Euler(0, 0, 180);
			iconGO.transform.rotation = newRot;
		}
		else
		{
			iconSR.sprite = ffIcons[0];
		}
    }

	public void ClearCellDisplay()
	{
		foreach (Transform t in transform)
		{
			GameObject.Destroy(t.gameObject);
		}
	}
	
#if UNITY_EDITOR
    private void OnDrawGizmos()
	{
		if (displayGrid)
		{
			if (curFlowField == null)
			{
				DrawGrid(gridController.gridSize, Color.yellow, gridController.cellRadius);
			}
			else
			{
				DrawGrid(gridSize, Color.green, cellRadius);
			}
		}
		
		if (curFlowField == null) { return; }

        GUIStyle style = new(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter
        };

        switch (curDisplayType)
		{
			case FlowFieldDisplayType.CostField:

				foreach (Cell curCell in curFlowField.cells)
				{
                    Handles.Label(curCell.worldPosition, curCell.cost.ToString(), style);
				}
				break;
				
			case FlowFieldDisplayType.IntegrationField:

				foreach (Cell curCell in curFlowField.cells)
				{
					Handles.Label(curCell.worldPosition, curCell.bestCost.ToString(), style);
				}
				break;
			default:
				break;
		}
	}
#endif

	private void DrawGrid(Vector2Int drawGridSize, Color drawColor, float drawCellRadius)
	{
		Gizmos.color = drawColor;
		for (int x = 0; x < drawGridSize.x; x++)
		{
			for (int y = 0; y < drawGridSize.y; y++)
			{
				Vector3 center = new(drawCellRadius * 2 * x + drawCellRadius, drawCellRadius * 2 * y + drawCellRadius, 0);
				Vector3 size = Vector3.one * drawCellRadius * 2;
				Gizmos.DrawWireCube(center, size);
			}
		}
	}
}
