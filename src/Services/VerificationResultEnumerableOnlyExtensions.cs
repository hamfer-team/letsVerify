using HamferTeam.Kernel.Utils;
using HamferTeam.Verification.Models;
using HamferTeam.Verification.Models.Errors;

namespace HamferTeam.Verification.Services;

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
  public static VerificationResult AssertIsMemeberOf<T>(this VerificationResult result, IEnumerable<T>? list)
  => Handle(result, "عضوی از فهرست بودن", () =>
  {
    if (list == null)
    {
      throw new LetsVerifyError("فهرست مقادیر نباید خالی (تهی) باشد!");
    }

    if (!IsEnumerable)
    {
      if (list.All(w => w != PropertyValue))
      {
        throw new LetsVerifyAssertStringError($"مقدار {Name} در فهرست متناظر آن یافت نشد!");
      }
    }
    else
    {
      if (PropertyValue != null)
      {
        foreach (object item in PropertyValue)
        {
          result = For(result, item, "عضو فهرست").AssertIsMemeberOf(list);
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
  public static VerificationResult AssertForEachBy(this VerificationResult result, Func<VerificationResult, VerificationResult> func)
  {
    if (!IsEnumerable)
    {
      return result;
    }

    if (PropertyValue != null)
    {
      foreach (var item in PropertyValue)
      {
        result = func.Invoke(result);
      }
    }

    return result;
  }

  /// <summary>
  /// Invoke verification of all members of a list which are of type `IVerifiable`
  /// </summary>
  /// <param name="result">Current verification-result instance</param>
  /// <returns>An instance of `VerificationResult` that updated from current verification-result instance</returns>
  public static VerificationResult VerifyAllItems(this VerificationResult result)
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
            item.Verify();
        }
      }
    }

    return result;
  }
}