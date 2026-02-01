using Hamfer.Kernel.Errors;

namespace Hamfer.Verification.Errors;

public class LetsVerifyError : KernelError
{
  public LetsVerifyError(string? message = null, Exception? innerError = null)
    : base(message, innerError)
  {
  }
}