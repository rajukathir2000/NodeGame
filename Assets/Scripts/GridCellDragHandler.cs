using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class GridCellDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private bool isDragging = false;
    private Color initialColor, defaultColor;
    private List<Image> draggedCells;
    private Image clickedCellImage;
    private GameManager gameManager;
    private Vector2Int endpoint;

    private void Start()
    {
        
        defaultColor = new Color(0.1132075f, 0.1019936f, 0.1019936f, 0.2705882f);
        draggedCells = new List<Image>();
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = false;
        GameObject clickedCell = eventData.pointerCurrentRaycast.gameObject;
        if (clickedCell != null && clickedCell.CompareTag("BGCell"))
        {
            clickedCellImage = clickedCell.GetComponent<Image>();
            if (clickedCellImage != null)
            {
                LevelData currentLevelData = gameManager.gameData.levels.Find(l => l.levelNumber == PlayerPrefs.GetInt("SelectedLevel", 1));
                if (currentLevelData != null)
                {
                    foreach (var node in currentLevelData.nodes)
                    {
                        if (ColorUtility.TryParseHtmlString(node.color, out Color nodeColor))
                        {
                            Vector2Int startCell = new Vector2Int(node.start[0], node.start[1]);
                            Vector2Int endCell = new Vector2Int(node.end[0], node.end[1]);
                            Vector2Int clickedPosition = gameManager.GetCellPosition(clickedCell.name);
                            if (clickedPosition == startCell || clickedPosition == endCell)
                            {
                                initialColor = nodeColor;
                                endpoint = clickedPosition == startCell ? endCell : startCell;
                                isDragging = true;
                                Debug.Log($"Started dragging from {clickedCell.name} with color {initialColor}. Endpoint: {endpoint}");
                                return;
                            }
                        }
                    }
                }
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
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
                if (targetImage != null && targetImage.color == defaultColor)
                {
                    targetImage.color = initialColor;
                    if (!draggedCells.Contains(targetImage))
                    {
                        draggedCells.Add(targetImage);
                    }
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDragging) return;
            GameObject releasedCell = eventData.pointerCurrentRaycast.gameObject;
        if (releasedCell != null && releasedCell.CompareTag("BGCell"))
        {
            Vector2Int releasedPosition = gameManager.GetCellPosition(releasedCell.name);

            if (releasedPosition != endpoint)
            {
                ResetDraggedCells();
            }
        }
        else
        {
            ResetDraggedCells();
        }
        isDragging = false;
        if (CheckGameCompletion())
        {
            gameManager.ShowWinText();
        }
    }

    private bool CheckGameCompletion()
    {
        Transform gridParent = gameManager.gridParent.transform;
        foreach (Transform cellTransform in gridParent)
        {
            Image cellImage = cellTransform.GetComponent<Image>();
            if (cellImage != null && cellImage.color == defaultColor)
            {
                return false;
            }
        }
        return true;
    }

    private void ResetDraggedCells()
    {
        foreach (var cell in draggedCells)
        {
            cell.color = defaultColor;
        }
        draggedCells.Clear();
    }
}
