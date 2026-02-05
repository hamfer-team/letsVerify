using Hamfer.Kernel.Utils;
using Hamfer.Verification.Errors;
using Hamfer.Verification.Models;

namespace Hamfer.Verification.Services;

public static partial class VerificationResultExtensions
{
  /// <summary>
  /// Check if value of proprty is member of a list of values
  /// If property is enumerable this will be check all members of it over member of that list 
  /// </summary>
  /// <typeparam name="T">Type of items of the enumerable</typeparam>
  /// <param name="list">The list of values</param>
  /// <param name="result">Current verification-result instance</param>
  /// <returns>An instance of `VerificationResult` that updated from current verification-result instance</returns>
  /// <exception cref="LetsVerifyError"></exception>
  /// <exception cref="LetsVerifyAssertStringError"></exception>
  public static VerificationResult IsMemeberOf<T>(this VerificationResult result, IEnumerable<T>? list)
  => Handle(result, "عضوی از فهرست بودن", () =>
  {
    if (list == null)
    {
      throw new LetsVerifyError(result.objectName, "فهرست مقادیر نباید خالی (تهی) باشد!");
    }

    if (!IsEnumerable)
    {
      if (list.All(w => w != PropertyValue))
      {
        throw new LetsVerifyAssertStringError(result.objectName, $"مقدار {Name} در فهرست متناظر آن یافت نشد!");
      }
    }
    else
    {
      if (PropertyValue != null)
      {
        foreach (object item in PropertyValue)
        {
          result = Assert(result, item, "عضو فهرست").IsMemeberOf(list);
        }
      }
    }
  });

  /// <summary>
  /// Check a custom verification over each member of value of an enumerable proprty
  /// </summary>
  /// <param name="func">The custom verification</param>
  /// <param name="result">Current verification-result instance</param>
  /// <returns>An instance of `VerificationResult` that updated from current verification-result instance</returns>
  public static VerificationResult ForEachBy<TItem>(this VerificationResult result, Func<VerificationResult, TItem, VerificationResult> func, string? itemName = null)
  {
    if (!IsEnumerable)
    {
      return result;
    }

    if (PropertyValue != null)
    {
      foreach (TItem item in PropertyValue)
      {
        VerificationResult itemResult = LetsVerify.On(item, itemName);
        itemResult.parentResult = result;
        result = func.Invoke(itemResult, item).End();
      }
    }

    return result;
  }

  /// <summary>
  /// Invoke verification of all members of a list which are of type `IVerifiable`
  /// </summary>
  /// <param name="result">Current verification-result instance</param>
  /// <returns>An instance of `VerificationResult` that updated from current verification-result instance</returns>
  public static VerificationResult VerifyAll(this VerificationResult result)
  {
    if (!IsEnumerable)
    {
      return result;
    }

    if (PropertyValue != null)
    {
      foreach (dynamic item in PropertyValue)
      {
        if(ReferenceTypeHelper.IsDerivedOfGenericInterface(item.GetType(), typeof(IVerifiable<>)))
        {
            item.verify();
        }
      }
    }

    return result;
  }
}