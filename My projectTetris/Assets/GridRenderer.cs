using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GridRenderer : MonoBehaviour
{
    public int width = 10;
    public int height = 20;
    public float cellSize = 1.0f;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = (width + 1) * 2 + (height + 1) * 2;
        lineRenderer.useWorldSpace = true;

        DrawGrid();
    }

    void DrawGrid()
    {
        int index = 0;

        for (int x = 0; x <= width; x++)
        {
            lineRenderer.SetPosition(index++, new Vector3(x * cellSize, 0, 0));
            lineRenderer.SetPosition(index++, new Vector3(x * cellSize, height * cellSize, 0));
        }

        for (int y = 0; y <= height; y++)
        {
            lineRenderer.SetPosition(index++, new Vector3(0, y * cellSize, 0));
            lineRenderer.SetPosition(index++, new Vector3(width * cellSize, y * cellSize, 0));
        }
    }
}
