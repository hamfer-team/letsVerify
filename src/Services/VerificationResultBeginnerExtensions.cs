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
      EnumerableCount = PropertyType.IsArray ? (PropertyValue?.Length ?? 0) : ((PropertyValue as IEnumerable<object>)?.Count() ?? 0);
      // Console.WriteLine($"🔆 [{name}] is IsEnumerable with {EnumerableCount} members!");
    }

    result.addLog($"+++. فیلدی با نام {name} جهت بررسی تعیین شد.");
    return result;
  }

  /// <summary>
  /// THEN verify ON a new object.
  /// **Note**: Use `End()` for finidhing this child veification.
  /// </summary>
  /// <typeparam name="TObject">Type of verifing object</typeparam>
  /// <param name="result">A new child verification-result instance</param>
  /// <param name="object">The vrifing object</param>
  /// <param name="objectName">The name of verifing object</param>
  /// <returns>An instance of `VerificationResult` as a child</returns>
  public static VerificationResult ThenOn<TObject>(this VerificationResult result, TObject @object, string? objectName = null)
    where TObject : class, IVerifiable<TObject>
  {
    VerificationResult vr = LetsVerify.On(@object, objectName);
    vr.parentResult = result;
    return vr;
  }

  /// <summary>
  /// End of `THENON` that considered about a child and returns parent to continue verification on it.
  /// </summary>
  /// <param name="result">The parent or current verification-result</param>
  /// <returns>An instance of parent or current verification-result</returns>
  public static VerificationResult End(this VerificationResult result)
  {
    return result.parentResult ?? result;
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