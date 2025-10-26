using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeManager : MonoBehaviour
{
    [Header("Skilltree")]
    public TextMeshProUGUI skillpointsText;
    public TextMeshProUGUI tierPageText;

    [Header("Lines")]
    public GameObject LineConnectorPrefab;

    [Header("Activate Skill")]
    public Button activateButton;

    [Header("Activate Skill")]
    public TextMeshProUGUI detailHeadlineText;
    public TextMeshProUGUI detailText;
    public Image SkillImage;


    [System.Serializable]
    public class SkillPage
    {
        public int counter;
        public int max;
        public bool completed;
        public TextMeshProUGUI pageText;
        public bool skillLimit;
    }
    [Header("Pages")]
    public List<SkillPage> pages = new List<SkillPage>();
    private int currentPageIndex = 0;


    [System.Serializable]
    public class PagesForSkills
    {
        public RectTransform skillPage;
    }
    public List<PagesForSkills> pagesForSkills = new List<PagesForSkills>();

    [Header("SkillPointsDisplay")]
    public RectTransform skillPointsDisplayRect;
    public RectTransform skillPointPrefab;
    public SkillPointDisplayScript skillPointDisplayScript;
    public GameObject limitHint;


    public ScrollSkills scrollSkillsSkilltree;
    public TextMeshProUGUI page1Text;
    public int skillCouterPage1 = 0;
    public int skillMaxPage1 = 6;
    private bool page1Cpl = false;
    public TextMeshProUGUI page2Text;
    public int skillCouterPage2 = 0;
    public int skillMaxPage2 = 1;
    private bool page2Cpl = false;

    [Header("Misc")]
    private NewPlayer mainPlayer;
    public GameObject allSkillObjectsParent;
    public GameObject uiLineConnectorParent;

    private SkillButtonUI[] allSkillButtonUIArray;
    private UILineConnector[] allLinesUIArray;

    private SkillButtonUI currentSkillButtonUI;

    private bool skilltreeInPosition = false;

    public SkillData[] allSkills;




    public void SetSkilltree()
    {
        if (!skilltreeInPosition)
        {
            skilltreeInPosition = true;

            // Main Player
            mainPlayer = TurnManager.Instance.Players.Find(p => p.isMainPlayer);
            if (mainPlayer == null) return;

            // All Skill-Buttons
            GetAllSkillButtons();
            CheckAllSkillButtons();

            SetConnectors();
            CheckColorLines();

            scrollSkillsSkilltree.Initialize();

            GetFirstSkillOnStart();
            SetSkillpointDisplay();

            activateButton.interactable = false;
        }
        // Skill Points
        UpdateSkillPoints();
    }

    private void UpdateSkillPoints()
    {
        skillpointsText.text = "Skillpunkte: " + mainPlayer.skillPoints;
    }

    private void GetAllSkillButtons()
    {
        allSkillButtonUIArray = allSkillObjectsParent.GetComponentsInChildren<SkillButtonUI>();
        if (allSkillButtonUIArray.Length == 0) { Debug.Log("(allSkillButtonUIArray.Length == 0"); };
    }

    private void CheckAllSkillButtons()
    {
        foreach (SkillButtonUI skillButtonUI in allSkillButtonUIArray)
        {
            skillButtonUI.CheckButton();
        }
    }

    private void SetConnectors()
    {
        foreach (SkillButtonUI skillButtonUI in allSkillButtonUIArray)
        {
            if (skillButtonUI.nextSkillsToUnlock.Count != 0)
            {
                foreach (var item in skillButtonUI.nextSkillsToUnlock)
                {
                    GameObject newLine = Instantiate(LineConnectorPrefab, uiLineConnectorParent.transform);
                    if (newLine == null) { Debug.LogError("newLine == null"); }
                    newLine.transform.localScale = new Vector3(.6f, .6f, .6f);

                    UILineConnector uILineConnector = newLine.GetComponent<UILineConnector>();
                    if (uILineConnector == null) { Debug.LogError("uILineConnector == null"); }

                    uILineConnector.SetTargets(skillButtonUI.GetComponent<RectTransform>(), item.GetComponent<RectTransform>());

                    skillButtonUI.GetConnectorLinesToNextSkills(uILineConnector);
                }
            }
        }
    }

    private void CheckColorLines()
    {
        foreach (SkillButtonUI skillButtonUI in allSkillButtonUIArray)
        {
            skillButtonUI.CkeckLineColors();
        }
    }

    public void GetCurrentSkillButtenUI(SkillButtonUI skillButtonUI)
    {
        currentSkillButtonUI = skillButtonUI;
        if (currentSkillButtonUI == null) { Debug.Log("currentSkillButtonUI == null"); };

        if (currentSkillButtonUI.isUnlocked && !currentSkillButtonUI.isActivated && mainPlayer.skillPoints > 0)
        {
            activateButton.interactable = true;
        }
        else if (!currentSkillButtonUI.isUnlocked || currentSkillButtonUI.isActivated)
        {
            activateButton.interactable = false;
        }
        else
        {
            activateButton.interactable = false;
        }

        ShowDetails();
    }

    private void ShowDetails()
    {
        TextMeshProUGUI detailtextHeadlineCurrent = currentSkillButtonUI.detailHeadlineText;
        if (detailtextHeadlineCurrent == null) { Debug.Log("detailtextHeadlineCurrent == null"); };
        detailHeadlineText.text = detailtextHeadlineCurrent.text;

        TextMeshProUGUI detailtextCurrent = currentSkillButtonUI.detailText;
        if (detailtextCurrent == null) { Debug.Log("detailtextCurrent == null"); };
        detailText.text = detailtextCurrent.text;

        Image skillImageCurrent = currentSkillButtonUI.iconImage;
        if (skillImageCurrent == null) { Debug.Log("skillImageCurrent == null"); };
        SkillImage.sprite = skillImageCurrent.sprite;
    }

    private void GetFirstSkillOnStart()
    {
        detailHeadlineText.text = allSkillButtonUIArray[0].detailHeadlineText.text;
        detailText.text = allSkillButtonUIArray[0].detailText.text;
        SkillImage.sprite = allSkillButtonUIArray[0].iconImage.sprite;
    }

    public void ActivateSkill()
    {
        if (mainPlayer.skillPoints > 0)
        {
            Debug.Log("Skill aktiviert");
            currentSkillButtonUI.isActivated = true;
            currentSkillButtonUI.CheckButton();

            mainPlayer.skillPoints--;

            foreach (GameObject gameObject in currentSkillButtonUI.nextSkillsToUnlock)
            {
                SkillButtonUI skillButtonUI = gameObject.GetComponent<SkillButtonUI>();
                if (skillButtonUI == null) { Debug.Log("skillButtonUI == null!"); }

                skillButtonUI.isUnlocked = true;
                skillButtonUI.CheckButton();
            }

            currentSkillButtonUI.ActivateDetails();
            SetCounter();
            UpdateSkillPoints();
            CheckColorLines();
            SetSkillpointDisplay();
        }
    }


    private void SetCounter()
    {
        // aktuelle sichtbare Seite holen und auf 0-basiert umrechnen
        int pageIndex = scrollSkillsSkilltree.counter - 1;

        // Sicherheits-Check
        if (pageIndex < 0 || pageIndex >= pages.Count)
            return;

        SkillPage currentPage = pages[pageIndex];

        // Seite schon fertig? Dann nichts tun
        if (currentPage.completed)
            return;

        // Zähler hoch
        currentPage.counter++;

        // Seite voll? Dann abschließen und neue freischalten
        if (currentPage.counter >= currentPage.max)
        {
            if (currentPage.skillLimit)
            {
                SkillButtonUI[] skillButtonUIArray = pagesForSkills[pageIndex].skillPage.GetComponentsInChildren<SkillButtonUI>();
                if (skillButtonUIArray.Length == 0) { Debug.Log("skillButtonUIArray.Length == 0!"); }

                foreach (SkillButtonUI skillButtonUI in skillButtonUIArray)
                {
                    if (!skillButtonUI.isActivated)
                    {
                        skillButtonUI.SetSkillBlock();
                    }
                }
            }


            scrollSkillsSkilltree.SetNewPage();
            currentPage.completed = true;
        }
    }



    private void GetAllLines()
    {
        allLinesUIArray = uiLineConnectorParent.GetComponentsInChildren<UILineConnector>();
        if (allLinesUIArray.Length == 0) { Debug.Log("(allLinesUIArray.Length == 0"); };
    }  

    private List<RectTransform> skillPointPool = new List<RectTransform>();

    public void SetSkillpointDisplay()
    {
        RebuildSkillpoints();
    }

    private void RebuildSkillpoints()
    {
        int maxSkills = pages[scrollSkillsSkilltree.counter - 1].max;
        int currentSkills = pages[scrollSkillsSkilltree.counter - 1].counter;

        Vector2 size = skillPointPrefab.sizeDelta;
        float width = size.x - 5f;
        float height = size.y;

        float row = 0;
        int freeSkillCounter = maxSkills - currentSkills;

        // 1) Erstmal alle im Pool deaktivieren
        foreach (var point in skillPointPool)
            point.gameObject.SetActive(false);

        // 2) Skillpoints neu aufbauen
        for (int i = 0; i < maxSkills; i++)
        {
            RectTransform newItem;

            // Falls im Pool ein freies Element existiert → nimm das
            if (i < skillPointPool.Count)
            {
                newItem = skillPointPool[i];
            }
            else
            {
                // Neues Instanzieren und in den Pool packen
                newItem = Instantiate(skillPointPrefab, skillPointsDisplayRect, false);
                skillPointPool.Add(newItem);
            }

            // Aktivieren + Position setzen
            newItem.gameObject.SetActive(true);
            newItem.localPosition = new Vector2(width * -i, height * row);

            // Script holen
            SkillPointFieldScript skillPointFieldScript = newItem.GetComponent<SkillPointFieldScript>();
            if (skillPointFieldScript == null) { Debug.Log("(skillPointFieldScript == null"); }

            // Limit-Behandlung
            if (pages[scrollSkillsSkilltree.counter - 1].skillLimit)
            {
                skillPointDisplayScript.SetLimitText(maxSkills, currentSkills);
            }
            else
            {
                skillPointDisplayScript.SetNoLimitText(maxSkills, currentSkills);
            }

            if (freeSkillCounter > 0)
            {
                skillPointFieldScript.SetNoMaterial();
                freeSkillCounter--;
            }
            else
            {
                skillPointFieldScript.SetMaterialNoLimit();
            }
        }

        // Hint ein- / ausblenden
        if (pages[scrollSkillsSkilltree.counter - 1].skillLimit)
        {
            limitHint.SetActive(true);
        }
        else
        {
            limitHint.SetActive(false);
        }

    }










    //void BuildTree()
    //{
    //    foreach (Transform child in buttonContainer)
    //        Destroy(child.gameObject);

    //    foreach (var skill in allSkills)
    //    {
    //        var obj = Instantiate(skillButtonPrefab, buttonContainer);
    //        var buttonUI = obj.GetComponent<SkillButtonUI>();
    //        buttonUI.Setup(skill, OnSkillClicked);
    //    }
    //}

    //void OnSkillClicked(SkillData skill)
    //{
    //    if (skill.unlocked) return;

    //    foreach (var req in skill.requiredSkills)
    //    {
    //        if (!req.unlocked)
    //            return;
    //    }

    //    skill.unlocked = true;
    //    Debug.Log("Unlocked: " + skill.skillName);
    //    BuildTree();
    //}
}
