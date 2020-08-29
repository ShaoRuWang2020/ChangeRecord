using System ;
using System.IO ;
using System.Collections ;
using System.Windows.Forms ;
using System.Globalization ;
using System.Text ;

namespace CS_Note1
{
	/// <summary>
	/// 
	/// </summary>
	public class TextInFile
	{
		private bool _IsChanged = false ;
		private string _FileName = null ;
        private TreeNode _CurrentTreeNode = null;
		private ArrayList _Lines = new ArrayList (8) ;
        private string _Reader =null;
        private string _Rtf = null;

        public TextInFile()
		{
			_FileName = "" ;
		}
		public TextInFile (string path)
		{
			this._FileName = path ;
			LoadFile (_FileName) ;
		}
        public TextInFile(TreeNode node)
        {
            this._CurrentTreeNode = node;
        }
		private void LoadFile (string path)
		{
			if (File.Exists(path))
			{  StreamReader rtfSr = new StreamReader(path, Encoding.Default);
                string strNextLine = null ;
				try 
				{ 
                    _Rtf = rtfSr.ReadToEnd(); 
				}
				catch (Exception exc)
				{
					MessageBox.Show (exc.ToString(), "error - LoadFile()") ;
				}
				finally
				{
                    rtfSr.Close();
                }
			}
			else
			{
				FileStream fs = null ;
				try 
				{
					fs = File.Create (path) ;
				}
				catch (DirectoryNotFoundException dire)
				{
					MessageBox.Show (dire.Message + "\n文件可能在其他程序中被修改，强烈建议保存其他文件后重起JNote", 
						"CreateFile Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) ;
				}
				finally
				{
					if (fs != null) 
						fs.Close () ;
				}
			}
			
		}
     

        public void ContentTakeIn(string ContentInBox)//往具体的textedit中添加内容。
        {
            //this._Lines.Clear();
            RTF = string.Copy(ContentInBox);
            this._IsChanged = true;
        }

        public void Save() 
		{
			StreamWriter sw = null ;
			try 
			{
				sw = new StreamWriter (this._FileName, false, Encoding.Default) ;
				foreach (string line in this._Lines)
				{
					//sw.WriteLine (line) ;
				}
                //sw.WriteLine(_Reader);
                sw.WriteLine(RTF);
			}
			catch (DirectoryNotFoundException dire)
			{
				MessageBox.Show (dire.Message + "\n文件可能在其他程序中被修改，请重起JNote", 
					"SaveFile Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) ;
			}
			finally
			{
				if (sw != null) 
					sw.Close () ;
			}
			this._IsChanged = false ; 
		}

		public void ReName (string newpath) 
		{
			string sOld = _FileName ;
			try 
			{
				File.Move (_FileName, newpath) ;
				_FileName = newpath ;
			}
			catch (IOException ioe)
			{
				_FileName = sOld ;
				throw ioe ;
			}
		}

		public void ReFolderName (string oldpath, string newpath) 
		{
			if (this._FileName.StartsWith (oldpath))
			{
				this._FileName = newpath + _FileName.Substring(oldpath.Length) ; ;
				MessageBox .Show (_FileName) ;
			}
		}
		// Attribute
		// 完整的文件路径
		public string FileName
		{
			get
			{
				return _FileName ;
			}
		}

        public TreeNode CurrentTreeNode
        {
            get
            {
                return _CurrentTreeNode;
            }
        }


        public object[] Lines 
		{
			get 
			{
				return this._Lines.ToArray() ;
			}
		}

		public int LineCount
		{
			get
			{
				return this.Lines.Length ;
			}
		}
		
		public bool IsChanged
		{
			get
			{
				return this._IsChanged ;
			}
		}

        public string Reader
        {
            get
            {
                return this._Reader;
            }
            set
            {
                this._Reader = value;
            }
        }

        public string RTF
        {
            get
            {
                return this._Rtf;
            }
            set
            {
                this._Rtf = value;
            }
        }

    }

}
