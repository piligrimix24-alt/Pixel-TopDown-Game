using System;
using UnityEngine;

public class BreakableObjects : MonoBehaviour
{
    public event EventHandler OnBreakableTakeDamage;
    //===============================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Sword>())
        {
            OnBreakableTakeDamage?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
            NavMesgSurfaceManagment.Instance.RebakeNavMeshSurface();
        }
    }
}
