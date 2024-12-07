using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NodeData
{
    public string color; // Color for the node
    public int[] start;  // Start position as [x, y]
    public int[] end;    // End position as [x, y]
}

[System.Serializable]
public class LevelData
{
    public int levelNumber; // Level number
    public int gridSize;    // Size of the grid (e.g., 5 for a 5x5 grid)
    public List<NodeData> nodes; // List of nodes in the level
}

[System.Serializable]
public class GameData
{
    public List<LevelData> levels; // List of all levels in the game
}

public class GameManager : MonoBehaviour
{
    [Header("Grid Setup")]
    public GameObject gridParent;       // Parent object to hold grid cells
    public GameObject gridCellPrefab;  // Prefab for grid cells
    public int defaultGridSize = 5;    // Default grid size if level data is missing

    public GameData gameData; // Parsed JSON game data
    public int gridSize;      // Current level's grid size

    void Start()
    {
        int currentLevel = PlayerPrefs.GetInt("SelectedLevel", 1); // Get selected level from PlayerPrefs
        LoadLevelData(currentLevel);
    }

    private void LoadLevelData(int level)
    {
        // Load JSON file from Resources folder
        TextAsset jsonFile = Resources.Load<TextAsset>("LevelData");
        if (jsonFile == null)
        {
            Debug.LogError("LevelData.json not found in Resources folder!");
            return;
        }

        // Parse JSON file
        string jsonData = jsonFile.text;
        try
        {
            gameData = JsonUtility.FromJson<GameData>(jsonData);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error parsing JSON: {ex.Message}");
            return;
        }

        // Find level data
        LevelData levelData = gameData.levels.Find(l => l.levelNumber == level);
        if (levelData != null)
        {
            gridSize = levelData.gridSize;
            CreateGrid();
            PlaceNodes(levelData.nodes);
        }
        else
        {
            Debug.LogError($"Level {level} not found in JSON data! Using default grid size.");
            gridSize = defaultGridSize;
            CreateGrid();
        }
    }

    private void CreateGrid()
    {
        // Clear existing grid cells
        foreach (Transform child in gridParent.transform)
        {
            Destroy(child.gameObject);
        }

        // Define grid cell size and spacing
        float cellSize = 50f; // Adjust size as per your design
        float spacing = 5f;   // Space between cells

        // Create grid cells
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                // Instantiate grid cell prefab
                GameObject newCell = Instantiate(gridCellPrefab, gridParent.transform);

                // Adjust position and size
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(j * (cellSize + spacing), -i * (cellSize + spacing));
                rectTransform.sizeDelta = new Vector2(cellSize, cellSize);

                // Name the cell for debugging
                newCell.name = $"Cell ({i},{j})";
            }
        }
    }

    private void PlaceNodes(List<NodeData> nodes)
    {
        foreach (NodeData node in nodes)
        {
            // Read positions for start and end nodes
            int startX = node.start[0];
            int startY = node.start[1];
            int endX = node.end[0];
            int endY = node.end[1];

            Debug.Log($"Placing {node.color} node from ({startX},{startY}) to ({endX},{endY})");

            // Example visualization (you can modify it as needed)
            CreateMarkerAtPosition(startX, startY, node.color);
            CreateMarkerAtPosition(endX, endY, node.color);
        }
    }

    private void CreateMarkerAtPosition(int x, int y, string colorName)
    {
        // Find the corresponding grid cell
        string cellName = $"Cell ({x},{y})";
        Transform cellTransform = gridParent.transform.Find(cellName);

        if (cellTransform != null)
        {
            // Change cell color or add a marker
            Image cellImage = cellTransform.GetComponent<Image>();
            if (cellImage != null)
            {
                // Convert color name to Color
                Color nodeColor;
                if (ColorUtility.TryParseHtmlString(colorName, out nodeColor))
                {
                    cellImage.color = nodeColor;
                }
                else
                {
                    Debug.LogWarning($"Invalid color name: {colorName}");
                }
            }
        }
        else
        {
            Debug.LogError($"Grid cell not found: {cellName}");
        }
    }
}
