namespace Hamfer.Verification.Errors;

public class LetsVerifyAssertNotFoundError : LetsVerifyError
{
  private const string DEFAULT_MESSAGE = "مورد مد نظر یافت نشد!";
  public string itemValue { get; set; }

  public LetsVerifyAssertNotFoundError(string? objectName, string value, string? message = null) : base(objectName, message ?? DEFAULT_MESSAGE)
  {
    this.itemValue = value;
  }
}