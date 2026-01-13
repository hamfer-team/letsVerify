using HamferTeam.Verification.Models;

namespace HamferTeam.Verification.Services;

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