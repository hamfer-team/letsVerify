using Hamfer.Kernel.Errors;

namespace Hamfer.Verification.Models;

public interface IVerifiable<TModel>
  where TModel : class
{
  void Verify();
  bool TryVerify(out KernelError? error);
}