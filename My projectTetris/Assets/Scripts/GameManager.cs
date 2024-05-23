using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ゲームボードの幅
    public static int width = 10;
    // ゲームボードの高さ
    public static int height = 20;
    // ゲームボード上の各位置にブロックがあるかどうかを管理する2次元配列
    public Transform[,] grid = new Transform[width, height];

    // テトリミノのプレハブ（作成済みのブロックオブジェクト）の配列
    public GameObject[] tetrominoes;

    // GameManagerクラスのインスタンス（実体）を保持するための変数
    public static GameManager instance;

    private void Awake()
    {
        // インスタンスがまだ存在しない場合、現在のインスタンスを設定
        if (instance == null)
        {
            instance = this;
        }
        // 既にインスタンスが存在する場合、現在のオブジェクトを破棄
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // フレームレートを60に固定
        Application.targetFrameRate = 30;
        // テトリミノを生成
        SpawnTetromino();
    }

    // テトリミノをランダムに生成する関数
    public void SpawnTetromino()
    {
        // tetrominoes配列からランダムに1つのテトリミノを選んで生成
        int index = Random.Range(0, tetrominoes.Length);
        Instantiate(tetrominoes[index], new Vector3(width / 2, height, 0), Quaternion.identity);
    }

    // 指定した位置がゲームボード内にあるかどうかを確認する関数
    public bool IsInsideGrid(Vector2 pos)
    {
        // xが0以上であり、かつwidthより小さい場合、yが0以上である場合
        return ((int)pos.x >= 0 && (int)pos.x < width && (int)pos.y >= 0);
    }

    // 座標を丸める関数
    public Vector2 Round(Vector2 pos)
    {
        // 座標を四捨五入する
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    // 指定した位置にブロックがあるかどうかを取得する関数
    public Transform GetTransformAtGridPosition(Vector2 pos)
    {
        // yがheight-1より大きい場合はnullを返す
        if (pos.y > height - 1) return null;
        // grid配列の指定位置にあるTransformを返す
        return grid[(int)pos.x, (int)pos.y];
    }

    // ラインが揃っているかどうかを確認し、揃っていれば消去する関数
    public void CheckForLines()
    {
        // 各ラインをチェック
        for (int y = 0; y < height; y++)
        {
            // ラインが揃っている場合
            if (IsFullLineAt(y))
            {
                // ラインを削除
                DeleteLine(y);
                // 上の全てのラインを下に移動
                MoveAllRowsDown(y + 1);
                // チェックをやり直すため、インデックスをデクリメント
                y--;
            }
        }
    }

    // 指定したラインが満杯かどうかを確認する関数
    bool IsFullLineAt(int y)
    {
        // ラインの各位置をチェック
        for (int x = 0; x < width; x++)
        {
            // 指定位置が空の場合、ラインは満杯ではない
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        // ラインが満杯の場合
        return true;
    }

    // 指定したラインを削除する関数
    void DeleteLine(int y)
    {
        // ラインの各位置を削除
        for (int x = 0; x < width; x++)
        {
            // オブジェクトを破棄
            Destroy(grid[x, y].gameObject);
            // 配列の位置をnullに設定
            grid[x, y] = null;
        }
    }

    // 全てのラインを下に移動させる関数
    void MoveAllRowsDown(int startY)
    {
        // 各ラインを下に移動
        for (int y = startY; y < height; y++)
        {
            MoveRowDown(y);
        }
    }

    // 指定したラインを下に移動させる関数
    void MoveRowDown(int y)
    {
        // ラインの各位置を下に移動
        for (int x = 0; x < width; x++)
        {
            // 指定位置にブロックがある場合
            if (grid[x, y] != null)
            {
                // ブロックを1つ下に移動
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += Vector3.down;
            }
        }
    }
}
