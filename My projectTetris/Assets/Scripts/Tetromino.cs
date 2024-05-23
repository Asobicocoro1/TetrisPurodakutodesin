using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public float fallSpeed = 1.0f;
    private float fallTime = 0;
    private float fastFallMultiplier = 10.0f;

    void Update()
    {
        Move();
        Rotate();
        Fall();
    }

    void Move()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left;
            if (!IsValidPosition())
            {
                transform.position += Vector3.right;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += Vector3.right;
            if (!IsValidPosition())
            {
                transform.position += Vector3.left;
            }
        }
    }

    void Rotate()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, 90);
            if (!IsValidPosition())
            {
                transform.Rotate(0, 0, -90);
            }
        }
    }

    void Fall()
    {
        float currentFallSpeed = fallSpeed;

        if (Input.GetKey(KeyCode.DownArrow))
        {
            currentFallSpeed /= fastFallMultiplier;
        }

        if (Time.time - fallTime >= currentFallSpeed)
        {
            transform.position += Vector3.down;
            fallTime = Time.time;
            if (!IsValidPosition())
            {
                transform.position += Vector3.up;
                AddToGrid();
                FindObjectOfType<GameManager>().CheckForLines();
                FindObjectOfType<GameManager>().SpawnTetromino();
                enabled = false;
            }
        }
    }

    bool IsValidPosition()
    {
        foreach (Transform child in transform)
        {
            Vector2 pos = FindObjectOfType<GameManager>().Round(child.position);
            if (!FindObjectOfType<GameManager>().IsInsideGrid(pos))
            {
                return false;
            }
            if (FindObjectOfType<GameManager>().GetTransformAtGridPosition(pos) != null)
            {
                return false;
            }
        }
        return true;
    }

    void AddToGrid()
    {
        foreach (Transform child in transform)
        {
            Vector2 pos = FindObjectOfType<GameManager>().Round(child.position);
            FindObjectOfType<GameManager>().grid[(int)pos.x, (int)pos.y] = child;
        }
    }
}
