using UnityEngine;

public class Node : MonoBehaviour
{
    [Header("Node Components")]
    [SerializeField] private GameObject _point;        // Central point of the node
    [SerializeField] private GameObject _topEdge;      // Top edge of the node
    [SerializeField] private GameObject _bottomEdge;   // Bottom edge of the node
    [SerializeField] private GameObject _leftEdge;     // Left edge of the node
    [SerializeField] private GameObject _rightEdge;    // Right edge of the node

    [HideInInspector] public Vector2Int Position;      // Position in the grid
    public int ColorID { get; private set; }           // Unique color ID for the node

    public void ResetNode()
    {
        // Reset state
        _point.SetActive(false);
        _topEdge.SetActive(false);
        _bottomEdge.SetActive(false);
        _leftEdge.SetActive(false);
        _rightEdge.SetActive(false);
        ColorID = -1; // Reset color ID
    }

    public void SetColor(int colorId, Color color)
    {
        // Activate the point and assign its color
        ColorID = colorId;
        _point.SetActive(true);
        _point.GetComponent<SpriteRenderer>().color = color;
    }

    public void ActivateEdge(Vector2Int direction)
    {
        // Activate the appropriate edge
        if (direction == Vector2Int.up)
            _topEdge.SetActive(true);
        else if (direction == Vector2Int.down)
            _bottomEdge.SetActive(true);
        else if (direction == Vector2Int.left)
            _leftEdge.SetActive(true);
        else if (direction == Vector2Int.right)
            _rightEdge.SetActive(true);
    }
}
