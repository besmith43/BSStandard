using System.Threading.Tasks;

namespace BSmith.Console.Net48.Commands
{
    public interface ICommand
    {
        Task<bool> ExecuteAsync();
    }
}