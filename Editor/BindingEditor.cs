using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace Gameframe.Bindings.Editor
{
    [CustomEditor(typeof(BindingBehaviour), editorForChildClasses:true)]
    public class BindingEditor : UnityEditor.Editor
    {
        private SerializedProperty pDataContext;
        private SerializedProperty pPath;
        
        private void OnEnable()
        {
            pDataContext = serializedObject.FindProperty("dataContext");
            pPath = serializedObject.FindProperty("propertyPath");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            
            var dataContext = pDataContext.objectReferenceValue;

            if (dataContext == null)
            {
                //Display some kind of manual setup UI like typing in the path or whatever   
                GUILayout.Label("No Data Context");
                return;
            }
            
            GUILayout.Label($"DataContext: {dataContext.GetType()}");

            var properties = dataContext.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.GetField);
            var propertyNames = properties.Select(x => x.Name).ToList();
            int i = propertyNames.IndexOf(pPath.stringValue);
            if (i < 0)
            {
                i = 0;
            }
            i = EditorGUILayout.Popup("Path", i, propertyNames.ToArray());
            pPath.stringValue = propertyNames[i];

            serializedObject.ApplyModifiedProperties();
        }
        
    }
}
