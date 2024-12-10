using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class NodeData
{
    public string color;
    public int[] start, end;
}

[System.Serializable]
public class LevelData
{
    public int levelNumber, gridSize;
    public List<NodeData> nodes;
}

[System.Serializable]
public class GameData
{
    public List<LevelData> levels;
}

public class GameManager : MonoBehaviour
{
    [Header("Grid Setup")]
    public GameObject gridParent,gridCellPrefab;
    public int defaultGridSize = 5, gridSize, currentLevel;

    public GameData gameData; //JSON game data (parsed)
    public TMP_Text winText;

    void Start()
    {
        winText.gameObject.SetActive(false);
        currentLevel = PlayerPrefs.GetInt("SelectedLevel", 1);
        Debug.Log($"Loading level {currentLevel}");
        LoadLevelData(currentLevel);
    }

    private void LoadLevelData(int level)
    {        
        TextAsset jsonFile = Resources.Load<TextAsset>("LevelData");
        if (jsonFile == null)
        {
            Debug.LogError("LevelData.json not found in Resources folder!");
            return;
        }
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
        foreach (Transform child in gridParent.transform)
        {
            Destroy(child.gameObject);
        }
        float cellSize = 50f, spacing = 5f;
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                GameObject newCell = Instantiate(gridCellPrefab, gridParent.transform);
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(j * (cellSize + spacing), -i * (cellSize + spacing));
                rectTransform.sizeDelta = new Vector2(cellSize, cellSize);
                newCell.name = $"Cell ({i},{j})";
            }
        }
    }

    private void PlaceNodes(List<NodeData> nodes)
    {
        foreach (NodeData node in nodes)
        {
            int startX = node.start[0];
            int startY = node.start[1];
            int endX = node.end[0];
            int endY = node.end[1];
            CreateMarkerAtPosition(startX, startY, node.color);
            CreateMarkerAtPosition(endX, endY, node.color);
        }
    }

    private void CreateMarkerAtPosition(int x, int y, string colorName)
    {
        string cellName = $"Cell ({x},{y})";
        Transform cellTransform = gridParent.transform.Find(cellName);

        if (cellTransform != null)
        {
            Image cellImage = cellTransform.GetComponent<Image>();
            if (cellImage != null)
            {
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

    public Vector2Int GetEndpointForColor(Color color)
    {
        LevelData currentLevelData = gameData.levels.Find(l => l.levelNumber == currentLevel);

        if (currentLevelData != null)
        {
            foreach (var node in currentLevelData.nodes)
            {
                if (ColorUtility.TryParseHtmlString(node.color, out Color nodeColor) && nodeColor == color)
                {
                    return new Vector2Int(node.end[0], node.end[1]); // Return endpoint for the color
                }
            }
        }
        Debug.LogError($"No endpoint found for color {color} in level {currentLevel}!");
        return Vector2Int.one * -1; // Invalid endpoint
    }

    public Vector2Int GetCellPosition(string cellName)
    {
        string[] parts = cellName.Replace("Cell (", "").Replace(")", "").Split(',');
        int x = int.Parse(parts[0]);
        int y = int.Parse(parts[1]);
        return new Vector2Int(x, y);
    }

    public void ShowWinText()
    {
        if (winText != null)
        {
            winText.gameObject.SetActive(true);
        }
    }

}
