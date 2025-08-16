using UnityEngine;

namespace GizmosSystem
{
    [System.Serializable]
    public abstract class GizmoModuleBase
    {
        public abstract void DrawGizmos(Transform context);
    }
}
