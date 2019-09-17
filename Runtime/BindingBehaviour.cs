#if UNITY_EDITOR
//Disable warning about unused variable in exception in the OnValidate function
#pragma warning disable 0168
#endif

using System;
using System.ComponentModel;
using UnityEngine;

namespace GameJam.Bindings
{
  public abstract class BindingBehaviour : MonoBehaviour
  {

    [SerializeField]
    protected UnityEngine.Object dataContext;
    public UnityEngine.Object DataContext
    {
      get { return dataContext; }
      set {
        dataContext = value;
        Initialize();
        Refresh();
      }
    }

    [SerializeField]
    string propertyPath;
    public string PropertyPath
    {
      get { return propertyPath; }
      set { propertyPath = value; }
    }

    INotifyPropertyChanged propertyChangedNotifier;
    private INotifyPropertyChanged PropertyChangedNotifier
    {
      get { return propertyChangedNotifier; }
      set
      {
        if ( propertyChangedNotifier != null )
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

    void PropertyChanged(object sender, PropertyChangedEventArgs args)
    {
      if ( PropertyPath.Contains(args.PropertyName) )
      {
        Refresh();
      }
    }

    protected object GetPropertyValue()
    {
      object obj = dataContext;

      foreach (string property in propertyPath.Split('.'))
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

    void OnEnable()
    {
      Initialize();
      Refresh();
    }

    protected void Initialize()
    {
      PropertyChangedNotifier = dataContext as INotifyPropertyChanged;
    }

    protected abstract void Refresh();

#if UNITY_EDITOR

    void OnValidate()
    {
      if (dataContext != null)
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