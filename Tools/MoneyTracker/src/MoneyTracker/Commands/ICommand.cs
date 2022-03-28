using System.Threading.Tasks;

namespace MoneyTracker.Commands
{
    public interface ICommand
    {
        Task<bool> ExecuteAsync();
    }
}