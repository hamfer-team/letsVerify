namespace Hamfer.Verification.Errors;

public class LetsVerifyAssertStringError : LetsVerifyError
{
  private const string DEFAULT_MESSAGE = "متن فیلد معتبر نمی باشد!";

  public LetsVerifyAssertStringError(string? objectName, string? message = null) : base(objectName, message ?? DEFAULT_MESSAGE)
  {
  }
}