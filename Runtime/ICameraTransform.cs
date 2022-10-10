using UnityEngine;

namespace CameraTransforms
{
    public interface ICameraTransform
    {
        /// <summary>
        /// Target Field of View
        /// </summary>
        float FieldOfView { get; set; }
        /// <summary>
        /// Target Position
        /// </summary>
        Vector3 Position { get; set; }
        /// <summary>
        /// Target rotation
        /// </summary>
        Quaternion Rotation { get; set; }
        /// <summary>
        /// Forward direction
        /// </summary>
        Vector3 Forward { get; }
        /// <summary>
        /// Right direction
        /// </summary>
        Vector3 Right { get; }
    }
}
