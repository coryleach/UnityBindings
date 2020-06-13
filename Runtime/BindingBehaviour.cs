using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gameframe.Bindings
{
    /// <summary>
    /// Serves as the base class for components that create bindings
    /// See TextBinding as an example
    /// </summary>
    public abstract class BindingBehaviour : MonoBehaviour
    {
        [SerializeField]
        private BindingDataContextInfo _sourceDataInfo = new BindingDataContextInfo();

        [NonSerialized]
        private Binding _binding;

        public void SetSource(Object obj, string path)
        {
            _sourceDataInfo.dataContext = obj;
            _sourceDataInfo.component = null;
            _binding?.SetSource(obj,path,enabled);
        }
        
        private void OnEnable()
        {
            if (_binding == null)
            {
                InitializeBinding();
            }
            else
            {
                _binding.Enabled = true;
                Refresh();
            }
        }

        private void OnDisable()
        {
            if (_binding != null)
            {
                _binding.Enabled = false;
            }
        }

        private void OnDestroy()
        {
            if (_binding != null)
            {
                _binding.Dispose();
                _binding = null;
            }
        }

        private void InitializeBinding()
        {
            _binding = new Binding();
            _binding.SetSource(_sourceDataInfo.BindableDataContext,_sourceDataInfo.property,false);
            SetupBindingTarget(_binding);
            Refresh();
        }

        protected abstract void SetupBindingTarget(Binding binding);

        public virtual void Refresh()
        {
            _binding?.Refresh();
        }
    }
}