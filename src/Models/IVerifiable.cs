using Hamfer.Kernel.Errors;

namespace Hamfer.Verification.Models;

public interface IVerifiable<TModel>
  where TModel : class
{
  void verify(string? name);
  bool tryVerify(out KernelError? error, string? name);
}