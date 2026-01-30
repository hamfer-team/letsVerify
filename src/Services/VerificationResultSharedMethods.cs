using Hamfer.Verification.Models.Errors;
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
  private static void AssertMinLength(ushort minLength)
  {
    if (PropertyValue?.Length < minLength)
    {
      throw new LetsVerifyAssertStringError($"متن {Name} باید حداقل {minLength} حرف داشته باشد!");
    }
  }
  
  /// <summary>
  /// Assert for minimum count of a list
  /// </summary>
  /// <param name="minCount">The mimimum count</param>
  /// <exception cref="LetsVerifyAssertStringError"></exception>
  private static void AssertMinCount(ushort minCount)
  {
    if (EnumerableCount < minCount)
    {
      throw new LetsVerifyAssertStringError($"فهرست {Name} باید حداقل شامل {minCount} قلم باشد!");
    }
  }

  /// <summary>
  /// Assert for maximum size of length
  /// </summary>
  /// <param name="maxLength">The maximum size of length</param>
  /// <exception cref="LetsVerifyAssertStringError"></exception>
  private static void AssertMaxLength(ushort maxLength)
  {
    if (PropertyValue?.Length > maxLength)
    {
      throw new LetsVerifyAssertStringError($"متن {Name} باید حداکثر {maxLength} حرف داشته باشد!");
    }
  }

  /// <summary>
  /// Assert for maximum count of a list
  /// </summary>
  /// <param name="maxCount">The maximum count</param>
  /// <exception cref="LetsVerifyAssertStringError"></exception>
  private static void AssertMaxCount(ushort maxCount)
  {
    if (EnumerableCount > maxCount)
    {
      throw new LetsVerifyAssertStringError($"فهرست {Name} نباید بیشتر از {maxCount} قلم داشته باشد!");
    }
  }
}