using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageBinder : PropertyBinder
{
    private Image m_image;

    private void Awake()
    {
        m_image = GetComponent<Image>();
    }

    protected void Apply(Sprite value)
    {
        m_image.sprite = value;
        m_image.enabled = value != null;
    }

    protected override void Apply(object value)
    {
        Logger.LogError($"Provided value of type {value.GetType()} does not match expected type Sprite. Image binding failed.", m_logProfile);
    }
}
