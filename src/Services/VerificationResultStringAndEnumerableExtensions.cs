using HamferTeam.Verification.Models;
using HamferTeam.Verification.Models.Errors;

namespace HamferTeam.Verification.Services;

public static partial class VerificationResultExtensions
{
  /// <summary>
  /// Check if value of a proprty of string or list is not null and is not empty or has a member
  /// </summary>
  /// <param name="result">Current verification-result instance</param>
/// <returns>An instance of `VerificationResult` that updated from current verification-result instance</returns>
  /// <exception cref="LetsVerifyAssertNullError"></exception>
  /// <exception cref="LetsVerifyAssertStringError"></exception>
  public static VerificationResult AssertNotNullOrEmpty(this VerificationResult result)
  => Handle(result, "خالی نبودن", () =>
  {
    if (IsNull)
    {
      throw new LetsVerifyAssertNullError(Name);
    }

    if (PropertyType == typeof(string))
    {
      if (PropertyValue == string.Empty) // (string.IsNullOrEmpty(_propertyValue))
      {
        throw new LetsVerifyAssertStringError($"متن {Name} نباید خالی باشد!");
      }
    }

    if (IsEnumerable && EnumerableCount < 1)
    {
      throw new LetsVerifyAssertStringError($"فهرست {Name} نباید خالی باشد!");
    }

  }, ignoreNull: false);

  /// <summary>
  /// Check if value of property of a string or enumerable is more than a certain length or count
  /// </summary>
  /// <param name="min">The mimimum size</param>
  /// <param name="result">Current verification-result instance</param>
  /// <returns>An instance of `VerificationResult` that updated from current verification-result instance</returns>
  public static VerificationResult AssertMinLength(this VerificationResult result, ushort min)
  => Handle(result, "داشتن حداقل طول", () =>
  {
    if (PropertyType == typeof(string))
    {
      AssertMinLength(min);
    }

    if (IsEnumerable)
    {
      AssertMinCount(min);
    }
  });

  /// <summary>
  /// Check if value of property of a string or enumerable is less than a certain length or count
  /// </summary>
  /// <param name="max">The maximum size</param>
  /// <param name="result">Current verification-result instance</param>
  /// <returns>An instance of `VerificationResult` that updated from current verification-result instance</returns>
  public static VerificationResult AssertMaxLength(this VerificationResult result, ushort max)
  => Handle(result, "داشتن حداکثر طول", () =>
  {
    if (PropertyType == typeof(string))
    {
      AssertMaxLength(max);
    }

    if (IsEnumerable)
    {
      AssertMaxCount(max);
    }
  });

  /// <summary>
  /// Check if value of property of a string or enumerable is more than a certain length or count
  /// </summary>
  /// <param name="min">The mimimum size</param>
  /// <param name="max">The maximum size</param>
  /// <param name="result">Current verification-result instance</param>
  /// <returns>An instance of `VerificationResult` that updated from current verification-result instance</returns>
  public static VerificationResult AssertLengthBetween(this VerificationResult result, ushort min, ushort max)
  => Handle(result, "داشتن طول محدود", () =>
  {
    if (PropertyType == typeof(string))
    {
      AssertMinLength(min);
      AssertMaxLength(max);
    }

    if (IsEnumerable)
    {
      AssertMinCount(min);
      AssertMaxCount(max);
    }
  });

  /// <summary>
  /// Check if value of property of a string or enumerable has a certain length or count
  /// </summary>
  /// <param name="length">The length or count</param>
  /// <param name="result">Current verification-result instance</param>
  /// <returns>An instance of `VerificationResult` that updated from current verification-result instance</returns>
  /// <exception cref="LetsVerifyAssertStringError"></exception>
  public static VerificationResult AssertLength(this VerificationResult result, ushort length)
  => Handle(result, "داشتن طول مشخص", () =>
  {
    if (PropertyType == typeof(string))
    {
      if (PropertyValue?.Length != length)
      {
        throw new LetsVerifyAssertStringError($"متن {Name} باید دقیقاً {length} حرف داشته باشد!");
      }
    }

    if (IsEnumerable)
    {
      if (EnumerableCount != length)
      {
        throw new LetsVerifyAssertStringError($"فهرست {Name} باید فقط و فقط {length} قلم داشته باشد!");
      }
    }
  });
}