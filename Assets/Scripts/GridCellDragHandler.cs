////using UnityEngine;
////using UnityEngine.EventSystems;
////using UnityEngine.UI;

////public class GridCellDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
////{
////    private bool isDragging = false;
////    private Color currentColor;  // Variable to store the color of the clicked cell

////    public void OnPointerDown(PointerEventData eventData)
////    {
////        // Start dragging
////        isDragging = true;

////        // Get the GameObject the pointer is on
////        GameObject clickedCell = eventData.pointerCurrentRaycast.gameObject;

////        if (clickedCell != null && clickedCell.CompareTag("BGcell")) // Check if the cell has the tag "BGcell"
////        {
////            // Get the Image component of the clicked cell
////            Image cellImage = clickedCell.GetComponent<Image>();

////            if (cellImage != null)
////            {
////                // Store the color of the clicked cell
////                currentColor = cellImage.color;

////                // Optionally log the color
////                Debug.Log($"Selected color: {currentColor}");
////            }
////        }
////    }

////    public void OnDrag(PointerEventData eventData)
////    {
////        // Check if dragging is active
////        if (isDragging)
////        {
////            // Get the cell the mouse is currently over
////            GameObject targetCell = eventData.pointerCurrentRaycast.gameObject;

////            // Ensure it's a valid cell and has the tag "BGcell"
////            if (targetCell != null && targetCell.CompareTag("BGcell"))
////            {
////                // Get the Image component of the target cell
////                Image targetImage = targetCell.GetComponent<Image>();

////                if (targetImage != null)
////                {
////                    // Change the target cell's color to the selected color
////                    targetImage.color = currentColor;

////                    // Optionally, log the target cell's position for debugging
////                    string cellName = targetCell.name;
////                    Debug.Log($"Dragging over: {cellName} with color {currentColor}");
////                }
////            }
////        }
////    }

////    public void OnPointerUp(PointerEventData eventData)
////    {
////        // Stop dragging
////        if (isDragging)
////        {
////            isDragging = false;

////            // Get the cell where the mouse was released
////            GameObject targetCell = eventData.pointerCurrentRaycast.gameObject;

////            if (targetCell != null && targetCell.CompareTag("BGcell")) // Check if the cell has the tag "BGcell"
////            {
////                // Get the Image component of the target cell
////                Image targetImage = targetCell.GetComponent<Image>();

////                if (targetImage != null)
////                {
////                    // Change the target cell's color to the selected color
////                    targetImage.color = currentColor;

////                    // Extract cell indices from the name (if needed)
////                    string cellName = targetCell.name;
////                    string[] nameParts = cellName.Split(new char[] { '(', ',', ')' }, System.StringSplitOptions.RemoveEmptyEntries);

////                    if (nameParts.Length == 2)
////                    {
////                        int x = int.Parse(nameParts[0]);
////                        int y = int.Parse(nameParts[1]);

////                        // Debug log where the mouse was released
////                        Debug.Log($"Released on cell with index: ({x}, {y})");
////                    }
////                }
////            }
////        }
////    }
////}


//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;
//using System.Collections.Generic;

//public class GridCellDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
//{
//    private bool isDragging = false;
//    private Color initialColor;             // Normalized color of the clicked cell
//    private List<Image> draggedCells;       // List to store all the cells dragged over
//    private Color defaultColor;             // Default normalized grid cell color

//    private void Start()
//    {
//        // Initialize the default color (normalized form of RGB: 29, 26, 26)
//        defaultColor = new Color(29f / 255f, 26f / 255f, 26f / 255f);
//    }

//    public void OnPointerDown(PointerEventData eventData)
//    {
//        isDragging = true;
//        draggedCells = new List<Image>();

//        // Get the GameObject the pointer is on
//        GameObject clickedCell = eventData.pointerCurrentRaycast.gameObject;

//        if (clickedCell != null && clickedCell.CompareTag("BGcell"))
//        {
//            Image cellImage = clickedCell.GetComponent<Image>();

//            if (cellImage != null)
//            {
//                initialColor = cellImage.color; // Store normalized color
//                Debug.Log($"Initial color selected: {initialColor}");
//            }
//        }
//    }

//    public void OnDrag(PointerEventData eventData)
//    {
//        if (isDragging)
//        {
//            // Perform a raycast to detect the current cell under the pointer
//            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
//            {
//                position = eventData.position
//            };

//            List<RaycastResult> results = new List<RaycastResult>();
//            EventSystem.current.RaycastAll(pointerEventData, results);

//            foreach (var result in results)
//            {
//                GameObject targetCell = result.gameObject;

//                if (targetCell != null && targetCell.CompareTag("BGcell"))
//                {
//                    Image targetImage = targetCell.GetComponent<Image>();
//                    Debug.Log("On drag");

//                    if (targetImage != null && targetImage.color == defaultColor)
//                    {
//                        targetImage.color = initialColor; // Change color to the initial selected color
//                        if (!draggedCells.Contains(targetImage))
//                        {
//                            draggedCells.Add(targetImage); // Add to list of dragged cells
//                        }

//                        Debug.Log($"Dragging over: {targetCell.name}");
//                    }
//                }
//            }
//        }
//    }

//    public void OnPointerUp(PointerEventData eventData)
//    {
//        isDragging = false;

//        // Get the cell where the mouse was released
//        GameObject targetCell = eventData.pointerCurrentRaycast.gameObject;

//        if (targetCell != null && targetCell.CompareTag("BGcell"))
//        {
//            Image targetImage = targetCell.GetComponent<Image>();

//            if (targetImage != null)
//            {
//                if (targetImage.color != initialColor) // Compare normalized colors
//                {
//                    // Reset dragged cells to default color
//                    foreach (Image cell in draggedCells)
//                    {
//                        cell.color = defaultColor;
//                    }

//                    Debug.Log("Released cell color doesn't match. Resetting dragged cells.");
//                }
//                else
//                {
//                    Debug.Log("Released cell color matches. Drag confirmed.");
//                }
//            }
//        }

//        // Clear dragged cells list
//        draggedCells.Clear();
//    }
//}

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

    private void Start()
    {
        // Initialize the default grid cell color (normalized to 0-1 range)
        defaultColor = new Color(29f , 26f , 26f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        draggedCells = new List<Image>();

        // Detect the clicked cell
        GameObject clickedCell = eventData.pointerCurrentRaycast.gameObject;

        if (clickedCell != null && clickedCell.CompareTag("BGcell"))
        {
            Image cellImage = clickedCell.GetComponent<Image>();

            if (cellImage != null)
            {
                initialColor = cellImage.color; // Store the initial color of the clicked cell
                Debug.Log($"Initial color selected: {initialColor}");
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

                if (targetCell != null && targetCell.CompareTag("BGcell"))
                {
                    Debug.Log("Over the cell");
                    Image targetImage = targetCell.GetComponent<Image>();

                    if (targetImage != null && targetImage.color == defaultColor)
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
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;

        // Get the cell where the mouse is released
        GameObject targetCell = eventData.pointerCurrentRaycast.gameObject;

        if (targetCell != null && targetCell.CompareTag("BGcell"))
        {
            Image targetImage = targetCell.GetComponent<Image>();

            if (targetImage != null)
            {
                // Check if the release cell's color matches the initial color
                if (targetImage.color != initialColor)
                {
                    // Reset all dragged cells to their default color
                    foreach (Image cell in draggedCells)
                    {
                        cell.color = defaultColor;
                    }

                    Debug.Log("Released cell color doesn't match. Resetting dragged cells.");
                }
                else
                {
                    Debug.Log("Released cell color matches. Drag confirmed.");
                }
            }
        }

        // Clear the list of dragged cells
        draggedCells.Clear();
    }
}

