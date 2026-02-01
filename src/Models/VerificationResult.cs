using Hamfer.Verification.Errors;

namespace Hamfer.Verification.Models;

public class VerificationResult
{
  public List<LetsVerifyError> errors { get; }

  public List<string> verificationLogs { get; }

  public VerificationResult()
  {
      errors = [];
      verificationLogs = [];
  }

  public bool hasError 
      => errors.Count > 0;

  public void addError(LetsVerifyError error)
      => errors.Add(error);

  public void addLog(string log)
      => verificationLogs.Add(log);
}
