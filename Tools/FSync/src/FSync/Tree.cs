using System;
using System.Collections.Generic;

namespace FSync
{
	public class Tree
	{
		public TreeNode root;
		public Tree(string rootPath)
		{
			root = new TreeNode(rootPath);
		}

		public List<SyncNode> CompareTo(Tree compTree)
		{
			return root.CompareTo(compTree.root);
		}
	}
}
