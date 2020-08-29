using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO ;
using System.Globalization ;
using System.Runtime.Serialization ;
using System.Runtime.Serialization.Formatters.Binary ;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Cognex.VS.Utility;

namespace CS_Note1
{
	/// <summary>
	/// FormJ 的摘要说明。
	/// </summary>

	
	public class FormJ : System.Windows.Forms.Form
	{
		#region field

		private System.Windows.Forms.Panel MdiTabOwner;
		private System.Windows.Forms.Panel MdiTabs;
		private System.Windows.Forms.TreeView DocTree;
		private System.Windows.Forms.Panel JChannel;
		private System.ComponentModel.IContainer components;
		private System.Timers.Timer TrundleTimer;
		
		private int NewTextDI = 0;
		// Pen
		private Pen FramePen ;
		private Pen BlackPen ;
		// Brush
		private SolidBrush TabBrush ;
		private SolidBrush CurTabBrush ;
		// Font
		private Font TabFont ;
		private Font CurTabFont ;

		// 用于计时器
		private EventHandler ehTimeTab_Tick_L ;
		private EventHandler ehTimeTab_Tick_R ; 
		private EventHandler ehTimeTab_Tick = null ;
        private EventHandler ehRTFEditor_ContentChanged;

        // picture 
        private Image ImgX ;
		private Image ImgGongJu ;
		private Image ImgNailFocusIn ;
		private Image ImgNailUnFocusIn ;
		private Image ImgNailFocusOut ;
		private Image ImgNailUnFocusOut ;
		// Mypath
		public string AppPath ;
		// Tab 是否更改 是否要重画 TextPane(richtextbox)
		// 在 MdiTabs_MouseDown 与 MdiTabs_MouseUp 中使用
		private bool TabChange = false ;
		//
		private System.Windows.Forms.Splitter JWndSpliter;

		// this.GenericPane state
		private WndState GenericPaneState = WndState.Outing ;
		private WndMouseState GenericPaneMouseState = WndMouseState.None ;

        private WndState SplitGenericPaneState = WndState.Outing;
        private WndMouseState SplitGenericPaneMouseState = WndMouseState.None;

		private WndMouseState MdiContainerMouseState = WndMouseState.None ;


		private System.Windows.Forms.ImageList TreeVImgList;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem DTMenuOpen;
		private System.Windows.Forms.MenuItem DTMenuOpenInCurWnd;
		private System.Windows.Forms.MenuItem DTMenuDel;
		private System.Windows.Forms.ContextMenu DocTreeMenu;
		private System.Windows.Forms.MenuItem DTMenuDao;
		private System.Windows.Forms.MenuItem DTMenuDaoFile;
		private System.Windows.Forms.MenuItem DTMenuDaoFolder;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.Timer TimeTab;
		private System.Windows.Forms.ContextMenu TabsMenu;
		private System.Windows.Forms.MainMenu JMenu;
		private System.Windows.Forms.MenuItem JMenuFile;
		private System.Windows.Forms.MenuItem JMenuNew;
		private System.Windows.Forms.MenuItem JMenuOpen;
		private System.Windows.Forms.MenuItem JMenuLine1;
		private System.Windows.Forms.MenuItem JMenuSave;
		private System.Windows.Forms.MenuItem JMenuLine2;
		private System.Windows.Forms.MenuItem JMenuExit;
        protected MenuItem TabsMenuSave;
        protected MenuItem TabsMenuClose;
        private System.Windows.Forms.ContextMenu TPaneMenu;
		private System.Windows.Forms.MenuItem TPaneMenuUnDo;
		private System.Windows.Forms.MenuItem TPaneMenuReDo;
		private System.Windows.Forms.MenuItem TPaneMenuLine1;
		private System.Windows.Forms.MenuItem TPaneMenuCut;
		private System.Windows.Forms.MenuItem TPaneMenuCopy;
		private System.Windows.Forms.MenuItem TPaneMenuLine2;
		private System.Windows.Forms.MenuItem TPaneMenuSelectA;
		private System.Windows.Forms.MenuItem TPaneMenuPaste;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem JMenuWrap;
		private System.Windows.Forms.MenuItem JMenuFont;
		private System.Windows.Forms.MenuItem JMenuSaveAll;
		private System.Windows.Forms.MenuItem DTMenuNew;
		private System.Windows.Forms.MenuItem DTMenuReName;
		private System.Windows.Forms.MenuItem DTMenuNewF;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem JMenuHelpAbout;
        private Panel MdiContainer;
        private MenuItem menuItem3;
        private MenuItem DTMenuUpdateName;

        // One and only DocObject as 
        public MdiTextInBox MdiDocManager ;

        //XML文件操作
        OpXMLFile opxml = new OpXMLFile();
        private ImageList imageListDrag;
        public string OpmlFile = Application.StartupPath + "\\Change Record.XML";
        public string ParameterFile = Application.StartupPath + "\\Configuration_General.ini";
        public string ChangeAndRollbackFold = Application.StartupPath + "\\Change Record";
        public string InspFold = Application.StartupPath + "\\Inspections";

        #endregion
        // Node being dragged
        private FileTreeNode dragNode = null;

        // Temporary drop node for selection
        private FileTreeNode tempDropNode = null;

        // Timer for scrolling
        private Timer _Timer = new Timer();

        public  List<string> branchNameList = null;
        private Panel RollbackContainer;
        private Button TBN_RollBackToLatestInsp;
        private GroupBox GroupBox_LatestFile;
        private CheckedListBox ChkListBox_LatestInspFiles;
        private GroupBox GroupBox_CondidateFiles;
        private CheckedListBox ChkListBox_CandidateInspFiles;
        private ComboBox ComBox_Inspections;
        private ComboBox ComBox_Branchs;
        private Button TBN_Rollback;
        private static object locker = new object();//定义对象
        public const string inspectionNodeName = "VPP_ChangeList";
        public const string RTFFoldName = "RTF";
        private MenuItem DTMenuVersion;
        public RollBackProcess rollback;

        public RollBackFilePath SelectFilePath = new RollBackFilePath(); 
        private RTFEditorUI.RTFEditor MsgBox;
        private SplitContainer SplitGenericPane;
        public RollBackFilePath TargetFilePath = new RollBackFilePath();
        public SelectedNodeInformation SelectedNodeInformation = new SelectedNodeInformation();
        public bool stopMonitorInspeCombox = false;
        private MenuItem TabsMenuVersion;
        private GroupBox groupBox1;
        private TextBox textBox1;
        private DataGridView LatestVersionDateView;
        public IFrameworkSupport m_framework;


        public FormJ(IFrameworkSupport framework)
		{
            //
            // Windows 窗体设计器支持所必需的
            m_framework = framework;
            InitializeComponent();
			FramePen = new Pen (Color.FromArgb (172,168,153), 1) ;
			BlackPen = new Pen (Color.Black, 1) ;
			TabFont = new Font (this.MdiTabs.Font.Name, (float)8.5) ;
			TabBrush = new SolidBrush (Color.FromArgb (129,126,115)) ;
			CurTabFont  = new Font (this.MdiTabs.Font.Name, (float)8.5, FontStyle.Bold) ; 
			CurTabBrush = new SolidBrush (Color.FromArgb (236,233,216)) ;


            this.AppPath = Application.StartupPath;// Path.GetDirectoryName (Application.StartupPath) ;
			// Load Picture 
			try 
			{
				ImgX = Image.FromFile (this.AppPath + @"\Res\X.bmp") ;
				ImgGongJu = Image.FromFile (this.AppPath+ @"\Res\GongJu.bmp") ;
				ImgNailFocusIn = Image.FromFile (this.AppPath + @"\Res\Nail1.bmp") ;
				ImgNailFocusOut = Image.FromFile (this.AppPath + @"\Res\Nail2.bmp") ;
				ImgNailUnFocusIn = Image.FromFile (this.AppPath + @"\Res\Nail3.bmp") ;
				ImgNailUnFocusOut = Image.FromFile (this.AppPath + @"\Res\Nail4.bmp") ;
			}
			catch (Exception e)
			{
				MessageBox.Show (e.Message + "\nPlease inspect the path--..\\Res.") ;
			}

			this.MdiDocManager = new MdiTextInBox () ;
			this.MdiDocManager.FirstTimeOpenFileMsg += new System.EventHandler(
				this.MdiDocManager_FirstTimeOpenFileMsg) ;
			this.MdiDocManager.AllClosedMsg += new System.EventHandler(
				this.MdiDocManager_AllClosedMsg) ;

            rollback = new RollBackProcess(this);

            //
            ehTimeTab_Tick_L = new EventHandler (this.TimeTab_Tick_L) ;
			ehTimeTab_Tick_R = new EventHandler (this.TimeTab_Tick_R) ;
			// 

		}

        public FormJ()
        {
            //
            // Windows 窗体设计器支持所必需的
            //
            InitializeComponent();
            FramePen = new Pen(Color.FromArgb(172, 168, 153), 1);
            BlackPen = new Pen(Color.Black, 1);
            TabFont = new Font(this.MdiTabs.Font.Name, (float)8.5);
            TabBrush = new SolidBrush(Color.FromArgb(129, 126, 115));
            CurTabFont = new Font(this.MdiTabs.Font.Name, (float)8.5, FontStyle.Bold);
            CurTabBrush = new SolidBrush(Color.FromArgb(236, 233, 216));


            this.AppPath = Application.StartupPath;// Path.GetDirectoryName (Application.StartupPath) ;
                                                   // Load Picture 
            try
            {
                ImgX = Image.FromFile(this.AppPath + @"\Res\X.bmp");
                ImgGongJu = Image.FromFile(this.AppPath + @"\Res\GongJu.bmp");
                ImgNailFocusIn = Image.FromFile(this.AppPath + @"\Res\Nail1.bmp");
                ImgNailFocusOut = Image.FromFile(this.AppPath + @"\Res\Nail2.bmp");
                ImgNailUnFocusIn = Image.FromFile(this.AppPath + @"\Res\Nail3.bmp");
                ImgNailUnFocusOut = Image.FromFile(this.AppPath + @"\Res\Nail4.bmp");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\nPlease inspect the path--..\\Res.");
            }

            this.MdiDocManager = new MdiTextInBox();
            this.MdiDocManager.FirstTimeOpenFileMsg += new System.EventHandler(
                this.MdiDocManager_FirstTimeOpenFileMsg);
            this.MdiDocManager.AllClosedMsg += new System.EventHandler(
                this.MdiDocManager_AllClosedMsg);

            rollback = new RollBackProcess(this);

            //
            ehTimeTab_Tick_L = new EventHandler(this.TimeTab_Tick_L);
            ehTimeTab_Tick_R = new EventHandler(this.TimeTab_Tick_R);
            // 

        }




        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormJ));
            this.MdiContainer = new System.Windows.Forms.Panel();
            this.MsgBox = new RTFEditorUI.RTFEditor();
            this.MdiTabOwner = new System.Windows.Forms.Panel();
            this.MdiTabs = new System.Windows.Forms.Panel();
            this.TabsMenu = new System.Windows.Forms.ContextMenu();
            this.TabsMenuVersion = new System.Windows.Forms.MenuItem();
            this.TabsMenuSave = new System.Windows.Forms.MenuItem();
            this.TabsMenuClose = new System.Windows.Forms.MenuItem();
            this.DocTree = new System.Windows.Forms.TreeView();
            this.DocTreeMenu = new System.Windows.Forms.ContextMenu();
            this.DTMenuNew = new System.Windows.Forms.MenuItem();
            this.DTMenuNewF = new System.Windows.Forms.MenuItem();
            this.DTMenuOpen = new System.Windows.Forms.MenuItem();
            this.DTMenuOpenInCurWnd = new System.Windows.Forms.MenuItem();
            this.DTMenuUpdateName = new System.Windows.Forms.MenuItem();
            this.DTMenuReName = new System.Windows.Forms.MenuItem();
            this.DTMenuVersion = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.DTMenuDao = new System.Windows.Forms.MenuItem();
            this.DTMenuDaoFile = new System.Windows.Forms.MenuItem();
            this.DTMenuDaoFolder = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.DTMenuDel = new System.Windows.Forms.MenuItem();
            this.TreeVImgList = new System.Windows.Forms.ImageList(this.components);
            this.TPaneMenu = new System.Windows.Forms.ContextMenu();
            this.TPaneMenuUnDo = new System.Windows.Forms.MenuItem();
            this.TPaneMenuReDo = new System.Windows.Forms.MenuItem();
            this.TPaneMenuLine1 = new System.Windows.Forms.MenuItem();
            this.TPaneMenuCut = new System.Windows.Forms.MenuItem();
            this.TPaneMenuCopy = new System.Windows.Forms.MenuItem();
            this.TPaneMenuPaste = new System.Windows.Forms.MenuItem();
            this.TPaneMenuLine2 = new System.Windows.Forms.MenuItem();
            this.TPaneMenuSelectA = new System.Windows.Forms.MenuItem();
            this.JMenu = new System.Windows.Forms.MainMenu(this.components);
            this.JMenuFile = new System.Windows.Forms.MenuItem();
            this.JMenuNew = new System.Windows.Forms.MenuItem();
            this.JMenuOpen = new System.Windows.Forms.MenuItem();
            this.JMenuLine1 = new System.Windows.Forms.MenuItem();
            this.JMenuSave = new System.Windows.Forms.MenuItem();
            this.JMenuSaveAll = new System.Windows.Forms.MenuItem();
            this.JMenuLine2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.JMenuExit = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.JMenuWrap = new System.Windows.Forms.MenuItem();
            this.JMenuFont = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.JMenuHelpAbout = new System.Windows.Forms.MenuItem();
            this.JChannel = new System.Windows.Forms.Panel();
            this.TrundleTimer = new System.Timers.Timer();
            this.JWndSpliter = new System.Windows.Forms.Splitter();
            this.TimeTab = new System.Windows.Forms.Timer(this.components);
            this.imageListDrag = new System.Windows.Forms.ImageList(this.components);
            this.RollbackContainer = new System.Windows.Forms.Panel();
            this.LatestVersionDateView = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.TBN_RollBackToLatestInsp = new System.Windows.Forms.Button();
            this.GroupBox_LatestFile = new System.Windows.Forms.GroupBox();
            this.ChkListBox_LatestInspFiles = new System.Windows.Forms.CheckedListBox();
            this.GroupBox_CondidateFiles = new System.Windows.Forms.GroupBox();
            this.TBN_Rollback = new System.Windows.Forms.Button();
            this.ChkListBox_CandidateInspFiles = new System.Windows.Forms.CheckedListBox();
            this.ComBox_Inspections = new System.Windows.Forms.ComboBox();
            this.ComBox_Branchs = new System.Windows.Forms.ComboBox();
            this.SplitGenericPane = new System.Windows.Forms.SplitContainer();
            this.MdiContainer.SuspendLayout();
            this.MdiTabOwner.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrundleTimer)).BeginInit();
            this.RollbackContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LatestVersionDateView)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.GroupBox_LatestFile.SuspendLayout();
            this.GroupBox_CondidateFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitGenericPane)).BeginInit();
            this.SplitGenericPane.Panel1.SuspendLayout();
            this.SplitGenericPane.Panel2.SuspendLayout();
            this.SplitGenericPane.SuspendLayout();
            this.SuspendLayout();
            // 
            // MdiContainer
            // 
            this.MdiContainer.BackColor = System.Drawing.SystemColors.Control;
            this.MdiContainer.Controls.Add(this.MsgBox);
            this.MdiContainer.Location = new System.Drawing.Point(668, 12);
            this.MdiContainer.Name = "MdiContainer";
            this.MdiContainer.Size = new System.Drawing.Size(1152, 765);
            this.MdiContainer.TabIndex = 5;
            this.MdiContainer.SizeChanged += new System.EventHandler(this.MdiContainer_SizeChanged);
            this.MdiContainer.TabIndexChanged += new System.EventHandler(this.MdiContainer_TabIndexChanged);
            this.MdiContainer.Paint += new System.Windows.Forms.PaintEventHandler(this.MdiContainer_Paint);
            this.MdiContainer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MdiContainer_MouseDown);
            this.MdiContainer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MdiContainer_MouseMove);
            this.MdiContainer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MdiTabs_MouseUp);
            // 
            // MsgBox
            // 
            this.MsgBox.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.MsgBox.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.MsgBox.Location = new System.Drawing.Point(-3, 30);
            this.MsgBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MsgBox.Name = "MsgBox";
            this.MsgBox.Size = new System.Drawing.Size(1024, 777);
            this.MsgBox.TabIndex = 8;
            // 
            // MdiTabOwner
            // 
            this.MdiTabOwner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(251)))), ((int)(((byte)(233)))));
            this.MdiTabOwner.Controls.Add(this.MdiTabs);
            this.MdiTabOwner.Location = new System.Drawing.Point(656, 2);
            this.MdiTabOwner.Name = "MdiTabOwner";
            this.MdiTabOwner.Size = new System.Drawing.Size(835, 35);
            this.MdiTabOwner.TabIndex = 3;
            this.MdiTabOwner.SizeChanged += new System.EventHandler(this.MdiTabOwner_SizeChanged);
            this.MdiTabOwner.Paint += new System.Windows.Forms.PaintEventHandler(this.MdiTabOwner_Paint);
            this.MdiTabOwner.MouseClick += new System.Windows.Forms.MouseEventHandler(this.JChannel_MouseDown);
            this.MdiTabOwner.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MdiContainer_MouseDown);
            // 
            // MdiTabs
            // 
            this.MdiTabs.ContextMenu = this.TabsMenu;
            this.MdiTabs.Font = new System.Drawing.Font("Courier New", 8F);
            this.MdiTabs.Location = new System.Drawing.Point(4, 1);
            this.MdiTabs.Name = "MdiTabs";
            this.MdiTabs.Size = new System.Drawing.Size(94, 106);
            this.MdiTabs.TabIndex = 0;
            this.MdiTabs.SizeChanged += new System.EventHandler(this.MdiTabs_SizeChanged);
            this.MdiTabs.Paint += new System.Windows.Forms.PaintEventHandler(this.MdiTabs_Paint);
            this.MdiTabs.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MdiTabs_MouseDown);
            this.MdiTabs.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MdiTabs_MouseUp);
            // 
            // TabsMenu
            // 
            this.TabsMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.TabsMenuVersion,
            this.TabsMenuSave,
            this.TabsMenuClose});
            this.TabsMenu.Popup += new System.EventHandler(this.TabsMenu_Popup);
            // 
            // TabsMenuVersion
            // 
            this.TabsMenuVersion.Index = 0;
            this.TabsMenuVersion.Text = "Add Version";
            this.TabsMenuVersion.Click += new System.EventHandler(this.TabsMenuVersion_Click);
            // 
            // TabsMenuSave
            // 
            this.TabsMenuSave.Index = 1;
            this.TabsMenuSave.Text = "Save(&S)";
            this.TabsMenuSave.Click += new System.EventHandler(this.TabsMenuSave_Click);
            // 
            // TabsMenuClose
            // 
            this.TabsMenuClose.Index = 2;
            this.TabsMenuClose.Text = "Close(&C)";
            this.TabsMenuClose.Click += new System.EventHandler(this.TabsMenuClose_Click);
            // 
            // DocTree
            // 
            this.DocTree.AllowDrop = true;
            this.DocTree.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.DocTree.ContextMenu = this.DocTreeMenu;
            this.DocTree.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DocTree.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.DocTree.ImageIndex = 0;
            this.DocTree.ImageList = this.TreeVImgList;
            this.DocTree.LabelEdit = true;
            this.DocTree.LineColor = System.Drawing.Color.Silver;
            this.DocTree.Location = new System.Drawing.Point(0, 20);
            this.DocTree.Name = "DocTree";
            this.DocTree.SelectedImageIndex = 1;
            this.DocTree.Size = new System.Drawing.Size(631, 359);
            this.DocTree.TabIndex = 2;
            this.DocTree.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.DocTree_AfterLabelEdit);
            this.DocTree.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.DocTree_AfterCollapse);
            this.DocTree.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.DocTree_AfterSelect);
            this.DocTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.DocTree_ItemDrag);
            this.DocTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.DocTree_AfterSelect);
            this.DocTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.DocTree_NodeMouseClick);
            this.DocTree.DragDrop += new System.Windows.Forms.DragEventHandler(this.DocTree_DragOver);
            this.DocTree.DragEnter += new System.Windows.Forms.DragEventHandler(this.DocTree_DragEnter);
            this.DocTree.DragOver += new System.Windows.Forms.DragEventHandler(this.DocTree_DragOver);
            this.DocTree.DragLeave += new System.EventHandler(this.DocTree_DragLeave);
            this.DocTree.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.DocTree_GiveFeedback);
            this.DocTree.DoubleClick += new System.EventHandler(this.DTMenuOpen_Click);
            this.DocTree.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DocTree_KeyDown);
            this.DocTree.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DocTree_MouseDoubleClick);
            this.DocTree.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DocTree_MouseUp);
            // 
            // DocTreeMenu
            // 
            this.DocTreeMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.DTMenuNew,
            this.DTMenuNewF,
            this.DTMenuOpen,
            this.DTMenuOpenInCurWnd,
            this.DTMenuUpdateName,
            this.DTMenuReName,
            this.DTMenuVersion,
            this.menuItem6,
            this.DTMenuDao,
            this.menuItem4,
            this.DTMenuDel});
            this.DocTreeMenu.Popup += new System.EventHandler(this.DocTreeMenu_Popup);
            // 
            // DTMenuNew
            // 
            this.DTMenuNew.Index = 0;
            this.DTMenuNew.Text = "New File(&N)";
            this.DTMenuNew.Click += new System.EventHandler(this.JMenuNew_Click);
            // 
            // DTMenuNewF
            // 
            this.DTMenuNewF.Index = 1;
            this.DTMenuNewF.Text = "New Fold(F)";
            this.DTMenuNewF.Click += new System.EventHandler(this.DTMenuNewF_Click);
            // 
            // DTMenuOpen
            // 
            this.DTMenuOpen.Index = 2;
            this.DTMenuOpen.Text = "Open(&O)";
            this.DTMenuOpen.Click += new System.EventHandler(this.DTMenuOpen_Click);
            // 
            // DTMenuOpenInCurWnd
            // 
            this.DTMenuOpenInCurWnd.Index = 3;
            this.DTMenuOpenInCurWnd.Text = "Current Open(&C)";
            // 
            // DTMenuUpdateName
            // 
            this.DTMenuUpdateName.Index = 4;
            this.DTMenuUpdateName.Text = "Update Name";
            this.DTMenuUpdateName.Click += new System.EventHandler(this.DTMenuUpdateName_Click);
            // 
            // DTMenuReName
            // 
            this.DTMenuReName.Index = 5;
            this.DTMenuReName.Text = "Rename(&M)";
            this.DTMenuReName.Click += new System.EventHandler(this.DTMenuReName_Click);
            // 
            // DTMenuVersion
            // 
            this.DTMenuVersion.Index = 6;
            this.DTMenuVersion.Text = "Version";
            this.DTMenuVersion.Click += new System.EventHandler(this.menuItem7_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 7;
            this.menuItem6.Text = "-";
            // 
            // DTMenuDao
            // 
            this.DTMenuDao.Index = 8;
            this.DTMenuDao.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.DTMenuDaoFile,
            this.DTMenuDaoFolder});
            this.DTMenuDao.Text = "Import(&I)";
            // 
            // DTMenuDaoFile
            // 
            this.DTMenuDaoFile.Index = 0;
            this.DTMenuDaoFile.Text = "文件(&F)..";
            this.DTMenuDaoFile.Click += new System.EventHandler(this.DTMenuDaoFile_Click);
            // 
            // DTMenuDaoFolder
            // 
            this.DTMenuDaoFolder.Index = 1;
            this.DTMenuDaoFolder.Text = "文件夹(&D)..";
            this.DTMenuDaoFolder.Click += new System.EventHandler(this.DTMenuDaoFolder_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 9;
            this.menuItem4.Text = "-";
            // 
            // DTMenuDel
            // 
            this.DTMenuDel.Index = 10;
            this.DTMenuDel.Text = "Delete(&D)";
            this.DTMenuDel.Click += new System.EventHandler(this.DTMenuDel_Click);
            // 
            // TreeVImgList
            // 
            this.TreeVImgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("TreeVImgList.ImageStream")));
            this.TreeVImgList.TransparentColor = System.Drawing.Color.Transparent;
            this.TreeVImgList.Images.SetKeyName(0, "");
            this.TreeVImgList.Images.SetKeyName(1, "");
            // 
            // TPaneMenu
            // 
            this.TPaneMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.TPaneMenuUnDo,
            this.TPaneMenuReDo,
            this.TPaneMenuLine1,
            this.TPaneMenuCut,
            this.TPaneMenuCopy,
            this.TPaneMenuPaste,
            this.TPaneMenuLine2,
            this.TPaneMenuSelectA});
            this.TPaneMenu.Popup += new System.EventHandler(this.TPaneMenu_Popup);
            // 
            // TPaneMenuUnDo
            // 
            this.TPaneMenuUnDo.Index = 0;
            this.TPaneMenuUnDo.Shortcut = System.Windows.Forms.Shortcut.CtrlU;
            this.TPaneMenuUnDo.Text = "撤消(&U)";
            this.TPaneMenuUnDo.Click += new System.EventHandler(this.TPaneMenuUnDo_Click);
            // 
            // TPaneMenuReDo
            // 
            this.TPaneMenuReDo.Index = 1;
            this.TPaneMenuReDo.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
            this.TPaneMenuReDo.Text = "重复(&R)";
            this.TPaneMenuReDo.Click += new System.EventHandler(this.TPaneMenuReDo_Click);
            // 
            // TPaneMenuLine1
            // 
            this.TPaneMenuLine1.Index = 2;
            this.TPaneMenuLine1.Text = "-";
            // 
            // TPaneMenuCut
            // 
            this.TPaneMenuCut.Index = 3;
            this.TPaneMenuCut.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
            this.TPaneMenuCut.Text = "剪切(&T)";
            this.TPaneMenuCut.Click += new System.EventHandler(this.TPaneMenuCut_Click);
            // 
            // TPaneMenuCopy
            // 
            this.TPaneMenuCopy.Index = 4;
            this.TPaneMenuCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.TPaneMenuCopy.Text = "复制(&C)";
            this.TPaneMenuCopy.Click += new System.EventHandler(this.TPaneMenuCopy_Click);
            // 
            // TPaneMenuPaste
            // 
            this.TPaneMenuPaste.Index = 5;
            this.TPaneMenuPaste.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
            this.TPaneMenuPaste.Text = "粘贴(&P)";
            this.TPaneMenuPaste.Click += new System.EventHandler(this.TPaneMenuPaste_Click);
            // 
            // TPaneMenuLine2
            // 
            this.TPaneMenuLine2.Index = 6;
            this.TPaneMenuLine2.Text = "-";
            // 
            // TPaneMenuSelectA
            // 
            this.TPaneMenuSelectA.Index = 7;
            this.TPaneMenuSelectA.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
            this.TPaneMenuSelectA.Text = "全选(&S)";
            this.TPaneMenuSelectA.Click += new System.EventHandler(this.TPaneMenuSelectA_Click);
            // 
            // JMenu
            // 
            this.JMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.JMenuFile,
            this.menuItem1,
            this.menuItem2});
            // 
            // JMenuFile
            // 
            this.JMenuFile.Index = 0;
            this.JMenuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.JMenuNew,
            this.JMenuOpen,
            this.JMenuLine1,
            this.JMenuSave,
            this.JMenuSaveAll,
            this.JMenuLine2,
            this.menuItem3,
            this.JMenuExit});
            this.JMenuFile.Text = "File(&F)";
            // 
            // JMenuNew
            // 
            this.JMenuNew.Index = 0;
            this.JMenuNew.Text = "New(&N)";
            this.JMenuNew.Click += new System.EventHandler(this.JMenuNew_Click);
            // 
            // JMenuOpen
            // 
            this.JMenuOpen.Index = 1;
            this.JMenuOpen.Text = "Open File(&O)...";
            this.JMenuOpen.Click += new System.EventHandler(this.JMenuOpen_Click);
            // 
            // JMenuLine1
            // 
            this.JMenuLine1.Index = 2;
            this.JMenuLine1.Text = "-";
            // 
            // JMenuSave
            // 
            this.JMenuSave.Index = 3;
            this.JMenuSave.Text = "Save(&S)";
            this.JMenuSave.Click += new System.EventHandler(this.TabsMenuSave_Click);
            // 
            // JMenuSaveAll
            // 
            this.JMenuSaveAll.Index = 4;
            this.JMenuSaveAll.Text = "Save All(&L)";
            this.JMenuSaveAll.Click += new System.EventHandler(this.JMenuSaveAll_Click);
            // 
            // JMenuLine2
            // 
            this.JMenuLine2.Index = 5;
            this.JMenuLine2.Text = "-";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 6;
            this.menuItem3.Text = "Add New Branch";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // JMenuExit
            // 
            this.JMenuExit.Index = 7;
            this.JMenuExit.Text = "Exit(&X)";
            this.JMenuExit.Click += new System.EventHandler(this.JMenuExit_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 1;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.JMenuWrap,
            this.JMenuFont});
            this.menuItem1.Text = "Tool(&V)";
            // 
            // JMenuWrap
            // 
            this.JMenuWrap.Index = 0;
            this.JMenuWrap.Text = "WordWrap(&W)";
            this.JMenuWrap.Click += new System.EventHandler(this.JMenuWrap_Click);
            // 
            // JMenuFont
            // 
            this.JMenuFont.Index = 1;
            this.JMenuFont.Text = "Font(&F)...";
            this.JMenuFont.Click += new System.EventHandler(this.JMenuFont_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 2;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.JMenuHelpAbout});
            this.menuItem2.Text = "Help(&H)";
            // 
            // JMenuHelpAbout
            // 
            this.JMenuHelpAbout.Index = 0;
            this.JMenuHelpAbout.Text = "Abort &J_Note...";
            this.JMenuHelpAbout.Click += new System.EventHandler(this.JMenuHelpAbout_Click);
            // 
            // JChannel
            // 
            this.JChannel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(251)))), ((int)(((byte)(233)))));
            this.JChannel.Dock = System.Windows.Forms.DockStyle.Left;
            this.JChannel.Location = new System.Drawing.Point(0, 0);
            this.JChannel.Name = "JChannel";
            this.JChannel.Size = new System.Drawing.Size(20, 735);
            this.JChannel.TabIndex = 4;
            this.JChannel.Paint += new System.Windows.Forms.PaintEventHandler(this.JChannel_Paint);
            this.JChannel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JChannel_MouseDown);
            // 
            // TrundleTimer
            // 
            this.TrundleTimer.Interval = 30D;
            this.TrundleTimer.SynchronizingObject = this;
            this.TrundleTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.TrundleTimer_Elapsed);
            // 
            // JWndSpliter
            // 
            this.JWndSpliter.BackColor = System.Drawing.SystemColors.Control;
            this.JWndSpliter.Location = new System.Drawing.Point(169, 0);
            this.JWndSpliter.Name = "JWndSpliter";
            this.JWndSpliter.Size = new System.Drawing.Size(2, 417);
            this.JWndSpliter.TabIndex = 8;
            this.JWndSpliter.TabStop = false;
            // 
            // TimeTab
            // 
            this.TimeTab.Interval = 30;
            // 
            // imageListDrag
            // 
            this.imageListDrag.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListDrag.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListDrag.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // RollbackContainer
            // 
            this.RollbackContainer.BackColor = System.Drawing.SystemColors.ControlDark;
            this.RollbackContainer.Controls.Add(this.LatestVersionDateView);
            this.RollbackContainer.Controls.Add(this.groupBox1);
            this.RollbackContainer.Controls.Add(this.TBN_RollBackToLatestInsp);
            this.RollbackContainer.Controls.Add(this.GroupBox_LatestFile);
            this.RollbackContainer.Controls.Add(this.GroupBox_CondidateFiles);
            this.RollbackContainer.Controls.Add(this.ComBox_Inspections);
            this.RollbackContainer.Controls.Add(this.ComBox_Branchs);
            this.RollbackContainer.Location = new System.Drawing.Point(3, 3);
            this.RollbackContainer.Name = "RollbackContainer";
            this.RollbackContainer.Size = new System.Drawing.Size(628, 420);
            this.RollbackContainer.TabIndex = 7;
            // 
            // LatestVersionDateView
            // 
            this.LatestVersionDateView.AllowUserToAddRows = false;
            this.LatestVersionDateView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LatestVersionDateView.Location = new System.Drawing.Point(9, 122);
            this.LatestVersionDateView.Name = "LatestVersionDateView";
            this.LatestVersionDateView.RowTemplate.Height = 24;
            this.LatestVersionDateView.Size = new System.Drawing.Size(106, 69);
            this.LatestVersionDateView.TabIndex = 17;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new System.Drawing.Point(3, 197);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(594, 112);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Record";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 21);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(577, 85);
            this.textBox1.TabIndex = 0;
            // 
            // TBN_RollBackToLatestInsp
            // 
            this.TBN_RollBackToLatestInsp.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TBN_RollBackToLatestInsp.Location = new System.Drawing.Point(306, 79);
            this.TBN_RollBackToLatestInsp.Name = "TBN_RollBackToLatestInsp";
            this.TBN_RollBackToLatestInsp.Size = new System.Drawing.Size(119, 37);
            this.TBN_RollBackToLatestInsp.TabIndex = 13;
            this.TBN_RollBackToLatestInsp.Text = "Copy To";
            this.TBN_RollBackToLatestInsp.UseVisualStyleBackColor = true;
            this.TBN_RollBackToLatestInsp.Click += new System.EventHandler(this.TBN_RollBackToLatestInsp_Click);
            // 
            // GroupBox_LatestFile
            // 
            this.GroupBox_LatestFile.Controls.Add(this.ChkListBox_LatestInspFiles);
            this.GroupBox_LatestFile.Location = new System.Drawing.Point(431, 17);
            this.GroupBox_LatestFile.Name = "GroupBox_LatestFile";
            this.GroupBox_LatestFile.Size = new System.Drawing.Size(166, 174);
            this.GroupBox_LatestFile.TabIndex = 12;
            this.GroupBox_LatestFile.TabStop = false;
            this.GroupBox_LatestFile.Text = "CurrentBranch";
            // 
            // ChkListBox_LatestInspFiles
            // 
            this.ChkListBox_LatestInspFiles.FormattingEnabled = true;
            this.ChkListBox_LatestInspFiles.Location = new System.Drawing.Point(6, 21);
            this.ChkListBox_LatestInspFiles.Name = "ChkListBox_LatestInspFiles";
            this.ChkListBox_LatestInspFiles.Size = new System.Drawing.Size(149, 140);
            this.ChkListBox_LatestInspFiles.TabIndex = 10;
            this.ChkListBox_LatestInspFiles.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ChkListBox_LatestInspFiles_ItemCheck);
            this.ChkListBox_LatestInspFiles.SelectedIndexChanged += new System.EventHandler(this.ChkListBox_LatestInspFiles_SelectedIndexChanged);
            // 
            // GroupBox_CondidateFiles
            // 
            this.GroupBox_CondidateFiles.Controls.Add(this.TBN_Rollback);
            this.GroupBox_CondidateFiles.Controls.Add(this.ChkListBox_CandidateInspFiles);
            this.GroupBox_CondidateFiles.Location = new System.Drawing.Point(139, 17);
            this.GroupBox_CondidateFiles.Name = "GroupBox_CondidateFiles";
            this.GroupBox_CondidateFiles.Size = new System.Drawing.Size(161, 174);
            this.GroupBox_CondidateFiles.TabIndex = 11;
            this.GroupBox_CondidateFiles.TabStop = false;
            this.GroupBox_CondidateFiles.Text = "groupBox1";
            // 
            // TBN_Rollback
            // 
            this.TBN_Rollback.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TBN_Rollback.Location = new System.Drawing.Point(27, 128);
            this.TBN_Rollback.Name = "TBN_Rollback";
            this.TBN_Rollback.Size = new System.Drawing.Size(116, 40);
            this.TBN_Rollback.TabIndex = 14;
            this.TBN_Rollback.Text = "RollBack";
            this.TBN_Rollback.UseVisualStyleBackColor = true;
            this.TBN_Rollback.Click += new System.EventHandler(this.TBN_Rollback_Click);
            // 
            // ChkListBox_CandidateInspFiles
            // 
            this.ChkListBox_CandidateInspFiles.FormattingEnabled = true;
            this.ChkListBox_CandidateInspFiles.Location = new System.Drawing.Point(6, 21);
            this.ChkListBox_CandidateInspFiles.Name = "ChkListBox_CandidateInspFiles";
            this.ChkListBox_CandidateInspFiles.Size = new System.Drawing.Size(149, 106);
            this.ChkListBox_CandidateInspFiles.TabIndex = 10;
            this.ChkListBox_CandidateInspFiles.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ChkListBox_CandidateInspFiles_ItemCheck);
            this.ChkListBox_CandidateInspFiles.SelectedIndexChanged += new System.EventHandler(this.ChkListBox_CandidateInspFiles_SelectedIndexChanged);
            // 
            // ComBox_Inspections
            // 
            this.ComBox_Inspections.FormattingEnabled = true;
            this.ComBox_Inspections.Location = new System.Drawing.Point(3, 92);
            this.ComBox_Inspections.Name = "ComBox_Inspections";
            this.ComBox_Inspections.Size = new System.Drawing.Size(121, 24);
            this.ComBox_Inspections.TabIndex = 9;
            this.ComBox_Inspections.SelectedIndexChanged += new System.EventHandler(this.ComBox_Inspections_SelectedIndexChanged);
            this.ComBox_Inspections.Click += new System.EventHandler(this.ComBox_Inspections_Click);
            // 
            // ComBox_Branchs
            // 
            this.ComBox_Branchs.FormattingEnabled = true;
            this.ComBox_Branchs.Location = new System.Drawing.Point(3, 17);
            this.ComBox_Branchs.Name = "ComBox_Branchs";
            this.ComBox_Branchs.Size = new System.Drawing.Size(121, 24);
            this.ComBox_Branchs.TabIndex = 8;
            this.ComBox_Branchs.SelectedIndexChanged += new System.EventHandler(this.ComBox_Branchs_SelectedIndexChanged);
            // 
            // SplitGenericPane
            // 
            this.SplitGenericPane.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.SplitGenericPane.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SplitGenericPane.Location = new System.Drawing.Point(26, 5);
            this.SplitGenericPane.Name = "SplitGenericPane";
            this.SplitGenericPane.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitGenericPane.Panel1
            // 
            this.SplitGenericPane.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.SplitGenericPane.Panel1.Controls.Add(this.DocTree);
            // 
            // SplitGenericPane.Panel2
            // 
            this.SplitGenericPane.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.SplitGenericPane.Panel2.Controls.Add(this.RollbackContainer);
            this.SplitGenericPane.Size = new System.Drawing.Size(636, 730);
            this.SplitGenericPane.SplitterDistance = 384;
            this.SplitGenericPane.SplitterWidth = 6;
            this.SplitGenericPane.TabIndex = 8;
            this.SplitGenericPane.SizeChanged += new System.EventHandler(this.SplitGenericPane_SizeChanged);
            this.SplitGenericPane.Paint += new System.Windows.Forms.PaintEventHandler(this.SplitGenericPane_Paint);
            this.SplitGenericPane.Enter += new System.EventHandler(this.SplitGenericPane_Enter);
            this.SplitGenericPane.Leave += new System.EventHandler(this.SplitGenericPane_Leave);
            this.SplitGenericPane.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SplitGenericPane_MouseDown);
            this.SplitGenericPane.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SplitGenericPane_MouseMove);
            this.SplitGenericPane.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SplitGenericPane_MouseUp);
            // 
            // FormJ
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(1734, 735);
            this.Controls.Add(this.SplitGenericPane);
            this.Controls.Add(this.MdiTabOwner);
            this.Controls.Add(this.JChannel);
            this.Controls.Add(this.MdiContainer);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Menu = this.JMenu;
            this.Name = "FormJ";
            this.Text = "ChangeList Ver1.0";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FormJ_Closing);
            this.Load += new System.EventHandler(this.FormJ_Load);
            this.SizeChanged += new System.EventHandler(this.FormJ_SizeChanged);
            this.MdiContainer.ResumeLayout(false);
            this.MdiTabOwner.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TrundleTimer)).EndInit();
            this.RollbackContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LatestVersionDateView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.GroupBox_LatestFile.ResumeLayout(false);
            this.GroupBox_CondidateFiles.ResumeLayout(false);
            this.SplitGenericPane.Panel1.ResumeLayout(false);
            this.SplitGenericPane.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitGenericPane)).EndInit();
            this.SplitGenericPane.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new FormJ());
        }

		private void FormJ_Load(object sender, System.EventArgs e)
		{
			//this.GenericPane.Location = new Point (20 - GenericPane.Size.Width, 0) ;
            this.SplitGenericPane.Location = new Point(20 - SplitGenericPane.Size.Width, 0);

			this.MdiContainer.Location = new Point (20, 0) ;
			//this.DocTree.Size = new Size (GenericPane.Size.Width - 4, GenericPane.Size.Height -25) ;
            this.DocTree.Size = new Size(SplitGenericPane.Size.Width - 4, SplitGenericPane.Panel1.Height - 25);

			this.TreeViewLoadNote ();
			// set Controls's state
			//this.GenericPaneState = WndState.Docking ;
            this.SplitGenericPaneState = WndState.Docking;

			this.JChannel.Visible = false ;
			//this.GenericPane.Dock = DockStyle.Left ;
            this.SplitGenericPane.Dock = DockStyle.Left;

			this.MdiContainer.Dock = DockStyle.Fill ; 
            this.Controls.Remove(this.SplitGenericPane);
            this.Controls.Add(this.JWndSpliter);
            this.Controls.Add(this.SplitGenericPane);
            this.DocTree.Size = new Size(SplitGenericPane.Size.Width - 2, SplitGenericPane.Panel1.Height - 20);
             
			this.OpenFileFromDocTree () ; 
            ehRTFEditor_ContentChanged = new EventHandler(this.richTextBoxExtended1_RichTextBox_TextChanged); 
            this.MsgBox.TabIndexChanged += this.ehRTFEditor_ContentChanged;

            this.MsgBox.RTFpath = Application.StartupPath+@"\\2.rtf"; 
            
            this._Timer.Interval = 200;
            this._Timer.Tick += new EventHandler(timer_Tick);
        }

		private void FormJ_SizeChanged(object sender, System.EventArgs e)
		{
            if (this.SplitGenericPaneState == WndState.Docking) return;
            this.MdiContainer.Size = new Size(this.ClientSize.Width - 20, this.ClientSize.Height);
            this.SplitGenericPane.Size = new Size(SplitGenericPane.Size.Width, this.ClientSize.Height);
		}

		private void FormJ_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (MdiDocManager.IsEmpty ())
				return ;
            MdiDocManager.ContentTakeIn(this.MsgBox.richTextBox.Rtf);//获取当前editor中的内容保存到当前tab的textinbox中。
			ArrayList Files = MdiDocManager.GetAllFileNoteSave () ;
			// 这里不完善
			if (Files == null) 
				return ;
			if (Files.Count != 0)
			{
				FormSave formS = new FormSave () ;
				int i = 0 ;
				foreach (TextInFile tf in Files)
				{
					formS.LBFlieName.Items.Add (Path.GetFileName (tf.FileName)) ;
					formS.LBFlieName.SelectedIndex = i ++ ;
				}
				switch (formS.ShowDialog (this))
				{
					case DialogResult.Yes : 
						for (i = 0 ; i < formS.LBFlieName.SelectedIndices.Count  ; i ++)
						{
							((TextInFile)Files[formS.LBFlieName.SelectedIndices[i]]).Save () ;
						}
						break ;
					case DialogResult.No :
						break ;
					case DialogResult.Cancel :
						e.Cancel = true ;
						break ;
				}
				formS.Dispose () ;
			}
			if (!e.Cancel)
			{
				// Serialize  TextEditPane
				this.SerializeTexePaneAtt () ;
			}
		}

        public void CurTab_Closing()
        {
            if (MdiDocManager.IsEmpty())
                return;
            MdiDocManager.ContentTakeIn(this.MsgBox.richTextBox.Rtf);//获取当前editor中的内容保存到当前tab的textinbox中。
            ArrayList Files = MdiDocManager.GetAllFileNoteSave(); 
            if (Files == null||Files.Count==0)
            {
                while (!MdiDocManager.IsEmpty())
                {
                    this.MdiDocManager.CloseCurTab();
                }
                this.TextPaneShowNewText();
                this.MdiTabs.Invalidate();
                return;
            }
                
            if (Files.Count != 0)
            {
                for (int i = 0; i < Files.Count; i++)
                {
                    ((TextInFile)Files[i]).Save();
                    this.MdiDocManager.CloseCurTab(); 
                }
                while (!MdiDocManager.IsEmpty())
                {
                    this.MdiDocManager.CloseCurTab();
                }
                this.TextPaneShowNewText();
                this.MdiTabs.Invalidate();
            }
        }

		private void JChannel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Graphics dc = e.Graphics ; 
			dc.DrawImageUnscaled (this.ImgGongJu, 0,0) ;
		}
  
		private void JChannel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			// 如果按下鼠标在工具箱图片上 
			// 并且 工具箱窗口正隐藏
			// 则 计时器开始 滚动窗口
			//if (e.Y < 80 && this.GenericPane.Location.X <= GenericPane.Size.Width - 20
			//	&& this.GenericPaneState != WndState.Docking)
			//{
			//	this.GenericPaneState = WndState.Outing ;
			//	this.TrundleTimer.Enabled = true ;
			//}

            if (e.Y < 80 && this.SplitGenericPane.Location.X <= SplitGenericPane.Size.Width - 20
                && this.SplitGenericPaneState != WndState.Docking)
            {
                this.SplitGenericPaneState = WndState.Outing;
                this.TrundleTimer.Enabled = true;
            }
        }
        
        private void SplitGenericPane_SizeChanged(object sender, EventArgs e)
        {
            if (this.SplitGenericPaneState == WndState.Docking)
                this.DocTree.Size = new Size(SplitGenericPane.Size.Width - 2, SplitGenericPane.Size.Height - 25);
            else
                this.DocTree.Size = new Size(SplitGenericPane.Size.Width - 4, SplitGenericPane.Size.Height - 25);
            this.SplitGenericPane.Invalidate();
        }

        private void SplitGenericPane_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.SplitGenericPaneMouseState == WndMouseState.LButtonDownNail)
            {
                if (this.SplitGenericPaneState != WndState.Docking)
                {
                    // set Controls's state
                    this.SplitGenericPaneState = WndState.Docking;
                    this.JChannel.Visible = false;
                    this.SplitGenericPane.Dock = DockStyle.Left;
                    this.MdiContainer.Dock = DockStyle.Fill;

                    // Set controls's order in the set.
                    this.Controls.Remove(this.SplitGenericPane);
                    this.Controls.Add(this.JWndSpliter);
                    this.Controls.Add(this.SplitGenericPane);
                    this.DocTree.Size = new Size(SplitGenericPane.Size.Width - 2, SplitGenericPane.Panel1.Size.Height - 25);
                    //
                    this.SplitGenericPane.Invalidate();
                }
                else // Docking
                {
                    // Resume controls's state
                    this.SplitGenericPaneState = WndState.Ining;
                    this.MdiContainer.Dock = DockStyle.None;
                    this.SplitGenericPane.Dock = DockStyle.None;
                    this.SplitGenericPane.Location = new Point(20, 0);

                    // Reset the controls's order
                    // 很笨的方法 控制 控件 Z 方向上的次序（即哪个控件窗口在上哪个在下）
                    this.Controls.Remove(this.JWndSpliter);
                    this.Controls.Remove(this.MdiContainer);
                    this.Controls.Add(this.MdiContainer);
                    this.JChannel.Visible = true;
                    this.DocTree.Size = new Size(SplitGenericPane.Size.Width - 4, SplitGenericPane.Size.Height - 25);
                    this.MdiContainer.Size = new Size(this.ClientSize.Width - 20, this.ClientSize.Height);
                    this.SplitGenericPane.Size = new Size(SplitGenericPane.Size.Width, this.ClientSize.Height);

                    // Prepare scroll.
                    this.SplitGenericPane.Invalidate();
                    this.TrundleTimer.Enabled = true;
                }
                this.SplitGenericPaneMouseState = 0;
            }
        }

        private void SplitGenericPane_MouseMove(object sender, MouseEventArgs e)
        {
            // 状态机
            if (e.Button == MouseButtons.Left)
            {
                switch (MouseIsOnNailOrX(e.X, e.Y))
                {
                    case WndMouseState.None:
                        if (this.SplitGenericPaneMouseState != WndMouseState.None)
                        {
                            this.SplitGenericPaneMouseState = WndMouseState.None;
                            this.SplitGenericPane.Invalidate();
                        }
                        break;
                    case WndMouseState.MouseOnNail:
                        if (this.SplitGenericPaneMouseState != WndMouseState.LButtonDownNail)
                        {
                            this.SplitGenericPaneMouseState = WndMouseState.LButtonDownNail;
                            this.SplitGenericPane.Invalidate();
                        }
                        break;
                    case WndMouseState.MouseOnX:
                        if (this.SplitGenericPaneMouseState != WndMouseState.LButtonDownX)
                        {
                            this.SplitGenericPaneMouseState = WndMouseState.LButtonDownX;
                            this.SplitGenericPane.Invalidate();
                        }
                        break;
                }
            }
            else if (e.Button == MouseButtons.None)
            {
                switch (MouseIsOnNailOrX(e.X, e.Y))
                {
                    case WndMouseState.None:
                        if (this.SplitGenericPaneMouseState != WndMouseState.None)
                        {
                            this.SplitGenericPaneMouseState = WndMouseState.None;
                            this.SplitGenericPane.Invalidate();
                        }
                        break;
                    case WndMouseState.MouseOnNail:
                        if (this.SplitGenericPaneMouseState != WndMouseState.MouseOnNail)
                        {
                            this.SplitGenericPaneMouseState = WndMouseState.MouseOnNail;
                            this.SplitGenericPane.Invalidate();
                        }
                        break;
                    case WndMouseState.MouseOnX:
                        if (this.SplitGenericPaneMouseState != WndMouseState.MouseOnX)
                        {
                            this.SplitGenericPaneMouseState = WndMouseState.MouseOnX;
                            this.SplitGenericPane.Invalidate();
                        }
                        break;
                }
            }
        }

        private void SplitGenericPane_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.SplitGenericPaneMouseState == WndMouseState.MouseOnX)
                {
                    this.SplitGenericPaneMouseState = WndMouseState.LButtonDownX;
                }
                else if (this.SplitGenericPaneMouseState == WndMouseState.MouseOnNail)
                {
                    this.SplitGenericPaneMouseState = WndMouseState.LButtonDownNail;
                }
                this.SplitGenericPane.Invalidate();
                this.Update();
            }
            this.DocTree.Focus();
        }
         
        private void SplitGenericPane_Enter(object sender, EventArgs e)
        {
            this.SplitGenericPane.Invalidate();
            this.SplitGenericPane.Update();
        }

        private void SplitGenericPane_Leave(object sender, EventArgs e)
        {
            if (this.SplitGenericPaneState != WndState.Docking)
            {
                this.SplitGenericPaneState = WndState.Ining;
                this.TrundleTimer.Enabled = true;
            }
            this.SplitGenericPane.Invalidate();
            this.SplitGenericPane.Update();
        }
        private void MdiDocManager_FirstTimeOpenFileMsg (object sender, EventArgs e)
		{
			this.MdiContainer.Visible = true ;
		}

		private void MdiDocManager_AllClosedMsg(object sender, EventArgs e)
		{
			this.MdiContainer.Visible = false ;
			this.Invalidate () ;
		}
         
		private void MdiContainer_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Graphics dc = e.Graphics ;
			// Drow Frame
			dc.DrawRectangle (this.FramePen, 1, 0, 
				this.MdiContainer.Size.Width -2, 
				this.MdiContainer.Size.Height -1) ;
			dc.DrawRectangle (this.FramePen, 3, 25,
				this.MdiContainer.Size.Width -6, 
				this.MdiContainer.Size.Height -28) ;
			// Show pic "X"
			dc.DrawImageUnscaled (this.ImgX, this.MdiContainer.Size.Width -76, 2) ;

			// Drow butten effect
			switch (this.MdiContainerMouseState)
			{
				case WndMouseState.MouseOnLeftBtn :
					if ((this.GetTabsState() & TabsState.OutRight) == 0) break ;
					dc.DrawLine (Pens.White, this.MdiContainer.Size.Width -46,5,
						this.MdiContainer.Size.Width -34,5);
					dc.DrawLine (Pens.White, this.MdiContainer.Size.Width -46,16,
						this.MdiContainer.Size.Width -46,5);
					dc.DrawLine (Pens.Black, this.MdiContainer.Size.Width -34,5,
						this.MdiContainer.Size.Width -34,18);
					dc.DrawLine (Pens.Black, this.MdiContainer.Size.Width -34,18,
						this.MdiContainer.Size.Width -46,18);
					break ;
				case WndMouseState.MouseOnRightBtn :
					if ((GetTabsState() & TabsState.OutLeft) == 0) break ;
					dc.DrawLine (Pens.White, this.MdiContainer.Size.Width -32,5,
						this.MdiContainer.Size.Width -20,5);
					dc.DrawLine (Pens.White, this.MdiContainer.Size.Width -32,16,
						this.MdiContainer.Size.Width -32,5);
					dc.DrawLine (Pens.Black, this.MdiContainer.Size.Width -20,5,
						this.MdiContainer.Size.Width -20,18);
					dc.DrawLine (Pens.Black, this.MdiContainer.Size.Width -20,18,
						this.MdiContainer.Size.Width -32,18);
					break ;
				case WndMouseState.MouseOnX :
					dc.DrawLine (Pens.White, this.MdiContainer.Size.Width -18,5,
						this.MdiContainer.Size.Width -5,5);
					dc.DrawLine (Pens.White, this.MdiContainer.Size.Width -18,16,
						this.MdiContainer.Size.Width -18,5);
					dc.DrawLine (Pens.Black, this.MdiContainer.Size.Width -5,5,
						this.MdiContainer.Size.Width -5,18);
					dc.DrawLine (Pens.Black, this.MdiContainer.Size.Width -5,18,
						this.MdiContainer.Size.Width -18,18);
					break ;
				case WndMouseState.LButtonDownLeftBtn :
					if ((GetTabsState() & TabsState.OutRight) == 0) break ;
					dc.DrawLine (Pens.Black, this.MdiContainer.Size.Width -46,5,
						this.MdiContainer.Size.Width -34,5);
					dc.DrawLine (Pens.Black, this.MdiContainer.Size.Width -46,16,
						this.MdiContainer.Size.Width -46,5);
					dc.DrawLine (Pens.White, this.MdiContainer.Size.Width -34,5,
						this.MdiContainer.Size.Width -34,18);
					dc.DrawLine (Pens.White, this.MdiContainer.Size.Width -34,18,
						this.MdiContainer.Size.Width -46,18);
					break ;
				case WndMouseState.LButtonDownRightBtn :
					if ((GetTabsState() & TabsState.OutLeft) == 0) break ;
					dc.DrawLine (Pens.Black, this.MdiContainer.Size.Width -32,5,
						this.MdiContainer.Size.Width -20,5);
					dc.DrawLine (Pens.Black, this.MdiContainer.Size.Width -32,16,
						this.MdiContainer.Size.Width -32,5);
					dc.DrawLine (Pens.White, this.MdiContainer.Size.Width -20,5,
						this.MdiContainer.Size.Width -20,18);
					dc.DrawLine (Pens.White, this.MdiContainer.Size.Width -20,18,
						this.MdiContainer.Size.Width -32,18);
					break ;
				case WndMouseState.LButtonDownX :
					dc.DrawLine (Pens.Black, this.MdiContainer.Size.Width -18,5,
						this.MdiContainer.Size.Width -5,5);
					dc.DrawLine (Pens.Black, this.MdiContainer.Size.Width -18,16,
						this.MdiContainer.Size.Width -18,5);
					dc.DrawLine (Pens.White, this.MdiContainer.Size.Width -5,5,
						this.MdiContainer.Size.Width -5,18);
					dc.DrawLine (Pens.White, this.MdiContainer.Size.Width -5,18,
						this.MdiContainer.Size.Width -18,18);
					break ;
			}
			// 画图当 "<" ">" 按钮生效时 的效果
			// Point
			Point[] pointLs = 
			{
				new Point (MdiContainer.Size.Width -42, 12) ,
				new Point (MdiContainer.Size.Width -37, 7) ,
				new Point (MdiContainer.Size.Width -37, 16) ,
			} ;

			Point[] pointRs =
			{
				new Point (MdiContainer.Size.Width -23, 12) ,
				new Point (MdiContainer.Size.Width -28, 7) ,
				new Point (MdiContainer.Size.Width -28, 16) ,

			} ;
			// 画实心 ">"
			if ((GetTabsState() & TabsState.OutRight) !=0)
				dc.FillPolygon (this.TabBrush, pointLs) ;
			// 画实心 "<"
			if ((GetTabsState() & TabsState.OutLeft) !=0)
				dc.FillPolygon (this.TabBrush, pointRs) ;

		}

		private void MdiContainer_SizeChanged(object sender, System.EventArgs e)
		{
            this.MsgBox.Size = new Size(this.MdiContainer.Size.Width - 22,
                this.MdiContainer.Size.Height - 30);
            this.MdiTabOwner.Size = new Size (this.MdiContainer.Size.Width -76,22) ;
			this.MdiContainer.Invalidate () ;
		}

		private void MdiContainer_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			// 状态机模型 实现按钮效果
			if (e.Button == MouseButtons.Left)
			{ 
				if (this.MdiContainerMouseState == WndMouseState.MouseOnX) 
				{
					this.MdiContainerMouseState = WndMouseState.LButtonDownX ;
				}
				else if (this.MdiContainerMouseState == WndMouseState.MouseOnLeftBtn) 
				{
					this.MdiContainerMouseState = WndMouseState.LButtonDownLeftBtn ;
					if ((GetTabsState() & TabsState.OutRight) != 0)
					{
						this.ehTimeTab_Tick = this.ehTimeTab_Tick_L ;
						this.TimeTab.Tick += this.ehTimeTab_Tick ;
						this.TimeTab.Enabled = true ;
					}
				}
				else if (this.MdiContainerMouseState == WndMouseState.MouseOnRightBtn) 
				{
					this.MdiContainerMouseState = WndMouseState.LButtonDownRightBtn ;
					if ((GetTabsState() & TabsState.OutLeft) != 0)
					{
						this.ehTimeTab_Tick = this.ehTimeTab_Tick_R ;
						this.TimeTab.Tick += this.ehTimeTab_Tick ;
						this.TimeTab.Enabled = true ;
					
					}
				}
				this.MdiContainer.Invalidate (
					new Rectangle (this.MdiContainer.Size.Width - 50,0,50,20)) ;						
				this.Update () ;
			}
		}

		private void MdiContainer_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			// 一个状态机模型 实现按钮效果
			if (e.Button == MouseButtons.Left)
			{
				switch (MouseIsOnLeftOrRightOrX (e.X, e.Y))
				{
					case WndMouseState.None :
						if (this.MdiContainerMouseState != WndMouseState.None)
						{
							this.MdiContainerMouseState = WndMouseState.None ;
							this.MdiContainer.Invalidate (
								new Rectangle (this.MdiContainer.Size.Width - 50,0,50,20)) ;
						}
						break ;
					case WndMouseState.MouseOnLeftBtn :
						if (this.MdiContainerMouseState != WndMouseState.LButtonDownLeftBtn)
						{
							this.MdiContainerMouseState = WndMouseState.LButtonDownLeftBtn ;
							this.MdiContainer.Invalidate (
								new Rectangle (this.MdiContainer.Size.Width - 50,0,50,20)) ;						
						}
						break ;
					case WndMouseState.MouseOnRightBtn :
						if (this.MdiContainerMouseState != WndMouseState.LButtonDownRightBtn)
						{
							this.MdiContainerMouseState = WndMouseState.LButtonDownRightBtn ;
							this.MdiContainer.Invalidate (
								new Rectangle (this.MdiContainer.Size.Width - 50,0,50,20)) ;						
						}
						break ;
					case WndMouseState.MouseOnX :
						if (this.MdiContainerMouseState!= WndMouseState.LButtonDownX)
						{
							this.MdiContainerMouseState = WndMouseState.LButtonDownX ;
							this.MdiContainer.Invalidate (
								new Rectangle (this.MdiContainer.Size.Width - 50,0,50,20)) ;						
						}
						break ;
				}
			}
			else if (e.Button == MouseButtons.None)
			{
				switch (MouseIsOnLeftOrRightOrX (e.X, e.Y))
				{
					case WndMouseState.None :
						if (this.MdiContainerMouseState != WndMouseState.None)
						{
							this.MdiContainerMouseState = WndMouseState.None ;
							this.MdiContainer.Invalidate (
								new Rectangle (this.MdiContainer.Size.Width - 50,0,50,20)) ;						
						}
						break ;
					case WndMouseState.MouseOnLeftBtn :
						if (this.MdiContainerMouseState != WndMouseState.MouseOnLeftBtn)
						{
							this.MdiContainerMouseState = WndMouseState.MouseOnLeftBtn ;
							this.MdiContainer.Invalidate (
								new Rectangle (this.MdiContainer.Size.Width - 50,0,50,20)) ;						
						}
						break ;
					case WndMouseState.MouseOnRightBtn :
						if (this.MdiContainerMouseState != WndMouseState.MouseOnRightBtn)
						{
							this.MdiContainerMouseState = WndMouseState.MouseOnRightBtn ;
							this.MdiContainer.Invalidate (
								new Rectangle (this.MdiContainer.Size.Width - 50,0,50,20)) ;						
						}
						break ;
					case WndMouseState.MouseOnX :
						if (this.MdiContainerMouseState!= WndMouseState.MouseOnX)
						{
							this.MdiContainerMouseState = WndMouseState.MouseOnX ;
							this.MdiContainer.Invalidate (
								new Rectangle (this.MdiContainer.Size.Width - 50,0,50,20)) ;						
						}
						break ;
				}
			}
		}

		private void MdiContainer_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			switch (MdiContainerMouseState)
			{
					// 单击 X 
					// 关闭当前文档
				case WndMouseState.LButtonDownX :
					TabsMenuClose_Click(sender, e) ;
					break ;
				case WndMouseState.LButtonDownLeftBtn :
				case WndMouseState.LButtonDownRightBtn :
					this.TimeTab.Enabled = false ;
					this.TimeTab.Tick -= this.ehTimeTab_Tick ; 
					break ;
			}
			this.MdiContainerMouseState = WndMouseState.None ;
			this.MdiContainer.Invalidate (
				new Rectangle (this.MdiContainer.Size.Width - 50,0,50,20)) ; 
            this.MsgBox.Focus();
		}


		private void MdiTabs_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Graphics dc = this.CreateGraphics();
			int nTabX = 10 ;
			string[] arrTitles = MdiDocManager.GetTabsTitle () ;
			for (int i = 0 ; i < arrTitles.Length ; i++)
			{
				// 叠加标题文本的长度 为了搜索被单击的TAB 
				if (i == MdiDocManager.CurTab)
					nTabX += (int)(dc.MeasureString (arrTitles[i], CurTabFont).Width) + 10 ;
				else 
					nTabX += (int)(dc.MeasureString (arrTitles[i], TabFont).Width) + 10 ;
				// 与 e.X 比较 找到了被单击 的TAB 了
				if (nTabX > e.X )
				{
					// 如果 被单击的TAB 就是当前 TAB 
					// 则什么也不做
					if (i == MdiDocManager.CurTab) 
						break ;
                    // 否则 
                    // 中文本变化将其保存到 MdiDocManager 中 
                    this.MdiDocManager.ContentTakeIn(this.MsgBox.getRichTextBoxContent());
                    //   this.htmlEditor1.testChange
                    // 被单击的TAB 设置为 当前TAB 并重画TABS 窗口
                    MdiDocManager.CurTab = i ;
					this.MdiTabs.Invalidate () ;
					// 设置 TabChange 标志 在MouseUp时更新 TextBox 的内容
					TabChange = true ;	
					break ;
				}
			}
			dc.Dispose () ;

			if (e.Button == MouseButtons.Right)
			{
				// 在出现菜单前 调用 MouseUp 更新 TextBox 的内容
				MdiTabs_MouseUp(sender, e) ;	
				// 为了不要更新 2 次设置标志 为假
				TabChange = false ;
			}
		}

        //在tab上切换各个不同的编辑文档。
        private void MdiTabs_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (this.TabChange)
			{
				this.TextPaneShowNewText () ;
				TabChange = false ;
			}
		}

		private void MdiTabs_SizeChanged(object sender, System.EventArgs e)
		{
			if (this.MdiTabs.Size.Width - MdiTabs.Location.X > this.MdiTabOwner.Size.Width)
			{
				this.MdiTabs.Location = new Point (this.MdiTabOwner.Size.Width
					- MdiTabs.Size.Width , 0) ;
				this.MdiContainer.Invalidate (
					new Rectangle (this.MdiContainer.Size.Width - 50,0,50,20)) ;
			}
		} 
		private void MdiTabs_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Graphics dc = e.Graphics ; 
			int nTabLen = 10 ;
			string[] arrTitles = null ;
			
			if (MdiDocManager.IsEmpty () ) return ;

			// Draw a white line on botton 
			dc.DrawLine (Pens.White, 0, MdiTabs.Size.Height -1, MdiTabs.Size.Width, MdiTabs.Size.Height -1) ;

			// Draw Tabs's text.
			arrTitles = MdiDocManager.GetTabsTitle () ;
			int i ; // for loop.
			for (i = 0 ; i < MdiDocManager.CurTab ; i ++)
			{
				dc.DrawString (arrTitles[i], TabFont, TabBrush, nTabLen, 7) ; 
				nTabLen += (int)(dc.MeasureString (arrTitles[i], TabFont).Width) + 10 ;
				dc.DrawLine (this.FramePen, nTabLen-5, 6, nTabLen-5, 20) ;
			}
			// Draw CurTab's Text
			// Tab effect
			Rectangle rect = new Rectangle (nTabLen-5, 4, 
				(int)(dc.MeasureString (arrTitles[i], CurTabFont).Width) + 10, 18) ;
			dc.FillRectangle (this.CurTabBrush, rect) ; 
			dc.DrawLine (Pens.Black, rect.X + rect.Width - 1 , rect.Y ,
				rect.X + rect.Width - 1, rect.Y + rect.Height - 1) ;
			dc.DrawLine (Pens.White, rect.X, rect.Y, rect.X , rect.Y + rect.Height - 1) ;
			dc.DrawLine (Pens.White, rect.X, rect.Y, rect.X + rect.Width - 1 , rect.Y ) ;
			// 
			dc.DrawString (arrTitles[i], CurTabFont, Brushes.Black, nTabLen, 7) ; 
			nTabLen += rect.Width ;
			i ++ ;
			// Draw other Tab.
			for (; i < arrTitles.Length ; i ++)
			{
				dc.DrawString (arrTitles[i], TabFont, TabBrush, nTabLen, 7) ; 
				nTabLen += (int)(dc.MeasureString (arrTitles[i], TabFont).Width) + 10 ;
				dc.DrawLine (this.FramePen, nTabLen-5, 6, nTabLen-5, 20) ;

			}
			// Reset pane's size.
			this.MdiTabs.Size = new Size (nTabLen, MdiTabs.Size.Height) ;
		}


		private void MdiTabOwner_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Graphics dc = e.Graphics ;
			dc.DrawLine (Pens.White, 0, MdiTabOwner.Size.Height-1 , MdiTabOwner.Size.Width, MdiTabOwner.Size.Height-1 ) ;
		}

		private void MdiTabOwner_SizeChanged(object sender, System.EventArgs e)
		{
			if (MdiTabs.Location.X != 0) 
			{
				if (MdiTabs.Size.Width + MdiTabs.Location.X < MdiTabOwner.Size.Width) 
				{
					MdiTabs.Location = new Point (MdiTabOwner.Size.Width - MdiTabs.Size.Width, 0) ;
					if (MdiTabs.Location.X > 0)
					{
						MdiTabs.Location = new Point (0, 0) ;
					}
				}

			}
		}
         
		private void TextEditPane_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			
		}

		private void TextEditPane_Enter(object sender, System.EventArgs e)
		{
			//GenericPane_Leave (sender, e) ;
            SplitGenericPane_Leave(sender, e);
		}

		private void TextEditPane_TextChanged(object sender, System.EventArgs e)
		{
         if (this.MdiDocManager.SetCurTabChangedFlag ())
			{
				this.MdiTabs.Invalidate () ;
			}
		}



        //双击打开对应的文档
		private void DocTree_DoubleClick(object sender, System.EventArgs e)//DoubleClick 事件是控件的更高级别的事件(x相比较与mousedoubleClick)
        {
			// 如果有新文件要打开...
			if (((FileTreeNode)this.DocTree.SelectedNode).NoteStyle == TreeNodeStyle.File)//对文件有效，对文件夹无效。
			{
				this.OpenFileFromDocTree () ;
			}
		}

		private void DocTree_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (DocTree.GetNodeAt(e.X, e.Y) != null)
			{
				this.DocTree.SelectedNode = DocTree.GetNodeAt(e.X, e.Y) ;
			}
		}

		private void DocTree_AfterCollapse(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			e.Node.ImageIndex = (int)TreeImgIndex.ClosedFolder ;
		}

		private void DocTree_AfterExpand(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			e.Node.ImageIndex = (int)TreeImgIndex.OpendFolder ;
		}

        //此事件在Node内容编辑完毕后触发，可以把修改节点名称后，对应的文件名称也执行修改的工作放在这里处理。
		private void DocTree_AfterLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
		{
			e.CancelEdit = true ;
			// 原结点
			FileTreeNode fnode = (FileTreeNode)e.Node ;

			// 新名字
			if (e.Label == null) return ;
			string sNewName = e.Label.Trim () ;

			// 新扩展名
			string sExtName = "" ;

			// 如果新名是空串
			if (Path.GetFileNameWithoutExtension(sNewName).Length == 0)
				return ;
			if (e.Node == DocTree.Nodes[0] || e.Node == DocTree.Nodes[1])
			{
				MessageBox.Show ("对不起 您不能从命名根结点！", "Rename Error", 
					MessageBoxButtons.OK, MessageBoxIcon.Warning) ;
				return ;
			}
			
			try 
			{
				if (fnode.NoteStyle == TreeNodeStyle.File)
				{
					sExtName = Path.GetExtension (sNewName) ;
					// 如果没写扩展名 则添加
					if (sExtName.Length == 0)
					{
						sNewName += Path.GetExtension (fnode.Text) ;
					}
						// 如果用户更改了扩展名 这不允许
					else if (sExtName != Path.GetExtension (fnode.Text))
					{
						MessageBox.Show ("对不起 您不能更改扩展名！", "Rename Error", 
							MessageBoxButtons.OK, MessageBoxIcon.Warning) ;
						return ;
					}
				}
			}
				// 如果新名中有非法的 字符
			catch (ArgumentException  ae)
			{
				MessageBox.Show (ae.Message, "Rename Error", MessageBoxButtons.OK, MessageBoxIcon.Warning) ;
				return ;
			}
			try //执行添加文件或修改名称后的操作。
			{
                string old = this.DocTree.SelectedNode.FullPath.ToString(); 
                if (fnode.NoteStyle == TreeNodeStyle.File)
                    this.ReNameFile(this.AppPath +"\\"+"Change Record"+ "\\" + GetRtfORRollbackPath(fnode.FullPath,"RTF"),
                      this.AppPath + "\\" + "Change Record" + "\\" + GetRtfORRollbackPath(fnode.Parent.FullPath,"RTF") + "\\" + sNewName);
                else
                    this.ReNameFolder(this.AppPath + "\\" + "Change Record" + "\\" + GetRtfORRollbackPath( fnode.FullPath,"RTF"),
                     this.AppPath + "\\" + "Change Record" + "\\" + GetRtfORRollbackPath(fnode.Parent.FullPath,"RTF") + "\\" + sNewName);
                OperateNewNodeNameToXML(OpmlFile, old, "body","Rename", sNewName, null);//修改子节点的名字
                //增加了backup的文件夹，需要考虑修改名称后，将backup文件夹内的名称对应修改。

            }
            catch (IOException ioe)
			{
				MessageBox.Show (ioe.Message ,"Rename Error", MessageBoxButtons.OK, MessageBoxIcon.Warning) ;
				return ;
			}
			fnode.Text = sNewName ;
			this.MdiTabs.Invalidate () ;
				
		}

		private void DocTree_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
				DTMenuDel_Click(sender, e) ;
		}


		private void DTMenuNewF_Click(object sender, System.EventArgs e)
		{
			if (this.DocTree.SelectedNode == null) 
				return ;
			FileTreeNode parentNode = null ;
			this.NewTextDI ++ ;	
			if (((FileTreeNode)this.DocTree.SelectedNode).NoteStyle == TreeNodeStyle.File)
			{
				parentNode = (FileTreeNode)this.DocTree.SelectedNode.Parent ;
			}
			else 
				parentNode = (FileTreeNode)this.DocTree.SelectedNode ;
			// 这里应该没有问题
			// 因为树结点是排序的并且没有重名的
			// 应该可以得到一个新名字
			foreach (TreeNode tn in parentNode.Nodes)
			{
				if (tn.Text == "New" + NewTextDI.ToString())
					NewTextDI ++ ;
			}
            FileTreeNode newftn = new FileTreeNode(TreeNodeStyle.Folder, "New" + this.NewTextDI.ToString()); ////确定创建的是文件夹类型。				
            string sNewName = "New" + this.NewTextDI.ToString();
            parentNode.Nodes.Add(newftn);
            this.DocTree.SelectedNode = newftn;
            Directory.CreateDirectory(this.AppPath + "\\" + "Change Record" + "\\" + newftn.FullPath);
            string oldPath = this.DocTree.SelectedNode.Parent.FullPath.ToString();
            OperateNewNodeNameToXML(OpmlFile, oldPath, "body","AddML", sNewName, null);

            newftn.BeginEdit();
        }

        public  void AcquireSelectedNodeInfo(TreeNode node)
        { 
            string oldPath = node.FullPath.ToString();//this.DocTree.SelectedNode.FullPath.ToString();
            int inspection_Index = node.Index + 1;//Index 从1开始
            string xpathstr = oldPath.Replace("\\", "/");
            string[] SARRAY = System.Text.RegularExpressions.Regex.Split(xpathstr, "/");
            string branchName = SARRAY[0];
            string vppName = SARRAY[SARRAY.Length - 1];
            string sExtName = Path.GetExtension(vppName);
            // 如果有写扩展名 则删除
            if (sExtName.Length != 0)
            {
                vppName = vppName.Remove(vppName.Length - sExtName.Length , sExtName.Length);
            }
            

            SelectedNodeInformation = new SelectedNodeInformation();
            SelectedNodeInformation._docTree = this.DocTree;
            SelectedNodeInformation._nodeStyle = TreeNodeStyle.File;
            SelectedNodeInformation._branchName = branchName;
            SelectedNodeInformation._inspIndex = inspection_Index;
            SelectedNodeInformation._isAllowBackup = true;
            SelectedNodeInformation._opmlPath = OpmlFile;
            SelectedNodeInformation._rootXMLNode = "Rollback";
            SelectedNodeInformation._vppName = vppName;
            SelectedNodeInformation._docTree =  this.DocTree;
            SelectedNodeInformation._lastVersion = AcquireCurrentNodeFileVersion(OpmlFile, "Rollback",branchName,inspectionNodeName ,vppName).Item1;

            string currBranch = branchName;
            string secNode = inspectionNodeName;
            string inspName = vppName + ".rtf";
            string[] path = new string[] { currBranch, secNode, inspName };
            this.DocTree.SelectedNode = GetNodeByPath(path, this.DocTree);

        }

        //double click the rtf file,打开对于的rtf文件，并且在editor中显示所点击的rtf文档内容。
        private void DTMenuOpen_Click(object sender, System.EventArgs e)
		{
			if (this.DocTree.SelectedNode == null)  return ;
			// Open file
			if (((FileTreeNode)this.DocTree.SelectedNode).NoteStyle == TreeNodeStyle.File)
			{
				this.OpenFileFromDocTree () ;
                AcquireSelectedNodeInfo(this.DocTree.SelectedNode );
            }
			// Open 文件夹
			else
			{
				if (!this.DocTree.SelectedNode.IsEditing)
				{
					this.DocTree.SelectedNode.Expand() ;
				}
			}
		}

        private void OperInspectionItem(TreeNode node)
        {
            if (node == null) return;
            // Open file
            if (((FileTreeNode)node).NoteStyle == TreeNodeStyle.File)
            {
                this.OpenFileFromDocTree();
                AcquireSelectedNodeInfo(node);
            }
            // Open 文件夹
            else
            {
                if (!this.DocTree.SelectedNode.IsEditing)
                {
                    this.DocTree.SelectedNode.Expand();
                }
            }
        }

		private void DTMenuDaoFile_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog openfileDlg = new OpenFileDialog () ;
			try 
			{
				if (openfileDlg.ShowDialog() == DialogResult.OK)
				{			
					
					string strFileName = Path.GetFileName (openfileDlg.FileName) ;
					FileTreeNode nodeNew = (FileTreeNode)this.IsBeNode ((FileTreeNode)(this.DocTree.SelectedNode),
																		strFileName, TreeNodeStyle.File) ;
					if (nodeNew != null)
					{
						switch (MessageBox.Show("文件已存在，要覆盖吗？", "J",
							MessageBoxButtons.YesNo, MessageBoxIcon.Question))
						{
							case DialogResult.Yes :
								break ;
							case DialogResult.No :
								return ;
						}
					}
					else 
					{ 
						// Tree add Note.
						nodeNew = new FileTreeNode (TreeNodeStyle.File, strFileName) ;							
						this.DocTree.SelectedNode.Nodes.Add (nodeNew) ;
					}
					// Copy 
					File.Copy (openfileDlg.FileName, 
						       this.AppPath + "\\" + "Change Record" + @"\" + GetRtfORRollbackPath(this.DocTree.SelectedNode.FullPath,"RTF") +  @"\" +  strFileName,							  
							   true) ;
					this.DocTree.SelectedNode = nodeNew ;
				}
			}
			catch (Exception exc)
			{
				MessageBox.Show (exc.ToString ()) ;
			}
		}

		private void DTMenuDaoFolder_Click(object sender, System.EventArgs e)
		{
			FolderBrowserDialog folderDlg = new FolderBrowserDialog () ;
			folderDlg.ShowNewFolderButton = false ;
			folderDlg.Description = "Please Selete!" ;
			if (folderDlg.ShowDialog() == DialogResult.OK)
			{			
				string strFolder = Path.GetFileName (folderDlg.SelectedPath) ;
				TreeNode oldNode = this.IsBeNode ((FileTreeNode)(this.DocTree.SelectedNode), strFolder, TreeNodeStyle.Folder) ;
				
				if (oldNode != null)
				{
					if ((MessageBox.Show("文件夹已存在，要覆盖吗？", "J",
						MessageBoxButtons.YesNo, MessageBoxIcon.Question)) == DialogResult.No )
						return ;
					oldNode.Remove () ;
				}
				FileTreeNode newNode = null ;
				try 
				{
					newNode = new FileTreeNode (TreeNodeStyle.Folder, strFolder) ;							
					this.DocTree.SelectedNode.Nodes.Add (newNode) ;
					// Copy  
					Directory.CreateDirectory (newNode.FullPath) ;
					this.CopyDirectory (folderDlg.SelectedPath, this.AppPath + "\\" + "Change Record" + "\\" + newNode.FullPath) ;
					// Add new node
					this.AddSubNodeFromIO (newNode) ;
					this.DocTree.SelectedNode = newNode ;
				}
				catch (Exception exc)
				{
					if (newNode != null)    newNode.Remove () ;
					if (oldNode != null)    this.DocTree.SelectedNode.Nodes.Add (oldNode) ;
					MessageBox.Show (exc.ToString ()) ;
				}
			}
		}

		private void DTMenuDel_Click(object sender, System.EventArgs e)
		{
            // 我的 Note 与我的 Text 不允许用户删除
            if (this.DocTree.SelectedNode == null)
                return; 
			if (MessageBox.Show(@"真的要从磁盘删除 ..\" + GetRtfORRollbackPath(this.DocTree.SelectedNode.FullPath,"RTF") + @" 吗？", "J",
								MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				try 
				{
					// 删除文件
					if (((FileTreeNode)this.DocTree.SelectedNode).NoteStyle == TreeNodeStyle.Folder)
					{
						this.DeleteFolderForMdiDoc (this.AppPath + "\\" + "Change Record" + @"\" + GetRtfORRollbackPath(this.DocTree.SelectedNode.FullPath, "RTF")) ;
						Directory.Delete (this.AppPath + "\\" + "Change Record" + @"\" + GetRtfORRollbackPath(this.DocTree.SelectedNode.FullPath, "RTF"), true) ;
					}
					else if (((FileTreeNode)this.DocTree.SelectedNode).NoteStyle == TreeNodeStyle.File)
					{
						this.MdiDocManager.DeleteFile (this.AppPath + "\\" + "Change Record" + @"\" + GetRtfORRollbackPath(this.DocTree.SelectedNode.FullPath, "RTF")) ;
						File.Delete (this.AppPath + "\\" + "Change Record" + @"\" + GetRtfORRollbackPath(this.DocTree.SelectedNode.FullPath, "RTF")) ;
					} 
                    string old = this.DocTree.SelectedNode.FullPath;// GetRtfORRollbackPath(this.DocTree.SelectedNode.FullPath, "RTF").ToString();
                    OperateNewNodeNameToXML(OpmlFile, old,"body", "Del", null, null);//修改子节点的名字
                    OperateNewNodeNameToXML(OpmlFile, old, "Rollback", "Del", null, null);//修改子节点的名字
                   
                    this.DocTree.SelectedNode.Remove();//modify by sr 20200727 删除顺序很重要，需要先将XML文件中得对应内容删除掉，再从TreeNode中删除对应得节点。
                }
				catch (Exception exc)
				{
					MessageBox.Show (exc.Message) ;
				}	
			}	
			this.MdiTabs.Invalidate () ;
			this.TextPaneShowNewText () ;
		}
		
		private void DTMenuReName_Click(object sender, System.EventArgs e)
		{
			this.DocTree.SelectedNode.BeginEdit () ;
		}

		private void DocTreeMenu_Popup(object sender, System.EventArgs e)
		{
			if (this.DocTree.SelectedNode == null) 
			{
				this.DocTree.SelectedNode = DocTree.SelectedNode ;
			}
			// 如果依然没有将操作的结点（就是 DocTree.SelectedNode 也为 null) 则返回。
			if (this.DocTree.SelectedNode == null)  return ;

			// 修改弹出式菜单项 Enabled 属性
			if (((FileTreeNode)this.DocTree.SelectedNode).NoteStyle == TreeNodeStyle.File)
			{
                DTMenuVersion.Enabled = true;
                DTMenuOpenInCurWnd.Enabled = true ;
				DTMenuDao.Enabled = false ;
			}
			else
			{
                DTMenuVersion.Enabled = false;
                DTMenuOpenInCurWnd.Enabled = false ;
				DTMenuDao.Enabled = true ;
			}
		}


		private void TimeTab_Tick_L(object sender, System.EventArgs e)
		{
			if (this.MdiTabs.Location.X < 0 && 
				this.MdiTabs.Size.Width + this.MdiTabs.Location.X -20
				< this.MdiTabOwner.Size.Width)
			{
				this.MdiTabs.Location = new Point (this.MdiTabOwner.Size.Width - this.MdiTabs.Size.Width, 0) ;
				this.TimeTab.Enabled = false ;
				this.TimeTab.Tick -= ehTimeTab_Tick_L ;
				return ;
			}
			this.MdiTabs.Location = new Point (this.MdiTabs.Location.X - 20, 0) ;
		}

		private void TimeTab_Tick_R(object sender, System.EventArgs e)
		{
			if (this.MdiTabs.Location.X + 20 > 0 )
			{
				this.MdiTabs.Location = new Point (0, 0) ;
				this.TimeTab.Enabled = false ;
				this.TimeTab.Tick -= ehTimeTab_Tick_R ;
				return ;
			}
			this.MdiTabs.Location = new Point (this.MdiTabs.Location.X + 20, 0) ;
		}

		private void TrundleTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			// 将滚动出 窗口
			if (this.SplitGenericPaneState == WndState.Outing)
			{	
				// 如果已经差 15 象素即滚动完毕 则 
				// 设置好窗口位置 
				// 计时器停止 ;
				if (this.SplitGenericPane.Location.X + 15 >= 15)
				{
					this.SplitGenericPane.Location = new Point (20, 0) ;				
					this.DocTree.Focus () ;
					this.TrundleTimer.Enabled = false ;
					this.SplitGenericPane.Invalidate () ;
					return ;
				}
				else this.SplitGenericPane.Location = new Point (SplitGenericPane.Location.X + 15, 0) ;
			}
		
			// 缩回窗口
			else if (this.SplitGenericPaneState == WndState.Ining)
			{
				// 如果已经全部隐藏了窗口
				// 则 计时器停止 停止滚动
				if (20 - this.SplitGenericPane.Location.X  > this.SplitGenericPane.Size.Width)
				{
					this.TrundleTimer.Enabled = false ;
					return ;
				}
				else this.SplitGenericPane.Location = new Point (SplitGenericPane.Location.X - 15, 0) ; 
			}
		}


		private void JMenuNew_Click(object sender, System.EventArgs e)
		{
			if (this.DocTree.SelectedNode == null) 
				return ;
			FileTreeNode parentNode = null ;
			this.NewTextDI ++ ;	
			if (((FileTreeNode)this.DocTree.SelectedNode).NoteStyle == TreeNodeStyle.File)
			{
				parentNode = (FileTreeNode)this.DocTree.SelectedNode.Parent ;
			}
			else 
				parentNode = (FileTreeNode)this.DocTree.SelectedNode ;
			// 这里应该没有问题
			// 因为树结点是排序的并且没有重名的
			// 应该可以得到一个新名字
			foreach (TreeNode tn in parentNode.Nodes)
			{
				if (tn.Text == "New" + NewTextDI.ToString() + ".rtf")
					NewTextDI ++ ;
			}
            FileTreeNode newftn = new FileTreeNode(TreeNodeStyle.File, "New" + this.NewTextDI.ToString()
                                                    + ".rtf");//确定创建的是文件类型。
            string sNewName = "New" + this.NewTextDI.ToString() + ".rtf";
            parentNode.Nodes.Add(newftn);
            this.DocTree.SelectedNode = newftn;
            this.DocTree.Refresh();
            OpenFileFromDocTree();
            string oldPath = this.DocTree.SelectedNode.Parent.FullPath.ToString();
            OperateNewNodeNameToXML(OpmlFile, oldPath,"body", "AddML", sNewName, null);
            newftn.BeginEdit();
        }

        private void OperateNewNodeNameToXML(string XML_File, string oldPath,string Rootnode, string OperateMode, string newNodeName, string url)
        {
            string xpathstr = oldPath.Replace("\\", "/");
            string[] SARRAY = System.Text.RegularExpressions.Regex.Split(xpathstr, "/");
            opxml.OprationOpmlFile(XML_File, SARRAY, Rootnode, OperateMode, newNodeName, url);//创建子目录AddML
        }

        private void JMenuOpen_Click(object sender, System.EventArgs e)
		{ 
            this.MdiDocManager.ContentTakeIn(this.MsgBox.getRichTextBoxContent());
            OpenFileDialog ofDlg = new OpenFileDialog () ;
			ofDlg.Filter = "文本文件 (*.rtf;*.ini)|*.rtf;*.ini|所有文件 (*.*)|*.*" ;
			if(ofDlg.ShowDialog() == DialogResult.OK)
			{
				MdiDocManager.OpenFile (ofDlg.FileName,null) ;
				this.TextPaneShowNewText () ;
				this.MdiTabs.Invalidate () ;	
			}
			ofDlg.Dispose () ;
		}

		private void JMenuExit_Click(object sender, System.EventArgs e)
		{
			this.Close () ;
		}

		private void JMenuSaveAll_Click(object sender, System.EventArgs e)
		{ 
            MdiDocManager.ContentTakeIn(this.MsgBox.getRichTextBoxContent());
            this.MdiDocManager.SaveAll () ;
			this.MdiTabs.Invalidate () ;
		}

		private void JMenuHelpAbout_Click(object sender, System.EventArgs e)
		{
			FormAbout fa = new FormAbout ();
			fa.ShowDialog (this) ;
			fa.Dispose () ;
		}

		private void JMenuFColor_Click(object sender, System.EventArgs e)
		{
			ColorDialog cDlg = new ColorDialog () ;
			if (cDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK )
			{ 
				this.DocTree.ForeColor = cDlg.Color ; 
			}
			cDlg.Dispose () ;
		}

		private void JMenuBColor_Click(object sender, System.EventArgs e)
		{
			ColorDialog cDlg = new ColorDialog () ;
			if (cDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK )
			{
				 
			}
			this.MdiContainer.Invalidate () ;
			cDlg.Dispose () ;
		}

		private void JMenuFont_Click(object sender, System.EventArgs e)
		{
			FontDialog fDlg = new FontDialog() ;
			 
		}

		private void JMenuWrap_Click(object sender, System.EventArgs e)
		{
			this.JMenuWrap.Checked = !JMenuWrap.Checked ; 
            this.MsgBox.richTextBox.WordWrap = JMenuWrap.Checked;
        }


		private void TabsMenuSave_Click(object sender, System.EventArgs e)
		{  
            this.MsgBox.userName = "AdministratorName";//string userName = VS.Utility.AccessControl.gOnly.CurrentUserName;
            this.MsgBox.strVersion = SelectedNodeInformation._lastVersion;
            //this.rtfEditor1.SaveToRTF();
           // if(MdiDocManager.Tab)
            MdiDocManager.ContentTakeIn(this.MsgBox.getRichTextBoxContent());
            this.MdiDocManager.SaveCurTab();//在添加完毕签名后再做保存操作
            this.MdiTabs.Invalidate () ;
		}

		private void TabsMenuClose_Click(object sender, System.EventArgs e)
		{
			if (MdiDocManager.IsEmpty ())
				return ; 
            MdiDocManager.ContentTakeIn(this.MsgBox.richTextBox.Rtf);//保存修改的内容时，需要考虑加入签名。
		 	ArrayList Files = MdiDocManager.GetCurFilesNotSave ();
			// 这里不完善
			if (Files == null) 
				return ;
			if (Files.Count != 0)
			{
				FormSave formS = new FormSave () ;
				int i = 0 ;
				foreach (TextInFile tf in Files)
				{
					formS.LBFlieName.Items.Add (Path.GetFileName (tf.FileName)) ;
					formS.LBFlieName.SelectedIndex = i ++ ;
				}
				switch (formS.ShowDialog (this))
				{
					case DialogResult.Yes :
                        this.MsgBox.userName = "AdministratorName";//string userName = VS.Utility.AccessControl.gOnly.CurrentUserName;
                        this.MsgBox.strVersion = SelectedNodeInformation._lastVersion;
                        this.MsgBox.SaveToRTF();
                        MdiDocManager.ContentTakeIn(this.MsgBox.richTextBox.Rtf);//保存修改的内容时，需要考虑加入签名。
                        for (i = 0 ; i < formS.LBFlieName.SelectedIndices.Count  ; i ++)
						{
							((TextInFile)Files[formS.LBFlieName.SelectedIndices[i]]).Save () ;
						}
						this.MdiDocManager.CloseCurTab () ;
						this.TextPaneShowNewText () ;                 
                        this.MdiTabs.Invalidate () ;
						break ;
					case DialogResult.No :
                          this.MdiDocManager.CloseCurTab () ;
						this.TextPaneShowNewText () ; 
						this.MdiTabs.Invalidate () ;
						break ;
					case DialogResult.Cancel :
                         break ;
				}
				formS.Dispose () ;
			}
			else 
			{
                this.MdiDocManager.CloseCurTab () ;
				this.TextPaneShowNewText () ;
				this.MdiTabs.Invalidate () ;
			}
		}


		private void TPaneMenu_Popup(object sender, System.EventArgs e)
		{
			 
		}

		private void TPaneMenuUnDo_Click(object sender, System.EventArgs e)
		{
			 
		}

		private void TPaneMenuReDo_Click(object sender, System.EventArgs e)
		{
			 
		}

		private void TPaneMenuPaste_Click(object sender, System.EventArgs e)
		{ 

		}

		private void TPaneMenuCut_Click(object sender, System.EventArgs e)
		{
			 
		}

		private void TPaneMenuSelectA_Click(object sender, System.EventArgs e)
		{
			 
		}

		private void TPaneMenuCopy_Click(object sender, System.EventArgs e)
		{
			 
		}


		#region Private Method	
		private WndMouseState MouseIsOnNailOrX (int iPosX, int iPosY)
		{
			if (iPosY > 3 && iPosY < 18 )
			{
				if (iPosX > this.SplitGenericPane.Size.Width  - 45 &&
					iPosX < this.SplitGenericPane.Size.Width  - 25 )
				{
					return WndMouseState.MouseOnNail ;
				}
				else if (iPosX > this.SplitGenericPane.Size.Width  - 25 &&
					iPosX < this.SplitGenericPane.Size.Width  - 5)
				{
					return WndMouseState.MouseOnX ; 
				}
				return WndMouseState.None ;
			}
			return WndMouseState.None ;
		}
        
		private WndMouseState MouseIsOnLeftOrRightOrX (int iPosX, int iPosY)
		{
			if (iPosY > 2 && iPosY < 18 )
			{
				if (iPosX > this.MdiContainer.Size.Width  - 48 &&
					iPosX < this.MdiContainer.Size.Width  - 34 )
				{
					return WndMouseState.MouseOnLeftBtn ;
				}
				else if (iPosX > this.MdiContainer.Size.Width  - 34 &&
					iPosX < this.MdiContainer.Size.Width  - 20)
				{
					return WndMouseState.MouseOnRightBtn ; 
				}
				else if (iPosX > this.MdiContainer.Size.Width  - 20 &&
					iPosX < this.MdiContainer.Size.Width  - 3)
				{
					return WndMouseState.MouseOnX ;
				}

				return WndMouseState.None ;
			}
			return WndMouseState.None ;
		}

		// TreeView loads note
		private void TreeViewLoadNote ()
		{

            try 
			{
                LoadRssTreeList(OpmlFile);//从XML中读取信息，加载到目录树中。
                UpdateRollbackInfo(OpmlFile);//更新rollback中的信息
                this.DocTree.Refresh();

            }
			catch (Exception e)
			{
				MessageBox.Show (e.Message) ;
			}
	    } 

        public List<Tuple<string,int ,string,bool,bool>> AcquireBeenOpenNodeVersionList(TreeNode node,ref bool acquireSuccess)
        {
            List<Tuple<string, int, string, bool, bool>> resultList = new List<Tuple<string, int, string, bool, bool>>();
            string oldPath = node.FullPath.ToString();//this.DocTree.SelectedNode.FullPath.ToString();
            int inspection_Index = node.Index + 1;//Index 从1开始
            string xpathstr = oldPath.Replace("\\", "/");
            string[] SARRAY = System.Text.RegularExpressions.Regex.Split(xpathstr, "/");
            string branchName = SARRAY[0];
            string vppName = SARRAY[SARRAY.Length - 1];
            string sExtName = Path.GetExtension(vppName);
            // 如果有写扩展名 则删除
            if (sExtName.Length != 0)
            {
                vppName = vppName.Remove(vppName.Length - sExtName.Length , sExtName.Length);
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(OpmlFile);

            XmlNode body = doc.SelectSingleNode("//Rollback");//找到根节点

            XmlNode inspCombobox = body.SelectSingleNode(branchName);
            XmlNode secondNode = inspCombobox.SelectSingleNode(inspectionNodeName);
            if (secondNode == null)
            {
                acquireSuccess = false;
                return resultList;
            }
              
            XmlNode checkListCandidate = secondNode.SelectSingleNode(vppName);
            if (checkListCandidate == null)
            {
                acquireSuccess = false;
                return resultList;
            }

            XmlNodeList condidateList = checkListCandidate.ChildNodes;
            Tuple<string, int, string, bool, bool> temp;
            for (int m = 0; m < condidateList.Count; m++)
            {
                bool[] statsStr = AnalyzeStrings(condidateList[m].InnerText);//0_0:false:false;0_1:false_true;1_0:true_false;1_1:true_true.
                if (statsStr.Length > 1)
                {
                    temp = new Tuple<string, int, string, bool, bool>(vppName, inspection_Index, condidateList[m].Name, statsStr[0], statsStr[1]);
                    resultList.Add(temp);
                    acquireSuccess = true;
                }
                else
                {
                    acquireSuccess = false;
                    return resultList;
                }

            }
            return resultList;
        }

        public Tuple<string,bool,bool> AcquireCurrentNodeFileVersion(string xmlPath,string rootNode,string branchName,string inspectionNodeName,string vppName)
        {
            string version = "Version0.0.0.0";
            Tuple<string, bool, bool> versionVector=new Tuple<string, bool, bool>(version,false,false);
           
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);

            XmlNode body = doc.SelectSingleNode("//"+ rootNode);//找到根节点
            XmlNodeList FeedList = body.ChildNodes;//得到body的所有子元素(只包含下一级的子元素，而非所有的子元素)
            if (FeedList.Count <= 0)
                return versionVector;
            XmlNode inspCombobox = body.SelectSingleNode(branchName.ToString());
            if (inspCombobox == null)//如果没有第二层的节点，则返回；
                return versionVector;
            XmlNodeList SecondNodeList = inspCombobox.ChildNodes;
            XmlNodeList InspList = null; 
            for (int n = 0; n < SecondNodeList.Count; n++)
            {
                if (SecondNodeList[n].Name == inspectionNodeName)//inspectionNodeName=="VPP_ChangeList"
                {
                    InspList = SecondNodeList[n].ChildNodes;
                    break;
                }
            }
            if (InspList.Count <= 0)
                return versionVector;
            XmlNode checkListCandidate = inspCombobox.SelectSingleNode(inspectionNodeName).SelectSingleNode(vppName.ToString ());
            if (checkListCandidate == null)
                return versionVector;
            XmlNodeList condidateList = checkListCandidate.ChildNodes;
            bool[] strArr =new bool[2] { false,false};
            if (condidateList.Count > 0)
            {
                version = condidateList[condidateList.Count - 1].Name.ToString();
                strArr = AnalyzeStrings(condidateList[condidateList.Count - 1].InnerText);
                versionVector = new Tuple<string, bool, bool>(version, strArr[0], strArr[1]);
            }
               

            return versionVector;
        }
        public void UpdateRollbackInfo(string url)
        {
            try
            {
                this.ChkListBox_CandidateInspFiles.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.ChkListBox_CandidateInspFiles_ItemCheck);

                stopMonitorInspeCombox = false;//更新rollback信息时，不允许进行chang combobox操作；
                //opxml.WriteXML(OpmlFile, "Rollback", "CAMi_Branch5.0", "T1_Assembly", "Version_3.3.0.1", "FALSE");
                XmlDocument doc = new XmlDocument();
                doc.Load(url);

                XmlNode body = doc.SelectSingleNode("//Rollback");//找到根节点

                XmlNodeList FeedList = body.ChildNodes;//得到body的所有子元素(只包含下一级的子元素，而非所有的子元素)
                this.ComBox_Branchs.Items.Clear();
                for (int i=0;i<FeedList.Count;i++)
                {
                    this.ComBox_Branchs.Items.Add(FeedList[i].Name.ToString());
                }
                if (FeedList.Count > 0)
                    this.ComBox_Branchs.SelectedItem = FeedList[FeedList.Count - 1].Name.ToString();
                else
                    return;

                XmlNode inspCombobox = body.SelectSingleNode(this.ComBox_Branchs.SelectedItem.ToString());
                if (inspCombobox == null)//如果没有第二层的节点，则返回；
                    return;
                XmlNodeList SecondNodeList = inspCombobox.ChildNodes;
                XmlNodeList InspList = null; ;

                ComBox_Inspections.Items.Clear();
                ComBox_Inspections.Items.Add("All");
                for (int n=0;n< SecondNodeList.Count;n++)
                {
                    if(SecondNodeList[n].Name== inspectionNodeName)//inspectionNodeName=="VPP_ChangeList"
                    {
                        InspList = SecondNodeList[n].ChildNodes;
                        for (int ii = 0; ii < InspList.Count; ii++)
                        {
                            this.ComBox_Inspections.Items.Add(InspList[ii].Name.ToString());
                        }
                        break;
                    }                  
                }
               
                if (InspList.Count > 1)
                    this.ComBox_Inspections.SelectedItem = InspList[1].Name.ToString();
                else
                    return;

                this.GroupBox_CondidateFiles.Text = this.ComBox_Inspections.SelectedItem.ToString();
                XmlNode checkListCandidate = inspCombobox.SelectSingleNode(inspectionNodeName).SelectSingleNode(this.ComBox_Inspections.SelectedItem.ToString());
                if (checkListCandidate == null)
                    return;
                XmlNodeList condidateList = checkListCandidate.ChildNodes;
                this.ChkListBox_CandidateInspFiles.Items.Clear();
                 for (int m=0;m<condidateList.Count;m++)
                { 
                    bool[] stateArr = AnalyzeStrings(condidateList[m].InnerText);
                   
                    if (stateArr[1])
                    {
                        this.ChkListBox_CandidateInspFiles.Items.Add(condidateList[m].Name, stateArr[0]);
                    }
                    else
                    {
                        
                    } 
                }
                this.ChkListBox_CandidateInspFiles.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ChkListBox_CandidateInspFiles_ItemCheck);

                if (ComBox_Branchs.Items.Count > 0)
                    this.GroupBox_LatestFile.Text = ComBox_Branchs.Items[ComBox_Branchs.Items.Count-1].ToString();

                XmlNode latestInsp0 = body.SelectSingleNode(this.ComBox_Branchs.Items[ComBox_Branchs.Items.Count - 1].ToString());
                if (latestInsp0 == null)//如果没有该层的节点，则返回；
                    return;
                XmlNode latestInsp = latestInsp0.SelectSingleNode(inspectionNodeName);
                XmlNodeList CurrentInspList = latestInsp.ChildNodes;
                this.ChkListBox_LatestInspFiles.Items.Clear();
                for (int n = 0; n < CurrentInspList.Count; n++)
                {
                    this.ChkListBox_LatestInspFiles.Items.Add(CurrentInspList[n].Name.ToString(),false); 
                } 
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

		// 递归的深度优先 搜索文件夹或者文件 添加到树中
		private void AddSubNodeFromIO  (TreeNode note)
		{
			if (((FileTreeNode)note).NoteStyle == TreeNodeStyle.File) 
				return ;
			DirectoryInfo dir = new DirectoryInfo (this.AppPath + "\\" + "Change Record" + @"\" + note.FullPath ) ;
			
			// 递归获得文件夹结点
			DirectoryInfo[] dirs = dir.GetDirectories() ;
			foreach (DirectoryInfo diNext in dirs) 
			{
				FileTreeNode subnote = new FileTreeNode (TreeNodeStyle.Folder, diNext.Name) ;
				note.Nodes.Add (subnote) ;
				AddSubNodeFromIO  (subnote) ;
			}
			// 获得所有文件结点
			FileInfo[] files = dir.GetFiles () ;
			foreach (FileInfo fileNext in files)
			{
				FileTreeNode subnote = new FileTreeNode (TreeNodeStyle.File, fileNext.Name) ;
				note.Nodes.Add (subnote) ;
			}
		}

        TreeNode initOpenNode = null;
        public void LoadRssTreeList(string url)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(url);

            XmlNode body = doc.SelectSingleNode("//body");//找到根节点

            XmlNodeList FeedList = body.ChildNodes;//得到body的所有子元素
           
            int index = 0;
            this.DocTree.CollapseAll();
            foreach (XmlNode feed in FeedList)//分别对子元素依次循环操作
            {
                index++;
                TreeNodeStyle nodeStyle = TreeNodeStyle.Folder;
                string nodeText = feed.Name .ToString();
                if (nodeText.Contains("rtf"))//如果没有子节点 
                {
                    nodeStyle = TreeNodeStyle.File;
                }
                FileTreeNode treeNode = new FileTreeNode(nodeStyle, nodeText);
                AddRssFeedSonListTree(feed, treeNode);//判断当前节点有子节点吗
                if(treeNode.Parent==null&&FeedList.Count ==index)
                {
                    treeNode.Expand(); 
                }
                    
                treeNode.NodeFont = new Font("微软雅黑", 12, FontStyle.Regular);
                DocTree.Nodes.Add(treeNode);//把当前创建的这个树型节点加到TreeView控件中 
                this.DocTree.SelectedNode = initOpenNode; 
            }
        }

        // 递归完成对当前节点的遍历功能
        public void AddRssFeedSonListTree(XmlNode node, FileTreeNode PreTN)
        {
            if (node.HasChildNodes)//测试当前节点有没有子元素,如果没有的话，就认为其类型是文件类型，否则认为是文件夹类型。
            {
                for (int i = 0; i < node.ChildNodes.Count; i++)//循环所有子元素
                {
                    TreeNodeStyle nodeStyle = TreeNodeStyle.Folder;
                    string nodeText = node.ChildNodes[i].Name  .ToString();
                    if (nodeText.Contains("rtf"))//如果没有子节点 
                    {
                        nodeStyle = TreeNodeStyle.File;
                    }
                    FileTreeNode treeNode = new FileTreeNode(nodeStyle, nodeText);//节点类型为文件夹类型。
                    treeNode.NodeFont = new Font("微软雅黑", 12, FontStyle.Regular);
                    PreTN.Nodes.Add(treeNode);
                    if (treeNode.Text.Contains("Abstract"))
                    {
                        initOpenNode = treeNode;
                    }
                    AddRssFeedSonListTree(node.ChildNodes[i], treeNode);//递归开始

                }

            }
        }

        // 判断 parentNode 中是否有 给定名为strName 类型为noteStyle的结点
        // 如果有返回 该结点 否则返回 null.
        private TreeNode IsBeNode (FileTreeNode parentNode, string strName, TreeNodeStyle noteStyle) 
		{
			foreach (FileTreeNode tNode in parentNode.Nodes)
			{
				if (tNode.NoteStyle == noteStyle && tNode.Text == strName)
					return tNode ;
			}
			return null ;
		}
		
		// 复制文件夹中所有 文件/文件夹 到目标目录
		private void CopyDirectory (string sourceDirName, string destDirName) 
		{
			string[] strFileName = Directory.GetFiles (sourceDirName) ;
			foreach (string strName in strFileName)
			{
				File.Copy (strName, destDirName + "\\" + Path.GetFileName (strName), true) ;
			}
			string[] strFolderName = Directory.GetDirectories (sourceDirName) ;
			foreach (string strName in strFolderName)
			{
				Directory.CreateDirectory (destDirName + "\\" + Path.GetFileName (strName)) ;
				CopyDirectory (strName, destDirName + "\\" + Path.GetFileName (strName)) ;
			}
		}
			
		private void TextPaneShowNewText()
		{	
			if (MdiDocManager.IsEmpty ())//首先判断tab中是否已经存在打开项。		
				return ;
			// 新文档打开 此时 RichTextBox 的 TextChanged 事件不被响应			   		
			string[] str = new string[MdiDocManager.CurTabLinesCnt] ;
            this.MsgBox.richTextBox.TextChanged -= this.ehRTFEditor_ContentChanged;
            this.MsgBox.richTextBox.Clear(); 
            string rtf = null;
            this.MdiDocManager.ContentOutTo(ref rtf);

            this.MsgBox.richTextBox.Rtf = rtf;
            this.MsgBox.richTextBox.SelectionStart = this.MsgBox.richTextBox.TextLength;
            this.MsgBox.Focus();

            TreeNode currentTabTreeNode = null;
            this.MdiDocManager.GetTreeNode(ref currentTabTreeNode);
            string oldPath = currentTabTreeNode.FullPath.ToString();
            int inspection_Index = currentTabTreeNode.Index + 1;//Index 从1开始
            string xpathstr = oldPath.Replace("\\", "/");
            string[] SARRAY = System.Text.RegularExpressions.Regex.Split(xpathstr, "/");
            string branchName = SARRAY[0];
            string vppName = SARRAY[SARRAY.Length - 1];
            string sExtName = Path.GetExtension(vppName);
            // 如果有写扩展名 则删除
            if (sExtName.Length != 0)
            {
                vppName = vppName.Remove(vppName.Length - sExtName.Length , sExtName.Length);
            }
            AcquireSelectedNodeInfo(currentTabTreeNode); //获取当前tab对应的所有信息。
            this.MsgBox.richTextBox.TextChanged += this.ehRTFEditor_ContentChanged;//重新恢复RTFeditor的textchange事件响应。
        }
        private TabsState GetTabsState()
		{
			TabsState rtn = TabsState.None ;
			if (MdiTabs.Location.X < 0)
				rtn = rtn | TabsState.OutLeft ;
			if (MdiTabs.Size.Width + MdiTabs.Location.X > MdiTabOwner.Size.Width)
				rtn = rtn | TabsState.OutRight ;
			return rtn ;
		}

		//从目录树中打开点击的文件，鼠标事件触发
		private void OpenFileFromDocTree ()
		{
            this.MdiDocManager.ContentTakeIn(this.MsgBox.richTextBox.Rtf); 
            if (this.DocTree.SelectedNode == null)
                return;

            //当点击某个子节点时，首先获取对应的文件路径，然后将该文件暂存到临时位置，待后续事件切换时调用。
            this.MdiDocManager.OpenFile (this.AppPath + "\\" + "Change Record" + "\\" +GetRtfORRollbackPath ( this.DocTree.SelectedNode.FullPath,"RTF"),this.DocTree .SelectedNode);
            UpdateVersionInfo(this.DocTree.SelectedNode);

            this.TextPaneShowNewText () ; 
			this.MdiTabs.Invalidate () ;	
		}
		
        private void UpdateVersionInfo(TreeNode node)
        {
            bool getSuccess = false;
            List<Tuple<string, int, string, bool, bool>> versionList=AcquireBeenOpenNodeVersionList(node, ref getSuccess);
            if (getSuccess)
            {
                this.MdiDocManager.DeleteAllVersionInfo();
                for (int i = 0; i < versionList.Count; i++)
                {
                    this.MdiDocManager.AddNewVersion(versionList[i]);
                }
            }
            else
                return;
        }

		private void ReNameFile (string oldpath, string newpath) 
		{
			if (this.MdiDocManager.ReNameFile(oldpath, newpath))
				return ;
			File.Move (oldpath, newpath) ;
		}

		private void ReNameFolder (string oldpath, string newpath) 
		{ 	
			Directory.Move (oldpath, newpath) ;
			MdiDocManager.ForFolderNameChanged (oldpath, newpath) ;
		}
		private void DeleteFolderForMdiDoc (string path)
		{
			string[] sFileName = Directory.GetFiles (path) ;
			foreach (string sf in sFileName)
			{
				MdiDocManager.DeleteFile (sf) ;
			}
			string[] sFolderName = Directory.GetDirectories (path) ;
			foreach (string sd in sFolderName)
			{
				DeleteFolderForMdiDoc (sd) ;
			}
		}

		private void SerializeTexePaneAtt ()
		{
			// font
			IFormatter formatter = new BinaryFormatter();
			Stream stream = new FileStream(this.AppPath+"\\bin\\"+"font.bin", FileMode.Create, 
										   FileAccess.Write, FileShare.None);
			formatter.Serialize(stream, this.MsgBox.richTextBox.Font);
			stream.Close () ;
			// forecolor
			stream = new FileStream(this.AppPath+"\\bin\\"+"forecolor.bin", FileMode.Create, 
									FileAccess.Write, FileShare.None);
			formatter.Serialize(stream, this.MsgBox.richTextBox.ForeColor);
			stream.Close ();
			// backcolor
			stream = new FileStream(this.AppPath+"\\bin\\"+"backcolor.bin", FileMode.Create, 
				FileAccess.Write, FileShare.None);
			formatter.Serialize(stream, this.MsgBox.richTextBox.BackColor);
			stream.Close ();
		}
		private void DeserializeTexePaneAtt ()
		{
			IFormatter formatter = null;
			Stream stream = null ;
			try 
			{
				formatter = new BinaryFormatter();
				stream = new FileStream(this.AppPath+"\\bin\\"+"font.bin", FileMode.Open , 
					FileAccess.Read, FileShare.None);
				this.MsgBox.richTextBox.Font = (Font)formatter.Deserialize(stream);
			}
			catch{} // 忽略
			finally
			{
				if (stream != null) 
					stream.Close () ;
			}
			try {
				// forecolor
				stream = new FileStream(this.AppPath+"\\bin\\"+"forecolor.bin", FileMode.Open, 
					FileAccess.Read, FileShare.None);
				this.MsgBox.richTextBox.ForeColor = Color.FromArgb 
					(((Color)formatter.Deserialize(stream)).ToArgb()) ;
			}
			catch{} // 忽略
			finally
			{
				if (stream != null) 
					stream.Close () ;
			}
			try
			{
				// backcolor
				stream = new FileStream(this.AppPath+"\\bin\\"+"backcolor.bin", FileMode.Open, 
					FileAccess.Read, FileShare.None);
				this.MsgBox.richTextBox.BackColor =  Color.FromArgb 
					(((Color)formatter.Deserialize(stream)).ToArgb()) ;
			}
			catch{} // 忽略
			finally
			{
				if (stream != null) 
					stream.Close () ;
			}
		}
        #endregion

        private void DocTree_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void HtmlEditor_ContentChanged(object sender, EventArgs e)
        {
            if (this.MdiDocManager.SetCurTabChangedFlag())
            {
                this.MdiTabs.Invalidate();
            }
        }

        private void htmlEditor1_ContentChanged(object sender, EventArgs e)
        {
            if (this.MdiDocManager.SetCurTabChangedFlag())//在此处增加web content被修改时，需要执行的代码。
            {
                this.MdiTabs.Invalidate();
            }
        }
        private void MdiContainer_TabIndexChanged(object sender, EventArgs e)
        {

        }

        private void htmlEditor1_Load(object sender, EventArgs e)
        {

        }

        //生成新的branch
        private void menuItem3_Click(object sender, EventArgs e)
        {
            this.DocTree.CollapseAll();
             branchNameList = new List<string>();
            // string branchname = this.DocTree.Nodes[0];
            int num = this.DocTree.Nodes.Count;
            for(int i=0;i<this.DocTree.Nodes.Count;i++)
            {
                branchNameList.Add(this.DocTree.Nodes[i].Text.ToString());
            }

            NewBranch addNew = new NewBranch(this); 
            addNew.transPara(this.DocTree, OpmlFile, SelectedNodeInformation._branchName, SelectedNodeInformation._vppName, SelectedNodeInformation._inspIndex, null);

            switch (addNew.ShowDialog())
            {
                case DialogResult.Yes:
                    // CurTab_Closing();//关闭当前的tab，在关闭前进行保存。
                    opxml.UpdateOpmlFile(OpmlFile, DocTree, null, null);//更新XML文件,将treenode的内容更新到body节点下。
                    opxml.WriteXMLAbortRollback(OpmlFile, DocTree, null, inspectionNodeName, "AddNewBranch", null, null);
                    this.OpenFileFromDocTree();
                    UpdateRollbackInfo(OpmlFile);
                    break;
                case DialogResult.Cancel:
                    break;
            }
        }
        public string GetRtfORRollbackPath(string nodeFullPath,string key)
        {
            string path = null;
            string xpathstr = nodeFullPath.Replace("\\", "/");
            string[] SARRAY = System.Text.RegularExpressions.Regex.Split(xpathstr, "/");
            string[] tempStr = new string[SARRAY.Length + 1];
            for(int i=0;i<SARRAY.Length;i++)
            {
                if (i == 0)
                {
                    tempStr[i] = SARRAY[i];
                    tempStr[i + 1] = key;
                }
                else
                {
                    tempStr[i + 1] = SARRAY[i];
                }
              
            }

            path = String.Join("\\", tempStr);
            return path;
        }
        public bool AddNewBranch(string name)
        {
            FileTreeNode NewBranchNode = new FileTreeNode(TreeNodeStyle.Folder, name);
            this.DocTree.Nodes.Add(NewBranchNode);//增加主节点；
            this.DocTree.SelectedNode = NewBranchNode;

            string RTFPath = Application.StartupPath + "\\" + "Change Record" + "\\" + GetRtfORRollbackPath(DocTree.SelectedNode.FullPath, "RTF");
            string RollbackPath = Application.StartupPath + "\\" + "Change Record" + "\\" + GetRtfORRollbackPath(DocTree.SelectedNode.FullPath, "Backup");
            lock (locker)
            {
                Directory.CreateDirectory(RTFPath);
                Directory.CreateDirectory(RollbackPath);
            }


            FileTreeNode newAbstract = new FileTreeNode(TreeNodeStyle.File, "Abstract.rtf");
            newAbstract.NodeFont = new Font("微软雅黑", 12, FontStyle.Regular);
            NewBranchNode.Nodes.Add(newAbstract);//增加abstract
            this.DocTree.SelectedNode = newAbstract;
            CreateFile(Application.StartupPath + "\\" + "Change Record" + "\\" + this.DocTree.SelectedNode.FullPath);

            FileTreeNode proposalNode = new FileTreeNode(TreeNodeStyle.File, "Proposal.rtf");
            FileTreeNode msNode = new FileTreeNode(TreeNodeStyle.Folder, "MachineSupportAndPorposal");
            proposalNode.NodeFont = new Font("微软雅黑", 12, FontStyle.Regular);
            msNode.NodeFont = new Font("微软雅黑", 12, FontStyle.Regular);
            msNode.Nodes.Add(proposalNode);
            NewBranchNode.Nodes.Add(msNode);
            this.DocTree.SelectedNode = msNode;
            string MSPath = RTFPath + "\\" + this.DocTree.SelectedNode.Text;         
            Directory.CreateDirectory(MSPath);
            this.DocTree.SelectedNode = proposalNode;
            CreateFile(MSPath + "\\" + this.DocTree.SelectedNode.Text);

            List<string> newBranchVppNodeNameList = new List<string>();
            int vppNumber = 0;
            string tempNamestr = null;
            //  FrameworkConfiguration.gOnly.ReadGeneralSetting("InspectionSetup", "NumOfToolBlock", 0, out vppNumber);
            string vppNum = INIOperationClass.INIGetStringValue(ParameterFile, "InspectionSetup", "NumOfToolBlock", null);
            vppNumber = int.Parse(vppNum);

            FileTreeNode inspectionTempNode;
            for (int i = 0; i < vppNumber; i++)
            {
                string nameKey = "TB" + (i + 1).ToString() + "Name";
                tempNamestr = INIOperationClass.INIGetStringValue( ParameterFile, "InspectionSetup", nameKey, null);
                newBranchVppNodeNameList.Add(tempNamestr);
            }

            FileTreeNode vppNode = new FileTreeNode(TreeNodeStyle.Folder, "VPP_ChangeList");
            vppNode.NodeFont = new Font("微软雅黑", 12, FontStyle.Regular);
            NewBranchNode.Nodes.Add(vppNode);
            this.DocTree.SelectedNode = vppNode;
            string vppPath = RTFPath + "\\" + DocTree.SelectedNode.Text;
            Directory.CreateDirectory(vppPath);
            for (int n = 0; n < newBranchVppNodeNameList.Count; n++)
            {
                inspectionTempNode = new FileTreeNode(TreeNodeStyle.File, newBranchVppNodeNameList[n].ToString() + ".rtf");
                inspectionTempNode.NodeFont = new Font("微软雅黑", 12, FontStyle.Regular);
                vppNode.Nodes.Add(inspectionTempNode);

                this.DocTree.SelectedNode = inspectionTempNode;
                string inspectionFilePath = vppPath + "\\" + this.DocTree.SelectedNode.Text;
                 
                CreateFile(inspectionFilePath);
                string rollBackInspFoldPath = RollbackPath+ "\\" + "VPP_ChangeList" + "\\" + newBranchVppNodeNameList[n].ToString();
                Directory.CreateDirectory(rollBackInspFoldPath);
            }
            this.DocTree.SelectedNode = newAbstract;//加载后自动打开当前的abstract文档。
            //this.OpenFileFromDocTree();
            return true;
        }

        public void UpdateVPPName()
        {
            List<string> newBranchVppNodeNameList = new List<string>();
            int vppNumber = 0;
            string tempNamestr = null;
            //  FrameworkConfiguration.gOnly.ReadGeneralSetting("InspectionSetup", "NumOfToolBlock", 0, out vppNumber);
            string vppNum = INIOperationClass.INIGetStringValue(Application.StartupPath + "\\" + ParameterFile, "InspectionSetup", "NumOfToolBlock", null);
            vppNumber = int.Parse(vppNum);

            FileTreeNode inspectionTempNode;
            for (int i = 0; i < vppNumber; i++)
            {
                string nameKey = "TB" + (i + 1).ToString() + "Name";
                tempNamestr = INIOperationClass.INIGetStringValue(Application.StartupPath + "\\" + ParameterFile, "InspectionSetup", nameKey, null);
                newBranchVppNodeNameList.Add(tempNamestr);//获取INI文件中的各个inspection的最新名称；

            }

            //一一比较INI文件中的vpp名称和TreeNode中的子节点的名称，如果不同就替换掉，再负责将对应文件夹中的文件名称和XML中的名称进行修改。

        }

        private void CreateFile(string path)
        {
            lock (locker)
            {
                if (!File.Exists(path))
                {
                    System.Threading.Thread.Sleep(10);
                    FileStream fs = null;
                    try
                    {
                        fs = File.Create(path);
                    }
                    catch (DirectoryNotFoundException dire)
                    {
                        MessageBox.Show(dire.Message + "\n文件可能在其他程序中被修改，强烈建议保存其他文件后重起JNote",
                            "CreateFile Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    finally
                    {
                        if (fs != null)
                            fs.Close();
                    }
                }
            }
        }
        private void DocTree_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void DocTree_DragDrop(object sender, DragEventArgs e)
        {
            // Unlock updates
            DragHelper.ImageList_DragLeave(this.DocTree.Handle);

            // Get drop node
            FileTreeNode dropNode = (FileTreeNode)this.DocTree.GetNodeAt(this.DocTree.PointToClient(new Point(e.X, e.Y)));

            // If drop node isn't equal to drag node, add drag node as child of drop node
            if (this.dragNode != dropNode)
            {
                // Remove drag node from parent
                if (this.dragNode.Parent == null)
                {
                    this.DocTree.Nodes.Remove(this.dragNode);
                }
                else
                {
                    this.dragNode.Parent.Nodes.Remove(this.dragNode);
                }

                // Add drag node to drop node
                // dropNode.Nodes.Add(this.dragNode);
                dropNode.Parent .Nodes.Add(this.dragNode);

                dropNode.ExpandAll();

                // Set drag node to null
                this.dragNode = null;

                // Disable scroll timer
                this.TrundleTimer.Enabled = false;
            }
        }

        private void DocTree_DragEnter(object sender, DragEventArgs e)
        {
            DragHelper.ImageList_DragEnter(this.DocTree.Handle, e.X - this.DocTree.Left,
              e.Y - this.DocTree.Top);

            // Enable timer for scrolling dragged item
            this.TrundleTimer.Enabled = true;
        }

        private void DocTree_DragLeave(object sender, EventArgs e)
        {
            DragHelper.ImageList_DragLeave(this.DocTree.Handle);

            // Disable timer for scrolling dragged item
            this.TrundleTimer.Enabled = false;
        }

        private void DocTree_DragOver(object sender, DragEventArgs e)
        {
            // Compute drag position and move image
            Point formP = this.PointToClient(new Point(e.X, e.Y));
            DragHelper.ImageList_DragMove(formP.X - this.DocTree.Left, formP.Y - this.DocTree.Top);

            // Get actual drop node
            FileTreeNode dropNode = (FileTreeNode)this.DocTree.GetNodeAt(this.DocTree.PointToClient(new Point(e.X, e.Y)));
            if (dropNode == null)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.Move;

            // if mouse is on a new node select it
            if (this.tempDropNode != dropNode)
            {
                DragHelper.ImageList_DragShowNolock(false);
                this.DocTree.SelectedNode = dropNode;
                DragHelper.ImageList_DragShowNolock(true);
                tempDropNode = dropNode;
            }

            // Avoid that drop node is child of drag node 
            FileTreeNode tmpNode = dropNode;
            while (tmpNode.Parent != null)
            {
                if (tmpNode.Parent == this.dragNode) e.Effect = DragDropEffects.None;
                tmpNode = (FileTreeNode)tmpNode.Parent;
            }
        }

        private void DocTree_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (e.Effect == DragDropEffects.Move)
            {
                // Show pointer cursor while dragging
                e.UseDefaultCursors = false;
                this.DocTree.Cursor = Cursors.Default;
            }
            else e.UseDefaultCursors = true;
        }

        private void DocTree_ItemDrag(object sender, ItemDragEventArgs e)
        {
            // Get drag node and select it
            this.dragNode = (FileTreeNode)e.Item;
            this.DocTree.SelectedNode = this.dragNode;

            // Reset image list used for drag image
            this.imageListDrag.Images.Clear();
            this.imageListDrag.ImageSize = new Size(this.dragNode.Bounds.Size.Width + this.DocTree.Indent, this.dragNode.Bounds.Height);

            // Create new bitmap
            // This bitmap will contain the tree node image to be dragged
            Bitmap bmp = new Bitmap(this.dragNode.Bounds.Width + this.DocTree.Indent, this.dragNode.Bounds.Height);

            // Get graphics from bitmap
            Graphics gfx = Graphics.FromImage(bmp);

            // Draw node icon into the bitmap
            gfx.DrawImage(this.TreeVImgList.Images[0], 0, 0);

            // Draw node label into bitmap
            gfx.DrawString(this.dragNode.Text,
                this.DocTree.Font,
                new SolidBrush(this.DocTree.ForeColor),
                (float)this.DocTree.Indent, 1.0f);

            // Add bitmap to imagelist
            this.imageListDrag.Images.Add(bmp);

            /*
            //这里是原来的计算位置的算法，
            //当TreeView放在其他容器中如Panel时，位置计算不准确
            // Get mouse position in client coordinates
            Point p = this.treeView1.PointToClient(Control.MousePosition);
            // Compute delta between mouse position and node bounds
            int dx = p.X + this.treeView1.Indent - this.dragNode.Bounds.Left;
            int dy = p.Y - this.dragNode.Bounds.Top;
            */

            /*
             * 计算显示图片及内容的位置，该位置是相对于鼠标指针的位置
             * 即以鼠标指针为原点的位置，向左向上为正数，向右向下为负数
             * 鼠标原点的横坐标（X）为0，
             * 但纵坐标受TreeView相对于Form的位置的Location.Y限制
             * 这里的TreeView.Location.Y并非设计器属性里的数值
             * 因为TreeView有可能放在Panel里，那么这个值就是相对于Panel的值
             * 那么，实际值的计算方法是取得鼠标相对Form和TreeView的位置
             * 如果TreeView未设置Dock，且顶端与容器边缘有一定距离，还需要减去TreeView.Top
             * 则Y值的差即为鼠标指针相对于原点的纵坐标位置
             * 如本例子里TreeView的位置，鼠标指针处并非纵坐标的原点，
             * 而实际的原点要在鼠标指针向下100的位置处
             */
            //Control.MousePosition为鼠标相对于屏幕左上角的位置
            //计算鼠标在TreeView里的相对位置
            Point pt = DocTree.PointToClient(Control.MousePosition);
            //计算鼠标在窗体里的相对位置
            Point pf = this.PointToClient(Control.MousePosition);

            // Compute delta between mouse position and node bounds
            //设置为10是为了使图片超前于鼠标指针一点点
            int dx = 10;
            int dy = pf.Y - pt.Y - DocTree.Top + this.dragNode.Bounds.Height;
            // Begin dragging image
            if (DragHelper.ImageList_BeginDrag(this.imageListDrag.Handle, 0, dx, dy))
            {
                // Begin dragging
                this.DocTree.DoDragDrop(bmp, DragDropEffects.Move);
                // End dragging image
                DragHelper.ImageList_EndDrag();
            }
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            /*源代码处使用的是this.PointToClient(Control.MousePosition)
             * 这样对于向上拖拽时无法使滚动条滚动
             * 这里修改为this.treeView1.PointToClient(Control.MousePosition)
            // get node at mouse position
            Point pt = this.PointToClient(Control.MousePosition);
             */
            // get node at mouse position
            Point pt = this.DocTree.PointToClient(Control.MousePosition);
            FileTreeNode node = (FileTreeNode)this.DocTree.GetNodeAt(pt);

            if (node == null) return;

            // if mouse is near to the top, scroll up
            if (pt.Y < 30)
            {
                // set actual node to the upper one
                if (node.PrevVisibleNode != null)
                {
                    node = (FileTreeNode)node.PrevVisibleNode;

                    // hide drag image
                    DragHelper.ImageList_DragShowNolock(false);
                    // scroll and refresh
                    node.EnsureVisible();
                    this.DocTree.Refresh();
                    // show drag image
                    DragHelper.ImageList_DragShowNolock(true);

                }
            }
            // if mouse is near to the bottom, scroll down
            else if (pt.Y > this.DocTree.Size.Height - 30)
            {
                if (node.NextVisibleNode != null)
                {
                    node = (FileTreeNode)node.NextVisibleNode;

                    DragHelper.ImageList_DragShowNolock(false);
                    node.EnsureVisible();
                    this.DocTree.Refresh();
                    DragHelper.ImageList_DragShowNolock(true);
                }
            }
        }

        private void DTMenuMoveDown_Click(object sender, EventArgs e)
        {
            FileTreeNode trNode = (FileTreeNode)DocTree.SelectedNode;

            FileTreeNode nextNode = (FileTreeNode)trNode.NextNode;
            if (nextNode == null)   //下一个节点为Null时返回
            {
                return;
            }

            FileTreeNode NewNode = (FileTreeNode)trNode.Clone();
            trNode.Parent.Nodes.Insert(nextNode.Index + 1, NewNode);
            trNode.Remove();

            DocTree.SelectedNode = NewNode;
            bool writetoXML = opxml.UpdateOpmlFile(OpmlFile, DocTree, null, null);//后续加入log显示，记录每一步操作得流程。
        }

        private void DTMenuMoveUp_Click(object sender, EventArgs e)
        {
            FileTreeNode trNode = (FileTreeNode)DocTree.SelectedNode;//FileTreeNode
            FileTreeNode preNode = (FileTreeNode)trNode.PrevNode;
            if (preNode == null)  //选中节点的上一个节点为Null则返回
            {
                return;
            }
            FileTreeNode NewNode = (FileTreeNode)trNode.Clone();
            trNode.Parent.Nodes.Insert(preNode.Index, NewNode);
            trNode.Remove();
            DocTree.SelectedNode = NewNode;

            bool writetoXML=  opxml.UpdateOpmlFile(OpmlFile,DocTree, null, null);//根据TreeNode上内容更新ML文档内容。
        }

        private void ComBox_Branchs_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(OpmlFile);

                XmlNode body = doc.SelectSingleNode("//Rollback");//找到根节点
                XmlNode inspCombobox = body.SelectSingleNode(this.ComBox_Branchs.SelectedItem.ToString());
                if (inspCombobox == null)//如果没有第二层的节点，则返回；
                    return;
                XmlNodeList SecondNodeList = inspCombobox.ChildNodes;
                XmlNodeList InspList = null; ;

                ComBox_Inspections.Items.Clear();
                ComBox_Inspections.Items.Add("All");
                for (int n = 0; n < SecondNodeList.Count; n++)
                {
                    if (SecondNodeList[n].Name == inspectionNodeName)//inspectionNodeName=="VPP_ChangeList"
                    {
                        InspList = SecondNodeList[n].ChildNodes;
                        for (int ii = 0; ii < InspList.Count; ii++)
                        {
                            this.ComBox_Inspections.Items.Add(InspList[ii].Name.ToString());
                        }
                        break;
                    }
                }

                if (InspList.Count > 1)
                    this.ComBox_Inspections.SelectedItem = InspList[1].Name.ToString();
                else
                    return;
                ////

                this.GroupBox_CondidateFiles.Text = this.ComBox_Inspections.SelectedItem.ToString();
                SecondNodeList = inspCombobox.ChildNodes;

                XmlNode checkListCandidate = inspCombobox.SelectSingleNode(inspectionNodeName).SelectSingleNode(this.ComBox_Inspections.SelectedItem.ToString());
                if (checkListCandidate == null)
                    return;
                XmlNodeList condidateList =  checkListCandidate.ChildNodes;
                this.ChkListBox_CandidateInspFiles.Items.Clear();
                for (int m = 0; m < condidateList.Count; m++)
                {
                    bool[] stateArr = AnalyzeStrings(condidateList[m].InnerText);
                    
                    if (stateArr[1])
                    {
                        this.ChkListBox_CandidateInspFiles.Items.Add(condidateList[m].Name, stateArr[0]);                    
                    }
                    else
                    {
                        
                    }
                }

                if (this.ComBox_Branchs.SelectedIndex == (this.ComBox_Branchs.Items.Count - 1))
                {
                    
                    this.TBN_RollBackToLatestInsp.Visible = false;
                    this.GroupBox_LatestFile.Visible = false;
                    this.TBN_Rollback.Visible = true; ;
                }
                else
                {
                    this.TBN_RollBackToLatestInsp.Visible = true;
                    this.GroupBox_LatestFile.Visible = true;
                    this.TBN_Rollback.Visible = false;
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private bool[] AnalyzeStrings(string innerStr)
        {
            string[] strArr = innerStr.Trim().Split('_');
            bool[] statusArr = new bool[strArr.Length];
            for (int i = 0; i < strArr.Length; i++)
            {
                statusArr[i] = bool.Parse(strArr[i]);
            }
            return statusArr;
        }

        //选择combobox中的inspection
        private void ComBox_Inspections_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.TBN_Rollback.Visible = true;
                this.GroupBox_CondidateFiles.Visible = true;
                this.ChkListBox_CandidateInspFiles.Visible = true;
                this.TBN_RollBackToLatestInsp.Visible = true;
                this.GroupBox_LatestFile.Visible = true;
                this.TBN_RollBackToLatestInsp.Visible = true;
                this.LatestVersionDateView.Visible = false;

                if (stopMonitorInspeCombox == false)
                    return;
                XmlDocument doc = new XmlDocument();
                doc.Load(OpmlFile);

                XmlNode body = doc.SelectSingleNode("//Rollback");//找到根节点

                string branchName = this.ComBox_Branchs.SelectedItem.ToString();
                XmlNode inspCombobox = body.SelectSingleNode(this.ComBox_Branchs.SelectedItem.ToString());
                this.GroupBox_CondidateFiles.Text = this.ComBox_Inspections.SelectedItem.ToString();

                XmlNode secondNode = inspCombobox.SelectSingleNode(inspectionNodeName);
                if (secondNode == null)
                    return;
                string selInspectionName = this.ComBox_Inspections.SelectedItem.ToString();
                XmlNode checkListCandidate = secondNode.SelectSingleNode(selInspectionName);
                List<Tuple<string, string,bool>> allInspVersionList = new List<Tuple<string, string,bool>>();
                if (checkListCandidate == null|| selInspectionName.ToUpper()=="ALL")//当在xml中没有找到所选择的item时，判断是否选择的时“All”，如果不是则返回。
                {
                    Tuple<string, string, bool> inspLastVersion;// = new Tuple<string, string, bool>();
                    Tuple<string, bool, bool> tempInfo;
                    string tempInspName = null;
                   // AnalyzeStrings
                    //此处进行all-inspection的最后版本的显示处理。
                    if (selInspectionName.ToUpper() == "ALL")
                    {
                        for(int i=0;i<secondNode.ChildNodes.Count;i++)//判断vpp个数
                        {
                           tempInspName = secondNode.ChildNodes[i].Name.ToString();
                           tempInfo = AcquireCurrentNodeFileVersion(OpmlFile, "Rollback", branchName, inspectionNodeName, tempInspName);                           
                           inspLastVersion = new Tuple<string, string, bool>(tempInspName, tempInfo.Item1, tempInfo.Item2);
                           allInspVersionList.Add(inspLastVersion);
                        }
                        ReshowLatestVersion(allInspVersionList);
                        return;
                    }
                    else
                       return;
                }
                
                string currBranch = this.ComBox_Branchs.SelectedItem.ToString();
                string secNode =FormJ.inspectionNodeName;
                string inspName = this.ComBox_Inspections.SelectedItem.ToString() + ".rtf";
                string[] path = new string[] { currBranch, secNode, inspName };
                this.DocTree.SelectedNode = GetNodeByPath(path, this.DocTree);
                OperInspectionItem(this.DocTree.SelectedNode);//模拟操作双击rtf文件节点

                XmlNodeList condidateList = checkListCandidate.ChildNodes;
                this.ChkListBox_CandidateInspFiles.Items.Clear();
                for (int m = 0; m < condidateList.Count; m++)
                {
                    bool[] stateArr = AnalyzeStrings(condidateList[m].InnerText);
                    
                    if (stateArr[1])
                    {
                        this.ChkListBox_CandidateInspFiles.Items.Add(condidateList[m].Name, stateArr[0]);
                    }
                    else
                    {
                       
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

       
        private void ReshowLatestVersion(List<Tuple<string, string, bool>> LastVersionInfoList )
        {

            this.TBN_Rollback.Visible = false;
            this.GroupBox_CondidateFiles.Visible = false;
            this.ChkListBox_CandidateInspFiles.Visible = false;
            this.TBN_RollBackToLatestInsp.Visible = false;
            this.GroupBox_LatestFile.Visible = false;
            this.TBN_RollBackToLatestInsp.Visible = false;
            this.LatestVersionDateView.Visible = true;
            this.LatestVersionDateView.BringToFront();
            this.LatestVersionDateView.ReadOnly = false;

            this.LatestVersionDateView.Location = this.GroupBox_CondidateFiles.Location;
            this.LatestVersionDateView.Size = new Size((GroupBox_CondidateFiles.Size.Width * 3), GroupBox_CondidateFiles.Size.Height);
            this.LatestVersionDateView.DataSource = null;//清空当前datagridview
            this.LatestVersionDateView.Rows.Clear();
           // this.LatestVersionDateView.d

            DataGridViewTextBoxColumn IndexCol = new DataGridViewTextBoxColumn();
            IndexCol.HeaderText = "Index";
            LatestVersionDateView.Columns.Add(IndexCol);
            DataGridViewTextBoxColumn VersionCol = new DataGridViewTextBoxColumn();
            VersionCol.HeaderText = "Inspection Name";
            LatestVersionDateView.Columns.Add(VersionCol);
            DataGridViewTextBoxColumn myCol = new DataGridViewTextBoxColumn();
            myCol.HeaderText = "Latest Version";
            LatestVersionDateView.Columns.Add(myCol);
            DataGridViewColumn Isbackup = new DataGridViewCheckBoxColumn();
            Isbackup.HeaderText = "IsChecked";
            LatestVersionDateView.Columns.Add(Isbackup);

            List<Tuple<string, string, bool>> tempList = LastVersionInfoList; 
            for (int m = 0; m < tempList.Count; m++)
            { 
                LatestVersionDateView.Rows.Add();
                LatestVersionDateView.Rows[m].Cells[0].Value = (m + 1).ToString();
                LatestVersionDateView.Rows[m].Cells[1].Value = tempList[m].Item1;
                LatestVersionDateView.Rows[m].Cells[2].Value = tempList[m].Item2;
                LatestVersionDateView.Rows[m].Cells[3].Value = tempList[m].Item3; 
            }
            this.LatestVersionDateView.ReadOnly = true;
        }

        private TreeNode GetNodeByPath(string[] path, TreeView treeView)
        {
            TreeNode node = new TreeNode();
            string[] pathLevel = path;//.Split('\\');

            int i = 0;
            foreach (TreeNode topNode in treeView.Nodes)
            {
                if (topNode.Text == pathLevel[i].ToString())
                {
                    node = topNode;
                    i++;
                    break;
                }
            }

            if (i < pathLevel.Length)
            {
                node = GetSubNode(node, pathLevel, i);
            }


            return node;
        }

        private TreeNode GetSubNode(TreeNode node, string[] pathLevel, int i)
        {
            TreeNode newNode = new TreeNode();
            foreach (TreeNode subNode in node.Nodes)
            {
                if (subNode.Text == pathLevel[i].ToString())
                {
                    newNode = subNode;
                    i++;
                    if (i == pathLevel.Length)
                    {
                        break;
                    }

                    if (i < pathLevel.Length)
                    {
                        newNode = GetSubNode(newNode, pathLevel, i);
                    }
                }
            }
            return newNode;

        }

        //从backup文件夹中复制文件到Inspections文件夹中
        private void TBN_RollBackToLatestInsp_Click(object sender, EventArgs e)
        {
            //List<Tuple<string, int, string, bool, bool>> curVersionList = new List<Tuple<string, int, string, bool, bool>>();
           // MdiDocManager.VersionOutTo(ref curVersionList);
            
            RollBackFilePath selectFilePath = new RollBackFilePath();
            selectFilePath._CurrentUseRootPath = null;//Inspections

            string selectedItem = string.Empty;
            for (int i = 0; i < ChkListBox_CandidateInspFiles.Items.Count; i++)
            {
                if (ChkListBox_CandidateInspFiles.GetItemChecked(i))
                {
                    selectedItem = ChkListBox_CandidateInspFiles.Items[i].ToString();
                }
            }
            selectFilePath._FileName = "Inspection_" + (this.ComBox_Inspections.SelectedIndex + 1).ToString() + "_" + selectedItem;//Inspection_1_Ver3.3.0.1
            selectFilePath._BackupRootPath = Application.StartupPath + "\\" + "Change Record";
            selectFilePath._BranchName = this.ComBox_Branchs.SelectedItem.ToString();
            selectFilePath._InspectionName = this.ComBox_Inspections.SelectedItem.ToString();//eg.T1_Acq...
            selectFilePath._inspectionIndex = this.ComBox_Inspections.SelectedIndex + 1;//Inspection index(1,2...)
            selectFilePath.IsBackuped = false;
            selectFilePath._FileVersion = selectedItem;//Version3.3.0.1

            RollBackFilePath targetFilePath = new RollBackFilePath();
            targetFilePath._CurrentUseRootPath = Application.StartupPath + "\\" + "Inspections";
            selectedItem = string.Empty;
            int TargetSelectedIndex = -1;
            for (int i = 0; i < ChkListBox_LatestInspFiles.Items.Count; i++)
            {
                if (ChkListBox_LatestInspFiles.GetItemChecked(i))
                {
                    selectedItem = ChkListBox_LatestInspFiles.Items[i].ToString();
                    TargetSelectedIndex = i;
                }
            }
            if (TargetSelectedIndex == -1)
                return;//如果target中没有选项被选中，则返回。
            targetFilePath._FileName = "Inspection_" + (TargetSelectedIndex + 1).ToString();//Inspection_1
            targetFilePath._BackupRootPath = null;
            targetFilePath._BranchName = null;
            targetFilePath._InspectionName = selectedItem;//eg.T1_Acq...
            targetFilePath._inspectionIndex = TargetSelectedIndex + 1;//Inspection index(1,2...)
            targetFilePath.IsBackuped = false;
            targetFilePath._FileVersion = null;

            //提醒用户，准备备份Inspection文件。
            //将读取当前vpp存在的版本信息，list到editversion的form界面上去。
            string currBranch = ComBox_Branchs.Items[ComBox_Branchs.Items.Count - 1].ToString().ToString();
            string secNode = FormJ.inspectionNodeName;
            string inspName = selectedItem.ToString() + ".rtf";
            string[] path = new string[] { currBranch, secNode, inspName };
            this.DocTree.SelectedNode = GetNodeByPath(path, this.DocTree);
            OperInspectionItem(this.DocTree.SelectedNode);//模拟操作双击rtf文件节点
            List<Tuple<string, int, string, bool, bool>> curVersionList = new List<Tuple<string, int, string, bool, bool>>();
            MdiDocManager.VersionOutTo(ref curVersionList);
            EditVersion edit = new EditVersion(this, curVersionList);//编辑添加新的版本信息。
            edit.transPara(this.DocTree, OpmlFile, SelectedNodeInformation._branchName, SelectedNodeInformation._vppName, SelectedNodeInformation._inspIndex, null);

            //EditVersion edit = new EditVersion(this,curVersionList);//编辑添加新的版本信息。 
            switch (edit.ShowDialog())
            {
                case DialogResult.Yes://如果确定增加一个版本号,则将inspections中的文件以最新的版本号进行备份；
                    string backVersion = "Version" + edit.VersionEditBox.ipBox.IpAddressString;
                    bool IsChecked = true;
                    bool IsBackuped = edit.ChkBox_BackupNewVersionInsp.Checked;
                    RollBackFilePath tempselectFilePath = new RollBackFilePath();
                    RollBackFilePath temptargetFilePath = new RollBackFilePath();
                    tempselectFilePath = targetFilePath ;
                    temptargetFilePath = selectFilePath;
                    temptargetFilePath._BranchName = currBranch;
                    temptargetFilePath._FileVersion = backVersion;//非常重要，需要重新定义版本号。
                    temptargetFilePath._InspectionName = selectedItem;//eg.T1_Acq
                    temptargetFilePath._FileName = targetFilePath._FileName+"_"+ backVersion;
                    temptargetFilePath._inspectionIndex = targetFilePath._inspectionIndex;
                    rollback.CopyToFile(tempselectFilePath, temptargetFilePath, 1);

                    opxml.WriteXML(OpmlFile, "Rollback", currBranch, inspectionNodeName, selectedItem, backVersion, IsChecked + "_" + IsBackuped);//这里不需要删除XML中已经存在的项，如果XML里含有的话，则会直接更新。
                    UpdateRollbackInfo(OpmlFile);
                     
                    this.MsgBox.richTextBox.AppendText("When Rollback the inspection in the latest branch,backup the Inspection" +
                        "_" + (this.ComBox_Inspections.SelectedIndex + 1).ToString());
                    break;
                case DialogResult.Cancel://cancel时，则不进行备份操作。
                    break;
            }
            
            rollback.CopyToFile(selectFilePath, targetFilePath, 2);
            m_framework.ReLoadInspectionsWithGUI();//重新加载Framework下的所有inspections。
            MsgBox.Text = "Rollback Success! ";
        }
        //从rollback文件夹中复制需要的文件到inspections文件夹中
        private void TBN_Rollback_Click(object sender, EventArgs e)
        {
            //List<Tuple<string, int, string, bool, bool>> curVersionList = new List<Tuple<string, int, string, bool, bool>>();
            //MdiDocManager.VersionOutTo(ref curVersionList);
            
            RollBackFilePath selectFilePath = new RollBackFilePath();
            selectFilePath._CurrentUseRootPath = null;//Inspections
            string selectedItem = string.Empty;
            for (int i = 0; i < ChkListBox_CandidateInspFiles.Items.Count; i++)//目前checkListBox中只显示已经backup的程序版本，对于只有版本号但没有backup的文件不作显示。
            {
                if (ChkListBox_CandidateInspFiles.GetItemChecked(i))
                {
                    selectedItem = ChkListBox_CandidateInspFiles.Items[i].ToString();
                }
            }
            selectFilePath._FileName = "Inspection_" + (this.ComBox_Inspections.SelectedIndex+1).ToString()+"_"+ selectedItem;//Inspection_1_Ver3.3.0.1
            selectFilePath._BackupRootPath = Application.StartupPath + "\\" + "Change Record"; 
            selectFilePath._BranchName = this.ComBox_Branchs.SelectedItem.ToString();
            selectFilePath._InspectionName = this.ComBox_Inspections .SelectedItem.ToString();//eg.T1_Acq...
            selectFilePath._inspectionIndex = this.ComBox_Inspections.SelectedIndex + 1;//Inspection index(1,2...)
            selectFilePath.IsBackuped = false;
            selectFilePath._FileVersion = selectedItem;//Version3.3.0.1

            RollBackFilePath targetFilePath = new RollBackFilePath();
            targetFilePath._CurrentUseRootPath = Application.StartupPath + "\\"+"Inspections";
            targetFilePath._FileName = "Inspection_" + (this.ComBox_Inspections.SelectedIndex + 1).ToString();//Inspection_1
            targetFilePath._BackupRootPath = null;
            targetFilePath._BranchName = null;
            targetFilePath._InspectionName = this.ComBox_Inspections.SelectedItem.ToString();//eg.T1_Acq...
            targetFilePath._inspectionIndex = this.ComBox_Inspections.SelectedIndex + 1;//Inspection index(1,2...)
            targetFilePath.IsBackuped = false;
            targetFilePath._FileVersion = null;
            //appendtext

            //提醒用户，准备备份Inspection文件。
            //将读取当前vpp存在的版本信息，list到editversion的form界面上去。
            List<Tuple<string, int, string, bool, bool>> curVersionList = new List<Tuple<string, int, string, bool, bool>>();
            MdiDocManager.VersionOutTo(ref curVersionList);
            EditVersion edit = new EditVersion(this,curVersionList);//编辑添加新的版本信息。
            
            edit.transPara(this.DocTree, OpmlFile, SelectedNodeInformation._branchName, SelectedNodeInformation._vppName, SelectedNodeInformation._inspIndex, null);
            switch (edit.ShowDialog())
            {
                case DialogResult.Yes://如果确定增加一个版本号,则将inspections中的文件以最新的版本号进行备份；
                    string backVersion = "Version" + edit.VersionEditBox.ipBox.IpAddressString;
                    bool IsChecked = false;//当前刚备份的版本是不会被选择的。被选择的是待rollback的版本。
                    bool IsBackuped = edit.ChkBox_BackupNewVersionInsp.Checked;
                    RollBackFilePath tempselectFilePath = new RollBackFilePath();
                    RollBackFilePath temptargetFilePath = new RollBackFilePath();
                    tempselectFilePath = targetFilePath;
                    temptargetFilePath = selectFilePath;
                    temptargetFilePath._BranchName = ComBox_Branchs.Items[ComBox_Branchs.Items.Count - 1].ToString();// this.ComBox_Branchs.SelectedItem.ToString(); 
                    //temptargetFilePath._InspectionName = selectedItem;
                    temptargetFilePath._FileVersion = backVersion;//非常重要，需要重新定义版本号。
                   
                    temptargetFilePath._FileName = targetFilePath._FileName + "_" + backVersion;
                    temptargetFilePath._inspectionIndex = targetFilePath._inspectionIndex;
                    //selectFilePath._FileVersion = backVersion;
                    rollback.CopyToFile(tempselectFilePath, temptargetFilePath, 1);//备份当前程序
                    opxml.WriteXML(OpmlFile, "Rollback", temptargetFilePath._BranchName, inspectionNodeName, temptargetFilePath._InspectionName, backVersion, IsChecked + "_" + IsBackuped);//这里不需要删除XML中已经存在的项，如果XML里含有的话，则会直接更新。
                    UpdateRollbackInfo(OpmlFile);
                    // rollback.CopyToFile(targetFilePath,selectFilePath, 1);

                    this.MsgBox.richTextBox.AppendText("When Rollback the inspection in the latest branch,backup the Inspection" +
                        "_" + (this.ComBox_Inspections.SelectedIndex + 1).ToString());//后续考虑增加log记录功能。
                    break;
                case DialogResult.Cancel://cancel时，则不进行备份操作。
                    return;
                    //break;
            }

            if (selectedItem == string.Empty)
            {
                MsgBox.Text = "Rollback Failure!Please Check the selected Item!";
                return;
            }
            //当需要rollback时，对于当前的程序需要判断是否需要备份，如果已经备份了，直接rollback，不必弹出多余的对话框让用户判断。
            rollback.CopyToFile(selectFilePath, targetFilePath, 2);//将backup文件夹中的文件拷贝到Inspections文件夹中；
            m_framework.ReLoadInspectionsWithGUI();
            MsgBox.Text = "Rollback Success! ";//将执行的动作信息赋予RTF editor并记录。
        }
        
        //control the inspection version info
        private void menuItem7_Click(object sender, EventArgs e)
        {
            string oldPath = this.DocTree.SelectedNode.FullPath.ToString();
            int inspection_Index= this.DocTree.SelectedNode.Index+1;//Index 从1开始
            string xpathstr = oldPath.Replace("\\", "/");
            string[] SARRAY = System.Text.RegularExpressions.Regex.Split(xpathstr, "/");
            string branchName = SARRAY[0];
            CreateVersion version = new CreateVersion(this);
            string vppName = this.DocTree.SelectedNode.Text.ToString();
            string sExtName = Path.GetExtension(vppName);
            // 如果有写扩展名 则删除
            if (sExtName.Length != 0)
            {
                vppName = vppName.Remove(vppName.Length - sExtName.Length, sExtName.Length);
            }
            version.transPara(this.DocTree,OpmlFile, branchName, vppName, inspection_Index, null);
            version.Show();
        }

        private void richTextBoxExtended1_RichTextBox_TextChanged(object sender, EventArgs e)
        {
            if (this.MdiDocManager.SetCurTabChangedFlag())//在此处增加RTF content被修改时，需要执行的代码。
            {
                this.MdiTabs.Invalidate();
                
            }
        }
        
        private void ChkListBox_CandidateInspFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = "SelectedIndexChanged";
        }

        private void ChkListBox_CandidateInspFiles_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string currBranch = this.ComBox_Branchs.SelectedItem.ToString();
            string secNode = FormJ.inspectionNodeName;
            string inspName = this.ComBox_Inspections.SelectedItem.ToString() + ".rtf";
            string[] path = new string[] { currBranch, secNode, inspName };
            this.DocTree.SelectedNode = GetNodeByPath(path, this.DocTree);
            OperInspectionItem(this.DocTree.SelectedNode);//模拟操作双击rtf文件节点
            if (e.CurrentValue == CheckState.Checked) return;//取消选中就不用进行以下操作
            for (int i = 0; i < ((CheckedListBox)sender).Items.Count; i++)
            {
                if (e.CurrentValue == CheckState.Indeterminate)//如果之前是不能被操作状态，则继续保持不能被操作状态。
                {
                    e.NewValue = CheckState.Indeterminate;
                    continue;
                }
                ((CheckedListBox)sender).SetItemChecked(i, false);//将所有选项设为不选中
            }
            e.NewValue = CheckState.Checked;//刷新
            CheckedListBox chkBox = (CheckedListBox)sender;
            for(int i=0;i< ChkListBox_CandidateInspFiles.Items.Count;i++)
            {
                string checkedItem = ChkListBox_CandidateInspFiles.Items[e.Index].ToString(); //chkBox.Text;
                if(ChkListBox_CandidateInspFiles.Items[i].ToString()== checkedItem)
                {
                    opxml.WriteXML(OpmlFile, "Rollback", currBranch, secNode, this.ComBox_Inspections.SelectedItem.ToString(), ChkListBox_CandidateInspFiles.Items[i].ToString(), true.ToString() + "_" + true.ToString());//这里不需要删除XML中已经存在的项，如果XML里含有的话，则会直接更新。

                }
                else
                {
                    opxml.WriteXML(OpmlFile, "Rollback", currBranch, secNode, this.ComBox_Inspections.SelectedItem.ToString(), ChkListBox_CandidateInspFiles.Items[i].ToString(), false.ToString() + "_" + true.ToString());//这里不需要删除XML中已经存在的项，如果XML里含有的话，则会直接更新。

                }
            }
           
         }

        private void ChkListBox_LatestInspFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void ChkListBox_LatestInspFiles_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.CurrentValue == CheckState.Checked) return;//取消选中就不用进行以下操作
            for (int i = 0; i < ((CheckedListBox)sender).Items.Count; i++)
            {
                ((CheckedListBox)sender).SetItemChecked(i, false);//将所有选项设为不选中
            }
            e.NewValue = CheckState.Checked;//刷新
        }

        private void SplitGenericPane_Paint(object sender, PaintEventArgs e)
        {
            Graphics dc = e.Graphics;
            if (this.SplitGenericPaneState != WndState.Docking)
            {
                dc.DrawLine(this.FramePen, SplitGenericPane.Size.Width - 2, 0,
                    SplitGenericPane.Size.Width - 2, SplitGenericPane.Panel1.Size.Height);
                dc.DrawLine(this.BlackPen, SplitGenericPane.Size.Width - 1, 0,
                    SplitGenericPane.Size.Width - 1, SplitGenericPane.Panel1.Size.Height);
                if (this.DocTree.Focused)
                {
                    dc.FillRectangle(Brushes.Blue, 2, 2, this.SplitGenericPane.Size.Width - 6, 38);
                    dc.DrawImageUnscaled(this.ImgNailFocusOut, this.SplitGenericPane.Size.Width - 46, 2);
                    // 标题
                    dc.DrawString("MY TEXT", this.TabFont, Brushes.White, 5, 5);
                }
                else
                {
                    dc.DrawImageUnscaled(this.ImgNailUnFocusOut, this.SplitGenericPane.Size.Width - 46, 2);
                    dc.DrawRectangle(this.FramePen, 2, 2, this.SplitGenericPane.Size.Width - 6, 18);
                    // 标题
                    dc.DrawString("MY TEXT", this.TabFont, Brushes.Black, 5, 5);
                }
            }
            else  // Docking 
            {
                if (this.DocTree.Focused)
                {
                    dc.FillRectangle(Brushes.Blue, 2, 2, this.SplitGenericPane.Size.Width - 3, 38);
                    dc.DrawImageUnscaled(this.ImgNailFocusIn, this.SplitGenericPane.Size.Width - 47, 2);
                    // 标题
                    dc.DrawString("MY TEXT", this.TabFont, Brushes.White, 5, 5);
                }
                else
                {
                    dc.DrawImageUnscaled(this.ImgNailUnFocusIn, this.SplitGenericPane.Size.Width - 47, 2);
                    dc.DrawRectangle(this.FramePen, 2, 2, this.SplitGenericPane.Size.Width - 3, 18);
                    // 标题
                    dc.DrawString("MY TEXT", this.TabFont, Brushes.Black, 5, 5);
                }
            }

            // 实现按钮阴影效果
            switch (this.SplitGenericPaneMouseState)
            {
                case WndMouseState.MouseOnX:
                    dc.DrawLine(Pens.White, this.SplitGenericPane.Size.Width - 10, 5,
                        this.SplitGenericPane.Size.Width - 22, 5);
                    dc.DrawLine(Pens.White, this.SplitGenericPane.Size.Width - 22, 16,
                        this.SplitGenericPane.Size.Width - 22, 5);
                    dc.DrawLine(Pens.Black, this.SplitGenericPane.Size.Width - 9, 5,
                        this.SplitGenericPane.Size.Width - 9, 18);
                    dc.DrawLine(Pens.Black, this.SplitGenericPane.Size.Width - 9, 18,
                        this.SplitGenericPane.Size.Width - 22, 18);
                    break;
                case WndMouseState.MouseOnNail:
                    dc.DrawLine(Pens.White, this.SplitGenericPane.Size.Width - 30, 5,
                        this.SplitGenericPane.Size.Width - 42, 5);
                    dc.DrawLine(Pens.White, this.SplitGenericPane.Size.Width - 42, 16,
                        this.SplitGenericPane.Size.Width - 42, 5);
                    dc.DrawLine(Pens.Black, this.SplitGenericPane.Size.Width - 29, 5,
                        this.SplitGenericPane.Size.Width - 29, 18);
                    dc.DrawLine(Pens.Black, this.SplitGenericPane.Size.Width - 29, 18,
                        this.SplitGenericPane.Size.Width - 42, 18);
                    break;

                case WndMouseState.LButtonDownX:
                    dc.DrawLine(Pens.Black, this.SplitGenericPane.Size.Width - 10, 5,
                        this.SplitGenericPane.Size.Width - 22, 5);
                    dc.DrawLine(Pens.Black, this.SplitGenericPane.Size.Width - 22, 16,
                        this.SplitGenericPane.Size.Width - 22, 5);
                    dc.DrawLine(Pens.White, this.SplitGenericPane.Size.Width - 9, 5,
                        this.SplitGenericPane.Size.Width - 9, 18);
                    dc.DrawLine(Pens.White, this.SplitGenericPane.Size.Width - 9, 18,
                        this.SplitGenericPane.Size.Width - 22, 18);

                    break;
                case WndMouseState.LButtonDownNail:
                    dc.DrawLine(Pens.Black, this.SplitGenericPane.Size.Width - 30, 5,
                        this.SplitGenericPane.Size.Width - 42, 5);
                    dc.DrawLine(Pens.Black, this.SplitGenericPane.Size.Width - 42, 16,
                        this.SplitGenericPane.Size.Width - 42, 5);
                    dc.DrawLine(Pens.White, this.SplitGenericPane.Size.Width - 29, 5,
                        this.SplitGenericPane.Size.Width - 29, 18);
                    dc.DrawLine(Pens.White, this.SplitGenericPane.Size.Width - 29, 18,
                        this.SplitGenericPane.Size.Width - 42, 18);
                    break;

            }
        }

        //更新branch中的名称，与INI文件中一致。
        private void DTMenuUpdateName_Click(object sender, EventArgs e)
        {
            //DocTree_NodeMouseClick(sender, (TreeNodeMouseClickEventArgs)e);

            List<string> newBranchVppNodeNameList = new List<string>();
            int vppNumber = 0;
            string tempNamestr = null;
            //  FrameworkConfiguration.gOnly.ReadGeneralSetting("InspectionSetup", "NumOfToolBlock", 0, out vppNumber);
            string vppNum = INIOperationClass.INIGetStringValue(ParameterFile, "InspectionSetup", "NumOfToolBlock", null);
            vppNumber = int.Parse(vppNum);
           // string vppNum = INIOperationClass.INIGetStringValue(Application.StartupPath + "\\" + ParameterFile, "InspectionSetup", "NumOfToolBlock", null);
            vppNumber = int.Parse(vppNum);

            FileTreeNode inspectionTempNode;
            for (int i = 0; i < vppNumber; i++)
            {
                string nameKey = "TB" + (i + 1).ToString() + "Name"; 
                tempNamestr = INIOperationClass.INIGetStringValue(ParameterFile, "InspectionSetup", nameKey, null);
                newBranchVppNodeNameList.Add(tempNamestr);//获取INI文件中的各个inspection的最新名称；

            } 

            if (((FileTreeNode)this.DocTree.SelectedNode).NoteStyle == TreeNodeStyle.File) 
            {
                if(this.DocTree .SelectedNode.Parent.Text=="VPP_ChangeList")//确定所选的节点是inspection节点
                {
                    string curNodeName = this.DocTree.SelectedNode.Text.ToString();
                    int index = this.DocTree.SelectedNode.Index + 1;
                    string sExtName = Path.GetExtension(this.DocTree.SelectedNode.Text.ToString());

                    if (curNodeName.Remove(curNodeName.Length - sExtName.Length, sExtName.Length)!= newBranchVppNodeNameList[index-1])
                    {
                        ProcessUpdateName((FileTreeNode)this.DocTree.SelectedNode, newBranchVppNodeNameList[index - 1] + sExtName);
                    }
                }
            }

        }

        private void ProcessUpdateName(FileTreeNode node, string newName)
        {
            FileTreeNode fnode = node;//获取旧节点
            string sNewName = newName; // 新名字
            // 新扩展名
            string sExtName = "";

            // 如果新名是空串
            if (Path.GetFileNameWithoutExtension(sNewName).Length == 0)
                return;           
            try
            {
                if (fnode.NoteStyle == TreeNodeStyle.File)
                {
                    sExtName = Path.GetExtension(sNewName);
                    // 如果没写扩展名 则添加
                    if (sExtName.Length == 0)
                    {
                        sNewName += Path.GetExtension(fnode.Text);
                    }
                    // 如果用户更改了扩展名 这不允许
                    else if (sExtName != Path.GetExtension(fnode.Text))
                    {
                        MessageBox.Show("对不起 您不能更改扩展名！", "Rename Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            // 如果新名中有非法的 字符
            catch (ArgumentException ae)
            {
                MessageBox.Show(ae.Message, "Rename Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try //执行添加文件或修改名称后的操作。
            {
                string old = this.DocTree.SelectedNode.FullPath.ToString();
                if (fnode.NoteStyle == TreeNodeStyle.File)
                    this.ReNameFile(Application.StartupPath + "\\" + "Change Record" + "\\" + GetRtfORRollbackPath(fnode.FullPath, "RTF"),
                      Application.StartupPath + "\\" + "Change Record" + "\\" + GetRtfORRollbackPath(fnode.Parent.FullPath, "RTF") + "\\" + sNewName);
                else
                    this.ReNameFolder(Application.StartupPath + "\\" + "Change Record" + "\\" + GetRtfORRollbackPath(fnode.FullPath, "RTF"),
                     Application.StartupPath + "\\" + "Change Record" + "\\" + GetRtfORRollbackPath(fnode.Parent.FullPath, "RTF") + "\\" + sNewName);
                OperateNewNodeNameToXML(OpmlFile, old, "body", "Rename", sNewName, null);//修改子节点的名字
                OperateNewNodeNameToXML(OpmlFile, old, "Rollback", "Rename", sNewName.Remove(sNewName.Length - sExtName.Length, sExtName.Length), null);//修改子节点的名字

                this.ReNameFolder(Application.StartupPath + "\\" + "Change Record" + "\\" + GetRtfORRollbackPath((fnode.FullPath).Remove(fnode.FullPath.Length - sExtName.Length, sExtName.Length), "Backup"),
                     Application.StartupPath + "\\" + "Change Record" + "\\" + GetRtfORRollbackPath(fnode.Parent.FullPath, "Backup") + "\\" + sNewName.Remove(sNewName.Length - sExtName.Length, sExtName.Length));
                //增加了backup的文件夹，需要考虑修改名称后，将backup文件夹内的名称对应修改。

            }
            catch (IOException ioe)
            {
                MessageBox.Show(ioe.Message, "Rename Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            fnode.Text = sNewName;
            this.MdiTabs.Invalidate();
        }

        private void DocTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //if (e.Node.IsSelected)
            //{
            //    e.Node.Text = "XXXXX";
            //}
        }

        private void RTFEditorExtend_Load(object sender, EventArgs e)
        {

        }

        private void ComBox_Inspections_Click(object sender, EventArgs e)
        {
            string str = "Run Here!";
            stopMonitorInspeCombox = true; //允许执行chang操作。
        }

        private void TabsMenuVersion_Click(object sender, EventArgs e)
        { 
            if(this.DocTree.SelectedNode.Text.Contains("Abstract")|| this.DocTree.SelectedNode.Text.Contains("Proposal"))
            {
                return;//如果不是inspections的rtf，则直接返回，不进行版本管控。
            }
            List<Tuple<string, int, string, bool, bool>> curVersionList = new List<Tuple<string, int, string, bool, bool>>();
            MdiDocManager.VersionOutTo(ref curVersionList);
            EditVersion edit = new EditVersion(this,curVersionList);//编辑添加新的版本信息。
            edit.transPara(this.DocTree, OpmlFile, SelectedNodeInformation._branchName, SelectedNodeInformation._vppName, SelectedNodeInformation._inspIndex, null);    
            CreateVersion versionDlg = new CreateVersion(this);
            edit.ChkBox_BackupNewVersionInsp.Checked = false;//当从编辑文本处添加新的版本信息时，首先默认该版本不需要保存，用户可以选择是否真的需要保存。
            switch (edit.ShowDialog())
            {
                case DialogResult.Yes:
                    edit.BTNSave.Text = "Close";//修改Save按钮为“Close”
                    break;
                case DialogResult.OK:
                    break;
                case DialogResult.Cancel:
                    break;

            }
            this.MsgBox.userName = "AdministratorName";//string userName = VS.Utility.AccessControl.gOnly.CurrentUserName;
            this.MsgBox.strVersion = SelectedNodeInformation._lastVersion;
            this.MsgBox.SaveToRTF();
            // if(MdiDocManager.Tab)
            MdiDocManager.ContentTakeIn(this.MsgBox.getRichTextBoxContent());
            this.MdiDocManager.SaveCurTab();//在添加完毕签名后再做保存操作
            this.MdiTabs.Invalidate();

        }

        private void TabsMenu_Popup(object sender, EventArgs e)
        {

        }
    }// Class FormJ
}// namespace CS_Note1
