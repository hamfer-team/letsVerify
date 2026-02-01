using System.Text.RegularExpressions;
using Hamfer.Kernel.Utils;
using Hamfer.Verification.Errors;
using Hamfer.Verification.Models;

namespace Hamfer.Verification.Services;

public static partial class VerificationResultExtensions
{
  /// <summary>
  /// Check if value of a string proprty can match with a regular-expression pattern
  /// </summary>
  /// <param name="regexPattern">The reqular-expression pattern</param>
  /// <param name="result">Current verification-result instance</param>
  /// <returns>An instance of `VerificationResult` that updated from current verification-result instance</returns>
  public static VerificationResult Match(this VerificationResult result, string regexPattern)
  => Match(result, new Regex(regexPattern));

  /// <summary>
  /// Check if value of a string proprty can match with a regular-expression
  /// </summary>
  /// <param name="regex">The regular-expression instance</param>
  /// <param name="result">Current verification-result instance</param>
  /// <returns>An instance of `VerificationResult` that updated from current verification-result instance</returns>
  /// <exception cref="LetsVerifyAssertStringError"></exception>
  public static VerificationResult Match(this VerificationResult result, Regex regex)
  => Handle(result, "تطبیق قالب", () =>
  {
    if (PropertyType != typeof(string) || string.IsNullOrEmpty(PropertyValue))
    {
      return;
    }

    if (!regex.IsMatch(PropertyValue))
    {
      throw new LetsVerifyAssertStringError($"متن {Name} منطبق بر قالب درخواستی نمی‌باشد!");
    }
  });

  /// <summary>
  /// Check if value of a string property can be a `National-Code` value
  /// </summary>
  /// <param name="result">Current verification-result instance</param>
  /// <returns>An instance of `VerificationResult` that updated from current verification-result instance</returns>
  /// <exception cref="LetsVerifyAssertStringError"></exception>
  public static VerificationResult NationalCode(this VerificationResult result)
  => Handle(result, "کد ملی بودن", () =>
  {
    if (PropertyType != typeof(string) || string.IsNullOrEmpty(PropertyValue))
    {
      return;
    }

    if (!PersianStringHelper.VerifyNationalCodeCheckSum(PropertyValue))
    {
      throw new LetsVerifyAssertStringError($"مقدار {Name} یک کد ملی معتبر نمی‌باشد!");
    }
  });

  /// <summary>
  /// Check if a value of a string property can be a `Mobile-Number` value
  /// </summary>
  /// <param name="result">Current verification-result instance</param>
  /// <returns>An instance of `VerificationResult` that updated from current verification-result instance</returns>
  /// <exception cref="LetsVerifyAssertStringError"></exception>
  public static VerificationResult MobileNo(this VerificationResult result)
  => Handle(result, "شماره همراه بودن", () =>
  {
    if (PropertyType != typeof(string) || string.IsNullOrEmpty(PropertyValue))
    {
      return;
    }

    if (!Regex.IsMatch(PropertyValue, @"^09[0-9]{9}$"))
    {
      throw new LetsVerifyAssertStringError($"مقدار {Name}({PropertyValue}) یک شماره همراه معتبر نمی‌باشد!");
    }
  });

  /// <summary>
  /// Check if value of a string property can be a `Persion-Date` value like "yyyy/mm/dd hh:MM:ss"
  /// </summary>
  /// <param name="result">Current verification-result instance</param>
  /// <returns>An instance of `VerificationResult` that updated from current verification-result instance</returns>
  /// <exception cref="LetsVerifyAssertStringError"></exception>
  public static VerificationResult PersianDate(this VerificationResult result)
  => Handle(result, "تاریخ شمسی بودن", () =>
  {
    if (PropertyType != typeof(string) || string.IsNullOrEmpty(PropertyValue))
    {
      return;
    }

    if (!Regex.IsMatch(PropertyValue, @"^\d{4}[\\\/\- ]\d\d?[\\\/\- ]\d\d?([ ,\-]\d\d?[\\\.\:\,](\d\d)?([\\\.\:\,](\d\d)?([[\\\.\:\,](\d{1,6})])?)?)?$"))
    {
      throw new LetsVerifyAssertStringError($"مقدار {Name} یک تاریخ شمسی معتبر نمی‌باشد!");
    }
  });

  /// <summary>
  /// Check if value of this string property can be a `File-Name` value
  /// </summary>
  /// <param name="result">Current verification-result instance</param>
  /// <returns>An instance of `VerificationResult` that updated from current verification-result instance</returns>
  /// <exception cref="LetsVerifyAssertStringError"></exception>
  public static VerificationResult FileName(this VerificationResult result)
  => Handle(result, "مناسب نام فایل بودن", () =>
  {
    if (PropertyType != typeof(string) || string.IsNullOrEmpty(PropertyValue))
    {
      return;
    }

    if (!IOHelper.IsFileNameValid(PropertyValue))
    {
      throw new LetsVerifyAssertStringError($"مقدار {Name} نمی‌تواند نام فایل باشد!");
    }
  });

  /// <summary>
  /// Check if value of this string proprty is `Comma-Separated-Value` value and preparing a list of it rows for verifing
  /// </summary>
  /// <param name="rows">The prepared list of rows of a csv</param>
  /// <param name="result">Current verification-result instance</param>
  /// <returns>An instance of `VerificationResult` that updated from current verification-result instance</returns>
  /// <exception cref="LetsVerifyAssertStringError"></exception>
  public static VerificationResult Csv(this VerificationResult result, out string[]? rows)
  {
    rows = null;
    var isOk = false;

    var vresult = Handle(result, "تطبیق سی-اس-وی", () =>
    {
      if (PropertyType != typeof(string) || string.IsNullOrEmpty(PropertyValue))
      {
        return;
      }

      if (!Regex.IsMatch(PropertyValue, @"^\s*([^,]+)(,[^,\r\n]+)+\s*$"))
      {
        throw new LetsVerifyAssertStringError($"متن {Name} منطبق بر قالب سی-اس-وی نمی‌باشد!");
      }

      isOk = true;
    });

    if (isOk)
    {
      rows = PropertyValue?.replace(new Regex(@"\s*[\r\n]"), "\n").Split('\n');
    }

    return vresult;
  }

  /// <summary>
  /// Check if value of this string proprty is a single row of a `Comma-Separated-Value` value and preparing a list of it columns for verifing
  /// </summary>
  /// <param name="columns">The prepared list of columns of a csv row</param>
  /// <param name="result">Current verification-result instance</param>
  /// <returns>An instance of `VerificationResult` that updated from current verification-result instance</returns>
  /// <exception cref="LetsVerifyAssertStringError"></exception>
  public static VerificationResult CsvRow(this VerificationResult result, out string[]? columns)
  {
    columns = null;
    var isOk = false;

    var vresult = Handle(result, "تطبیق سطر سی-اس-وی", () =>
    {
      if (PropertyType != typeof(string) || string.IsNullOrEmpty(PropertyValue))
      {
        return;
      }

      if (!Regex.IsMatch(PropertyValue, @"^\s*([^,]+)(,[^,]+)+\s*$"))
      {
        throw new LetsVerifyAssertStringError($"متن {Name} منطبق بر قالب سطر سی-اس-وی نمی‌باشد!");
      }

      isOk = true;
    });

    if (isOk)
    {
      columns = PropertyValue?.replace(new Regex(@"\s"), "").Split(',');
    }

    return vresult;
  }
}