using Hamfer.Verification.Models;
using Hamfer.Verification.Models.Errors;

namespace Hamfer.Verification.Services;

public static partial class VerificationResultExtensions
{
  private static string? Name;
  private static Type? PropertyType;
  private static dynamic? PropertyValue;
  private static bool IsNull;
  private static bool IsNullChecked;
  private static bool IsEnumerable;
  private static int EnumerableCount;
  private static dynamic? DefaultValue;

  private static VerificationResult Handle(VerificationResult result, string actionName, Action? act = null, bool ignoreNull = true)
  {
    try
    {
      if (!IsNullChecked)
      {
        CheckNullValue();
      }

      // بررسی مقدار نال نیازی نمی‌باشد
      if (IsNull && ignoreNull)
      {
        result.AddLog($"بررسی {actionName} به علت تهی بودن انجام نشد.");
        return result;
      }

      act?.Invoke();

      result.AddLog($"در بررسی {actionName} اشکالی یافت نشد.");
      return result;
    }
    catch (LetsVerifyError error)
    {
        result.AddError(error);
        result.AddLog($"خطا در بررسی {actionName}:" + error.Message);
    }
    catch (Exception error)
    {
        var unhandledError = new LetsVerifyUnhandledError(error);
        result.AddError(unhandledError);
        result.AddLog($"###. خطای مدیریت نشده در بررسی {actionName}:" + error.Message);
    }

    return result;
  }

  private static LetsVerifyAggregateError? PrepareErrors(this VerificationResult result)
  {
    if (result.HasError)
    {
      return new LetsVerifyAggregateError($"در بررسی {result.Errors.Count} مورد اشکال شناسایی گردید.", [.. result.Errors]);
    }

    return null;
  }
}