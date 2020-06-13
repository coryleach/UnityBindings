using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Gameframe.Bindings.Editor
{
    [CustomPropertyDrawer(typeof(BindingDataContextInfo))]
    public class BindingDataContextInfoDrawer : PropertyDrawer
    {
        private VisualElement rootContainer;
        private PopupField<Object> componentPopup;
        private PopupField<string> propertyPopup;
        
        private SerializedProperty pDataContext;
        private SerializedProperty pComponent;
        private SerializedProperty pProperty;
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create property container element.
            rootContainer = new VisualElement
            {
                style =
                {
                    paddingTop = 5,
                    paddingBottom = 5,
                    paddingLeft = 5,
                    paddingRight = 5,
                    marginBottom = 10,
                    marginTop = 10,
                    borderBottomWidth = 1,
                    borderLeftWidth = 1,
                    borderRightWidth = 1,
                    borderTopWidth = 1,
                    borderColor = Color.black,
                    backgroundColor = new Color(0,0,0,0.1f) 
                }
            };

            rootContainer.Bind(property.serializedObject);
            
            // Create property fields.
            pDataContext = property.FindPropertyRelative("dataContext");
            pComponent = property.FindPropertyRelative("component");
            pProperty = property.FindPropertyRelative("property");

            rootContainer.Add(new Label(property.displayName));

            //Data Context Field is always available
            var dataContextField = new ObjectField("Object")
            {
                objectType = typeof(Object), 
                bindingPath = pDataContext.propertyPath
            };
            
            dataContextField.SetValueWithoutNotify(pDataContext.objectReferenceValue);
            
            rootContainer.Add(dataContextField);
            
            UpdateSelectableComponents(pDataContext.objectReferenceValue);
            UpdateSelectableProperties();
            
            dataContextField.RegisterCallback<ChangeEvent<Object>>(evt =>
            {
                pDataContext.objectReferenceValue = evt.newValue;

                //Clear component when a new data context is set
                pComponent.objectReferenceValue = null;
                pProperty.stringValue = string.Empty;
                //Update components and properties
                UpdateSelectableComponents(evt.newValue);
                UpdateSelectableProperties();
            });
            
            return rootContainer;
        }

        private void UpdateSelectableComponents(Object dataContext)
        {
            if (componentPopup != null)
            {
                componentPopup.RemoveFromHierarchy();
                componentPopup = null;
            }

            var targetGameObject = dataContext as GameObject;
            if (targetGameObject == null)
            {
                return;
            }
            
            var components = new List<Component>();
            targetGameObject.GetComponents(components);

            var selectableObjects = components.Cast<Object>().ToList();
            selectableObjects.Insert(0, dataContext);
            
            int defaultIndex = selectableObjects.IndexOf(pComponent.objectReferenceValue);
            if (defaultIndex < 0)
            {
                defaultIndex = 0;
            }

            //Make sure we're currently set to the default value
            if (selectableObjects.Count > 0)
            {
                pComponent.objectReferenceValue = selectableObjects[defaultIndex];
                pComponent.serializedObject.ApplyModifiedProperties();
            }
            
            componentPopup = new PopupField<Object>("Component", selectableObjects,defaultIndex, FormatComponentType,FormatComponentTypePopup);
            
            componentPopup.RegisterCallback<ChangeEvent<Object>>(evt =>
            {
                pComponent.objectReferenceValue = evt.newValue is GameObject ? null : evt.newValue;
                pProperty.stringValue = string.Empty;
                UpdateSelectableProperties();
                pComponent.serializedObject.ApplyModifiedProperties();
            });

            rootContainer.Insert(2,componentPopup);
        }
        
        private void UpdateSelectableProperties()
        {
            if (propertyPopup != null)
            {
                propertyPopup.RemoveFromHierarchy();
                propertyPopup = null;
            }
            
            Object targetObject = pComponent.objectReferenceValue;
            if (targetObject == null)
            {
                targetObject = pDataContext.objectReferenceValue;
            }

            if (targetObject == null)
            {
                return;
            }
            
            var properties = targetObject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.GetField);
            var propertyNames = properties.Select(x => x.Name).ToList();

            int defaultIndex = propertyNames.IndexOf(pProperty.stringValue);
            if (defaultIndex < 0)
            {
                defaultIndex = 0;
            }
            
            //Make sure we're currently set to the default value
            if (propertyNames.Count > 0)
            {
                pProperty.stringValue = propertyNames[defaultIndex];
                pProperty.serializedObject.ApplyModifiedProperties();
            }
            
            propertyPopup = new PopupField<string>("Property",propertyNames,defaultIndex);
            propertyPopup.RegisterCallback<ChangeEvent<string>>(evt =>
            {
                pProperty.stringValue = evt.newValue;
                pProperty.serializedObject.ApplyModifiedProperties();
            });
            
            rootContainer.Add(propertyPopup);
        }
        
        private static string FormatComponentType(Object obj)
        {
            return $"{obj.GetType().Name}";
        }
        
        private static string FormatComponentTypePopup(Object obj)
        {
            return $"{obj.GetType().Name} - InstanceId: {obj.GetInstanceID()}";
        }

    }
}

