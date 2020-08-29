using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;

namespace CS_Note1
{
	/// <summary>
	/// 
	/// </summary>
	public class MdiTextInBox
	{
		// Primer ArrayList
		private ArrayList Tabs = new ArrayList (4) ;
		private int _CurTab =  -1 ;

		public event EventHandler FirstTimeOpenFileMsg ;
		public event EventHandler AllClosedMsg ;

		public MdiTextInBox()
		{
		}

		protected virtual void OnFirstTimeOpenFile (EventArgs e)
		{
			if (FirstTimeOpenFileMsg != null)
			{
				FirstTimeOpenFileMsg (this, e) ;
			}
		}

		protected virtual void OnAllClosed (EventArgs e)
		{
			if (AllClosedMsg != null)
			{
				AllClosedMsg (this, e) ;
			}
		}


		private void LoadFileToNewTab (string strFilePath)
		{
			this.Tabs.Add (new TextInBox ()) ;
			this._CurTab = Tabs.Count - 1 ;
			((TextInBox)Tabs[CurTab]).AddFile (strFilePath) ;
           
        }

        private void LoadVersionToNewTab(string version)
        {
            ((TextInBox)Tabs[CurTab]).AddVersion(version);//添加新的版本信息，需要考虑异常情况？？？
        }

        private void LoadVersionInfoToNewTab(Tuple<string, int, string, bool, bool> versionInfo)
        {
            ((TextInBox)Tabs[CurTab]).AddVersionInfo(versionInfo);//添加新的版本信息，需要考虑异常情况？？？
        }

        public void DeleteAllVersionInfo()
        {
            ((TextInBox)Tabs[CurTab]).DeleteAllVersionInfo();
        }

        public void DeleteLastRowVersionInfo()
        {
            ((TextInBox)Tabs[CurTab]).DeleteLastRowVersionInfo();
        }

        public void AddNewVersion(Tuple<string, int, string, bool, bool> versionInfo)
        {
            //判断version格式正确与否
            LoadVersionInfoToNewTab(versionInfo);
        }
        public void OpenFile (string path,TreeNode node)
		{
			if (IsEmpty())
			{
				EventArgs e = new EventArgs () ;
				OnFirstTimeOpenFile (e) ;
			}
			else 
			{
				// 遍历 Tabs 查看文件是否已经打开
				for (int i = 0 ; i < Tabs.Count ; i++)
				{
					// 如果已打开 设置当前 TAB 后 返回
					if ( ((TextInBox)Tabs[i]).IsContainFile (path) )
					{
						this._CurTab = i ;
						return ;
					}
				}
			}
			// 的确是未打开的文件 
			this.LoadFileToNewTab(path) ;
            this.LoadTreeNodeToNewTab(node);

            return ;
		}

        private void LoadTreeNodeToNewTab(TreeNode node)
        {
            ((TextInBox)Tabs[CurTab]).AddTreeNode(node);//如果为空时，需要考虑？？
        } 
 
		public string[] GetTabsTitle()
		{
			int len = this.Tabs.Count ;
			string[] tt = new string[len] ;
			for (int i = 0 ; i < len ; i ++)
			{
				tt[i] = ((TextInBox)Tabs[i]).Title ;
				if (((TextInBox)Tabs[i]).IsChanged)		tt[i] += "*" ;	
			}
			return tt ;
		}

		public bool IsEmpty()
		{	
			if (this.Tabs.Count > 0)
				return false ;
			return true ;
		}

		public void TextOutTo (ref string[] lines)
		{
			((TextInBox)this.Tabs[this.CurTab]).TextOutTo (ref lines) ;
		}

        public void HtmlOutTo(ref string htmlStr)
        {
            ((TextInBox)this.Tabs[this.CurTab]).TextOutTo(ref htmlStr);
        }

        public void ContentOutTo(ref string rtf)
        {
            ((TextInBox)this.Tabs[this.CurTab]).ContentOutTo(ref rtf);
        }

        public void VersionOutTo(ref List<Tuple<string, int, string, bool, bool>> rtf)
        {
            ((TextInBox)this.Tabs[this.CurTab]).VersionOutTo(ref rtf);
        }

        public void GetTreeNode(ref TreeNode node)
        {
            ((TextInBox)this.Tabs[this.CurTab]).GetTreeNode(ref node);
        }



        public void ContentTakeIn(string ContentInBox)
        {
            if (this._CurTab < 0)
                return;
            ((TextInBox)this.Tabs[this.CurTab]).ContentTakeIn(ContentInBox);//
        }

        public ArrayList GetCurFilesNotSave ()
		{
			if (CurTab < 0) 
				return null ;
			return 	((TextInBox)this.Tabs[this.CurTab]).GetFilesNotSave() ;
		}

		public ArrayList GetAllFileNoteSave ()
		{
			ArrayList alRtn = new ArrayList () ;
			ArrayList alFiles = null ;
			foreach (TextInBox tb in this.Tabs)
			{
				alFiles = tb.GetFilesNotSave ()  ;
				if (alFiles != null)
					alRtn.AddRange (alFiles) ;
			}
			return alRtn ;
		}

		public void CloseCurTab ()
		{
			if (CurTab < 0) 
				return ;
			this.Tabs.RemoveAt (CurTab) ;
			if (CurTab == Tabs.Count)
				CurTab -- ;
		}

		public void SaveCurTab ()
		{
			((TextInBox)this.Tabs[this.CurTab]).Save() ;
		}

		public void SaveAll ()
		{
			foreach (TextInBox tb in this.Tabs)
			{
				tb.Save () ;
			}
		}

		public bool SetCurTabChangedFlag () 
		{
			if (((TextInBox)this.Tabs[this.CurTab]).IsChanged == true)
				return false ;
			((TextInBox)this.Tabs[this.CurTab]).IsChanged = true  ;
			return true ;
		}
		
		public void CancelCurTabChangedFlag () 
		{
			((TextInBox)this.Tabs[this.CurTab]).IsChanged = false ; ;
		}

		public bool ReNameFile (string oldpath, string newpath) 
		{
			foreach (TextInBox tb in Tabs)
			{
				if (tb.ReNameFile (oldpath, newpath))
					return true ;
			}
			return false ;
		}

		public int CurTab
		{
			get 
			{
				return _CurTab ;
			}
			set
			{
				_CurTab = value ;
				if (_CurTab < 0)
				{
					EventArgs e = new EventArgs () ;
					OnAllClosed (e) ;
				}			
			}
		}

		public int CurTabLinesCnt
		{
			get 
			{
				return ((TextInBox)Tabs[CurTab]).LineCount ;
			}
		}

		public void ForFolderNameChanged (string oldpath, string newpath) 
		{
			foreach (TextInBox tb in Tabs)
			{
				tb.ForFolderNameChanged (oldpath, newpath) ;
			}
		}
		public void DeleteFile (string path)
		{
			for (int i = 0 ; i < Tabs.Count ; i ++)
			{
				((TextInBox)this.Tabs[i]).DeleteFile (path) ;
				if (((TextInBox)this.Tabs[i]).FileCnt == 0)
				{
					Tabs.RemoveAt (i) ;
					if (this.CurTab == Tabs.Count)
						CurTab -- ;
				}
			}
		}


	}
}
