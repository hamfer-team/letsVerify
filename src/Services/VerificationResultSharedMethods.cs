using Hamfer.Verification.Errors;
using Microsoft.CSharp.RuntimeBinder;

namespace Hamfer.Verification.Services;

public static partial class VerificationResultExtensions
{
  /// <summary>
  /// Check if value is null or not
  /// With considering it may already checked
  /// </summary>
  private static void CheckNullValue()
  {
    IsNullChecked = true;

    try
    {
      if (DefaultValue == null && PropertyValue == null)
      {
        IsNull = true;
      }
    }
    catch(RuntimeBinderException error) when (error.Message.Trim().StartsWith("operator '=='", StringComparison.InvariantCultureIgnoreCase))
    {
      // `==` operator is not defined for checking null so it should be a kind of ValueType!!!
      IsNull = false;
    }
    catch
    {
      throw;
    }
  }

  /// <summary>
  /// Assert for minimum size of length
  /// </summary>
  /// <param name="minLength">The minimum size of length</param>
  /// <exception cref="LetsVerifyAssertStringError"></exception>
  private static void MinLength(ushort minLength, string? objectName)
  {
    if (PropertyValue?.Length < minLength)
    {
      throw new LetsVerifyAssertStringError(objectName, $"متن {Name} باید حداقل {minLength} حرف داشته باشد!");
    }
  }
  
  /// <summary>
  /// Assert for minimum count of a list
  /// </summary>
  /// <param name="minCount">The mimimum count</param>
  /// <exception cref="LetsVerifyAssertStringError"></exception>
  private static void MinCount(ushort minCount, string? objectName)
  {
    if (EnumerableCount < minCount)
    {
      throw new LetsVerifyAssertStringError(objectName, $"فهرست {Name} باید حداقل شامل {minCount} قلم باشد!");
    }
  }

  /// <summary>
  /// Assert for maximum size of length
  /// </summary>
  /// <param name="maxLength">The maximum size of length</param>
  /// <exception cref="LetsVerifyAssertStringError"></exception>
  private static void MaxLength(ushort maxLength, string? objectName)
  {
    if (PropertyValue?.Length > maxLength)
    {
      throw new LetsVerifyAssertStringError(objectName, $"متن {Name} باید حداکثر {maxLength} حرف داشته باشد!");
    }
  }

  /// <summary>
  /// Assert for maximum count of a list
  /// </summary>
  /// <param name="maxCount">The maximum count</param>
  /// <exception cref="LetsVerifyAssertStringError"></exception>
  private static void MaxCount(ushort maxCount, string? objectName)
  {
    if (EnumerableCount > maxCount)
    {
      throw new LetsVerifyAssertStringError(objectName, $"فهرست {Name} نباید بیشتر از {maxCount} قلم داشته باشد!");
    }
  }
}