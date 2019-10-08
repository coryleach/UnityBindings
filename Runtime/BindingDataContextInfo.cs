using System;
using System.Collections;
using System.Collections.Generic;
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