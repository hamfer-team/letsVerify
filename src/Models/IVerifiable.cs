using HamferTeam.Kernel.Models.Errors;

namespace HamferTeam.LetsVerify.Models;

public interface IVerifiable<TModel>
  where TModel : class
{
  void Verify();
  bool TryVerify(out KernelError? exception);
}