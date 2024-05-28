using UnityEngine;

public class BombBlock : MonoBehaviour
{
    // ���e���j�􂷂�͈́i�Ⴆ�΁A3x3�͈̔́j
    public float explosionRadius = 1.5f;
    public LayerMask blockLayer;

    void OnDestroy()
    {
        // �������ʂ𔭓�
        Collider[] blocksToDestroy = Physics.OverlapSphere(transform.position, explosionRadius, blockLayer);
        foreach (Collider block in blocksToDestroy)
        {
            Destroy(block.gameObject);
        }
    }
}
