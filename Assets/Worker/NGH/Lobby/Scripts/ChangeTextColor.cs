using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ChangeTextColor : UIBInder, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI buttonText; // 변경할 텍스트

    private Color _hoverColor = new Color(178f / 255f, 217f / 255f, 116f / 255f);
    private Color _defaultColor = new Color(94f / 255f, 204f / 255f, 58f / 255f);

    // 마우스가 버튼에 들어올 때 호출되는 메서드
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = _hoverColor; // 원하는 색상으로 변경
    }

    // 마우스가 버튼을 벗어날 때 호출되는 메서드
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = _defaultColor; // 원래 색상으로 복원
    }
}
