using UnityEngine;
using UnityEngine.UI;

namespace GameJam.Bindings
{
  public class TextBindingBehaviour : BindingBehaviour
  {
    [SerializeField]
    Text text;

    [SerializeField]
    string format;

    [ContextMenu("Refresh")]
    protected override void Refresh()
    {
      if (text != null)
      {
        var value = GetPropertyValue();
        if (value != null)
        {
          if ( string.IsNullOrEmpty(format) )
          {
            text.text = value.ToString();
          }
          else
          {
            text.text = string.Format(format, value);
          }
        }
        else
        {
          text.text = string.Empty;
        }
      }
    }

  }

}
