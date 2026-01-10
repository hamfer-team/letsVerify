namespace HamferTeam.LetsVerify.Models.Errors;

public class LetsVerifyAssertStringError : LetsVerifyError
{
  private const string DefaultMEssage = "متن فیلد معتبر نمی باشد!";

  public LetsVerifyAssertStringError(string? message = null) : base(message ?? DefaultMEssage)
  {
  }
}