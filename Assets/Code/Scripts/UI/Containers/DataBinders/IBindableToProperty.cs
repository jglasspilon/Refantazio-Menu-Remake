using UnityEngine;

public interface IBindableToProperty
{
    public void BindToProperty(IPropertyProvider provier);
    public void UnBind();
}
