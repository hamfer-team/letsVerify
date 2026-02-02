namespace Hamfer.Verification.Errors;

public class LetsVerifyUnhandledError : LetsVerifyError
{
  private const string DEFAULT_MESSAGE = "در فرآیند بررسی یک خطای ناخواسته رخ داده است!";
  public LetsVerifyUnhandledError(string? objectName, Exception error) : base(objectName, DEFAULT_MESSAGE, error)
  {
  }
}