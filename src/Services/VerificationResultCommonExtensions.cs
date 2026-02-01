using System.Text.RegularExpressions;
using Hamfer.Kernel.Utils;
using Hamfer.Verification.Errors;
using Hamfer.Verification.Models;

namespace Hamfer.Verification.Services;

public static partial class VerificationResultExtensions
{
  /// <summary>
  /// Check value of property over a customized condition/clause
  /// </summary>
  /// <param name="result">Current verification-result instance</param>
  /// <param name="clause">The customized condition/clause</param>
  /// <returns>An instance of `VerificationResult` that updated from current verification-result instance</returns>
  /// <exception cref="LetsVerifyInvalidAssertError"></exception>
  public static VerificationResult By(this VerificationResult result, Func<dynamic, bool> clause)
  => Handle(result, "صحت داشتن", () =>
  {
    var isTrue = clause.Invoke(PropertyValue);

    if (!isTrue)
    {
      throw new LetsVerifyInvalidAssertError($"شرایط مورد در {Name} نظر وجود ندارد!");
    }
  });

  /// <summary>
  /// Check if value of property is equal to an expected value
  /// </summary>
  /// <typeparam name="TExpected">Type of expected value</typeparam>
  /// <param name="result">Current verification-result instance</param>
  /// <param name="expected">The expected value</param>
  /// <returns>An instance of `VerificationResult` that updated from current verification-result instance</returns>
  /// <exception cref="LetsVerifyInvalidAssertError"></exception>
  public static VerificationResult Equals<TExpected>(this VerificationResult result, TExpected expected) where TExpected : IEquatable<TExpected>
  => Handle(result, "برابری", () =>
  {
    // TODO Check for _propertyType IEquatable<TExpected>

    if (!PropertyValue?.Equals(expected))
    {
      throw new LetsVerifyInvalidAssertError($"مقدار !");
    }
  });

  /// <summary>
  /// Check if value of property is not `null`
  /// </summary>
  /// <param name="result">Current verification-result instance</param>
  /// <returns>An instance of `VerificationResult` that updated from current verification-result instance</returns>
  /// <exception cref="LetsVerifyAssertNullError"></exception>
  public static VerificationResult NotNull(this VerificationResult result)
  => Handle(result, "نال نبودن", ()=> 
  { 
    if(IsNull)
    {
      throw new LetsVerifyAssertNullError(Name);
    }
  }, ignoreNull: false);

  /// <summary>
  /// Check if value of property is a kind of number or is a numeric type
  /// </summary>
  /// <param name="result">Current verification-result instance</param>
  /// <returns>An instance of `VerificationResult` that updated from current verification-result instance</returns>
  /// <exception cref="LetsVerifyAssertStringError"></exception>
  public static VerificationResult IsNumeric(this VerificationResult result)
  => Handle(result, "عددی بودن", () =>
  {
    var throwIt = false;
    if (PropertyType == typeof(string))
    {
      if (string.IsNullOrEmpty(PropertyValue))
      {
        return;
      }

      if (!Regex.IsMatch(PropertyValue, @"^[0-9]+$"))
      {
        throwIt = true;
      }
    }
    else
    {
      if (!ValueTypeHelper.IsNumeric(PropertyType))
      {
        throwIt = true;
      }
    }

    if (throwIt)
    {
      throw new LetsVerifyAssertStringError($"مقدار {Name} باید عددی باشد!");
    }
  });

  /// <summary>
  /// Check if value of property can be a member of an `enum`
  /// </summary>
  /// <typeparam name="TEnum">Type of `enum`</typeparam>
  /// <param name="result">Current verification-result instance</param>
  /// <returns>An instance of `VerificationResult` that updated from current verification-result instance</returns>
  /// <exception cref="LetsVerifyAssertStringError"></exception>
  public static VerificationResult IsEnum<TEnum>(this VerificationResult result) where TEnum : struct
  => Handle(result, "مقدار شمارشی بودن", () =>
  {
    if (PropertyType == typeof(string) && string.IsNullOrEmpty(PropertyValue))
    {
      return;
    }

    if(!Enum.TryParse<TEnum>(PropertyValue?.ToString(), out TEnum _))
    {
      throw new LetsVerifyAssertStringError($"مقدار {Name} جزء مقادیر معتبر نمی‌باشد!");
    }
  });
}