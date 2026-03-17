using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ArchetypeSortingDisplay : MonoBehaviour
{
    private TextMeshProUGUI m_text;

    private void Awake()
    {
        m_text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        HandleSortChange(0);
    }

    public void HandleSortChange(EArchetypeSortType sortType)
    {
        m_text.text = sortType.ToString();
    }
}
