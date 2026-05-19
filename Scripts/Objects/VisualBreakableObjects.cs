using System;
using UnityEngine;

public class VisualBreakableObjects : MonoBehaviour
{
    [SerializeField] private BreakableObjects breakableObject;
    [SerializeField] private GameObject tvDeathVFXPrefab;
    //===============================================================================
    private void Start()
    {
        breakableObject.OnBreakableTakeDamage += BreakableObject_OnBreakableTakeDamage;
    }
    private void OnDestroy()
    {
        breakableObject.OnBreakableTakeDamage -= BreakableObject_OnBreakableTakeDamage;
    }
    //===============================================================================
    private void ShowDeathVFX()
    {
        Instantiate(tvDeathVFXPrefab, breakableObject.transform.position, Quaternion.identity);
    }
    private void BreakableObject_OnBreakableTakeDamage(object sender, EventArgs e)
    {
        ShowDeathVFX();
    }
}
