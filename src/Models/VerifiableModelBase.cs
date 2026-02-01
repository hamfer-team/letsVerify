using Hamfer.Kernel.Errors;

namespace Hamfer.Verification.Models;

public abstract class VerifiableModelBase<TModel> : IVerifiable<TModel>
  where TModel : class
{
    public abstract void verify();

    public bool tryVerify(out KernelError? error)
    {
        error = null;
        try
        {
            this.verify();
            return true;
        }
        catch (KernelError err)
        {
            error = err;
            return false;
        }
    }
}