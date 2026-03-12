using UnityEngine;

public interface IPropertyProvider
{
    bool TryGetPropertyRaw(string key, out object value);
    bool TryGetProperty<T>(string key, out ObservableProperty<T> value);
    string Name { get; }
}
