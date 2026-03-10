using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

[CustomEditor(typeof(PropertyBinder<>), true)]
public class PropertyBinderEditor : Editor
{
    private Type[] _providerTypes;
    private string[] _providerTypeNames;

    private int _providerIndex = 0;
    private int _propertyIndex = 0;

    private string[] _propertyKeys;

    private SerializedProperty _propertyKeyProp;

    private Type _binderValueType;

    private void OnEnable()
    {
        _propertyKeyProp = serializedObject.FindProperty("m_propertyKey");

        _binderValueType = GetBinderValueType();

        LoadProviderTypes();
        LoadPropertyKeysForSelectedProvider();
    }

    private Type GetBinderValueType()
    {
        var type = target.GetType();

        while (type != null)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(PropertyBinder<>))
                return type.GetGenericArguments()[0];

            type = type.BaseType;
        }

        return null;
    }

    private void LoadProviderTypes()
    {
        _providerTypes = TypeCache.GetTypesDerivedFrom<IPropertyProvider>()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .ToArray();

        _providerTypeNames = _providerTypes.Length > 0
            ? _providerTypes.Select(t => t.Name).ToArray()
            : new[] { "No providers found" };
    }

    private void LoadPropertyKeysForSelectedProvider()
    {
        if (_providerTypes.Length == 0)
        {
            _propertyKeys = Array.Empty<string>();
            return;
        }

        var providerType = _providerTypes[_providerIndex];

        _propertyKeys = Helper.DataHandling.GetObservablePropertyNamesFromType(providerType, _binderValueType).ToArray();
        _propertyIndex = Mathf.Max(0, Array.IndexOf(_propertyKeys, _propertyKeyProp.stringValue));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Binding", EditorStyles.boldLabel);

        // Provider type dropdown
        int newProviderIndex = EditorGUILayout.Popup("Provider Type", _providerIndex, _providerTypeNames);

        if (newProviderIndex != _providerIndex)
        {
            _providerIndex = newProviderIndex;
            LoadPropertyKeysForSelectedProvider();
        }

        // Property key dropdown
        EditorGUI.BeginDisabledGroup(_propertyKeys == null || _propertyKeys.Length == 0);

        string[] friendlyNames = _propertyKeys.Select(k => MakeFriendly(k)).ToArray();
        int newPropertyIndex = EditorGUILayout.Popup("Property", _propertyIndex, friendlyNames);
        EditorGUI.EndDisabledGroup();

        if (newPropertyIndex != _propertyIndex)
        {
            _propertyIndex = newPropertyIndex;
            if (_propertyKeys.Length > 0)
                _propertyKeyProp.stringValue = _propertyKeys[_propertyIndex];
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Binder Settings", EditorStyles.boldLabel);

        DrawPropertiesExcluding(serializedObject, "m_propertyKey", "m_Script");
        serializedObject.ApplyModifiedProperties();
    }

    private string MakeFriendly(string key)
    {
        return string.Join(" - ", key.Replace("m_", "").Split('.').Select(segment => char.ToUpper(segment[0]) + segment.Substring(1)));
    }
}