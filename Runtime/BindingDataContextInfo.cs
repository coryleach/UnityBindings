using System;
using UnityEngine;

namespace Gameframe.Bindings
{
    [Serializable]
    public class BindingDataContextInfo
    {
        public UnityEngine.Object dataContext;
        public Component component;
        public string property;

        public UnityEngine.Object BindableDataContext => component == null ? dataContext : component;
    }
}