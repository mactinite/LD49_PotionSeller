using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mactinite.ToolboxCommons;
using ElRaccoone.Tweens;

public class FlybyText : SingletonMonobehavior<FlybyText>
{
    public TMPro.TMP_Text textPrefab;
    public Transform textSpawn;
    public static void SpawnText(string text)
    {
        Instance.SpawnText_Internal(text);
    }

    public void SpawnText_Internal(string text)
    {
        StartCoroutine(TextFlybyCoroutine(text));
    }

    IEnumerator TextFlybyCoroutine(string text)
    {
        var spawnedText = Instantiate(textPrefab, textSpawn.transform.position, Quaternion.identity);
        spawnedText.text = text;
        yield return spawnedText.transform.TweenPositionX(-0.5f, 0.5f).Yield();
        yield return spawnedText.transform.TweenPositionX(0.5f, 2f).Yield();
        yield return spawnedText.transform.TweenPositionX(-textSpawn.transform.position.x, 0.5f).SetEaseQuintOut().Yield();
        Destroy(spawnedText.gameObject);
    }
}
