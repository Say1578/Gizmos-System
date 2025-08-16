using UnityEngine;

namespace GizmosSystem
{
    [System.Serializable]
    public class GizmoSphere : GizmoModuleBase
    {
        public Color gizmoColor = Color.cyan;

        public float radius = 0.5f;
        public Vector2 centerOffset = Vector2.zero;

        public bool showGizmo = true;

        public override void DrawGizmos(Transform context)
        {
            if (showGizmo)
            {
                Vector2 center = context.TransformPoint(centerOffset);

                Gizmos.color = gizmoColor;
                Gizmos.DrawWireSphere(center, radius);
            }
        }
    }
}