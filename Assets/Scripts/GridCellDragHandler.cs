using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridCellDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private bool isDragging = false;
    private Color currentColor;  // Variable to store the color of the clicked cell

    public void OnPointerDown(PointerEventData eventData)
    {
        // Start dragging
        isDragging = true;

        // Get the GameObject the pointer is on
        GameObject clickedCell = eventData.pointerCurrentRaycast.gameObject;

        if (clickedCell != null)
        {
            // Get the Image component of the clicked cell
            Image cellImage = clickedCell.GetComponent<Image>();

            if (cellImage != null)
            {
                // Store the color of the clicked cell
                currentColor = cellImage.color;

                // Optionally log the color
                Debug.Log($"Selected color: {currentColor}");
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Check if dragging is active
        if (isDragging)
        {
            // Get the cell the mouse is currently over
            GameObject targetCell = eventData.pointerCurrentRaycast.gameObject;

            // Ensure it's a valid cell
            if (targetCell != null)
            {
                // Get the Image component of the target cell
                Image targetImage = targetCell.GetComponent<Image>();

                if (targetImage != null)
                {
                    // Change the target cell's color to the selected color
                    targetImage.color = currentColor;

                    // Optionally, log the target cell's position for debugging
                    string cellName = targetCell.name;
                    Debug.Log($"Dragging over: {cellName} with color {currentColor}");
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Stop dragging
        if (isDragging)
        {
            isDragging = false;

            // Get the cell where the mouse was released
            GameObject targetCell = eventData.pointerCurrentRaycast.gameObject;

            if (targetCell != null)
            {
                // Get the Image component of the target cell
                Image targetImage = targetCell.GetComponent<Image>();

                if (targetImage != null)
                {
                    // Change the target cell's color to the selected color
                    targetImage.color = currentColor;

                    // Extract cell indices from the name (if needed)
                    string cellName = targetCell.name;
                    string[] nameParts = cellName.Split(new char[] { '(', ',', ')' }, System.StringSplitOptions.RemoveEmptyEntries);

                    if (nameParts.Length == 2)
                    {
                        int x = int.Parse(nameParts[0]);
                        int y = int.Parse(nameParts[1]);

                        // Debug log where the mouse was released
                        Debug.Log($"Released on cell with index: ({x}, {y})");
                    }
                }
            }
        }
    }
}
