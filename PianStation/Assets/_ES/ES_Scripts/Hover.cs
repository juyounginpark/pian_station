using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;  // TMP 관련 네임스페이스

public class Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text text;          // 연결할 TMP_Text 컴포넌트
    public Color normalColor = Color.white;
    public Color hoverColor = Color.cyan;

    void Start()
    {
        if (text == null)
            text = GetComponent<TMP_Text>();

        text.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = normalColor;
    }
}
