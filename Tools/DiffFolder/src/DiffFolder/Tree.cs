using System;
using System.Collections.Generic;

namespace DiffFolder
{
	public class Tree
	{
		public TreeNode root;
		public Tree(string rootPath)
		{
			root = new TreeNode(rootPath);
		}

		public List<TreeNode> CompareTo(Tree compTree)
		{
			return root.CompareTo(compTree.root);
		}
	}
}