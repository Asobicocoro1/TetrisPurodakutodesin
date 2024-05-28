using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int width = 10; // フィールドの幅
    public static int height = 20; // フィールドの高さ
    public Transform[,] grid = new Transform[width, height];

    public GameObject[] tetrominoes; // テトリミノのプレハブ配列
    public Text gameOverText; // ゲームオーバーテキスト

    public static GameManager instance;

    private bool isGameOver = false; // ゲームオーバー状態を示すフラグ
    private bool useBombBlock = false; // 爆弾ブロックを使用するかどうかのフラグ

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Application.targetFrameRate = 60; // フレームレートを60に固定
        SpawnTetromino(); // テトリミノを生成
    }

    public void SpawnTetromino()
    {
        if (isGameOver)
        {
            return;
        }

        if (IsGameOver())
        {
            GameOver();
            return;
        }

        int index = Random.Range(0, tetrominoes.Length);
        GameObject newTetromino = Instantiate(tetrominoes[index], new Vector3(width / 2, height, 0), Quaternion.identity);

        // 爆弾ブロックを含める
        if (useBombBlock)
        {
            AddBombBlockTag(newTetromino);
            useBombBlock = false; // フラグをリセット
        }
    }

    // テトリミノに爆弾ブロックのタグを追加し、色を黒に変更するメソッド
    void AddBombBlockTag(GameObject tetromino)
    {
        // テトリミノのブロックの1つをランダムに選択
        Transform randomChild = tetromino.transform.GetChild(Random.Range(0, tetromino.transform.childCount));
        // 爆弾タグを追加
        randomChild.tag = "bomb";
        // ビジュアル的に爆弾ブロックを区別できるように色を黒に変更
        randomChild.GetComponent<Renderer>().material.color = Color.black;
    }

    public bool IsInsideGrid(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < width && (int)pos.y >= 0);
    }

    public Vector2 Round(Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    public Transform GetTransformAtGridPosition(Vector2 pos)
    {
        if (pos.y > height - 1) return null;
        return grid[(int)pos.x, (int)pos.y];
    }

    public void CheckForLines()
    {
        int linesCleared = 0;

        for (int y = 0; y < height; y++)
        {
            if (IsFullLineAt(y))
            {
                DeleteLine(y);
                MoveAllRowsDown(y + 1);
                y--;
                linesCleared++;
            }
        }

        // 2行同時に消滅した場合
        if (linesCleared >= 2)
        {
            useBombBlock = true; // 爆弾ブロックを使用するフラグをセット
        }
    }

    bool IsFullLineAt(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        return true;
    }

    void DeleteLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    void MoveAllRowsDown(int startY)
    {
        for (int y = startY; y < height; y++)
        {
            MoveRowDown(y);
        }
    }

    void MoveRowDown(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += Vector3.down;
            }
        }
    }

    bool IsGameOver()
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, height - 1] != null)
            {
                return true;
            }
        }
        return false;
    }

    void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        isGameOver = true;
    }
}
