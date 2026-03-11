using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(menuName = "Data/String Formatter/Larger Leading Text Formatter")]
public class LargerLeadingText : StringFormatter
{
    [SerializeField] private int m_affectedNumberOfCharacters = 1;
    [SerializeField] private float m_smallerTextPercent = 0.5f;
    public override string Format(object value, out string message)
    {
        message = null;
        string output = value.ToString();

        if (m_affectedNumberOfCharacters == 0 || output.Length < m_affectedNumberOfCharacters)
            return output;

        output = $"{output.Substring(0, m_affectedNumberOfCharacters)}<size=50%>{output.Substring(m_affectedNumberOfCharacters)}";
        return output;
    }
}
