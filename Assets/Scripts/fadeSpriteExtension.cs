using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class fadeSpriteExtension
{
    public static void fadeSprite(this SpriteRenderer spriteRenderer, MonoBehaviour mono, float duration, Action<SpriteRenderer> callback = null)
    {
        mono.StartCoroutine(SpriteCoroutine(spriteRenderer, duration, callback));
    }


    private static IEnumerator SpriteCoroutine(SpriteRenderer spriteRenderer, float duration, Action<SpriteRenderer> callback)
    {
        // fading animation
        float start = Time.time;
        while (Time.time <= start + duration)
        {
            Color color = spriteRenderer.color;
            color.a = 1f - Mathf.Clamp01((Time.time - start) / duration);
            //spriteRenderer.color = new Color(1, 0, 0, .5f);
            spriteRenderer.color = color;
            //yield return new WaitForEndOfFrame();
            yield return null;
        }

        // Callback
        if (callback != null)
            callback(spriteRenderer);
    }
}