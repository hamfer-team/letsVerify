namespace HamferTeam.Verification.Models.Errors;

public class LetsVerifyAssertNullError : LetsVerifyError
{
  private const string DefaultMEssagePattern = "مقدار خصوصیت «{0}» نباید تهی باشد.";

  public LetsVerifyAssertNullError(string? propertyName, string? message = null)
    : base(message ?? string.Format(DefaultMEssagePattern, propertyName))
  {
  }
}
