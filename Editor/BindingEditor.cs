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
    [CustomEditor(typeof(BindingBehaviour), editorForChildClasses: true)]
    public class BindingEditor : UnityEditor.Editor
    {
        private SerializedProperty pDataContext;
        private List<BindingDataContextInfoDrawer> _drawerList;
        
        private void OnEnable()
        {
            pDataContext = serializedObject.FindProperty("_sourceDataInfo");
            _drawerList = new List<BindingDataContextInfoDrawer>();
        }

        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement();

            var drawer = new BindingDataContextInfoDrawer();
            container.Add(drawer.CreatePropertyGUI(pDataContext));
            _drawerList.Add(drawer);
            
            var fields = serializedObject.targetObject.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                           BindingFlags.GetField);
            
            if (fields.Length <= 0)
            {
                return container;
            }

            var fieldContainer = new VisualElement
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
#if UNITY_2019_3_OR_NEWER
                    borderBottomColor = Color.black,
                    borderLeftColor = Color.black,
                    borderRightColor = Color.black,
                    borderTopColor = Color.black,                    
#else
                    borderColor = Color.black,
#endif
                    backgroundColor = new Color(0, 0, 0, 0.1f)
                }
            };

            foreach (var field in fields)
            {
                if (field.IsPublic || field.GetCustomAttribute<SerializeField>() != null)
                {
                    var property = serializedObject.FindProperty(field.Name);
                    if (field.FieldType == typeof(BindingDataContextInfo))
                    {
                        drawer = new BindingDataContextInfoDrawer();
                        container.Add(drawer.CreatePropertyGUI(property));
                        _drawerList.Add(drawer);
                    }
                    else
                    {
                        fieldContainer.Add(CreateFieldFromProperty(serializedObject.FindProperty(field.Name)));
                    }
                }
            }

            container.Add(fieldContainer);

            return container;
        }


        private static VisualElement CreateFieldFromProperty(SerializedProperty property)
        {
            SerializedPropertyType propertyType = property.propertyType;

            //Ripped this from some decompiled internal unity code so it's wonky. Could fix it if there is every an actual need
            switch (propertyType + 1)
            {
                case SerializedPropertyType.Integer:
                case SerializedPropertyType.Boolean:
                    return ConfigureField<IntegerField, int>(new IntegerField(), property);
                case SerializedPropertyType.Float:
                    return ConfigureField<Toggle, bool>(new Toggle(), property);
                case SerializedPropertyType.String:
                    return ConfigureField<FloatField, float>(new FloatField(), property);
                case SerializedPropertyType.Color:
                    return ConfigureField<TextField, string>(new TextField(), property);
                case SerializedPropertyType.ObjectReference:
                    return ConfigureField<ColorField, Color>(new ColorField(), property);

                case SerializedPropertyType.LayerMask:
                    ObjectField field1 = new ObjectField();
                    GetFieldInfoFromProperty(property, out Type type);
                    if ((object) type == null || !type.IsSubclassOf(typeof(UnityEngine.Object)) )
                    {
                        type = typeof(UnityEngine.Object);
                    }
                    field1.objectType = type;
                    return ConfigureField<ObjectField, Object>(field1, property);

                case SerializedPropertyType.Enum:
                    return ConfigureField<LayerMaskField, int>(new LayerMaskField(), property);
                case SerializedPropertyType.Vector2:
                    return ConfigureField<PopupField<string>, string>(
                        new PopupField<string>(((IEnumerable<string>) property.enumDisplayNames).ToList<string>(),
                            property.enumValueIndex, (Func<string, string>) null, (Func<string, string>) null)
                        {
                            index = property.enumValueIndex
                        }, property);

                case SerializedPropertyType.Vector3:
                    return ConfigureField<Vector2Field, Vector2>(new Vector2Field(), property);
                case SerializedPropertyType.Vector4:
                    return ConfigureField<Vector3Field, Vector3>(new Vector3Field(), property);
                case SerializedPropertyType.Rect:
                    return ConfigureField<Vector4Field, Vector4>(new Vector4Field(), property);
                case SerializedPropertyType.ArraySize:
                    return ConfigureField<RectField, Rect>(new RectField(), property);

                case SerializedPropertyType.Character:
                  IntegerField integerField = new IntegerField();
                  integerField.SetValueWithoutNotify(property.intValue);
                  integerField.isDelayed = true;
                  return ConfigureField<IntegerField, int>(integerField, property);

                case SerializedPropertyType.AnimationCurve:
                    TextField field2 = new TextField();
                    field2.maxLength = 1;
                    return ConfigureField<TextField, string>(field2, property);
                case SerializedPropertyType.Bounds:
                    return ConfigureField<CurveField, AnimationCurve>(new CurveField(), property);
                case SerializedPropertyType.Gradient:
                    return ConfigureField<BoundsField, Bounds>(new BoundsField(), property);
                case SerializedPropertyType.Quaternion:
                    return ConfigureField<GradientField, Gradient>(new GradientField(), property);
                case SerializedPropertyType.ExposedReference:
                    return null;
                case SerializedPropertyType.FixedBufferSize:
                    return null;
                case SerializedPropertyType.Vector2Int:
                    return null;
                case SerializedPropertyType.Vector3Int:
                    return ConfigureField<Vector2IntField, Vector2Int>(new Vector2IntField(), property);
                case SerializedPropertyType.RectInt:
                    return ConfigureField<Vector3IntField, Vector3Int>(new Vector3IntField(), property);
                case SerializedPropertyType.BoundsInt:
                    return ConfigureField<RectIntField, RectInt>(new RectIntField(), property);
                case SerializedPropertyType.Vector2 | SerializedPropertyType.Gradient:
                    return ConfigureField<BoundsIntField, BoundsInt>(new BoundsIntField(), property);
                default:
                    return null;
            }
        }

        private static VisualElement ConfigureField<TField, TValue>(
            TField field,
            SerializedProperty property)
            where TField : BaseField<TValue>
        {
            string str = !string.IsNullOrEmpty(property.displayName) ? property.displayName : property.name;
            field.bindingPath = property.propertyPath;
            field.name = "unity-input-" + property.propertyPath;
            field.label = str;
            Label label = field.Q<Label>(null, BaseField<TValue>.labelUssClassName);
            if (label != null)
            {
                label.userData = property.Copy();
            }

            field.labelElement.AddToClassList(PropertyField.labelUssClassName);
            return field;
        }

        private static void GetFieldInfoFromProperty(SerializedProperty property, out Type type)
        {
            var typeFromProperty = GetScriptTypeFromProperty(property);
            if ((object) typeFromProperty != null)
            {
                GetFieldInfoFromPropertyPath(typeFromProperty, property.propertyPath, out type);
                return;
            }
            type = null;
        }
        
        private static Type GetScriptTypeFromProperty(SerializedProperty property)
        {
            SerializedProperty property1 = property.serializedObject.FindProperty("m_Script");
            if (property1 == null)
            {
                return null;
            }
            MonoScript objectReferenceValue = property1.objectReferenceValue as MonoScript;
            if (objectReferenceValue == null)
            {
                return null;
            }
            return objectReferenceValue.GetClass();
        }

        private static FieldInfo GetFieldInfoFromPropertyPath(Type host, string path, out Type type)
        {
            FieldInfo fieldInfo1 = null;
            type = host;
            var strArray = path.Split('.');
            var index = 0;
            while ( index < strArray.Length )
            {
                var name = strArray[index];
                if (index < strArray.Length - 1 && name == "Array" && strArray[index + 1].StartsWith("data["))
                {
                    if (type.IsArrayOrList())
                    {
                        type = type.GetArrayOrListElementType();
                    }
                    index++;
                }
                else
                {
                    FieldInfo fieldInfo2 = null;
                    for (var type1 = type; (object) fieldInfo2 == null && (object) type1 != null; type1 = type1.BaseType)
                    {
                        fieldInfo2 = type1.GetField(name,BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    }
                    if ((object) fieldInfo2 == null)
                    {
                        type = null;
                        return null;
                    }
                    fieldInfo1 = fieldInfo2;
                    type = fieldInfo1.FieldType;
                }
                index++;
            }
            return fieldInfo1;
        }
    }

    public static class TypeExtension
    {
        internal static bool IsArrayOrList(this System.Type listType)
        {
            return listType.IsArray || listType.IsGenericType &&
                   (object) listType.GetGenericTypeDefinition() == (object) typeof(List<>);
        }

        internal static Type GetArrayOrListElementType(this System.Type listType)
        {
            if (listType.IsArray)
            {
                return listType.GetElementType();
            }

            if (listType.IsGenericType && listType.GetGenericTypeDefinition() == typeof(List<>))
            {
                return listType.GetGenericArguments()[0];
            }
            
            return null;
        }
    }
}