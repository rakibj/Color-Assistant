using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.rakib.colorassistant
{
    [ExecuteInEditMode]
    public class RenderGrayscale : MonoBehaviour
    {
        public Shader curShader;
        [Range(0f, 1f)] public float greyScaleAmount = 0f;
        private Material _screenMat;
        private static readonly int LUMINOSITY = Shader.PropertyToID("_Luminosity");

        public Material ScreenMat
        {
            get
            {
                if (_screenMat == null)
                {
                    _screenMat = new Material(curShader);
                    _screenMat.hideFlags = HideFlags.HideAndDontSave;
                }

                return _screenMat;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (curShader == null)
            {
                curShader = Resources.Load<Shader>("Shaders/ScreenGrayscale");
            }
        }

        private void Start()
        {
            if (!curShader || !curShader.isSupported)
                enabled = false;
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            if (curShader != null)
            {
                ScreenMat.SetFloat(LUMINOSITY, greyScaleAmount);
                Graphics.Blit(src, dest, _screenMat);
            }
            else
            {
                Graphics.Blit(src, dest);
            }
        }

        private void OnDisable()
        {
            if (_screenMat)
                DestroyImmediate(_screenMat);
        }
#endif
    }
}