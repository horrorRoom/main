// Copyright (c) 2018 Jakub Boksansky - All Rights Reserved
// Wilberforce Camera Lens Unity Plugin 1.1

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System.Reflection;

namespace Wilberforce.CameraLens
{

    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [HelpURL("https://projectwilberforce.github.io/cameralens/manual/")]
    [AddComponentMenu("Image Effects/Rendering/Camera Lens")]
    public class CameraLens : CameraLensCommandBuffer
    {

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            this.PerformOnRenderImage(source, destination);
        }

    }


#if UNITY_EDITOR

    [CustomEditor(typeof(CameraLens))]
    public class CameraLensEditorImageEffect : CameraLensEditor { }

#endif
}
