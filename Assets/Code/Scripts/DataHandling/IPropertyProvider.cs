using UnityEngine;

public interface IPropertyProvider
{
    bool TryGetProperty<T>(string key, out ObservableProperty<T> value);
    string Name { get; }
}
