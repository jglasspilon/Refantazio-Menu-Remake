using TMPro;
using UnityEngine;

public class PageIndexDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_pageIndexText;

    [SerializeField]
    private MainMenuPage m_parentPage;

    [SerializeField]
    private float m_nudgeDistance;

    [SerializeField]
    private float m_nudgeSpeed;

    private Vector3 m_startPosition;
    private Vector3 m_nudgePosition;
    private bool m_isNudged;

    private void Awake()
    {
        m_startPosition = transform.localPosition;
        m_nudgePosition = new Vector3(m_startPosition.x, m_startPosition.y - m_nudgeDistance, m_startPosition.z);
    }

    private void OnEnable()
    {
        m_parentPage.OnPageIndexChanged += UpdateText;
        UpdateText(m_parentPage.CurrentPageIndex);
    }

    private void OnDisable()
    {
        m_parentPage.OnPageIndexChanged -= UpdateText;
    }

    private void Update()
    {
        if (transform.localPosition.y < m_startPosition.y)
        {
            float step = m_nudgeSpeed * Time.deltaTime;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + step, transform.localPosition.z);
        }
        else if (transform.localPosition != m_startPosition)
        {
            transform.localPosition = m_startPosition;
            m_isNudged = false;
        }
    }

    private void UpdateText(int pageIndex)
    {
        SetPageText(pageIndex);

        if(!m_isNudged)
            Nudge();
    }

    private void Nudge()
    {
        m_isNudged = true;
        transform.localPosition = m_nudgePosition;
    }

    private void SetPageText(int pageIndex)
    {
        string result = pageIndex.ToString("0");
        string richTextFix = "";
        float xScale = 1.0f;

        //Manually fix the text size, voffset and xScale based on pageIndex since the font sheet used does not have consistent numeral glyph sizing
        switch (pageIndex)
        {
            case 1:
                xScale = 1.4f;
                break;
            case 2:
                break;
            case 3:
            case 4:
            case 5:
            case 7:
            case 9:
                richTextFix = "<size=85%><voffset=80>";
                break;
            case 6:
            case 8:
                richTextFix = "<size=85%><voffset=20>";
                break;
        }

        m_pageIndexText.transform.localScale = new Vector3(xScale, 1, 1);
        m_pageIndexText.text = $"{richTextFix}{result}";
    }
}
