#if CAMERA_TRANSFORMS_DOTWEEN
using DG.Tweening;
using DG.Tweening.Core.Easing;
#endif
using UnityEngine;

namespace CameraTransforms
{
    /// <summary>
    /// Blendable camera that takes on the transform of an assigned <see cref="ICameraTransform"/>
    /// </summary>
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class GameCamera : MonoBehaviour
    {
        private static GameCamera _mainCamera;

        [SerializeField]
        private bool _isMainCamera = false;
        
        private UnityEngine.Camera _camera;
        private ICameraTransform _prevCameraTransform;
        private ICameraTransform _cameraTransform;
        
#if CAMERA_TRANSFORMS_DOTWEEN
        private float _cameraBlendTime = 1.0f;
        private float _blendDuration = 1.0f;
        private Ease _easeType = Ease.Linear;
#endif

        public static GameCamera MainCamera
        {
            get => _mainCamera;
            set => _mainCamera = value;
        }

        /// <summary>
        /// Reference to the Unity Camera
        /// </summary>
        public UnityEngine.Camera Camera
        {
            get
            {
                if (_camera == null)
                {
                    _camera = GetComponent<Camera>();
                }
                
                return _camera;
            }
        }
    
        /// <summary>
        /// Currently assigned <see cref="ICameraTransform"/>
        /// </summary>
        public ICameraTransform CameraTransform
        {
            get
            {
                return _cameraTransform;
            }
        }

        private void Awake()
        {
            if (_mainCamera == null || _isMainCamera)
            {
                _mainCamera = this;
            }
        }

        /// <summary>
        /// Instantly sets the position of this camera to the target <see cref="ICameraTransform"/>
        /// </summary>
        /// <param name="cameraTransform"></param>
        public void JumpToCameraTransform(ICameraTransform cameraTransform)
        {
            _cameraTransform = cameraTransform;
#if CAMERA_TRANSFORMS_DOTWEEN
            _cameraBlendTime = 1.0f;
#endif
        }

#if CAMERA_TRANSFORMS_DOTWEEN
        /// <summary>
        /// Smoothly transitions this camera to the target <see cref="CameraTransform"/>
        /// </summary>
        public void BlendToCameraTransform(ICameraTransform targetTransform, float duration = 1.0f)
        {
            BlendToCameraTransform(targetTransform, Ease.Linear, false, duration);
        }

        /// <summary>
        /// Smoothly transitions this camera to the target <see cref="CameraTransform"/>
        /// </summary>
        public void BlendToCameraTransform(ICameraTransform targetTransform, Ease easeType, float duration)
        {
            BlendToCameraTransform(targetTransform, easeType, false, duration);
        }
    
        /// <summary>
        /// Smoothly transitions this camera to the target <see cref="CameraTransform"/>
        /// </summary>
        public void BlendToCameraTransform(ICameraTransform targetTransform, Ease easeType, bool freezeCurrentTransform, float duration = 1.0f)
        {
            _cameraBlendTime = 0.0f;
            _blendDuration = duration;

            if (freezeCurrentTransform || _cameraTransform == null)
            {
                _prevCameraTransform = new SnapshotCameraTransform
                {
                    Position = transform.position, 
                    Rotation = transform.rotation,
                    FieldOfView = Camera.fieldOfView,
                    Forward = transform.forward,
                    Right = transform.right
                };
            }
            else
            {
                _prevCameraTransform = _cameraTransform;    
            }
        
            _cameraTransform = targetTransform;
            _easeType = easeType;
        }
#endif
        
#if CAMERA_TRANSFORMS_DOTWEEN
        private void LateUpdate()
        {
            UpdateBlend();

            UpdateTransforms();
        }
        
        private void UpdateBlend()
        {
            if (_cameraBlendTime >= _blendDuration)
            {
                return;
            }
        
            _cameraBlendTime += Time.deltaTime;
            if (_cameraBlendTime >= _blendDuration)
            {
                _prevCameraTransform = null;
            }
        }

        private void UpdateTransforms()
        {
            if (_cameraTransform == null)
            {
                return;
            }
        
            if (_prevCameraTransform == null)
            {
                var position = _cameraTransform.Position;
                var rotation = _cameraTransform.Rotation;
                var fieldOfView = _cameraTransform.FieldOfView;

                transform.position = position;
                transform.rotation = rotation;
                Camera.fieldOfView = fieldOfView;
            }
            else
            {
                var interpolation = EaseManager.Evaluate(_easeType, null, _cameraBlendTime, _blendDuration, 0.0f, 0.0f);
            
                var position = Vector3.Lerp(
                    _prevCameraTransform.Position,
                    _cameraTransform.Position,
                    interpolation);

                var rotation = Quaternion.Lerp(
                    _prevCameraTransform.Rotation,
                    _cameraTransform.Rotation,
                    interpolation);

                var fieldOfView = Mathf.Lerp(
                    _prevCameraTransform.FieldOfView,
                    _cameraTransform.FieldOfView,
                    interpolation);

                transform.position = position;
                transform.rotation = rotation;
                Camera.fieldOfView = fieldOfView;
            }
        }
#endif
    }
}
