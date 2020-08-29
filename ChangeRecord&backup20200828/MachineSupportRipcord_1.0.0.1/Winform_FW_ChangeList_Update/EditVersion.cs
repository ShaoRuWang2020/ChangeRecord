using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CS_Note1
{
    public partial class EditVersion : Form
    {
        List<Tuple<string, int, string, bool, bool>> _versionList = new List<Tuple<string, int, string, bool, bool>>();
        FormJ _formj;
        OpXMLFile opxml = new OpXMLFile();
        RollBackProcess rollback = new RollBackProcess();
        TreeView _docTree;
        private string xmlfilepath;
        string _newversionNam;
        string _XmlSecNodeName;
        string _inspFold;
        string _backupFold;
        string _branchName;
        string _vppName;
        int _vppIndex;
        bool[] IsCurrVersionBackuped;
        string _lastTabVersion;
        public MdiTextInBox MdiDocManager;
        public EditVersion()
        {
            InitializeComponent();
        }
        public EditVersion(FormJ formj,List<Tuple<string, int, string, bool, bool>> _VersionInfoList)
        {
            InitializeComponent();
            _formj = formj;
            MdiDocManager = formj.MdiDocManager;
            _versionList = _VersionInfoList;
        }

        public void transPara(TreeView docTree, string file, string branchName, string title, int index, string Url)
        {
            _docTree = docTree;
            xmlfilepath = file;
            _branchName = branchName;
            _vppName = title;
            _vppIndex = index;

            return;
        }

        private void EditVersion_Load(object sender, EventArgs e)
        {
            xmlfilepath = _formj.OpmlFile;
            _XmlSecNodeName = FormJ.inspectionNodeName;//常量需要用类名来引入
            _inspFold = _formj.InspFold;
            _backupFold = _formj.ChangeAndRollbackFold;
            this.GridView_VersionList.DataSource = null;//清空当前datagridview

            DataGridViewTextBoxColumn IndexCol = new DataGridViewTextBoxColumn();
            IndexCol.HeaderText = "Index";
            GridView_VersionList.Columns.Add(IndexCol);
            DataGridViewTextBoxColumn VersionCol = new DataGridViewTextBoxColumn();
            VersionCol.HeaderText = "Version Name";
            GridView_VersionList.Columns.Add(VersionCol);
            DataGridViewColumn myCol = new DataGridViewCheckBoxColumn();
            myCol.HeaderText = "Checked";
            GridView_VersionList.Columns.Add(myCol);
            DataGridViewColumn Isbackup = new DataGridViewCheckBoxColumn();
            Isbackup.HeaderText = "Backuped";
            GridView_VersionList.Columns.Add(Isbackup);
            if (_versionList.Count > 0)
                this.groupBox1.Text = _versionList[0].Item1.ToString()+"_Current VersionList:";

            this.GridView_VersionList.DataSource = null;//清空当前datagridview
            if (GridView_VersionList.ReadOnly ==true)
            {
                GridView_VersionList.ReadOnly = false;
            }
            for (int i=0;i<_versionList.Count;i++)
            {
                Tuple<string, int, string, bool, bool> temp;
                temp = _versionList[i];
                GridView_VersionList.Rows.Add();
                GridView_VersionList.Rows[i].Cells[0].Value =(i+1).ToString();
                GridView_VersionList.Rows[i].Cells[1].Value = temp.Item3.ToString();
                GridView_VersionList.Rows[i].Cells[2].Value = temp.Item4;
                GridView_VersionList.Rows[i].Cells[3].Value = temp.Item5;
            }
            GridView_VersionList.ReadOnly = true;

            if(_versionList.Count >0)
            {
                string lastVersion = GridView_VersionList.Rows[_versionList.Count - 1].Cells[1].Value.ToString ();
                this.LabelTag.Text = "The last version of the Inspection is:" + lastVersion+"!Do you want to use the follow name?";
                string r = Regex.Replace(lastVersion, "[A-Za-z]", string.Empty);
                string[] arr = r.Trim().Split('.');
                int lastDigit = int.Parse(arr[arr.Length - 1]) + 1;//int.Parse(r[r.Length - 1].ToString())+1;

                string newVersionName = arr[0]+"."+arr[1]+"."+arr[2]+"."+ lastDigit.ToString();
                this.VersionEditBox.ipBox.IpAddress = newVersionName;
                this.VersionEditBox.ipBox.UpDateIpaddress();
            }
            else
            {
                this.LabelTag.Text = "The Inspection does not have any version, Please enter a new Name!" ;
            }            
        }

        private void BTNSave_Click(object sender, EventArgs e)
        {
            if (GridView_VersionList.ReadOnly == true)
            {
                GridView_VersionList.ReadOnly = false;
            }
            Button btn = (Button)sender;

            if (btn.Text.ToString() == "Save")
            {
                GridView_VersionList.Rows.Add();
                GridView_VersionList.Rows[GridView_VersionList.Rows.Count - 1].Cells[0].Value = GridView_VersionList.Rows.Count.ToString();
                GridView_VersionList.Rows[GridView_VersionList.Rows.Count - 1].Cells[1].Value = "Version" + this.VersionEditBox.ipBox.IpAddressString;
                for(int i=0;i<GridView_VersionList.Rows.Count;i++)
                {
                    GridView_VersionList.Rows[i].Cells[2].Value = false;//因为只考虑将最后一行的checked设置为true，所以先将之前的checked全部设置为false。
                }
                GridView_VersionList.Rows[GridView_VersionList.Rows.Count - 1].Cells[2].Value = true;// GridView_VersionList.Rows.Count.ToString();
                GridView_VersionList.Rows[GridView_VersionList.Rows.Count - 1].Cells[3].Value = this.ChkBox_BackupNewVersionInsp.Checked; //"Version" + this.VersionEditBox.ipBox.IpAddressString;
                string verName = null;
                int inspectionIndex = _vppIndex;
                string vppName = _vppName;
                bool isChecked = false;
                bool isBackuped = false;

                MdiDocManager.DeleteAllVersionInfo();
                Tuple<string, int, string, bool, bool> tuple;
                //
                for (int i = 0; i < this.GridView_VersionList.Rows.Count; i++)
                {
                    verName = this.GridView_VersionList.Rows[i].Cells[1].Value.ToString();
                    isChecked = (bool)this.GridView_VersionList.Rows[i].Cells[2].EditedFormattedValue;
                    isBackuped = (bool)this.GridView_VersionList.Rows[i].Cells[3].EditedFormattedValue;
                    tuple = new Tuple<string, int, string, bool, bool>(vppName, inspectionIndex, verName, isChecked, isBackuped);
                    MdiDocManager.AddNewVersion(tuple);//将所有的版本信息重新写入到对象的list中去。
                }

                WriteVersiontoXML(xmlfilepath, _branchName, _vppName, null);//更新XML信息，如果修改了名称，则直接修改xml中得version名称。
                _formj.UpdateRollbackInfo(xmlfilepath);//这里主要更新rollback对应得界面。 

                RollBackFilePath selectFilePath = new RollBackFilePath();
                selectFilePath._CurrentUseRootPath = _formj.AppPath + "\\" + "Inspections";
                selectFilePath._FileName = "Inspection_" + _vppIndex.ToString();
                selectFilePath._BackupRootPath = null;
                selectFilePath._BranchName = null;
                selectFilePath._InspectionName = _vppName;
                selectFilePath._inspectionIndex = _vppIndex;
                selectFilePath.IsBackuped = false;
                selectFilePath._FileVersion = _newversionNam;//最新的版本号。

                RollBackFilePath targetFilePath = new RollBackFilePath();
                targetFilePath._CurrentUseRootPath = null;
                targetFilePath._FileName = "Inspection_" + _vppIndex.ToString();
                targetFilePath._BackupRootPath = _formj.AppPath + "\\" + "Change Record";
                targetFilePath._BranchName = _branchName;
                targetFilePath._InspectionName = _vppName;
                targetFilePath._inspectionIndex = _vppIndex;
                targetFilePath.IsBackuped = false;
                targetFilePath._FileVersion = _newversionNam;//这里只考虑保存最新的版本inspection，这个add操作只考虑对最新添加得版本进行文件得backup。
                _formj.AcquireSelectedNodeInfo(_docTree.SelectedNode);//当鼠标双击打开某个rtf文件时，selectedNode被选中，但是在tab中切换时，这个node也需要update。
                if (ChkBox_BackupNewVersionInsp.Checked == true)
                    rollback.CopyToFile(selectFilePath, targetFilePath, 1);
  
                GridView_VersionList.ReadOnly = true;
                this.DialogResult = DialogResult.Yes;
            }
            else
                this.DialogResult = DialogResult.OK;
        }
        public void WriteVersiontoXML(string file, string branchName, string vppname, string Url)
        {
            string IsChecked = null;
            string IsBackuped = null;
            string versionName = null;
            string[] delArr = new string[] { branchName, _XmlSecNodeName, vppname };
            // opxml.OprationOpmlFile(file, delArr, "Rollback", "Del", null, null);// 
            opxml.WriteXMLAbortRollback(xmlfilepath, null, _branchName, _XmlSecNodeName, "Del_All", _vppName, null);
            for (int i = 0; i < this.GridView_VersionList.Rows.Count; i++)
            {
                versionName = this.GridView_VersionList.Rows[i].Cells[1].Value.ToString();
                IsChecked = this.GridView_VersionList.Rows[i].Cells[2].EditedFormattedValue.ToString();
                IsBackuped = this.GridView_VersionList.Rows[i].Cells[3].EditedFormattedValue.ToString();
                // opxml.WriteXMLAbortRollback(file, null, branchName, _XmlSecNodeName, "Del", vppname, versionName);

                opxml.WriteXML(file, "Rollback", branchName, _XmlSecNodeName, vppname, versionName, IsChecked + "_" + IsBackuped);//这里不需要删除XML中已经存在的项，如果XML里含有的话，则会直接更新。
            }
            if (this.GridView_VersionList.Rows.Count > 0)
            {
                _newversionNam = this.GridView_VersionList.Rows[GridView_VersionList.Rows.Count - 1].Cells[1].Value.ToString();//默认最后一个版本为最新版本，但需要考虑是否为选中项为最终版本。
            }
        }
        private void BTNCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void GridView_VersionList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            for (int i = 0; i < GridView_VersionList.Rows.Count; i++)
            {
                if (i != e.RowIndex && GridView_VersionList.CurrentCell.ColumnIndex == 2)
                {
                    //if (dataGridView1.CurrentCell.ReadOnly == true)
                    //    continue;
                    DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)GridView_VersionList.Rows[i].Cells[2];
                    cell.Value = false;
                }
                if (i == e.RowIndex && GridView_VersionList.CurrentCell.ColumnIndex == 2)
                {
                    DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)GridView_VersionList.Rows[i].Cells[2];
                    cell.Value = true;
                }
            }
        }
    }
}
