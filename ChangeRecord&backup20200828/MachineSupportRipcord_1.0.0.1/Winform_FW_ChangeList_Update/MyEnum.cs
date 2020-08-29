using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;

namespace CS_Note1
{
	public enum WndState
	{
		None, Ining, Outing, Docking, 
	} 
	public enum WndMouseState 
	{
		None, MouseOnX, MouseOnNail, LButtonDownX, LButtonDownNail,
		MouseOnLeftBtn, MouseOnRightBtn, LButtonDownLeftBtn,
		LButtonDownRightBtn, 
	} 
	public enum TreeImgIndex
	{
		OpendFolder = 1,
		ClosedFolder = 0 ,
		txt = 2 ,
	}
	public enum TreeNodeStyle 
	{
		File, Folder
	}

	[Flags]
	public enum TabsState
	{
		None      = 0x0000,
		OutLeft   = 0x0001,
		OutRight  = 0x0002,
		OutAll    = OutRight | OutLeft,
	}

    public struct RollBackFilePath
    {
        public string _CurrentUseRootPath;//null or .Cognex//Inspections
        public string _BackupRootPath;//null or .Cognex//ChangeReocrd
        public string _BranchName;//branch 名称
        public string _InspectionName;//T1_Acq or others
        public string _FileName;//Inspection_x or Inspection4_Branch1.0_Version3301
        public string _FileVersion;//Ver3.3.x.x
        public int _inspectionIndex;//Inspection index 
        public bool IsBackuped;
    }

    public struct SelectedNodeInformation
    {
        public TreeNodeStyle _nodeStyle;//所选的节点类型（file or Fold）
        public TreeView _docTree;
        public string _branchName;
        public string _vppName;//T1_Acq
        public int _inspIndex;//Inspection_1 index:1
        public bool _isAllowBackup;
        public string _opmlPath;//XML文件路径
        public string _rootXMLNode;//body or Rollback
        public string _lastVersion;//当前节点对应文件的最新版本信息
    }

} 
