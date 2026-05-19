using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    public static ActiveWeapon Instance { get; private set; }
    [SerializeField] private Sword sword;
    //=================================================================================================================
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (Player.Instance.IsAlive())
        {
            AdjustWeaponDirection();
        }
    }
    //=================================================================================================================
    public Sword GetActiveWeapon() => sword;
    //=================================================================================================================
    private void AdjustWeaponDirection()
    {
        int directionRunning = Player.Instance.DirectionRunning();
        Vector3 mousePosition = GameInput.Instance.GetMousePosition();
        Vector3 playerPosition = Player.Instance.GetPlayerPosition();
        if (directionRunning < 0) { transform.rotation = Quaternion.Euler(0, 180, 0); }
        else if (directionRunning == 0)
        {
            if (mousePosition.x < playerPosition.x) { transform.rotation = Quaternion.Euler(0, 180, 0); }
            else { transform.rotation = Quaternion.Euler(0, 0, 0); }
        }
        else { transform.rotation = Quaternion.Euler(0, 0, 0); }
    }
}
