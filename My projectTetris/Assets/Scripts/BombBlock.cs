using UnityEngine;

public class BombBlock : MonoBehaviour
{
    // 爆弾が破裂する範囲
    public void Explode(int explosionRadius)
    {
        GameManager gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogWarning("GameManager not found");
            return;
        }

        Vector2 pos = gameManager.Round(transform.position);

        // 指定された範囲のブロックを削除する
        for (int x = (int)pos.x - explosionRadius; x <= (int)pos.x + explosionRadius; x++)
        {
            for (int y = (int)pos.y - explosionRadius; y <= (int)pos.y + explosionRadius; y++)
            {
                // グリッド内かどうか確認
                if (x >= 0 && x < GameManager.width && y >= 0 && y < GameManager.height)
                {
                    // ブロックが存在する場合削除
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
