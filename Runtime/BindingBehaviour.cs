#if UNITY_EDITOR
//Disable warning about unused variable in exception in the OnValidate function
#pragma warning disable 0168
#endif

using UnityEngine;

namespace Gameframe.Bindings
{
    public abstract class BindingBehaviour : MonoBehaviour
    {
        [SerializeField]
        private BindingDataContextInfo _sourceDataInfo = new BindingDataContextInfo();

        private Binding _binding = null;

        public void SetSource(UnityEngine.Object obj, string path)
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

        /*private static PropertyInfo GetPropertyInfo(object obj, string path)
        {
            if (obj == null || path == null)
            {
                return null;
            }
            Type type = obj.GetType();
            return type.GetProperty(path);
            foreach (var property in path.Split('.'))
            {
                if (obj == null)
                {
                    return null;
                }

                Type type = obj.GetType();
                info = type.GetProperty(property);
                if (info == null)
                {
                    return null;
                }

                obj = info.GetValue(obj, null);
            }
            return info;
        }*/
        
#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (_binding != null)
            {
                _binding.Dispose();
                _binding = null;
            }
            InitializeBinding();
        }

#endif
    }
}