using UnityEngine;
using TMPro;
using System.Collections;

public class TypeTextTMP : MonoBehaviour
{
    [SerializeField] private float typingSpeed = 0.05f; // 한 글자당 표시 시간 (초)

    private TextMeshProUGUI textMeshPro;
    private string fullText;

    void Start()
    {
        // TextMeshProUGUI 컴포넌트 가져오기
        textMeshPro = GetComponent<TextMeshProUGUI>();
        if (textMeshPro == null)
        {
            Debug.LogWarning("TextMeshProUGUI component not found on this GameObject!");
            return;
        }

        // 전체 텍스트 저장 후 비우기
        fullText = textMeshPro.text;
        textMeshPro.text = "";

        // 타이핑 시작
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        foreach (char c in fullText)
        {
            textMeshPro.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}