using Hamfer.Kernel.Errors;

namespace Hamfer.Verification.Errors;

public class LetsVerifyAggregateError : LetsVerifyError, IAggregatedError<LetsVerifyError>
{
  public LetsVerifyAggregateError(string? message = null, params LetsVerifyError[] innerErrors)
    : base(message)
  {
    this.innerErrors = innerErrors;
  }

  public LetsVerifyError[] innerErrors { get; }

  public IEnumerable<Exception> getInnerErrors()
    => innerErrors;
}