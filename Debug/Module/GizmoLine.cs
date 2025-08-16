using UnityEngine;

namespace GizmosSystem
{
    [System.Serializable]
    public class GizmoLine : GizmoModuleBase
    {
        public Color lineColor = Color.red;

        public Vector2 lineStartOffset = new Vector2(0.3f, 0);
        public Vector2 lineEndOffset = new Vector2(1f, 0);

        public bool showGizmo = true;

        public override void DrawGizmos(Transform context)
        {
            if (showGizmo)
            {
                Vector2 startPoint = context.TransformPoint(lineStartOffset);
                Vector2 endPoint = context.TransformPoint(lineEndOffset);

                Gizmos.color = lineColor;
                Gizmos.DrawLine(startPoint, endPoint);
            }
        }
    }
}