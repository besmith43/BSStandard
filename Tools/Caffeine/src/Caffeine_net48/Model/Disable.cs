using System.Threading.Tasks;

namespace Caffeine.Model
{
	public class DisableInterface : CaffeineInterface
	{
		public Task ExecuteAsync()
		{
			return Task.Run(() => {

			});
		}
	}
}