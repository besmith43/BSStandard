using System;
using System.IO;
using System.Collections.Generic;

namespace DiffFolder
{
	public class TreeNode : IEquatable<TreeNode>
	{
		public string Name;
		public string NodePath;
		public long Size;
		public TreeNodeType Type;
		public List<TreeNode> SubFileNodes;
		public List<TreeNode> SubDirectoryNodes;

		public TreeNode(string _path)
		{
			SubFileNodes = new();
			SubDirectoryNodes = new();

			if (File.Exists(_path))
			{
				FileInfo tempFile = new FileInfo(_path);
				Name = tempFile.Name;
				NodePath = Path.GetFullPath(_path);
				Size = tempFile.Length;
				Type = TreeNodeType.File;
			}
			else if (Directory.Exists(_path))
			{
				DirectoryInfo tempDir = new DirectoryInfo(_path);
				Name = tempDir.Name;
				NodePath = Path.GetFullPath(_path);
				Size = 0;
				Type = TreeNodeType.Directory;

				string[] subFolders = Directory.GetDirectories(_path);

				foreach (var subFolder in subFolders)
				{
					try
					{
						SubDirectoryNodes.Add(new TreeNode(subFolder));
					}
					catch
					{
						Console.Error.WriteLine($"Error Reading SubFolder: { subFolder }");
					}
				}

				string[] subFiles = Directory.GetFiles(_path);

				foreach (var subFile in subFiles)
				{
					try
					{
						SubFileNodes.Add(new TreeNode(subFile));
					}
					catch
					{
						Console.Error.WriteLine($"Error Reading SubFile: { subFile }");
					}
				}
			}
		}

		public bool Equals(TreeNode other)
		{
			if (other == null)
			{
				return false;
			}
			
			return (this.Name.Equals(other.Name) && this.Size.Equals(other.Size) && this.Type.Equals(other.Type));
		}

		public List<TreeNode> CompareTo(TreeNode compRoot)
		{
			List<TreeNode> missingList = new();

			foreach (var fileNode in SubFileNodes)
			{
				if (!compRoot.SubFileNodes.Contains(fileNode))
				{
					missingList.Add(fileNode);
				}
			}

			foreach (var dirNode in SubDirectoryNodes)
			{
				if (!compRoot.SubDirectoryNodes.Contains(dirNode))
				{
					missingList.Add(dirNode);
				}
				else
				{
					int Index = compRoot.SubDirectoryNodes.IndexOf(dirNode);
					List<TreeNode> returnedMissingList = dirNode.CompareTo(compRoot.SubDirectoryNodes[Index]);

					foreach (var returnedNode in returnedMissingList)
					{
						missingList.Add(returnedNode);
					}
				}
			}

			return missingList;
		}

		public override string ToString()
		{
			return $"{ NodePath } { DetermineSize.ToPrettySize(Size) }";
		}
	}
}