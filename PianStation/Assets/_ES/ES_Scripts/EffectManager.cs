using UnityEngine;
using System.Collections;

public class EffectManager : MonoBehaviour
{
    public UIScreenShake screenShake;
    public BlinkEffect blinkEffect;

    public float minInterval = 2f;
    public float maxInterval = 6f;

    void Start()
    {
        StartCoroutine(RandomEffects());
    }

    IEnumerator RandomEffects()
    {
        while (true)
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            float shakeTime = Random.Range(0.4f, 1f);
            float shakePower = Random.Range(5f, 10f);
            screenShake.TriggerShake(shakeTime, shakePower);

            float blinkTime = Random.Range(0.5f, 1.5f);
            StartCoroutine(blinkEffect.Flash(blinkTime));
        }
    }
}
