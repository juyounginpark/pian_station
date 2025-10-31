using UnityEngine;
using System.Collections;

public class EffectManager : MonoBehaviour
{
    public ScreenShake screenShake;
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

            // ¨ç È­¸é Èçµé¸² ½ÇÇà
            float shakeTime = Random.Range(0.3f, 1f);
            float shakePower = Random.Range(0.05f, 0.2f);
            screenShake.TriggerShake(shakeTime, shakePower);

            // ¨è È­¸é ±ôºýÀÓ ½ÇÇà
            float blinkTime = Random.Range(0.5f, 1.5f);
            StartCoroutine(blinkEffect.Flash(blinkTime));
        }
    }
}
