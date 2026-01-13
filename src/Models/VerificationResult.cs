using HamferTeam.Verification.Models.Errors;

namespace HamferTeam.Verification.Models;

public class VerificationResult
{
  public List<LetsVerifyError> Exceptions { get; }

  public List<string> VerificationLogs { get; }

  public VerificationResult()
  {
      Exceptions = [];
      VerificationLogs = [];
  }

  public bool HasException
      => Exceptions.Count > 0;

  public void AddException(LetsVerifyError error)
      => Exceptions.Add(error);

  public void AddLog(string log)
      => VerificationLogs.Add(log);
}
