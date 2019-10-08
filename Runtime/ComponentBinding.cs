using System.Collections;
using System.Collections.Generic;
using Gameframe.Bindings;
using UnityEditor;
using UnityEngine;

namespace Gameframe.Bindings
{
    public class ComponentBinding : BindingBehaviour
    {
        [SerializeField]
        private BindingDataContextInfo targetBindingDataContextInfo;
        
        protected override void SetupBindingTarget(Binding binding)
        { 
            binding.SetTarget(targetBindingDataContextInfo.BindableDataContext,targetBindingDataContextInfo.property);   
        }
    }
}


