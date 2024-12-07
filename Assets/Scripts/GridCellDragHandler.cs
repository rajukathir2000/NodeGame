using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class GridCellDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private bool isDragging = false;            // Tracks if dragging is in progress
    private Color initialColor;                // Color of the initially clicked cell
    private List<Image> draggedCells;          // List of cells dragged over
    private Color defaultColor;                // Default color of grid cells (RGB: 29, 26, 26)
    private Image clickedCellImage;            // The cell that was initially clicked

    private void Start()
    {
        // Initialize the default grid cell color (normalized to 0-1 range)
        defaultColor = new Color(0.1132075f, 0.1019936f, 0.1019936f, 0.2705882f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Started");
        isDragging = true;
        draggedCells = new List<Image>();

        // Detect the clicked cell
        GameObject clickedCell = eventData.pointerCurrentRaycast.gameObject;

        if (clickedCell != null && clickedCell.CompareTag("BGCell"))
        {
            clickedCellImage = clickedCell.GetComponent<Image>();

            if (clickedCellImage != null)
            {
                // Check if the clicked cell's color is not the default color
                if (clickedCellImage.color != defaultColor)
                {
                    initialColor = clickedCellImage.color; // Store the initial color of the clicked cell
                    Debug.Log($"Initial color selected: {initialColor}");
                }
                else
                {
                    // If the clicked cell's color is the default, cancel the drag and give feedback
                    isDragging = false;
                    Debug.Log("Click ignored: Cell has the default color.");
                }
            }
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            // Perform a raycast to detect cells under the pointer during dragging
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = eventData.position
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            foreach (var result in results)
            {
                GameObject targetCell = result.gameObject;

                if (targetCell != null && targetCell.CompareTag("BGCell"))
                {
                    Image targetImage = targetCell.GetComponent<Image>();

                    if (targetImage != null)
                    {
                        // Allow dragging only on cells with default color or already changed to clicked color
                        if (targetImage.color == defaultColor)
                        {
                            // Change the dragged cell's color to the initially clicked cell's color
                            targetImage.color = initialColor;

                            // Add the cell to the list of dragged cells if not already present
                            if (!draggedCells.Contains(targetImage))
                            {
                                draggedCells.Add(targetImage);
                            }

                            Debug.Log($"Dragging over: {targetCell.name}, Color Changed: {initialColor}");
                        }
                        else if (targetImage.color == initialColor || targetImage == clickedCellImage)
                        {
                            // Allow dragging to proceed if the cell has already been changed to the clicked color
                            Debug.Log($"Dragging over valid cell: {targetCell.name}. Already colored.");
                        }
                        else
                        {
                            // If the dragged cell is neither the default nor the valid clicked color, cancel the drag
                            CancelDrag();
                            Debug.Log($"Dragging over invalid cell: {targetCell.name}. Drag cancelled.");
                            return;
                        }
                    }
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDragging)
        {
            isDragging = false;

            // Clear the list of dragged cells
            draggedCells.Clear();

            Debug.Log("Drag operation completed.");
        }
    }

    private void CancelDrag()
    {
        // Reset all dragged cells to their default color
        foreach (Image cell in draggedCells)
        {
            cell.color = defaultColor;
        }

        // Clear the list of dragged cells
        draggedCells.Clear();

        // Cancel dragging
        isDragging = false;

        Debug.Log("Drag operation cancelled. Cells reset to default.");
    }
}
