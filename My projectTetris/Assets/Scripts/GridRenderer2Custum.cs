using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridRenderer2Custum : MonoBehaviour

{
    public int width = 10; // フィールドの幅
    public int height = 20; // フィールドの高さ
    public float cellSize = 1.0f; // 各セルのサイズ
    public Color lineColor = Color.white; // グリッドラインの色

    private void OnDrawGizmos()
    {
        Gizmos.color = lineColor;

        // グリッドの線をx方向に-0.5、y方向に-0.5ずらすためのオフセット
        float xOffset = -0.5f;
        float yOffset = -0.5f;

        // 垂直線を描画
        for (int x = 0; x <= width; x++)
        {
            Gizmos.DrawLine(new Vector3((x + xOffset) * cellSize, yOffset * cellSize, 0), new Vector3((x + xOffset) * cellSize, (height + yOffset) * cellSize, 0));
        }

        // 水平線を描画
        for (int y = 0; y <= height; y++)
        {
            Gizmos.DrawLine(new Vector3(xOffset * cellSize, (y + yOffset) * cellSize, 0), new Vector3((width + xOffset) * cellSize, (y + yOffset) * cellSize, 0));
        }
    }
}




