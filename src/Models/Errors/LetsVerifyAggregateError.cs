using HamferTeam.Kernel.Errors;

namespace HamferTeam.Verification.Models.Errors;

public class LetsVerifyAggregateError : LetsVerifyError, IAggregatedError<LetsVerifyError>
{
  public LetsVerifyAggregateError(string? message = null, params LetsVerifyError[] innerErrors)
    : base(message)
  {
    InnerErrors = innerErrors;
  }

  public LetsVerifyError[] InnerErrors { get; }

  public IEnumerable<Exception> GetInnerErrors()
    => InnerErrors;
}