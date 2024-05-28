using UnityEngine;

public class BombBlock : MonoBehaviour
{
    // ”š’e‚ª”j—ô‚·‚é”ÍˆÍi—á‚¦‚ÎA3x3‚Ì”ÍˆÍj
    public float explosionRadius = 1.5f;
    public LayerMask blockLayer;

    void OnDestroy()
    {
        // ”š”­Œø‰Ê‚ğ”­“®
        Collider[] blocksToDestroy = Physics.OverlapSphere(transform.position, explosionRadius, blockLayer);
        foreach (Collider block in blocksToDestroy)
        {
            Destroy(block.gameObject);
        }
    }
}
