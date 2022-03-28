using System;

namespace FSync
{
	public class SyncNode
	{
		public TreeNode Origin;
		public string Destination;

		public SyncNode(TreeNode _origin, string _destination)
		{
			Origin = _origin;
			Destination = _destination;
		}
	}
}
