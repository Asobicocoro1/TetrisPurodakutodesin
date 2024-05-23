using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int width = 10;
    public static int height = 20;
    public Transform[,] grid = new Transform[width, height];

    public GameObject[] tetrominoes;

    public static GameManager instance;

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
        // フレームレートを60に固定
        Application.targetFrameRate = 60;
        // テトリミノを生成
        SpawnTetromino();
    }

    public void SpawnTetromino()
    {
        int index = Random.Range(0, tetrominoes.Length);
        Instantiate(tetrominoes[index], new Vector3(width / 2, height, 0), Quaternion.identity);
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
        for (int y = 0; y < height; y++)
        {
            if (IsFullLineAt(y))
            {
                DeleteLine(y);
                MoveAllRowsDown(y + 1);
                y--;
            }
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
}
