using UnityEngine;

public class Tetromino : MonoBehaviour
{
    // 通常の落下速度
    public float fallSpeed = 1.0f;
    // 前回の落下時間
    private float fallTime = 0;
    // 高速落下の倍率
    private float fastFallMultiplier = 10.0f;

    void Update()
    {
        // 毎フレームごとに移動、回転、落下の処理を行う
        Move();
        Rotate();
        Fall();
    }

    // テトリミノの移動処理
    void Move()
    {
        // 左矢印キーが押されたとき
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // テトリミノを左に移動
            transform.position += Vector3.left;
            // 移動後の位置が有効か確認
            if (!IsValidPosition())
            {
                // 無効な場合は元の位置に戻す
                transform.position += Vector3.right;
            }
        }
        // 右矢印キーが押されたとき
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // テトリミノを右に移動
            transform.position += Vector3.right;
            // 移動後の位置が有効か確認
            if (!IsValidPosition())
            {
                // 無効な場合は元の位置に戻す
                transform.position += Vector3.left;
            }
        }
    }

    // テトリミノの回転処理
    void Rotate()
    {
        // 上矢印キーが押されたとき
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // テトリミノを90度回転
            transform.Rotate(0, 0, 90);
            // 回転後の位置が有効か確認
            if (!IsValidPosition())
            {
                // 無効な場合は元の位置に戻す
                transform.Rotate(0, 0, -90);
            }
        }
    }

    // テトリミノの落下処理
    void Fall()
    {
        float currentFallSpeed = fallSpeed;

        // 下矢印キーが押されているときは落下速度を速くする
        if (Input.GetKey(KeyCode.DownArrow))
        {
            currentFallSpeed /= fastFallMultiplier;
        }

        // 前回の落下時間から現在の時間を引いた値が現在の落下速度以上の場合
        if (Time.time - fallTime >= currentFallSpeed)
        {
            // テトリミノを下に移動
            transform.position += Vector3.down;
            // 落下時間を現在の時間に更新
            fallTime = Time.time;
            // 移動後の位置が有効か確認
            if (!IsValidPosition())
            {
                // 無効な場合は元の位置に戻す
                transform.position += Vector3.up;
                // グリッドにテトリミノを追加
                AddToGrid();
                // ラインが揃っているか確認し、揃っている場合は削除
                FindObjectOfType<GameManager>().CheckForLines();
                // 新しいテトリミノを生成
                FindObjectOfType<GameManager>().SpawnTetromino();
                // 現在のテトリミノを無効にする
                enabled = false;
            }
        }
    }

    // テトリミノの位置が有効か確認するメソッド
    bool IsValidPosition()
    {
        // テトリミノの各ブロックの位置を確認
        foreach (Transform child in transform)
        {
            Vector2 pos = FindObjectOfType<GameManager>().Round(child.position);
            // ブロックの位置がグリッド内か確認
            if (!FindObjectOfType<GameManager>().IsInsideGrid(pos))
            {
                return false;
            }
            // その位置に既にブロックがあるか確認
            if (FindObjectOfType<GameManager>().GetTransformAtGridPosition(pos) != null)
            {
                return false;
            }
        }
        return true;
    }

    // テトリミノをグリッドに追加するメソッド
    void AddToGrid()
    {
        // テトリミノの各ブロックをグリッドに追加
        foreach (Transform child in transform)
        {
            Vector2 pos = FindObjectOfType<GameManager>().Round(child.position);
            // グリッドの範囲外に追加しないようにチェック
            if ((int)pos.x >= 0 && (int)pos.x < GameManager.width && (int)pos.y >= 0 && (int)pos.y < GameManager.height)
            {
                FindObjectOfType<GameManager>().grid[(int)pos.x, (int)pos.y] = child;
            }
        }
    }
}
