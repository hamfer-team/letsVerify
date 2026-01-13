namespace HamferTeam.Verification.Models.Errors;

public class LetsVerifyUnhandledError : LetsVerifyError
{
  private const string DefaultMessage = "در فرآیند بررسی یک خطای ناخواسته رخ داده است!";
  public LetsVerifyUnhandledError(Exception error) : base(DefaultMessage, error)
  {
  }
}