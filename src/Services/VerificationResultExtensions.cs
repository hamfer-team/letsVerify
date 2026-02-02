using Hamfer.Verification.Errors;
using Hamfer.Verification.Models;

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
        result.addLog($"بررسی {actionName} به علت تهی بودن انجام نشد.");
        return result;
      }

      act?.Invoke();

      result.addLog($"در بررسی {actionName} اشکالی یافت نشد.");
      return result;
    }
    catch (LetsVerifyError error)
    {
        result.addError(error);
        result.addLog($"خطا در بررسی {actionName}:" + error.Message);
    }
    catch (Exception error)
    {
        var unhandledError = new LetsVerifyUnhandledError(result.objectName, error);
        result.addError(unhandledError);
        result.addLog($"###. خطای مدیریت نشده در بررسی {actionName}:" + error.Message);
    }

    return result;
  }

  private static LetsVerifyAggregateError? PrepareErrors(this VerificationResult result)
  {
    if (result.hasError)
    {
      return new LetsVerifyAggregateError(result.objectName, $"در بررسی {result.errors.Count} مورد اشکال شناسایی گردید.", [.. result.errors]);
    }

    return null;
  }
}