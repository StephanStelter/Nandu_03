using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill Tree/Skill Data")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    public string description;

    public bool unlocked = false;

    [Header("Skill Dependencies")]
    public SkillData[] requiredSkills;
}
