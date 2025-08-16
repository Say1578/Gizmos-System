using System.Collections.Generic;
using UnityEngine;

namespace GizmosSystem
{
    [ExecuteAlways]
    public class GizmosDrawer : MonoBehaviour
    {
        [SerializeReference]
        public List<GizmoModuleBase> gizmoModules = new();

        private void OnDrawGizmos()
        {
            foreach (var module in gizmoModules)
            {
                module?.DrawGizmos(transform);
            }
        }
    }
}
