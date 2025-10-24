using UnityEngine;
using UnityEngine.UI;
using System;

public class TapeButton : MonoBehaviour
{
    public event Action OnTapeRemoved;
    public AudioSource AudioSource;
    private bool isRemoved = false;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        if (button != null)
            button.onClick.AddListener(RemoveTape);
    }

    private void RemoveTape()
    {
        if (isRemoved) return;

        AudioSource.Play();

        isRemoved = true;
        gameObject.SetActive(false);
        Debug.Log($"{name}: 테이프 제거 완료");

        OnTapeRemoved?.Invoke();
    }
}
