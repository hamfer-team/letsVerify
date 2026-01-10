using HamferTeam.Kernel.Utils;
using HamferTeam.LetsVerify.Models;
using HamferTeam.LetsVerify.Models.Errors;
using Microsoft.CSharp.RuntimeBinder;
using System.Collections;
using System.Text.RegularExpressions;

namespace HamferTeam.LetsVerify.Utils;

public static class VerificationResultExtensions
{
  private static string? _name;
  private static Type? _propertyType;
  private static dynamic? _propertyValue;
  private static bool _isNull;
  private static bool _isNullChecked;
  private static bool _isEnumerable;
  private static int _enumerableCount;
  private static dynamic? _defaultValue;

  #region Proprty Getter
  public static VerificationResult For<TProperty>(this VerificationResult result, TProperty property, string name)
  {
    _name = name;
    _propertyValue = property;
    _propertyType = property?.GetType() ?? typeof(TProperty);
    _isNull = false;
    _isNullChecked = false;
    _isEnumerable = typeof(IEnumerable).IsAssignableFrom(_propertyType) && _propertyType != typeof(string);
    _defaultValue = TypeHelper.GetDefault(_propertyType);

    if (_isEnumerable)
    {
      _enumerableCount = _propertyType.IsArray ? _propertyValue?.Length : (_propertyValue as IEnumerable<object>)?.Count();
    }

    result.AddLog($"+++. فیلدی با نام {name} جهت بررسی تعیین شد.");
    return result;
  }

  public static VerificationResult AndIgnore(this VerificationResult result, dynamic property, string name)
  {
    _propertyValue = property;
    result.AddLog($"---. از بررسی فیلدی با نام {name} صرف نظر شد.");
    return result;
  }
  #endregion

  #region Property Assertions
  public static VerificationResult AssertTrue(this VerificationResult result, Func<dynamic, bool> clause)
  => Handle(result, "صحت داشتن", () =>
  {
    var isTrue = clause.Invoke(_propertyValue);

    if (!isTrue)
    {
      throw new LetsVerifyInvalidAssertError($"شرایط مورد در {_name} نظر وجود ندارد!");
    }
  });

  public static VerificationResult AssertEquals<TExpected>(this VerificationResult result, TExpected expected) where TExpected : IEquatable<TExpected>
  => Handle(result, "برابری", () =>
  {
    // TODO Check for _propertyType IEquatable<TExpected>

    if (!_propertyValue?.Equals(expected))
    {
      throw new LetsVerifyInvalidAssertError($"مقدار !");
    }
  });

  public static VerificationResult AssertNotNull(this VerificationResult result)
  => Handle(result, "نال نبودن", ()=> 
  { 
    if(_isNull)
    {
      throw new LetsVerifyAssertNullError(_name);
    }
  }, ignoreNull: false);

  public static VerificationResult AssertIsNumeric(this VerificationResult result)
  => Handle(result, "عددی بودن", () =>
  {
    var throwIt = false;
    if (_propertyType == typeof(string))
    {
      if (string.IsNullOrEmpty(_propertyValue))
      {
        return;
      }

      if (!Regex.IsMatch(_propertyValue, @"^[0-9]+$"))
      {
        throwIt = true;
      }
    }
    else
    {
      if (!ValueTypeHelper.IsNumeric(_propertyType))
      {
        throwIt = true;
      }
    }

    if (throwIt)
    {
      throw new LetsVerifyAssertStringError($"مقدار {_name} باید عددی باشد!");
    }
  });

  public static VerificationResult AssertIsEnum<TEnum>(this VerificationResult result) where TEnum : struct
  => Handle(result, "مقدار شمارشی بودن", () =>
  {
    if (_propertyType == typeof(string) && string.IsNullOrEmpty(_propertyValue))
    {
      return;
    }

    if(!Enum.TryParse<TEnum>(_propertyValue?.ToString(), out TEnum _))
    {
      throw new LetsVerifyAssertStringError($"مقدار {_name} جزء مقادیر معتبر نمی‌باشد!");
    }
  });

  #region Property Assertions for STRINGS and for Enumerables!
  public static VerificationResult AssertNotNullOrEmpty(this VerificationResult result)
  => Handle(result, "خالی نبودن", () =>
  {
    if (_isNull)
    {
      throw new LetsVerifyAssertNullError(_name);
    }

    if (_propertyType == typeof(string))
    {
      if (_propertyValue == string.Empty) // (string.IsNullOrEmpty(_propertyValue))
      {
        throw new LetsVerifyAssertStringError($"متن {_name} نباید خالی باشد!");
      }
    }

    if (_isEnumerable && _enumerableCount < 1)
    {
      throw new LetsVerifyAssertStringError($"فهرست {_name} نباید خالی باشد!");
    }

  }, ignoreNull: false);

  public static VerificationResult AssertMinLength(this VerificationResult result, ushort min)
  => Handle(result, "داشتن حداقل طول", () =>
  {
    if (_propertyType == typeof(string))
    {
      AssertMinLength(min);
    }

    if (_isEnumerable)
    {
      AssertMinCount(min);
    }
  });

  public static VerificationResult AssertMaxLength(this VerificationResult result, ushort max)
  => Handle(result, "داشتن حداکثر طول", () =>
  {
    if (_propertyType == typeof(string))
    {
      AssertMaxLength(max);
    }

    if (_isEnumerable)
    {
      AssertMaxCount(max);
    }
  });

  public static VerificationResult AssertLengthBetween(this VerificationResult result, ushort min, ushort max)
  => Handle(result, "داشتن طول محدود", () =>
  {
    if (_propertyType == typeof(string))
    {
      AssertMinLength(min);
      AssertMaxLength(max);
    }

    if (_isEnumerable)
    {
      AssertMinCount(min);
      AssertMaxCount(max);
    }
  });

  public static VerificationResult AssertLength(this VerificationResult result, ushort length)
  => Handle(result, "داشتن طول مشخص", () =>
  {
    if (_propertyType == typeof(string))
    {
      if (_propertyValue?.Length != length)
      {
        throw new LetsVerifyAssertStringError($"متن {_name} باید دقیقاً {length} حرف داشته باشد!");
      }
    }

    if (_isEnumerable)
    {
      if (_enumerableCount != length)
      {
        throw new LetsVerifyAssertStringError($"فهرست {_name} باید فقط و فقط {length} قلم داشته باشد!");
      }
    }
  });
  #endregion

  #region Property Assertions for STRINGS only!
  public static VerificationResult AssertMatch(this VerificationResult result, string regexPattern)
  => Handle(result, "تطبیق قالب", () =>
  {
    if (_propertyType != typeof(string) || string.IsNullOrEmpty(_propertyValue))
    {
      return;
    }

    if (!Regex.IsMatch(_propertyValue, regexPattern))
    {
      throw new LetsVerifyAssertStringError($"متن {_name} منطبق بر قالب درخواستی نمی‌باشد!");
    }
  });

  public static VerificationResult AssertIsNationalCode(this VerificationResult result)
  => Handle(result, "کد ملی بودن", () =>
  {
    if (_propertyType != typeof(string) || string.IsNullOrEmpty(_propertyValue))
    {
      return;
    }

    if (!PersianStringHelper.VerifyNationalCodeCheckSum(_propertyValue))
    {
      throw new LetsVerifyAssertStringError($"مقدار {_name} یک کد ملی معتبر نمی‌باشد!");
    }
  });

  public static VerificationResult AssertIsMobileNo(this VerificationResult result)
  => Handle(result, "شماره همراه بودن", () =>
  {
    if (_propertyType != typeof(string) || string.IsNullOrEmpty(_propertyValue))
    {
      return;
    }

    if (!Regex.IsMatch(_propertyValue, @"^09[0-9]{9}$"))
    {
      throw new LetsVerifyAssertStringError($"مقدار {_name}({_propertyValue}) یک شماره همراه معتبر نمی‌باشد!");
    }
  });

  public static VerificationResult AssertIsPersianDate(this VerificationResult result)
  => Handle(result, "تاریخ شمسی بودن", () =>
  {
    if (_propertyType != typeof(string) || string.IsNullOrEmpty(_propertyValue))
    {
      return;
    }

    if (!Regex.IsMatch(_propertyValue, @"^\d{4}[\\\/\- ]\d\d?[\\\/\- ]\d\d?([ ,\-]\d\d?[\\\.\:\,](\d\d)?([\\\.\:\,](\d\d)?([[\\\.\:\,](\d{1,6})])?)?)?$"))
    {
      throw new LetsVerifyAssertStringError($"مقدار {_name} یک تاریخ شمسی معتبر نمی‌باشد!");
    }
  });

  public static VerificationResult AssertIsFileName(this VerificationResult result)
  => Handle(result, "مناسب نام فایل بودن", () =>
  {
    if (_propertyType != typeof(string) || string.IsNullOrEmpty(_propertyValue))
    {
      return;
    }

    if (!IOHelper.IsFileNameValid(_propertyValue))
    {
      throw new LetsVerifyAssertStringError($"مقدار {_name} نمی‌تواند نام فایل باشد!");
    }
  });

  public static VerificationResult AssertCSV(this VerificationResult result, out string[]? parts)
  {
    parts = null;
    var isOk = false;

    var vresult = Handle(result, "تطبیق سی-اس-وی", () =>
    {
      if (_propertyType != typeof(string) || string.IsNullOrEmpty(_propertyValue))
      {
        return;
      }

      if (!Regex.IsMatch(_propertyValue, @"^([^,]+)(,[^,\r\n]+)+$"))
      {
        throw new LetsVerifyAssertStringError($"متن {_name} منطبق بر قالب سی-اس-وی نمی‌باشد!");
      }

      isOk = true;
    });

    if (isOk)
    {
      parts = _propertyValue?.Split(',');
    }

    return vresult;
  }
  #endregion

  #region Property Assertions for ENUMERABLES only!
  public static VerificationResult AssertIsMemeberOf<T>(this VerificationResult result, IEnumerable<T> list)
  => Handle(result, "عضوی از فهرست بودن", () =>
  {
    if (!_isEnumerable)
    {
      if (list.All(w => w != _propertyValue))
      {
        throw new LetsVerifyAssertStringError($"مقدار {_name} در فهرست متناظر آن یافت نشد!");
      }
    }
    else
    {
      if (_propertyValue != null)
      {
        foreach (object item in _propertyValue)
        {
          result = For(result, item, "عضو فهرست").AssertIsMemeberOf(list);
        }
      }
    }
  });

  public static VerificationResult AssertForEachBy(this VerificationResult result, Func<VerificationResult, VerificationResult> func)
  {
    if (!_isEnumerable)
    {
      return result;
    }

    if (_propertyValue != null)
    {
      foreach (var item in _propertyValue)
      {
        result = func.Invoke(result);
      }
    }

    return result;
  }

  public static VerificationResult VerifyAllItems(this VerificationResult result)
  {
    if (!_isEnumerable)
    {
      return result;
    }

    if (_propertyValue != null)
    {
      foreach (dynamic item in _propertyValue)
      {
        if(ReferenceTypeHelper.IsDerivedOfGenericInterface(item.GetType(), typeof(IVerifiable<>)))
        {
            item.Verify();
        }
      }
    }

    return result;
  }
  #endregion
  #endregion

  #region Verification Finishers
  public static void ThenJustSendOutResult(this VerificationResult result, out VerificationResult outResult)
  {
    outResult = result;
  }

  public static void ThenRaiseException(this VerificationResult result)
  {
    var ex = result.ConverToException();

    if (ex != null)
    {
      throw ex;
    }
  }
  #endregion

  #region Private methods
  private static VerificationResult Handle(VerificationResult result, string actionName, Action? act = null, bool ignoreNull = true)
  {
    try
    {
      if (!_isNullChecked)
      {
        CheckNullValue();
      }

      // بررسی مقدار نال نیازی نمی‌باشد
      if (_isNull && ignoreNull)
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
        result.AddException(error);
        result.AddLog($"خطا در بررسی {actionName}:" + error.Message);
    }
    catch (Exception error)
    {
        var unhandledError = new LetsVerifyUnhandledError(error);
        result.AddException(unhandledError);
        result.AddLog($"###. خطای مدیریت نشده در بررسی {actionName}:" + error.Message);
    }

    return result;
  }

  private static LetsVerifyError? ConverToException(this VerificationResult result)
  {
    if (result.HasException)
    {
      return new LetsVerifyAggregateError($"در بررسی {result.Exceptions.Count} مورد اشکال شناسایی گردید.", [.. result.Exceptions]);
    }

    return null;
  }

  private static void CheckNullValue()
  {
    _isNullChecked = true;

    try
    {
      if (_defaultValue == null && _propertyValue == null)
      {
        _isNull = true;
      }
    }
    catch(RuntimeBinderException error) when (error.Message.Trim().StartsWith("operator '=='", StringComparison.InvariantCultureIgnoreCase))
    {
      // `==` operator is not defined for checking null so it should be a kind of ValueType!!!
      _isNull = false;
    }
    catch
    {
      throw;
    }
  }

  private static void AssertMinLength(ushort minLength)
  {
    if (_propertyValue?.Length < minLength)
    {
      throw new LetsVerifyAssertStringError($"متن {_name} باید حداقل {minLength} حرف داشته باشد!");
    }
  }

  private static void AssertMinCount(ushort minCount)
  {
    if (_enumerableCount < minCount)
    {
      throw new LetsVerifyAssertStringError($"فهرست {_name} باید حداقل شامل {minCount} قلم باشد!");
    }
  }

  private static void AssertMaxLength(ushort maxLength)
  {
    if (_propertyValue?.Length > maxLength)
    {
      throw new LetsVerifyAssertStringError($"متن {_name} باید حداکثر {maxLength} حرف داشته باشد!");
    }
  }

  private static void AssertMaxCount(ushort maxCount)
  {
    if (_enumerableCount > maxCount)
    {
      throw new LetsVerifyAssertStringError($"فهرست {_name} نباید بیشتر از {maxCount} قلم داشته باشد!");
    }
  }

  #endregion
}