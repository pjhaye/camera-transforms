using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if CAMERA_TRANSFORMS_DOTWEEN
using DG.Tweening;
#endif

namespace CameraTransforms
{
    /// <summary>
    /// Immediately blends the camera to the target CameraTransform OnEnable
    /// </summary>
    public class BlendGameCameraOnEnable : MonoBehaviour
    {
#if CAMERA_TRANSFORMS_DOTWEEN
        [SerializeField]
        private GameCamera _gameCamera;
        [SerializeField]
        private CameraTransform _cameraTransform;
        [SerializeField]
        private float _duration;
        [SerializeField]
        private Ease _easeType;
#endif

        private void OnEnable()
        {
#if CAMERA_TRANSFORMS_DOTWEEN
            if (_cameraTransform == null)
            {
                return;
            }

            if (_gameCamera == null)
            {
                return;
            }
            
            _gameCamera.BlendToCameraTransform(_cameraTransform, _easeType, _duration);
#endif
        }
    }
}
