using HamferTeam.Verification.Models.Errors;

namespace HamferTeam.Verification.Models;

public class VerificationResult
{
  public List<LetsVerifyError> Errors { get; }

  public List<string> VerificationLogs { get; }

  public VerificationResult()
  {
      Errors = [];
      VerificationLogs = [];
  }

  public bool HasError
      => Errors.Count > 0;

  public void AddError(LetsVerifyError error)
      => Errors.Add(error);

  public void AddLog(string log)
      => VerificationLogs.Add(log);
}
