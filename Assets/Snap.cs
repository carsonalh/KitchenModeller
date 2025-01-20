using UnityEngine;

public class Snap : MonoBehaviour
{
    public Vector3 normal;
    public float snapDistance = 0.5f;
    public BoxCollider boxCollider { get; private set; }

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        Debug.Assert(boxCollider != null, "A snap component must have a box collider");
    }
}
