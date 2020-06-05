using System;
using System.Collections;
using System.Collections.Generic;
using Gameframe.Bindings;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameframe.Bindings
{
    /// <summary>
    /// This component will create a binding between any two UnityEngine.Object types 
    /// </summary>
    public class ComponentBinding : BindingBehaviour
    {
        [SerializeField]
        private BindingDataContextInfo targetPropertyInfo = null;

        [SerializeField]
        private bool convertToString = false;
        
        protected override void SetupBindingTarget(Binding binding)
        {
            try
            {
                if (binding == null)
                {
                    return;
                }
                binding.Converter = Convert;
                binding.SetTarget(targetPropertyInfo?.BindableDataContext,targetPropertyInfo?.property);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
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


