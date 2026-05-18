using UnityEngine;

public class DepthController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    BoxCollider boxCollider;
    private int currentDepth;
    public int CurrentDepth
    {
        get { return currentDepth; }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        SetDepth(0);
    }

    public void SetDepth(int depth)
    {
        currentDepth = depth;
        spriteRenderer.sortingOrder = CurrentDepth;
        Vector3 newSize = boxCollider.size;
        newSize.z = 0.1f + (CurrentDepth * 0.05f);
    }
}
