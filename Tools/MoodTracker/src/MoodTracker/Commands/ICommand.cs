using System.Threading.Tasks;

namespace MoodTracker.Commands
{
    public interface ICommand
    {
        Task<bool> ExecuteAsync();
    }
}