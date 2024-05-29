using UnityEngine;

public class BombBlock : MonoBehaviour
{
    // ���e���j�􂷂�͈�
    public void Explode(int explosionRadius)
    {
        GameManager gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogWarning("GameManager not found");
            return;
        }

        Vector2 pos = gameManager.Round(transform.position);

        // �w�肳�ꂽ�͈͂̃u���b�N���폜����
        for (int x = (int)pos.x - explosionRadius; x <= (int)pos.x + explosionRadius; x++)
        {
            for (int y = (int)pos.y - explosionRadius; y <= (int)pos.y + explosionRadius; y++)
            {
                // �O���b�h�����ǂ����m�F
                if (x >= 0 && x < GameManager.width && y >= 0 && y < GameManager.height)
                {
                    // �u���b�N�����݂���ꍇ�폜
                    if (gameManager.grid[x, y] != null)
                    {
                        Destroy(gameManager.grid[x, y].gameObject);
                        gameManager.grid[x, y] = null;
                    }
                }
            }
        }
    }
}
