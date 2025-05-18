using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PathResourcesAttribute))]
public class PathResourcesAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var paths = PathResources.ALL_PATHS;

        //получить индекс в списке строк, соответствующий строке, выбранной в выпадающем списке
        int index = Mathf.Max(paths.IndexOf(property.stringValue), 0);

        index = EditorGUI.Popup(position, property.displayName, index, paths.ToArray());

        //обновить надпись выбранного элемента 
        property.stringValue = paths[index];
    }
}
