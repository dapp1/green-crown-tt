using UnityEngine;

public class OnPlatform : MonoBehaviour
{
    private void Start()
    {
        GetCollider().occupied = true;
    }

    
    private BoxPoint GetCollider()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if(collider)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(collider.bounds.center, collider.bounds.size, 0);
            foreach (Collider2D col in colliders)
            {
                if (col.gameObject != gameObject)
                {
                    if (TryGetComponent<BoxPoint>(out var otherObject))
                    {
                        return otherObject;
                    }
                }
            }
        }
        return null;
    }
}
