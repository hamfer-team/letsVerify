using HamferTeam.Kernel.Models.Errors;

namespace HamferTeam.LetsVerify.Models.Errors;

public class LetsVerifyAggregateError : LetsVerifyError, IAggregatedError<LetsVerifyError>
{
  public LetsVerifyAggregateError(string? message = null, params LetsVerifyError[] innerExceptions)
    : base(message)
  {
    InnerExceptions = innerExceptions;
  }

  public LetsVerifyError[] InnerExceptions { get; }

  public IEnumerable<Exception> GetInnerExceptions()
    => InnerExceptions;
}