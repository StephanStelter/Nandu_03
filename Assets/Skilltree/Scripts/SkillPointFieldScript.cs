using UnityEngine;
using UnityEngine.UI;

public class SkillPointFieldScript : MonoBehaviour
{
    public Image background;
    public Material materialNoLimit;
    public Material materialLimit;

    public void SetNoMaterial()
    {
        background.material = background.defaultMaterial;
    }

    public void SetMaterialNoLimit()
    {
        background.material = materialNoLimit;
    }

    public void SetMaterialLimit()
    {
        background.material = materialLimit;
    }
}
