using System ;
using System.IO ;
using System.Collections ;
using System.Windows.Forms;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;


namespace CS_Note1
{
	/// <summary>
	/// 
	/// </summary>
	public class TextInBox
	{
		private ArrayList Files = null ;
		private bool _IsChanged = false ;
        private TreeNode _TreeNode = null;
        private ArrayList _VersionList = null;//存储各个节点的版本信息
        private List<Tuple<string, int, string, bool, bool>> _VersionInfoList = new List<Tuple<string, int, string, bool, bool>>();
        private List<Tuple<string, int, string, bool, bool>> _BackupVersionInfoList = new List<Tuple<string, int, string, bool, bool>>();
        private int MaxVersionNum = 10;
		public TextInBox()
		{
			Files= new ArrayList (4) ;
            _VersionList = new ArrayList(MaxVersionNum);
            _VersionInfoList = new List<Tuple<string, int, string, bool, bool>>(MaxVersionNum);

        }
		public void AddFile (string strPath)
		{
			TextInFile TextKuai = new TextInFile (strPath) ;;
			Files.Add (TextKuai) ;
		} 

        public void AddTreeNode(TreeNode treeNode)
        {
            TextInFile TextKuai = new TextInFile(treeNode);
            _TreeNode = treeNode;
        }

        public void AddVersion(string version)
        {
            _VersionList.Add(version);
        }

        public void AddVersionInfo(Tuple<string, int, string, bool, bool> versioninfo)
        {
            _VersionInfoList.Add(versioninfo);
        }

        public bool IsContainFile (string path)
		{
			foreach (TextInFile tf in Files)
			{
				if (tf.FileName == path)
					return true ;
			}
			return false ;
		}

		public void TextOutTo (ref string[] lines)
		{
			int i = 0 ; // For lines.

			foreach (TextInFile tf in this.Files)
			{
				foreach (string str in tf.Lines)
				{
					lines[i ++] = str ;
				} 
			}
		}

        public void TextOutTo(ref string htmlStr)
        {
            foreach (TextInFile tf in this.Files)
            {
                htmlStr = tf.Reader;
            }
        }
        public void ContentOutTo(ref string rtf)
        {
            foreach (TextInFile tf in this.Files)
            {
                rtf = tf.RTF;
            }
        }

        public void VersionOutTo(ref List<Tuple<string, int, string, bool, bool>>  rtf)
        {
            rtf = _VersionInfoList;
        }

        public void GetTreeNode(ref TreeNode node)
        {
            node = _TreeNode;
        }

        public ArrayList GetFilesNotSave() 
		{
			ArrayList arrRtn = new ArrayList () ;
			foreach (TextInFile tf in Files)
			{
				if (tf.IsChanged)
					arrRtn.Add (tf);
			}
			return arrRtn ;
		}

		public void Save ()
		{
			foreach (TextInFile tf in Files)
			{
				tf.Save() ;
			}
			this.IsChanged = false ;
		}
 
        public void ContentTakeIn(string LinesInBox)
        {
            if (!this.IsChanged)
                return;
            ((TextInFile)Files[0]).ContentTakeIn(LinesInBox);
        }
        public string Title
		{
			get
			{
				if (Files.Count == 1)
					return
						Path.GetFileName(((TextInFile)Files[0]).FileName) ;
				else if (Files.Count > 1)
					return
						Path.GetFileName(((TextInFile)Files[0]).FileName) +
						@"..." + Path.GetFileName(((TextInFile)Files[Files.Count - 1]).FileName) ;
				else
					return "" ;
			}
		}

		public int LineCount
		{
			get 
			{
				int nRtn = 0 ;
				foreach (TextInFile tf in this.Files)
				{
					nRtn += tf.LineCount ;
				}
				return nRtn ;
			}
		}

		public bool ReNameFile (string oldpath, string newpath) 
		{
			foreach (TextInFile tf in Files)
			{
				if (tf.FileName == oldpath)
				{
					tf.ReName (newpath) ;
					return true ;
				}
			}
			return false ;
		}

		public int FileCnt
		{
			get
			{
				return this.Files.Count ;
			}
		}

		public bool IsChanged
		{
			get
			{
				return this._IsChanged ;
			}
			set
			{
				this._IsChanged = value ;
			}
		}
		public void ForFolderNameChanged (string oldpath, string newpath) 
		{
			foreach (TextInFile tf in Files)
			{
				tf.ReFolderName (oldpath, newpath) ;
			}
		}
		public void DeleteFile (string path)
		{
			for (int i = 0 ; i < Files.Count ; i ++)
			{
				if (((TextInFile)this.Files[i]).FileName == path)
					this.Files.RemoveAt (i) ;
			}
		}

        public void DeleteLastRowVersionInfo()
        {
            if (_VersionInfoList.Count > 0)
                _VersionInfoList.RemoveAt(_VersionInfoList.Count - 1);
            else return;
        }

        public void DeleteAllVersionInfo()
        {
            int verNums = _VersionInfoList.Count;
            while(_VersionInfoList.Count >0)
            {
                _VersionInfoList.RemoveAt(0);
            }
        }

        public ArrayList VersionList
        {
            get
            {
                return this._VersionList;
            }
            set
            {
                this._VersionList = value;
            }
        }
        public List<Tuple<string,int,string,bool,bool>> VersionInfoList
        {
            get
            {
                return this._VersionInfoList;
            }
            set
            {
                this._VersionInfoList = value;
            }
        }
    }
}
