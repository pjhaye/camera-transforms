using System;
using UnityEngine;

namespace CameraTransforms
{
    /// <summary>
    /// <see cref="ICameraTransform"/> whose positional values are based on a Unity <see cref="Transform"/>
    /// </summary>
    [RequireComponent(typeof(UnityEngine.Camera))]
    [ExecuteAlways]
    public class CameraTransform : MonoBehaviour, ICameraTransform
    {
        [SerializeField]
        private float _fieldOfView = 90.0f;

        private UnityEngine.Camera _previewCamera;

        private UnityEngine.Camera PreviewCamera
        {
            get
            {
                _previewCamera = GetComponent<UnityEngine.Camera>();
                
                if (_previewCamera == null)
                {
                    _previewCamera = gameObject.AddComponent<UnityEngine.Camera>();
                    _previewCamera.enabled = false;
                }

                if (_previewCamera == null)
                {
                    Debug.LogError($"Could not create preview camera for {nameof(CameraTransform)}");
                }

                return _previewCamera;
            }
        }
        
        public float FieldOfView 
        {
            get
            {
                return _fieldOfView;
            }
            set
            {
                _fieldOfView = value;
            }
        }
    
        public Vector3 Position {
            get
            {
                return transform.position;
            }
            set
            {
                transform.position = value;
            }
        }

        public Vector3 Forward {
            get
            {
                return transform.forward;
            }
        }

        public Vector3 Right
        {
            get
            {
                return transform.right;
            }
        }

        public Quaternion Rotation {
            get
            {
                return transform.rotation;
            }
            set
            {
                transform.rotation = value;
            }
        }
        
        private void Update()
        {
#if UNITY_EDITOR
            var previewCamera = PreviewCamera;
            previewCamera.fieldOfView = _fieldOfView;
            var previewCameraTransform = previewCamera.transform;
            previewCameraTransform.position = transform.position;
            previewCameraTransform.rotation = transform.rotation;
            previewCamera.enabled = false;
#endif
        }
    }
}
