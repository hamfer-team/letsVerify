using Hamfer.Kernel.Errors;

namespace Hamfer.Verification.Errors;

public class LetsVerifyError : KernelError
{
  public string? objectName { get; }

  public LetsVerifyError(string? objectName, string? message = null, Exception? innerError = null)
    : base(message, innerError)
  {
    this.objectName = objectName;
  }
}