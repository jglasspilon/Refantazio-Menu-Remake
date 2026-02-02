using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Logging Profile")]
public class LoggingProfile : ScriptableObject
{
    public bool LoggingEnabled = true;
    public string Prefix = "Undefined";
    public Color PrefixColor = Color.white;

    public string CreatePrefixString()
    {
        string hexColor = ColorUtility.ToHtmlStringRGB(PrefixColor);
        return $"<color=#{hexColor}>{Prefix}: </color>";
    }
}
