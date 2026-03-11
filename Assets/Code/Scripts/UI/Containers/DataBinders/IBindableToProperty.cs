using UnityEngine;
using System;

public interface IBindableToProperty
{
    public Type ProviderType { get; }
    public void BindToProperty(IPropertyProvider provider);
    public void UnBind();
}
