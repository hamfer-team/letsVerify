namespace Hamfer.Verification.Errors;

public class LetsVerifyAssertNullError : LetsVerifyError
{
  private const string DEFAULT_MESSAGE_PATTERN = "مقدار خصوصیت «{0}» نباید تهی باشد.";

  public LetsVerifyAssertNullError(string? objectName, string? propertyName, string? message = null)
    : base(objectName, message ?? string.Format(DEFAULT_MESSAGE_PATTERN, propertyName))
  {
  }
}
