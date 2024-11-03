using UnityEngine;

#if UNITY_EDITOR
namespace Helpers
{
    [ExecuteInEditMode]
    public class CameraFrustumDrawer : MonoBehaviour 
    {
        [Header("Gizmo Settings")]
        public bool showGizmo = true;
        public Color frustumColor = Color.white;
        [Range(0.1f, 5f)]
        public float directionLineLength = 2f;
        
        private void OnDrawGizmos()
        {
            if (!showGizmo) return;

            var cam = GetComponent<Camera>();
            if (cam == null) return;

            Matrix4x4 matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.matrix = matrix;

            Gizmos.color = frustumColor;
            Gizmos.DrawRay(Vector3.zero, Vector3.forward * directionLineLength);

            var near = cam.nearClipPlane;
            var far = cam.farClipPlane;
            var aspect = cam.aspect;
            var fov = cam.fieldOfView * 0.5f * Mathf.Deg2Rad;
            
            var nearHeight = Mathf.Tan(fov) * near;
            var nearWidth = nearHeight * aspect;
            var farHeight = Mathf.Tan(fov) * far;
            var farWidth = farHeight * aspect;

            var nearCorners = new Vector3[4];
            nearCorners[0] = new Vector3(nearWidth, nearHeight, near);
            nearCorners[1] = new Vector3(-nearWidth, nearHeight, near);
            nearCorners[2] = new Vector3(-nearWidth, -nearHeight, near);
            nearCorners[3] = new Vector3(nearWidth, -nearHeight, near);

            var farCorners = new Vector3[4];
            farCorners[0] = new Vector3(farWidth, farHeight, far);
            farCorners[1] = new Vector3(-farWidth, farHeight, far);
            farCorners[2] = new Vector3(-farWidth, -farHeight, far);
            farCorners[3] = new Vector3(farWidth, -farHeight, far);

            Gizmos.color = frustumColor;
            for (int i = 0; i < 4; i++)
            {
                Gizmos.DrawLine(nearCorners[i], nearCorners[(i + 1) % 4]);
            }

            Gizmos.color = frustumColor;
            for (int i = 0; i < 4; i++)
            {
                Gizmos.DrawLine(farCorners[i], farCorners[(i + 1) % 4]);
            }

            Gizmos.color = frustumColor;
            for (int i = 0; i < 4; i++)
            {
                Gizmos.DrawLine(Vector3.zero, nearCorners[i]);
                Gizmos.DrawLine(nearCorners[i], farCorners[i]);
            }
        }
    }
}
#endif