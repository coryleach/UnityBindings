#if UNITY_EDITOR
//Disable warning about unused variable in exception in the OnValidate function
#pragma warning disable 0168
#endif

using System;
using System.ComponentModel;
using UnityEngine;
using Component = UnityEngine.Component;

namespace Gameframe.Bindings
{
    [Serializable]
    public class BindingDataContextInfo
    {
        public UnityEngine.Object dataContext;
        public Component component;
        public string property;
    }
    
    public class BindingBehaviour : MonoBehaviour
    {
        [SerializeField]
        protected BindingDataContextInfo _dataContextInfo = new BindingDataContextInfo();
        
        public UnityEngine.Object DataContext
        {
            get => _dataContextInfo.component == null ? _dataContextInfo.dataContext : _dataContextInfo.component;
            set
            {
                _dataContextInfo.dataContext = value;
                _dataContextInfo.component = null;
                Initialize();
                Refresh();
            }
        }

        public string PropertyPath
        {
            get => _dataContextInfo.property;
            set => _dataContextInfo.property = value;
        }

        private INotifyPropertyChanged propertyChangedNotifier;

        private INotifyPropertyChanged PropertyChangedNotifier
        {
            get => propertyChangedNotifier;
            set
            {
                if (propertyChangedNotifier != null)
                {
                    propertyChangedNotifier.PropertyChanged -= PropertyChanged;
                }

                propertyChangedNotifier = value;
                if (propertyChangedNotifier != null)
                {
                    propertyChangedNotifier.PropertyChanged += PropertyChanged;
                }
            }
        }

        private void PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (PropertyPath.Contains(args.PropertyName))
            {
                Refresh();
            }
        }

        protected object GetPropertyValue()
        {
            object obj = DataContext;

            foreach (string property in PropertyPath.Split('.'))
            {
                if (obj == null)
                {
                    return null;
                }

                Type type = obj.GetType();
                System.Reflection.PropertyInfo info = type.GetProperty(property);
                if (info == null)
                {
                    return null;
                }

                obj = info.GetValue(obj, null);
            }

            return obj;
        }

        private void OnEnable()
        {
            Initialize();
            Refresh();
        }

        private void Initialize()
        {
            PropertyChangedNotifier = DataContext as INotifyPropertyChanged;
        }

        protected virtual void Refresh()
        {
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (DataContext != null)
            {
                try
                {
                    Refresh();
                }
                catch (System.Exception e)
                {
                    // ignored
                }
            }
        }

#endif
    }
}