using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillPointDisplayScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI textMeshProUGUI;


    public void OnPointerEnter(PointerEventData eventData)
    {
        textMeshProUGUI.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textMeshProUGUI.enabled = false;
    }

    public void SetLimitText(int maxSkills, int currentSkills)
    {
        textMeshProUGUI.text = $"Skilllimit! {currentSkills} / {maxSkills} (Nächste Seite)";
    }

    public void SetNoLimitText(int maxSkills, int currentSkills)
    {
        textMeshProUGUI.text = $"{currentSkills} / {maxSkills} (Nächste Seite)";
    }
}
