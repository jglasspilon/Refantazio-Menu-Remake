using UnityEngine;

public interface IBindableToProperty
{
    public void BindToProperty(IPropertyProvider provider);
    public void UnBind();
}
