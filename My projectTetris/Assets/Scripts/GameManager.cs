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
        // インスタンスを設定
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

    public void SpawnTetromino()
    {
        // ゲームオーバーの場合は新しいテトリミノを生成しない
        if (isGameOver)
        {
            return;
        }

        // ゲームオーバー状態を確認
        if (IsGameOver())
        {
            GameOver();
            return;
        }

        // ランダムにテトリミノを生成
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
        // 爆弾ブロックスクリプトを追加
        randomChild.gameObject.AddComponent<BombBlock>();
    }

    // 指定された位置がグリッド内にあるか確認
    public bool IsInsideGrid(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < width && (int)pos.y >= 0);
    }

    // 座標を丸める
    public Vector2 Round(Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    // 指定されたグリッド位置にあるTransformを取得
    public Transform GetTransformAtGridPosition(Vector2 pos)
    {
        if (pos.y > height - 1) return null;
        return grid[(int)pos.x, (int)pos.y];
    }

    // ラインが揃ったかどうかを確認し、揃った場合は削除する
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

        // 2行同時に消滅した場合、爆弾ブロックを使用するフラグをセット
        if (linesCleared >= 2)
        {
            useBombBlock = true;
        }
    }

    // 指定された行が満杯かどうかを確認
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

    // 指定された行を削除
    void DeleteLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] != null)
            {
                // 爆弾ブロックの爆発処理を追加
                if (grid[x, y].CompareTag("bomb"))
                {
                    Debug.Log($"Bomb block at grid[{x}, {y}] exploded.");
                    TriggerBombExplosion(grid[x, y].gameObject);
                }

                Destroy(grid[x, y].gameObject);
                grid[x, y] = null;
            }
            else
            {
                Debug.LogWarning($"grid[{x}, {y}] is null");
            }
        }
    }

    // 指定された行の上にある全ての行を1つ下に移動
    void MoveAllRowsDown(int startY)
    {
        for (int y = startY; y < height; y++)
        {
            MoveRowDown(y);
        }
    }

    // 指定された行を1つ下に移動
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

    // ゲームオーバー状態を確認
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

    // ゲームオーバー処理
    void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        isGameOver = true;
    }

    // 爆弾ブロックの爆発処理
    void TriggerBombExplosion(GameObject bomb)
    {
        if (bomb == null)
        {
            Debug.LogWarning("Bomb object is null");
            return;
        }

        BombBlock[] bombBlocks = FindObjectsOfType<BombBlock>();

        // 爆弾ブロックの数によって処理を変更
        if (bombBlocks.Length == 1)
        {
            BombBlock bombBlockComponent = bomb.GetComponent<BombBlock>();
            if (bombBlockComponent != null)
            {
                bombBlockComponent.Explode(2); // 爆発範囲が2
            }
            else
            {
                Debug.LogWarning("BombBlock component is null");
            }
        }
        else if (bombBlocks.Length == 2)
        {
            BombBlock bombBlockComponent = bomb.GetComponent<BombBlock>();
            if (bombBlockComponent != null)
            {
                bombBlockComponent.Explode(4); // 爆発範囲が4
            }
            else
            {
                Debug.LogWarning("BombBlock component is null");
            }
        }
        else if (bombBlocks.Length >= 3)
        {
            // すべての爆弾ブロックの位置に応じて、その行全体のブロックを削除
            foreach (BombBlock bombBlock in bombBlocks)
            {
                int row = (int)Round(bombBlock.transform.position).y;
                for (int x = 0; x < width; x++)
                {
                    if (grid[x, row] != null)
                    {
                        Destroy(grid[x, row].gameObject);
                        grid[x, row] = null;
                    }
                }
            }
        }
    }
}
