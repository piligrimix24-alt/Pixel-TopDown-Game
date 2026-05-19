using NavMeshPlus.Components;
using UnityEngine;

public class NavMesgSurfaceManagment : MonoBehaviour
{
    public static NavMesgSurfaceManagment Instance { get; private set; }
    private NavMeshSurface _navMeshSurface;

    private void Awake()
    {
        Instance = this;
        _navMeshSurface = GetComponent<NavMeshSurface>();
        _navMeshSurface.hideEditorLogs = true;
    }
    private void Start()
    {
        RebakeNavMeshSurface();
    }
    public void RebakeNavMeshSurface()
    {
        _navMeshSurface.BuildNavMesh();
    }
}
