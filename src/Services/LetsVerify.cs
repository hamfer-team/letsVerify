using Hamfer.Verification.Models;

namespace Hamfer.Verification.Services;

public static class LetsVerify
{
  /// <summary>
  /// Let's Verify on a new object.
  /// </summary>
  /// <typeparam name="T">Type of verifing object</typeparam>
  /// <param name="result">A new verification-result instance</param>
  /// <param name="src">The vrifing object</param>
  /// <param name="name">The name of verifing object</param>
  /// <returns>An instance of `VerificationResult`</returns>
  public static VerificationResult On<T>(T src, string? name = null)
  {
    return new VerificationResult(name ?? typeof(T).Name);
  }

  public static VerificationResult On()
  {
    return new VerificationResult();
  }
}