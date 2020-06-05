using UnityEngine;
using UnityEngine.UI;

namespace Gameframe.Bindings
{
  /// <summary>
  /// Used to display text via binding
  /// </summary>
  public class TextBinding : BindingBehaviour
  {
    [SerializeField] 
    private Text text = null;

    [SerializeField] 
    private string format = string.Empty;
    
    protected override void SetupBindingTarget(Binding binding)
    {
      binding.Converter = FormatText;
      binding.SetTarget(text,"text", false);
    }

    private object FormatText(object sourceValue)
    {
      if (sourceValue == null)
      {
        return null;
      }
      return string.IsNullOrEmpty(format) ? sourceValue.ToString() : string.Format(format, sourceValue);
    }

    [ContextMenu("Refresh")]
    public override void Refresh()
    {
      base.Refresh();
    }
    
#if UNITY_EDITOR
    protected override void OnValidate()
    {
      if (text == null)
      {
        text = GetComponent<Text>();
      }
      base.OnValidate();
    }
#endif
    
  }
}
