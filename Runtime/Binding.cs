using UnityEngine;
using System;
using System.Reflection;
using System.ComponentModel;

namespace Gameframe.Bindings
{
    public class Binding : IDisposable
    {
        private object _source = null;
        private object _target = null;

        private string _sourcePath = null;
        private string _targetPath = null;

        private PropertyInfo _sourceProperty = null;
        private PropertyInfo _targetProperty = null;
        
        private INotifyPropertyChanged _propertyChangedNotifier;

        public UnityEngine.Object ErrorContext = null;
        public bool Enabled = true;
        public Func<object, object> Converter = null;
        
        public void SetSource(object dataContext, string path, bool refresh = true)
        {
            _source = dataContext;
            _sourcePath = path;
            _sourceProperty = GetPropertyInfo(dataContext, path);
            SetPropertyChangedNotifier(_source as INotifyPropertyChanged);
            if (refresh)
            {
                Refresh();
            }
        }
        
        public void SetTarget(object dataContext, string path, bool refresh = true)
        {
            _target = dataContext;
            _targetPath = path;
            _targetProperty = GetPropertyInfo(dataContext, path);
            if (refresh)
            {
                Refresh();
            }
        }

        public void Refresh()
        {
            if (_target == null || _source == null || _targetProperty == null || _sourceProperty == null)
            {
                return;
            }
            
            try
            {
                var sourceValue = _sourceProperty.GetValue(_source, null);
                if (Converter != null)
                {
                    sourceValue = Converter(sourceValue);
                }
                _targetProperty.SetValue(_target,  sourceValue);
            }
            catch (Exception e)
            {
                Debug.LogException(e, ErrorContext);
            }
        }
        
        public void Dispose()
        { 
            SetPropertyChangedNotifier(null);   
        }
        
        private void PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if ( _targetProperty == null || !Enabled )
            {
                return;
            }
            
            if (_sourcePath.Contains(args.PropertyName))
            {
                Refresh();
            }
        }
        
        private static PropertyInfo GetPropertyInfo(object obj, string path)
        {
            if (obj == null || path == null)
            {
                return null;
            }
            var type = obj.GetType();
            return type.GetProperty(path);
        }
        
        private void SetPropertyChangedNotifier(INotifyPropertyChanged value)
        {
            if (_propertyChangedNotifier != null)
            {
                _propertyChangedNotifier.PropertyChanged -= PropertyChanged;
            }
            _propertyChangedNotifier = value;
            if (_propertyChangedNotifier != null)
            {
                _propertyChangedNotifier.PropertyChanged += PropertyChanged;
            }
        }
        
    }
}