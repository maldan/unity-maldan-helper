using UnityEngine;

namespace Camera
{
    public class CameraRestrictionZone : MonoBehaviour
    {
        public Vector3 Padding;

        public bool IsSetCameraRotation;
        public Vector3 SetCameraRotation;

        public bool IsSetZoom;
        public float Zoom;

        public bool IsSetOffset;
        public Vector3 Offset;
        
        public bool IsRestrictMinX;
        public bool IsRestrictMaxX;
        public bool IsRestrictMinY;
        public bool IsRestrictMaxY;
        public bool IsRestrictMinZ;
        public bool IsRestrictMaxZ;

        public float LeftBound =>
            IsRestrictMinX ? transform.position.x - transform.localScale.x / 2f + Padding.x : float.MinValue;

        public float RightBound =>
            IsRestrictMaxX ? transform.position.x + transform.localScale.x / 2f - Padding.x : float.MaxValue;

        public float BackBound =>
            IsRestrictMinZ ? transform.position.z - transform.localScale.z / 2f + Padding.z : float.MinValue;

        public float FrontBound =>
            IsRestrictMaxZ ? transform.position.z + transform.localScale.z / 2f - Padding.z : float.MaxValue;

        public float BottomBound =>
            IsRestrictMinY ? transform.position.y - transform.localScale.y / 2f + Padding.y : float.MinValue;

        public float TopBound =>
            IsRestrictMaxY ? transform.position.y + transform.localScale.y / 2f - Padding.y : float.MaxValue;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, transform.localScale - Padding);
            
            Gizmos.color = Color.red;
            
            if (IsRestrictMinX) Gizmos.DrawSphere(new Vector3(LeftBound, transform.position.y, transform.position.z), 1f);
            if (IsRestrictMaxX) Gizmos.DrawSphere(new Vector3(RightBound, transform.position.y, transform.position.z), 1f);
            
            if (IsRestrictMinY) Gizmos.DrawSphere(new Vector3(transform.position.x, BottomBound, transform.position.z), 1f);
            if (IsRestrictMaxY) Gizmos.DrawSphere(new Vector3(transform.position.x, TopBound, transform.position.z), 1f);
            
            if (IsRestrictMinZ) Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y, BackBound), 1f);
            if (IsRestrictMaxZ) Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y, FrontBound), 1f);
        }

        public void ApplyToCamera(TopCamera cameraObject)
        {
            cameraObject.transform.position = new Vector3(
                Mathf.Clamp(cameraObject.transform.position.x, LeftBound, RightBound),
                Mathf.Clamp(cameraObject.transform.position.y, BottomBound, TopBound),
                Mathf.Clamp(cameraObject.transform.position.z, BackBound, FrontBound)
            );

            if (IsSetCameraRotation)
            {
                cameraObject.transform.rotation = Quaternion.Lerp(cameraObject.transform.rotation, Quaternion.Euler(SetCameraRotation), Time.deltaTime * 3f);
            }
            
            if (IsSetZoom)
            {
                cameraObject.Zoom += (Zoom - cameraObject.Zoom) / (0.3f / Time.deltaTime);
            }

            if (IsSetOffset)
            {
                cameraObject.OffsetY = Offset.y;
                cameraObject.OffsetZ = Offset.z;
            }
        }
    }
}