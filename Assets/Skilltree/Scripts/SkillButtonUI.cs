using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SkillButtonUI : MonoBehaviour
{
    [Header("Button")]
    public Button ActivateButton;
    public bool isUnlocked = false;
    public bool isActivated = false;

    public Button skillButton;

    [Header("Button Visuals")]
    public Image background;

    [Header("Unlock Verknüpfungen")]
    public List<GameObject> nextSkillsToUnlock; // Später in Scriptable
    public List<UILineConnector> uILineConnectorsToNextSkills;
    public GameObject blockSkill;

    public SkillTreeManager skillTreeManager;


    public Image iconImage;
    public TMP_Text nameText;
    public GameObject lockOverlay;

    private SkillData skillData;
    public TextMeshProUGUI detailHeadlineText;
    public TextMeshProUGUI detailText;




    public void CheckButton()
    {
        if (!isUnlocked)
        {
            background.color = Color.gray;
        }

        if (isUnlocked && !isActivated)
        {
            background.color = Color.yellow;
        }
        if (isUnlocked && isActivated)
        {
           background.color = Color.green;
        }
    }

    public void GetConnectorLinesToNextSkills(UILineConnector newLine)
    {
        uILineConnectorsToNextSkills.Add(newLine);
    }

    public void CkeckLineColors()
    {
        if (uILineConnectorsToNextSkills.Count != 0 && nextSkillsToUnlock.Count != 0)
        {
            int counter = 0;

            foreach (UILineConnector uILineConnector in uILineConnectorsToNextSkills)
            {
                SkillButtonUI skillButtonUI = nextSkillsToUnlock[counter].GetComponent<SkillButtonUI>();
                if (skillButtonUI == null) { Debug.Log("skillButtonUI == null"); }

                if (!isUnlocked)
                {
                    uILineConnector.panelToFill.gameObject.SetActive(false);
                    Debug.Log("1.1");
                }
                else if (isUnlocked)
                {
                    if (isActivated)
                    {
                        if (skillButtonUI.isUnlocked && skillButtonUI.isActivated)
                        {
                            uILineConnector.panelToFill.gameObject.SetActive(true);
                            uILineConnector.SetActivated();
                            Debug.Log("2.5");
                            Debug.Log("Material gesetzt auf: " + uILineConnector.panelToFill.material.name);
                        }

                        else if (skillButtonUI.isUnlocked && !skillButtonUI.isActivated)
                        {
                            uILineConnector.panelToFill.gameObject.SetActive(true);
                            uILineConnector.SetUnlock();
                            Debug.Log("2.6");
                            Debug.Log("Material gesetzt auf: " + uILineConnector.panelToFill.material.name);
                        }
                    }
                    else if (!isActivated)
                    {
                        if (skillButtonUI.isUnlocked && !skillButtonUI.isActivated)
                        {
                            uILineConnector.panelToFill.gameObject.SetActive(true);
                            uILineConnector.SetUnlock();
                            Debug.Log("2.3");
                            Debug.Log("Material gesetzt auf: " + uILineConnector.panelToFill.material.name);
                        }
                        else
                        {
                            uILineConnector.panelToFill.gameObject.SetActive(false);
                            Debug.Log("2.4");
                        }
                    }

                    else
                        Debug.Log("3.6");
                }
                else
                    Debug.Log("3.7");

                counter++;





            }
        }



    }

    public void ActivateDetails()
    {
        skillTreeManager.GetCurrentSkillButtenUI(this);
    }

    public void SetSkillBlock()
    {
        blockSkill.SetActive(true);
        skillButton.interactable = false;
    }











    public void Setup(SkillData data, System.Action<SkillData> onClick)
    {
        skillData = data;

        iconImage.sprite = data.icon;
        nameText.text = data.skillName;
        lockOverlay.SetActive(!data.unlocked);

        ActivateButton.interactable = data.unlocked || RequirementsMet(data);

        ActivateButton.onClick.RemoveAllListeners();
        ActivateButton.onClick.AddListener(() => onClick?.Invoke(data));
    }

    private bool RequirementsMet(SkillData data)
    {
        foreach (var req in data.requiredSkills)
        {
            if (!req.unlocked)
                return false;
        }
        return true;
    }

    public void Click()
    {
        Debug.Log("Click");
    }
}
