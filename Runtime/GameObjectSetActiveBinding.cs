using System;
using UnityEngine;

namespace Gameframe.Bindings
{
  public class GameObjectSetActiveBinding : BindingBehaviour
  {
    public GameObject target;
    public ConversionType conversionType = ConversionType.EnableWhenNotEqual;
    public int numberCompareValue = 0;
    public string stringCompareValue = string.Empty;
    public bool invert = false;
    
    public enum ConversionType
    {
      EnableWhenNotEqual,
      EnableWhenGreaterThan,
      EnableWhenLessThan,
      EnableWhenNotNull,
      EnableWhenStringEquals
    }

    private class GameObjectEnabler
    {
      public GameObject target;
      public bool Active
      {
        get => target.activeSelf;
        set => target.SetActive(value);
      }
    }

    private GameObjectEnabler _enabler = new GameObjectEnabler();
    
    protected override void SetupBindingTarget(Binding binding)
    {
      binding.Converter = Converter;
      _enabler.target = target;
      binding.SetTarget(_enabler,nameof(GameObjectEnabler.Active), false);
    }

    private object Converter(object sourceValue)
    {
      if (invert)
      {
        return !ConvertValue(sourceValue);
      }
      return ConvertValue(sourceValue);
    }

    private bool ConvertValue(object sourceValue)
    {
      if (sourceValue == null)
      {
        return false;
      }

      switch (conversionType)
      {
        case ConversionType.EnableWhenNotNull:
          return true;
        case ConversionType.EnableWhenNotEqual:
          return ConvertNotEqual(sourceValue);
        case ConversionType.EnableWhenGreaterThan:
          return ConvertGreaterThan(sourceValue);
        case ConversionType.EnableWhenLessThan:
          return ConvertLessThan(sourceValue);
        case ConversionType.EnableWhenStringEquals:
          return ((string)sourceValue) == stringCompareValue;
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

  }
}

