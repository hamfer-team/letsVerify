using Hamfer.Kernel.Errors;

namespace Hamfer.Verification.Errors;

public class LetsVerifyAggregateError : LetsVerifyError, IAggregatedError<LetsVerifyError>
{
  public LetsVerifyAggregateError(string? objectName, string? message = null, params LetsVerifyError[] innerErrors)
    : base(objectName, message)
  {
    this.innerErrors = innerErrors;
  }

  public LetsVerifyError[] innerErrors { get; }

  public IEnumerable<Exception> getInnerErrors()
    => innerErrors;

  public void writeMessages()
  {
    if (this.objectName != null)
    {
      Console.WriteLine($"💢 {this.objectName} has <{this.innerErrors.Length}> error(s):");
    }
    
    for (int i = 0; i < this.innerErrors.Length; i++)
    {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.Write($"{i + 1}. ");
      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine(this.innerErrors[i].Message);
    }
  }
}