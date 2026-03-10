using UnityEngine;
using System;

public interface IObservableProperty
{
    Type ValueType { get; }
    object UntypedValue { get; }
}
