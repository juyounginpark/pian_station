using UnityEngine;
using System.Collections;

public class ActivateAfterDelay : MonoBehaviour
{
    public GameObject ActiveObject;
    public bool ActivateWhenStart = true;

    public float Seconds = 4f;
    void Start()
    {
        if(ActivateWhenStart)
        {
            StartActivate();
        }
    }
    public void StartActivate()
    {
        ActiveObject.SetActive(false); // 시작 시 비활성화
        StartCoroutine(ActivateAfterThreeSeconds());
    }

    private IEnumerator ActivateAfterThreeSeconds()
    {
        yield return new WaitForSeconds(Seconds); // 3초 대기
        ActiveObject.SetActive(true); // 활성화
    }
}