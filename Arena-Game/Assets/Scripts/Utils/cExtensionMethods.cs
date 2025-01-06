using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ArenaGame.Utils
{
    public static class cExtensionMethods
    {
        public static void SetParentResetTransform(this Transform transform, Transform parent, bool scale = true)
        {
            transform.SetParent(parent);
            transform.ResetTransform(scale);
        }
    
        public static void ResetTransform(this Transform transform, bool scale = true)
        {
            if (Application.isPlaying)
            {
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                if(scale) transform.localScale = Vector3.one;
            }
            else
            {
#if UNITY_EDITOR
                SerializedObject serializedObject = new SerializedObject(transform);
                serializedObject.FindProperty("m_LocalPosition").vector3Value = Vector3.zero;
                serializedObject.FindProperty("m_LocalRotation").quaternionValue = Quaternion.identity;
                if(scale) serializedObject.FindProperty("m_LocalScale").vector3Value = Vector3.one;
                serializedObject.ApplyModifiedProperties();
#endif
            }
        }

        public static void PlayWithClear(this ParticleSystem particleSystem)
        {
            particleSystem.StopWithClear();
            particleSystem.Play(true);
        }
    
        public static void StopWithClear(this ParticleSystem particleSystem)
        {
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    
        public static void SetLayerAllChildren(this GameObject go, int layer)
        {
            foreach (Transform VARIABLE in go.transform)
            {
                VARIABLE.gameObject.layer = layer;
                VARIABLE.gameObject.SetLayerAllChildren(layer);
            }
        }
    
        public static Vector3 RandomPointInBounds(this Bounds bounds) {
            return new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
            );
        }
    
        public static float Remap (this float value, float from1, float to1, float from2, float to2) {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
        
        public static float RemapClamped (this float value, float from1, float to1, float from2, float to2)
        {
            var remap = value.Remap(from1, to1, from2, to2);
            return Mathf.Clamp(remap, to1, to2);
        }

        public static void DelayedMethod(this MonoBehaviour monoBehaviour, float delay ,Action lambda)
        {
            monoBehaviour.StartCoroutine(Delay());
            IEnumerator Delay()
            {
                yield return new WaitForSeconds(delay);
                lambda.Invoke();
            }
        }
    
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach(T item in enumeration)
            {
                action(item);
            }
        }
    
        public static T RandomItem<T>(this IEnumerable<T> enumeration)
        {
            return enumeration.OrderBy((arg => Random.Range(0, 999999))).FirstOrDefault();
        }
        
        public static bool IsEmpty<T>(this IEnumerable<T> enumeration)
        {
            return !enumeration.Any();
        }

        public static List<Transform> GetChilds(this GameObject target)
        {
            List<Transform> childs = new List<Transform>();
            for (int i = 0; i < target.transform.childCount; i++)
            {
                childs.Add(target.transform.GetChild(i));
            }
            return childs;
        }
        
        public static void Shuffle<T>(this List<T> list) {
            int swapIndex;
            for (int i = 0; i < list.Count; ++i)
            {
                swapIndex = Random.Range(0, list.Count);
                T temp = list[i];
                list[i] = list[swapIndex];
                list[swapIndex] = temp;
            }
        }
    
        public static void SuccessShakeUI(this Transform transform)
        {
            transform.DOComplete();
    
            transform.DOShakeScale(.3f, .2f);
    
            foreach (var VARIABLE in transform.GetComponentsInChildren<Image>())
            {
                VARIABLE.DOComplete();
                Color colorToLerp = Color.green;
                colorToLerp.a = VARIABLE.color.a;
                VARIABLE.DOColor(colorToLerp, .15f).SetLoops(2, LoopType.Yoyo);
            }
        }
        
        public static void FailShakeUI(this Transform transform)
        {
            transform.DOComplete();
    
            transform.DOShakeRotation(.3f, 10, 25);
    
            foreach (var VARIABLE in transform.GetComponentsInChildren<Image>())
            {
                VARIABLE.DOComplete();
                Color colorToLerp = Color.red;
                colorToLerp.a = VARIABLE.color.a;
                VARIABLE.DOColor(colorToLerp, .15f).SetLoops(2, LoopType.Yoyo);
            }
        }
        
        public static string ColorHtmlString(this string text, Color color)
        {
            return "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + text + "</color>";
        }
        
        public static Texture2D DuplicateTexture(this Texture2D source)
        {
            var width = 128;
            var height = 128;
        
            RenderTexture renderTex = RenderTexture.GetTemporary(
                width,
                height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.sRGB);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(width, height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }
    }
}

public static class RectTransformExtensions
{
    public static void SetLeft(this RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }
}