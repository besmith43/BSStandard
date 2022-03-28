using System.Threading.Tasks;

namespace BSmith.Console.Commands
{
    public interface ICommand
    {
        Task<bool> ExecuteAsync();
    }
}