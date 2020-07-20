using System;
using UnityEngine;

namespace Gameframe.Bindings
{
  /// <summary>
  /// Binding component for controlling the active state of a GameObject
  /// </summary>
  public class GameObjectSetActiveBinding : BindingBehaviour
  {
    public enum ConversionType
    {
      None,
      EnableWhenNumberNotEqual,
      EnableWhenNumberGreaterThan,
      EnableWhenNumberLessThan,
      EnableWhenObjectNotNull,
      EnableWhenStringEquals
    }
    
    [SerializeField] private GameObject target;
    [SerializeField] private ConversionType conversionType = ConversionType.EnableWhenNumberNotEqual;
    [SerializeField] private int numberCompareValue = 0;
    [SerializeField] private string stringCompareValue = string.Empty;
    [SerializeField] private bool invert = false;

    private readonly GameObjectEnabler _enabler = new GameObjectEnabler();
    
    public GameObject Target
    {
      get => target;
      set => target = value;
    }

    public ConversionType Conversion
    {
      get => conversionType;
      set => conversionType = value;
    }

    public int NumberCompareValue
    {
      get => numberCompareValue;
      set => numberCompareValue = value;
    }

    public string StringCompareValue
    {
      get => stringCompareValue;
      set => stringCompareValue = value;
    }

    public bool Invert
    {
      get => invert;
      set => invert = value;
    }
    
    protected override void SetupBindingTarget(Binding binding)
    {
      binding.Converter = Converter;
      _enabler.Target = target;
      binding.SetTarget(_enabler,nameof(GameObjectEnabler.Active), false);
    }

    private object Converter(object sourceValue)
    {
      try
      {
        if (invert)
        {
          return !ConvertValue(sourceValue);
        }

        return ConvertValue(sourceValue);
      }
      catch (Exception exception)
      {
        Debug.LogError($"GameObjectSetActiveBinding conversion failed with exception: {exception}", this);
        enabled = false;
        return false;
      }
    }

    private bool ConvertValue(object sourceValue)
    {
      if (sourceValue == null)
      {
        return false;
      }

      switch (conversionType)
      {
        case ConversionType.EnableWhenObjectNotNull:
          return true;
        case ConversionType.EnableWhenNumberNotEqual:
          return ConvertNotEqual(sourceValue);
        case ConversionType.EnableWhenNumberGreaterThan:
          return ConvertGreaterThan(sourceValue);
        case ConversionType.EnableWhenNumberLessThan:
          return ConvertLessThan(sourceValue);
        case ConversionType.EnableWhenStringEquals:
          return ((string)sourceValue) == stringCompareValue;
        case ConversionType.None:
          return (bool)sourceValue;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private bool ConvertNotEqual(object sourceValue)
    {
      switch (Type.GetTypeCode(sourceValue.GetType()))
      {
        case TypeCode.Boolean:
          return (bool)sourceValue;
        case TypeCode.Int16:
          return (short)sourceValue != numberCompareValue;
        case TypeCode.Int32:
          return (int)sourceValue != numberCompareValue;
        case TypeCode.Int64:
          return (long)sourceValue != numberCompareValue;
        case TypeCode.Byte:
          return (byte)sourceValue != numberCompareValue;
        case TypeCode.Char:
          return (char)sourceValue != numberCompareValue;
        case TypeCode.Decimal:
          return (decimal)sourceValue != numberCompareValue;
        case TypeCode.SByte:
          return (sbyte)sourceValue != numberCompareValue;
        case TypeCode.Single:
          return (float)sourceValue != numberCompareValue;
        case TypeCode.Double:
          return (double)sourceValue != numberCompareValue;
        case TypeCode.UInt16:
          return (ushort)sourceValue != numberCompareValue;
        case TypeCode.UInt32:
          return (uint)sourceValue != numberCompareValue;
        case TypeCode.UInt64:
          return (ulong)sourceValue != (ulong)numberCompareValue;
        default:
          return false;
      }
    }
    
    private bool ConvertGreaterThan(object sourceValue)
    {
      switch (Type.GetTypeCode(sourceValue.GetType()))
      {
        case TypeCode.Boolean:
          return (bool)sourceValue;
        case TypeCode.Int16:
          return (short)sourceValue > numberCompareValue;
        case TypeCode.Int32:
          return (int)sourceValue > numberCompareValue;
        case TypeCode.Int64:
          return (long)sourceValue > numberCompareValue;
        case TypeCode.Byte:
          return (byte)sourceValue > numberCompareValue;
        case TypeCode.Char:
          return (char)sourceValue > numberCompareValue;
        case TypeCode.Decimal:
          return (decimal)sourceValue > numberCompareValue;
        case TypeCode.SByte:
          return (sbyte)sourceValue > numberCompareValue;
        case TypeCode.Single:
          return (float)sourceValue > numberCompareValue;
        case TypeCode.Double:
          return (double)sourceValue > numberCompareValue;
        case TypeCode.UInt16:
          return (ushort)sourceValue > numberCompareValue;
        case TypeCode.UInt32:
          return (uint)sourceValue > numberCompareValue;
        case TypeCode.UInt64:
          return (ulong)sourceValue > (ulong)numberCompareValue;
        default:
          return false;
      }
    }
    
    private bool ConvertLessThan(object sourceValue)
    {
      switch (Type.GetTypeCode(sourceValue.GetType()))
      {
        case TypeCode.Boolean:
          return (bool)sourceValue;
        case TypeCode.Int16:
          return (short)sourceValue < numberCompareValue;
        case TypeCode.Int32:
          return (int)sourceValue < numberCompareValue;
        case TypeCode.Int64:
          return (long)sourceValue < numberCompareValue;
        case TypeCode.Byte:
          return (byte)sourceValue < numberCompareValue;
        case TypeCode.Char:
          return (char)sourceValue < numberCompareValue;
        case TypeCode.Decimal:
          return (decimal)sourceValue < numberCompareValue;
        case TypeCode.SByte:
          return (sbyte)sourceValue < numberCompareValue;
        case TypeCode.Single:
          return (float)sourceValue < numberCompareValue;
        case TypeCode.Double:
          return (double)sourceValue < numberCompareValue;
        case TypeCode.UInt16:
          return (ushort)sourceValue < numberCompareValue;
        case TypeCode.UInt32:
          return (uint)sourceValue < numberCompareValue;
        case TypeCode.UInt64:
          return (ulong)sourceValue < (ulong)numberCompareValue;
        default:
          return false;
      }
    }
    
    private class GameObjectEnabler
    {
      private GameObject target;

      public GameObject Target
      {
        get => target;
        set => target = value;
      }

      public bool Active
      {
        get => target.activeSelf;
        set => target.SetActive(value);
      }
    }

  }
}

