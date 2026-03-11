using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

[CustomEditor(typeof(PropertyBinder), true)]
public class PropertyBinderEditor : Editor
{
    private Type[] _providerTypes;
    private string[] _providerTypeNames;

    private int _providerIndex = 0;
    private int _propertyIndex = 0;
    private int _selectedTypeIndex = 0;

    private string[] _filteredPropertyKeys = Array.Empty<string>();
    private string[] _availableTypes = Array.Empty<string>();

    private SerializedProperty _propertyKeyProp;
    private SerializedProperty _providerTypeProp;
    private SerializedProperty _sourceTypeProp;

    private Type _selectedSourceType;

    private void OnEnable()
    {
        _propertyKeyProp = serializedObject.FindProperty("m_propertyKey");
        _providerTypeProp = serializedObject.FindProperty("m_selectedProviderType");
        _sourceTypeProp = serializedObject.FindProperty("m_selectedSourceType");

        LoadProviderTypes();
        LoadAvailableTypesForProvider();
        LoadPropertyKeysForSelectedProvider();
    }

    // ---------------------------------------------------------
    // Provider Type Discovery
    // ---------------------------------------------------------
    private void LoadProviderTypes()
    {
        _providerTypes = TypeCache.GetTypesDerivedFrom<IPropertyProvider>().Where(t => !t.IsAbstract && !t.IsInterface).ToArray();
        _providerTypeNames = _providerTypes.Select(t => t.Name).ToArray();

        // Restore saved provider selection
        if (!string.IsNullOrEmpty(_providerTypeProp.stringValue))
        {
            int idx = Array.IndexOf(_providerTypeNames, _providerTypeProp.stringValue);
            _providerIndex = Mathf.Clamp(idx, 0, _providerTypeNames.Length - 1);
        }

    }

    // ---------------------------------------------------------
    // Type Dropdown (unique ObservableProperty<T> types)
    // ---------------------------------------------------------
    private void LoadAvailableTypesForProvider()
    {
        var providerType = _providerTypes[_providerIndex];

        _availableTypes = Helper.DataHandling.GetObservablePropertyTypesFromProviderType(providerType, null).Distinct().Select(MakeFriendlyTypeName).ToArray();

        // Restore saved source type
        if (!string.IsNullOrEmpty(_sourceTypeProp.stringValue))
        {
            int idx = Array.IndexOf(_availableTypes, _sourceTypeProp.stringValue);
            _selectedTypeIndex = Mathf.Clamp(idx, 0, _availableTypes.Length - 1);
        }

        _selectedSourceType = ResolveType(_availableTypes[_selectedTypeIndex]);
    }

    private Type ResolveType(string typeName)
    {
        // Manual handling for common primitive aliases
        switch (typeName)
        {
            case "Int32":
            case "int": return typeof(int);

            case "Single":
            case "float": return typeof(float);

            case "Boolean":
            case "bool": return typeof(bool);

            case "String":
            case "string": return typeof(string);

            case "Double":
            case "double": return typeof(double);

            case "Int64":
            case "long": return typeof(long);

            case "Int16":
            case "short": return typeof(short);

            case "Byte":
            case "byte": return typeof(byte);

            case "Char":
            case "char": return typeof(char);

            case "Decimal":
            case "decimal": return typeof(decimal);
        }

        // Fallback: resolve ANY type by simple name
        var type = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a =>
            {
                Type[] types = null;
                try { types = a.GetTypes(); }
                catch { /* ignore reflection errors */ }
                return types ?? Array.Empty<Type>();
            })
            .FirstOrDefault(t => t.Name == typeName);

        if (type != null)
            return type;

        // Final fallback: try full name or assembly-qualified name
        return Type.GetType(typeName);

    }

    // ---------------------------------------------------------
    // Property Key Filtering
    // ---------------------------------------------------------
    private void LoadPropertyKeysForSelectedProvider()
    {
        if (_providerTypes.Length == 0)
        {
            _filteredPropertyKeys = Array.Empty<string>();
            return;
        }

        var providerType = _providerTypes[_providerIndex];

        _filteredPropertyKeys = Helper.DataHandling.GetObservablePropertyNamesFromType(providerType, _selectedSourceType).ToArray();
        _propertyIndex = Mathf.Max(0, Array.IndexOf(_filteredPropertyKeys, _propertyKeyProp.stringValue));
    }

    // ---------------------------------------------------------
    // Inspector GUI
    // ---------------------------------------------------------
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Binding", EditorStyles.boldLabel);

        // Provider dropdown
        int newProviderIndex = EditorGUILayout.Popup("Provider Type", _providerIndex, _providerTypeNames);
        if (_providerTypeProp.stringValue != _providerTypeNames[newProviderIndex])
        {
            _providerIndex = newProviderIndex;
            _providerTypeProp.stringValue = _providerTypeNames[_providerIndex];
            LoadAvailableTypesForProvider();
        }

        // Type dropdown
        string[] friendlyTypes = _availableTypes.Select(MakeFriendlyTypeName).ToArray();
        int newTypeIndex = EditorGUILayout.Popup("Property Type", _selectedTypeIndex, friendlyTypes);

        if (_sourceTypeProp.stringValue != _availableTypes[newTypeIndex])
        {
            _selectedTypeIndex = newTypeIndex;
            _sourceTypeProp.stringValue = _availableTypes[_selectedTypeIndex];
            _selectedSourceType = ResolveType(_availableTypes[_selectedTypeIndex]);
            LoadPropertyKeysForSelectedProvider();
        }

        // Property key dropdown
        EditorGUI.BeginDisabledGroup(_filteredPropertyKeys.Length == 0);
        string[] friendlyNames = _filteredPropertyKeys.Select(MakeFriendlyProperty).ToArray();
        int newPropertyIndex = EditorGUILayout.Popup("Property", _propertyIndex, friendlyNames);
        EditorGUI.EndDisabledGroup();

        _propertyIndex = newPropertyIndex;

        if (_filteredPropertyKeys.Length > 0)
            _propertyKeyProp.stringValue = _filteredPropertyKeys[_propertyIndex];

        DrawPropertiesExcluding(serializedObject, "m_propertyKey", "m_Script", "m_selectedProviderType", "m_selectedSourceType");
        serializedObject.ApplyModifiedProperties();
    }

    // ---------------------------------------------------------
    // Friendly Name Formatter
    // ---------------------------------------------------------
    private string MakeFriendlyProperty(string key)
    {
        return string.Join(" - ",
            key.Replace("m_", "")
               .Split('.')
               .Select(segment => char.ToUpper(segment[0]) + segment.Substring(1)));
    }

    public static string MakeFriendlyTypeName(string typeName)
    {
        switch (typeName)
        {
            case "Int32": return "int";
            case "Single": return "float";
            case "Boolean": return "bool";
            case "String": return "string";
            case "Int64": return "long";
            case "Double": return "double";
            case "Char": return "char";
            case "Byte": return "byte";
            case "Decimal": return "decimal";
            case "Void": return "void";
            default:
                return typeName; 
        }
    }

}