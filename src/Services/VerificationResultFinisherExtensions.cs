using Hamfer.Verification.Models;

namespace Hamfer.Verification.Services;

public static partial class VerificationResultExtensions
{
  /// <summary>
  /// When checking of all verifications done, Then send result out to be used for other uses
  /// </summary>
  /// <param name="result">Current verification-result instance</param>
  /// <param name="outResult">Current verification-result instance</param>
  public static void ThenJustSendOutResult(this VerificationResult result, out VerificationResult outResult)
  {
    outResult = result;
  }

  /// <summary>
  /// When checking of all verifications done, Then Throw errors to be handled
  /// </summary>
  /// <param name="result">Current verification-result instance</param>
  public static void ThenThrowErrors(this VerificationResult result)
  {
    var errors = result.PrepareErrors();

    if (errors != null)
    {
      throw errors;
    }
  }
}