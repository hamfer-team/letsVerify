using HamferTeam.Kernel.Models.Errors;

namespace HamferTeam.LetsVerify.Models;

public abstract class VerifiableModelBase<TModel> : IVerifiable<TModel>
  where TModel : class
{
    public abstract void Verify();

    public bool TryVerify(out KernelError? error)
    {
        error = null;
        try
        {
            this.Verify();
            return true;
        }
        catch (KernelError err)
        {
            error = err;
            return false;
        }
    }
}