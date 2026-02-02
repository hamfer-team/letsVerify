using Hamfer.Verification.Models;

namespace Hamfer.Verification.Services;

public static class LetsVerify
{
  public static VerificationResult On<TObject>(TObject @object, string? objectName = null)
    where TObject : class, IVerifiable<TObject>
  {
    return new VerificationResult(objectName ?? typeof(TObject).Name);
  }

  public static VerificationResult On()
  {
    return new VerificationResult();
  }
}