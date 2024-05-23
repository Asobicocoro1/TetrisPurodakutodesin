using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    // 通常の落下速度
    public float fallSpeed = 1.0f;
    // 前回の落下時間
    private float fallTime = 0;
    // 高速落下の倍率
    private float fastFallMultiplier = 15.0f;

    void Update()
    {
        Move();  // テトリミノの移動処理
        Rotate();  // テトリミノの回転処理
        Fall();  // テトリミノの落下処理
    }

    // テトリミノの移動処理
    void Move()
    {
        // 左矢印キーが押されたとき
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // テトリミノを左に移動
            transform.position += Vector3.left;
            // 移動後の位置が無効な場合、元に戻す
            if (!IsValidPosition())
            {
                transform.position += Vector3.right;
            }
        }
        // 右矢印キーが押されたとき
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // テトリミノを右に移動
            transform.position += Vector3.right;
            // 移動後の位置が無効な場合、元に戻す
            if (!IsValidPosition())
            {
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
            // 回転後の位置が無効な場合、元に戻す
            if (!IsValidPosition())
            {
                transform.Rotate(0, 0, -90);
            }
        }
    }

    // テトリミノの落下処理
    void Fall()
    {
        float currentFallSpeed = fallSpeed;

        // 矢印キー（下）が押されている間、落下速度を10倍にする
        if (Input.GetKey(KeyCode.DownArrow))
        {
            currentFallSpeed /= fastFallMultiplier;
        }

        // 現在の時間と前回の落下時間の差が落下速度より大きい場合、落下を行う
        if (Time.time - fallTime >= currentFallSpeed)
        {
            // テトリミノを下に移動
            transform.position += Vector3.down;
            // 現在の時間をfallTimeに更新
            fallTime = Time.time;
            // 移動後の位置が無効な場合
            if (!IsValidPosition())
            {
                // テトリミノを元の位置に戻す
                transform.position += Vector3.up;
                // テトリミノをグリッドに追加
                AddToGrid();
                // ラインが揃っているかチェックし、揃っていれば削除
                FindObjectOfType<GameManager>().CheckForLines();
                // 新しいテトリミノを生成
                FindObjectOfType<GameManager>().SpawnTetromino();
                // このスクリプトを無効にする
                enabled = false;
            }
        }
    }

    // テトリミノの位置が有効かどうかを確認する関数
    bool IsValidPosition()
    {
        // テトリミノの各ブロックについて
        foreach (Transform child in transform)
        {
            // 子ブロックの位置を丸めた座標を取得
            Vector2 pos = FindObjectOfType<GameManager>().Round(child.position);
            // その位置がグリッドの内側でない場合
            if (!FindObjectOfType<GameManager>().IsInsideGrid(pos))
            {
                return false;
            }
            // その位置に他のブロックが存在する場合
            if (FindObjectOfType<GameManager>().GetTransformAtGridPosition(pos) != null)
            {
                return false;
            }
        }
        return true;
    }

    // テトリミノをグリッドに追加する関数
    void AddToGrid()
    {
        // テトリミノの各ブロックについて
        foreach (Transform child in transform)
        {
            // 子ブロックの位置を丸めた座標を取得
            Vector2 pos = FindObjectOfType<GameManager>().Round(child.position);
            // その位置に子ブロックを追加
            FindObjectOfType<GameManager>().grid[(int)pos.x, (int)pos.y] = child;
        }
    }
}
