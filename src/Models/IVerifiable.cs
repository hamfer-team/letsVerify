using HamferTeam.Kernel.Errors;

namespace HamferTeam.Verification.Models;

public interface IVerifiable<TModel>
  where TModel : class
{
  void Verify();
  bool TryVerify(out KernelError? exception);
}