using HamferTeam.Kernel.Models.Errors;

namespace HamferTeam.LetsVerify.Models.Errors;

public class LetsVerifyError : KernelError
{
  public LetsVerifyError(string? message = null, Exception? innerException = null)
    : base(message, innerException)
  {
  }
}