using UnityEngine;

namespace GizmosSystem
{
    [System.Serializable]
    public class GizmosCircle : GizmoModuleBase
    {
        public Color gizmoColor;

        public bool showGizmo = true;

        public override void DrawGizmos(Transform context)
        {
            if (showGizmo)
            {
                
            }
        }
    }
}