using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Gameframe.Bindings
{
    /// <summary>
    /// ScriptableObject that provides property change events for bindings via the INotifyPropertyChanged interface
    /// </summary>
    public abstract class BindableScriptableObject : ScriptableObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnEnable()
        {
            //We need to clear out the subscribed bindings between editor play sessions
            PropertyChanged = null;
        }

        protected virtual void OnDisable()
        {
            PropertyChanged = null;
        }
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Use this inside property setters to raise property changed events which are needed by bindingssss
        /// </summary>
        /// <param name="field">field to which the value will be set</param>
        /// <param name="value">value you want to assign to storage</param>
        /// <param name="propertyName">name of the property being set</param>
        /// <typeparam name="T">Type of the property being set</typeparam>
        /// <returns>True if property was set. False if it was already equal to the given value.</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null )
        {
            if (EqualityComparer<T>.Default.Equals(field,value))
            {
                return false;
            }
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}