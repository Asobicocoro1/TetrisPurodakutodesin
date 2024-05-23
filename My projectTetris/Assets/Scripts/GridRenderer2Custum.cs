using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridRenderer2Custum : MonoBehaviour

{
    public int width = 10; // �t�B�[���h�̕�
    public int height = 20; // �t�B�[���h�̍���
    public float cellSize = 1.0f; // �e�Z���̃T�C�Y
    public Color lineColor = Color.white; // �O���b�h���C���̐F

    private void OnDrawGizmos()
    {
        Gizmos.color = lineColor;

        // �O���b�h�̐���x������-0.5�Ay������-0.5���炷���߂̃I�t�Z�b�g
        float xOffset = -0.5f;
        float yOffset = -0.5f;

        // ��������`��
        for (int x = 0; x <= width; x++)
        {
            Gizmos.DrawLine(new Vector3((x + xOffset) * cellSize, yOffset * cellSize, 0), new Vector3((x + xOffset) * cellSize, (height + yOffset) * cellSize, 0));
        }

        // ��������`��
        for (int y = 0; y <= height; y++)
        {
            Gizmos.DrawLine(new Vector3(xOffset * cellSize, (y + yOffset) * cellSize, 0), new Vector3((width + xOffset) * cellSize, (y + yOffset) * cellSize, 0));
        }
    }
}




