using Hamfer.Kernel.Errors;
using Hamfer.Verification.Errors;

namespace Hamfer.Verification.Models;

public abstract class VerifiableModelBase<TModel> : IVerifiable<TModel>
  where TModel : class
{
    public abstract void verify(string? name = null);

    public bool tryVerify(out KernelError? error, string? name = null)
    {
        error = null;
        try
        {
            this.verify(name);
            return true;
        }
        catch (LetsVerifyAggregateError err)
        {
            error = err;
            return false;
        }
        catch (LetsVerifyError err)
        {
            error = err;
            return false;
        }
        catch (KernelError err)
        {
            error = err;
            return false;
        }
    }
}