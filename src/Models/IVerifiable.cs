using Hamfer.Kernel.Errors;

namespace Hamfer.Verification.Models;

public interface IVerifiable<TModel>
  where TModel : class
{
  void verify();
  bool tryVerify(out KernelError? error);
}