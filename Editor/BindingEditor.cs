using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Gameframe.Bindings.Editor
{
    [CustomEditor(typeof(BindingBehaviour), editorForChildClasses:true)]
    public class BindingEditor : UnityEditor.Editor
    {
        private SerializedProperty pDataContext;
        
        private void OnEnable()
        {
            pDataContext = serializedObject.FindProperty("_dataContextInfo");
        }

        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement();
            
            // Create property fields.
            // Add fields to the container.
            var dataContextField = new PropertyField(pDataContext);
            container.Add(dataContextField);
            
            var fields = serializedObject.targetObject.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField);

            if (fields.Length <= 0)
            {
                return container;
            }
            
            var fieldContainer = new VisualElement()
            {
                style =
                {
                    paddingTop = 5,
                    paddingBottom = 5,
                    paddingLeft = 5,
                    paddingRight = 5,
                    marginBottom = 5,
                    marginTop = 5,
                    borderBottomWidth = 1,
                    borderLeftWidth = 1,
                    borderRightWidth = 1,
                    borderTopWidth = 1,
                    borderColor = Color.black,
                    backgroundColor = new Color(0,0,0,0.1f)
                }
            };
            
            foreach (var field in fields)
            {
                if (field.Name.Equals("_dataContextInfo"))
                {
                    continue;
                }
                if (field.IsPublic || field.GetCustomAttribute<SerializeField>() != null)
                {
                    fieldContainer.Add(new PropertyField(serializedObject.FindProperty(field.Name)));
                }
            }
            
            container.Add(fieldContainer);
            
            return container;
        }
        
    }
}
