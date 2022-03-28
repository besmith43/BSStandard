using System;

namespace DiffFolder
{
    public class DetermineSize
    {
		private const long OneKB = 1024;
    	private const long OneMB = OneKB * 1024;
    	private const long OneGB = OneMB * 1024;
    	private const long OneTB = OneGB * 1024;

		public static string ToPrettySize(long value, int decimalPlaces = 0)
    	{
    	    var asTB = Math.Round((double)value / OneTB, decimalPlaces);
    	    var asGB = Math.Round((double)value / OneGB, decimalPlaces);
    	    var asMB = Math.Round((double)value / OneMB, decimalPlaces);
    	    var asKB = Math.Round((double)value / OneKB, decimalPlaces);
    	    string chosenValue = asTB > 1 ? string.Format("{0}TB",asTB)
    	        : asGB > 1 ? string.Format("{0}GB",asGB)
    	        : asMB > 1 ? string.Format("{0}MB",asMB)
    	        : asKB > 1 ? string.Format("{0}KB",asKB)
    	        : string.Format("{0}B", Math.Round((double)value, decimalPlaces));
    	    return chosenValue;
    	}
	}
}