using UnityEngine;

namespace CameraTransforms
{
    /// <summary>
    /// Simple assignable CameraTransform values
    /// </summary>
    public struct SnapshotCameraTransform : ICameraTransform
    {
        public float FieldOfView { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Forward { get; set; }
        public Vector3 Right { get; set; }
        public Quaternion Rotation { get; set; }
    }
}
