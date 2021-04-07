using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.rakib.colorassistant
{
    [ExecuteInEditMode]
    public class RenderBSC : MonoBehaviour
    {
        public Shader curShader;
        [Range(0f, 2f)] public float brightness = 1.0f;
        [Range(0f, 2f)] public float saturation = 1.0f;
        [Range(0f, 3f)] public float contrast = 1.0f;
        private Material _screenMat;
        private static readonly int BRIGHTNESS = Shader.PropertyToID("_Brightness");
        private static readonly int SATURATION = Shader.PropertyToID("_Saturation");
        private static readonly int CONTRAST = Shader.PropertyToID("_Contrast");

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
                curShader = Resources.Load<Shader>("Shaders/ScreenBSC");
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
                ScreenMat.SetFloat(BRIGHTNESS, brightness);
                ScreenMat.SetFloat(SATURATION, saturation);
                ScreenMat.SetFloat(CONTRAST, contrast);
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