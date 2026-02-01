using UnityEngine;

public class Logger : MonoBehaviour
{
    [SerializeField]
    private bool m_logEnabled;

    [SerializeField]
    private string m_prefix;

    [SerializeField]
    private Color m_prefixColor;

    private string m_prefixString;

    private void Awake()
    {
        string hexColor = ColorUtility.ToHtmlStringRGB(m_prefixColor);
        m_prefixString = $"<color=#{hexColor}>{m_prefix}: </color>";
    }

    public void Log(string message, GameObject owner)
    {
        if (!m_logEnabled)
            return;

        string output = m_prefixString + message;
        Debug.Log(output, owner);
    }

    public void LogError(string message, GameObject owner)
    {
        string output = m_prefixString + message;
        Debug.LogError(output, owner);
    }
}
