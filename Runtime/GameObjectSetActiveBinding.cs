using System;
using UnityEngine;

namespace Gameframe.Bindings
{
  public class GameObjectSetActiveBinding : BindingBehaviour
  {
    public GameObject target;
    public ConversionType conversionType = ConversionType.EnableWhenNumberNotEqual;
    public int numberCompareValue = 0;
    public string stringCompareValue = string.Empty;
    public bool invert = false;
    
    public enum ConversionType
    {
      None,
      EnableWhenNumberNotEqual,
      EnableWhenNumberGreaterThan,
      EnableWhenNumberLessThan,
      EnableWhenObjectNotNull,
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

  }
}

