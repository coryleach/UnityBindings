using System.Collections;
using System.Collections.Generic;
using Gameframe.Bindings;
using UnityEditor;
using UnityEngine;

namespace Gameframe.Bindings
{
    public class ComponentBinding : BindingBehaviour
    {
        [SerializeField, Header("Target")]
        private BindingDataContextInfo targetBindingDataContextInfo;

        [SerializeField]
        private bool convertToString = false;
        
        protected override void SetupBindingTarget(Binding binding)
        {
            binding.Converter = Convert;
            binding.SetTarget(targetBindingDataContextInfo.BindableDataContext,targetBindingDataContextInfo.property);   
        }

        private object Convert(object sourceData)
        {
            if (convertToString)
            {
                return sourceData.ToString();
            }
            return sourceData;
        }
        
    }
}


