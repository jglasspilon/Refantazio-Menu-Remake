using System.Collections.Generic;
using UnityEngine;

public interface ISubPropertyProvider
{
    IEnumerable<KeyValuePair<string, IObservableProperty>> GetSubProperties(string parentKey);

}
