using HamferTeam.Kernel.Errors;

namespace HamferTeam.Verification.Models.Errors;

public class LetsVerifyError : KernelError
{
  public LetsVerifyError(string? message = null, Exception? innerError = null)
    : base(message, innerError)
  {
  }
}