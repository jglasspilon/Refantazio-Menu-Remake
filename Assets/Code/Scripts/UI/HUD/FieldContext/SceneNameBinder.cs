using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class SceneNameBinder : MonoBehaviour
{
    [SerializeField] private bool m_displayParentArea;
    [SerializeField] private int m_firstLetterSizePercent = 120;

    private TextMeshProUGUI m_text;

    public void BindAreaDisplay(SSceneData sceneData)
    {
        Initialize();
        if(m_displayParentArea)
        {
            m_text.text = Helper.StringFormatting.StylizeWords(sceneData.ParentAreaName, m_firstLetterSizePercent);
            return;
        }

        m_text.text = sceneData.AreaName;
    }

    private void Initialize()
    {
        if(m_text == null)
            m_text = GetComponent<TextMeshProUGUI>();
    }
}
