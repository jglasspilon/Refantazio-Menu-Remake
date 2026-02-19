using UnityEngine;

public static class Logger
{
    private static LoggingProfile m_fallbackLogProfile = ScriptableObject.CreateInstance(typeof(LoggingProfile)) as LoggingProfile;

    public static void Log(string message, GameObject owner, LoggingProfile logProfile)
    {
        if(logProfile == null) 
            logProfile = m_fallbackLogProfile;

        if (!logProfile.LoggingEnabled)
            return;

        string output = logProfile.CreatePrefixString() + message;
        Debug.Log(output, owner);
    }

    public static void Log(string message, LoggingProfile logProfile)
    {
        if (logProfile == null)
            logProfile = m_fallbackLogProfile;

        if (!logProfile.LoggingEnabled)
            return;

        string output = logProfile.CreatePrefixString() + message;
        Debug.Log(output);
    }

    public static void LogError(string message, GameObject owner, LoggingProfile logProfile)
    {
        if (logProfile == null)
            logProfile = m_fallbackLogProfile;

        string output = logProfile.CreatePrefixString() + message;
        Debug.LogError(output, owner);
    } 

    public static void LogError(string message, LoggingProfile logProfile)
    {
        if (logProfile == null)
            logProfile = m_fallbackLogProfile;

        string output = logProfile.CreatePrefixString() + message;
        Debug.LogError(output);
    }
}
