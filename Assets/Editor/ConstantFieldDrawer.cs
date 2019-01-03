using UnityEngine;
using UnityEditor;

/// <summary>
/// ConstantField属性の描画を行うクラス
/// </summary>
[CustomPropertyDrawer(typeof(ConstantFieldAttribute))]
public class ConstantFieldDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label.text += " (Constant)";
        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.PropertyField(position, property, label);
        EditorGUI.EndDisabledGroup();
    }
}