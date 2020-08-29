using System ;
using System.Windows.Forms ;
namespace CS_Note1
{
	/// <summary>
	/// 
	/// </summary>
	public class FileTreeNode : TreeNode
	{
		private TreeNodeStyle  _NoteStyle ;
		private string         _FileOrFolderName ;

		public FileTreeNode()
		{
			this._NoteStyle = TreeNodeStyle.File ;
			this.Text = @"???" ;
		}

		public FileTreeNode (TreeNodeStyle notestylt, string txt)
		{
			this.NoteStyle = notestylt ;
			this.Text = txt ;
			if (notestylt == TreeNodeStyle.File)
			{
				this.ImageIndex = (int)TreeImgIndex.txt ;
				this.SelectedImageIndex = (int)TreeImgIndex.txt ;
			}
			else if (notestylt == TreeNodeStyle.Folder)
			{
				this.ImageIndex = (int)TreeImgIndex.ClosedFolder ;			
			}
		}

		public TreeNodeStyle  NoteStyle
		{
			get
			{
				return this._NoteStyle ;
			}
			set
			{
				_NoteStyle = value ;
			}
		}

		public string FileOrFolderName
		{
			get
			{
				return this._FileOrFolderName ;
			}
			set
			{
				_FileOrFolderName = value ;
			}
		}
	}
}
