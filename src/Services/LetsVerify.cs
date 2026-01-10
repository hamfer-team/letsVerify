using HamferTeam.LetsVerify.Models;

namespace HamferTeam.LetsVerify.Services;

public static class LetsVerify
{
  public static VerificationResult On<TObject>(TObject @object)
    where TObject : class, IVerifiable<TObject>
  {
    return new VerificationResult();
  }

  public static VerificationResult OnNothing()
  {
    return new VerificationResult();
  }
}