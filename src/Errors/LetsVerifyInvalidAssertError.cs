namespace Hamfer.Verification.Errors;

public class LetsVerifyInvalidAssertError : LetsVerifyError
{
  public LetsVerifyInvalidAssertError(string? objectName, string message) : base(objectName, message)
  {
  }
}
