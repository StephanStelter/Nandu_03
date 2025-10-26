using UnityEngine;
using UnityEngine.UI;

public class UILineConnector : MonoBehaviour
{
    public Image panelToFill;

    public RectTransform targetA;
    public RectTransform targetB;
    private RectTransform rectTransform;

    public Material lineFillingUnlocked;
    public Material lineFillingActivated;
    public RectTransform glowImage;
    public RectTransform ShadowImage;


    private void Initalize()
    {
        rectTransform = GetComponent<RectTransform>();
        panelToFill.fillAmount = 0;
    }

   private void SetPosition()
    {
        if (targetA == null || targetB == null) return;

        // 1. Weltpositionen beider Targets
        Vector3 worldPosA = targetA.position;
        Vector3 worldPosB = targetB.position;

        // 2. Mittelpunkt berechnen
        Vector3 middle = (worldPosA + worldPosB) / 2f;
        rectTransform.position = middle;

        // 3. Richtung & Winkel berechnen
        Vector3 dir = worldPosB - worldPosA;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rectTransform.rotation = Quaternion.Euler(0f, 0f, angle);

        // 4. Länge setzen
        float distance = dir.magnitude;
        rectTransform.sizeDelta = new Vector2(distance, rectTransform.sizeDelta.y);
    }


    public void ChangeColor()
    {
        SkillObject skillObjectTargertA = targetA.GetComponentInChildren<SkillObject>();
        if (skillObjectTargertA == null) { Debug.Log("skillObjectTargertA == null"); }

        SkillObject skillObjectTargertB = targetB.GetComponentInChildren<SkillObject>();
        if (skillObjectTargertB == null) { Debug.Log("skillObjectTargertB == null"); }

        //if(skillObjectTargertA.activatedSkill || skillObjectTargertB.activatedSkill)
        //{
        //    panelToFill.color = Color.white;
        //    panelToFill.fillAmount = 1;
        //}

        if (skillObjectTargertA.activatedSkill && skillObjectTargertB.activatedSkill)
        {
            panelToFill.color = Color.green;
            panelToFill.fillAmount = 1;
        }
    }

    public void SetTargets(RectTransform _targetA, RectTransform _targetB)
    {
        targetA = _targetA;
        targetB = _targetB;

        Initalize();

        SetPosition();
    }

    public void SetUnlock()
    {
        panelToFill.material = lineFillingUnlocked;
    }

    public void SetActivated()
    {
        panelToFill.material = lineFillingActivated;
        glowImage.gameObject.SetActive(true);
        ShadowImage.gameObject.SetActive(false);
    }

}
