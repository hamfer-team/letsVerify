using Hamfer.Kernel.Utils;
using Hamfer.Verification.Models;
using System.Collections;

namespace Hamfer.Verification.Services;

public static partial class VerificationResultExtensions
{
  /// <summary>
  /// Assign a property of the instance the you want to verfiy over it
  /// </summary>
  /// <typeparam name="TProperty">Type of property</typeparam>
  /// <param name="result">Current verification-result instance</param>
  /// <param name="property">The property of instance or any other variable</param>
  /// <param name="name">The name of property for refering to it in messages</param>
  /// <returns>An instance of `VerificationResult` that updated from current verification-result instance</returns>
  public static VerificationResult Assert<TProperty>(this VerificationResult result, TProperty property, string name)
  {
    Name = name;
    PropertyValue = property;
    PropertyType = property?.GetType() ?? typeof(TProperty);
    IsNull = false;
    IsNullChecked = false;
    IsEnumerable = typeof(IEnumerable).IsAssignableFrom(PropertyType) && PropertyType != typeof(string);
    DefaultValue = TypeHelper.GetDefault(PropertyType);

    if (IsEnumerable)
    {
      EnumerableCount = PropertyType.IsArray ? PropertyValue?.Length : (PropertyValue as IEnumerable<object>)?.Count();
    }

    result.addLog($"+++. فیلدی با نام {name} جهت بررسی تعیین شد.");
    return result;
  }

  /// <summary>
  /// Assign a property in the ignored-list.
  /// </summary>
  /// <param name="result">Current verification-result instance</param>
  /// <param name="property">The property of instance</param>
  /// <param name="name">The name of property for refering to it in messages</param>
  /// <returns>An instance of `VerificationResult` that updated from current verification-result instance</returns>
  public static VerificationResult Ignore(this VerificationResult result, dynamic property, string name)
  {
    PropertyValue = property;
    result.addLog($"---. از بررسی فیلدی با نام {name} صرف نظر شد.");
    return result;
  }
}