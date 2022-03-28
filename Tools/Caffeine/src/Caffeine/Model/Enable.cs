using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Caffeine.Model
{
	public class EnableInterface : CaffeineInterface
	{
		[DllImport("user32.dll")]
		public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);
		const int VK_F15 = 0x7E;
		const uint KEYEVENTF_KEYUP = 0x0002;
		const uint KEYEVENTF_EXTENDEDKEY = 0x0001;

		public Task ExecuteAsync()
		{
			return Task.Run(() => {
				keybd_event((byte)VK_F15, 0, KEYEVENTF_EXTENDEDKEY | 0, 0);
			});
		}
	}
}