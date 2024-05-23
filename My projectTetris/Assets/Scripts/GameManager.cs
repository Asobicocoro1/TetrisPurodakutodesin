using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // ゲームフィールドの幅
    public static int width = 10;
    // ゲームフィールドの高さ
    public static int height = 20;
    // ゲームフィールドの各位置にあるブロックを管理するための配列
    public Transform[,] grid = new Transform[width, height];

    // テトリミノのプレハブ（ブロックのテンプレート）の配列
    public GameObject[] tetrominoes;
    // ゲームオーバー時に表示するテキスト
    public Text gameOverText;

    // GameManagerのインスタンスを保持するための変数
    public static GameManager instance;

    // ゲームオーバー状態を示すフラグ
    private bool isGameOver = false;

    private void Awake()
    {
        // インスタンスがまだ存在しない場合は設定し、存在する場合は削除
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
        // 最初のテトリミノを生成
        SpawnTetromino();
    }

    // 新しいテトリミノを生成するメソッド
    public void SpawnTetromino()
    {
        // ゲームオーバー状態ならブロックを生成しない
        if (isGameOver)
        {
            return;
        }

        // 一番上の行にブロックがあるかチェック
        if (IsGameOver())
        {
            GameOver();
            return;
        }

        // テトリミノをランダムに選んで生成
        int index = Random.Range(0, tetrominoes.Length);
        Instantiate(tetrominoes[index], new Vector3(width / 2, height, 0), Quaternion.identity);
    }

    // 指定された位置がゲームフィールド内にあるかどうかを確認するメソッド
    public bool IsInsideGrid(Vector2 pos)
    {
        // xが0以上width未満、かつyが0以上である場合
        return ((int)pos.x >= 0 && (int)pos.x < width && (int)pos.y >= 0);
    }

    // 座標を整数に丸めるメソッド
    public Vector2 Round(Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    // 指定された位置にブロックがあるかどうかを取得するメソッド
    public Transform GetTransformAtGridPosition(Vector2 pos)
    {
        // yがフィールドの高さより大きい場合はnullを返す
        if (pos.y > height - 1) return null;
        return grid[(int)pos.x, (int)pos.y];
    }

    // ラインが揃っているか確認し、揃っていれば削除するメソッド
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

    // 指定された行が満杯かどうかを確認するメソッド
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

    // 指定された行を削除するメソッド
    void DeleteLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    // 指定された行から上の全ての行を1つ下に移動するメソッド
    void MoveAllRowsDown(int startY)
    {
        for (int y = startY; y < height; y++)
        {
            MoveRowDown(y);
        }
    }

    // 指定された行を1つ下に移動するメソッド
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

    // 一番上の行にブロックがあるかどうかを確認するメソッド
    bool IsGameOver()
    {
        // 一番上の行にブロックがあるかチェック
        for (int x = 0; x < width; x++)
        {
            if (grid[x, height - 1] != null)
            {
                return true;
            }
        }
        return false;
    }

    // ゲームオーバー処理を行うメソッド
    void GameOver()
    {
        // ゲームオーバーテキストを表示
        gameOverText.gameObject.SetActive(true);
        // ゲームオーバーフラグを設定
        isGameOver = true;
    }
}
