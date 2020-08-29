using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 
using System.Xml; 
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Cognex.VS.Utility;

namespace CS_Note1
{
    public partial class CreateVersion : Form
    {
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
        public CreateVersion()
        {
            InitializeComponent();
        }

        public CreateVersion(FormJ formj)
        {
            _formj = formj;
            InitializeComponent();
            MdiDocManager = formj.MdiDocManager; 
            if(MdiDocManager.CurTab<0)
            {
                this.BTNAddVision.Visible = false;
                this.BTNDelVersion.Visible = false;
                this.button1.Visible = false;
            }
            else
            {
                this.BTNAddVision.Visible = true;
                this.BTNDelVersion.Visible = true;
                this.button1.Visible = true;
            }
        }

        private void BTNDelVersion_Click(object sender, EventArgs e)
        {
            string SelectVerName = null;
            DataGridViewRow selectedRow = dataGridView1.SelectedCells[0].OwningRow;
            if(selectedRow==null)
            {
                return;
            }
            else
            {
                if (string.IsNullOrEmpty(Convert.ToString(selectedRow.Cells[1].Value )))
                    return;
                else
                {
                    SelectVerName = selectedRow.Cells[1].Value.ToString();
                }
            }
            string[] xpathstr = new string[] { _branchName, _XmlSecNodeName, _vppName, SelectVerName };
            opxml.WriteXMLAbortRollback(xmlfilepath, null, _branchName, _XmlSecNodeName, "Del", _vppName, SelectVerName);
            dataGridView1.Rows.Remove(selectedRow);
            _formj.UpdateRollbackInfo(xmlfilepath);
            //  toRollbackFile = TargetFilePath._BackupRootPath + "\\" + TargetFilePath._BranchName + "\\" + "Backup\\" + TargetFilePath._InspectionName + "\\" + "Inspection_" + TargetFilePath._inspectionIndex + "_" + TargetFilePath._FileVersion+".vpp";
            File.Delete(_formj.AppPath + "\\" + "Change Record" + @"\" + _branchName+"\\"+ "Backup" +"\\VPP_ChangeList\\"+ _vppName.ToString() +"\\"+ "Inspection_" + _vppIndex.ToString()+"_"+ SelectVerName.ToString ()+".vpp");
        }

        private void BTNAddVision_Click(object sender, EventArgs e)
        {
            this.button1.Visible = false;
            //将读取当前vpp存在的版本信息，list到editversion的form界面上去。
            List<Tuple<string, int, string, bool, bool>> curVersionList=new List<Tuple<string, int, string, bool, bool>>();
            MdiDocManager.VersionOutTo(ref curVersionList);
            EditVersion edit = new EditVersion(_formj,curVersionList);//编辑添加新的版本信息。
            edit.ChkBox_BackupNewVersionInsp.Checked = false;//当从编辑文本处添加新的版本信息时，首先默认该版本不需要保存，用户可以选择是否真的需要保存。
            switch(edit.ShowDialog())
            {
                case DialogResult.Yes://如果确定增加一个版本号，则在datagridview中增加一行；
                    dataGridView1.Rows.Add();
                    this.dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = dataGridView1.Rows.Count;
                    this.dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Value="Version"+ edit.VersionEditBox.ipBox.IpAddressString;
                    //if (dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].ReadOnly == true)
                    //{
                    //    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].ReadOnly = false;
                    //    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].ReadOnly = false;
                    //}
                    for (int i=0; i<dataGridView1.Rows.Count;i++)
                    {
                        DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells[2];
                        cell.Value = false;
                    }
                    this.dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].Value = true;
                    this.dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].Value = edit.ChkBox_BackupNewVersionInsp.Checked;
                    this.dataGridView1.CurrentCell = this.dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1];
                    SaveCurVersionList();//在进行add操作时，如果添加完version后，点击OK后，自动进行相关信息得保存，包括备份文件的生成。
                    //dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].ReadOnly = true;
                    //dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].ReadOnly = true;
                    break;
                case DialogResult.Cancel:
                    break;
            }
        }
        public void WriteVersiontoXML(string file, string branchName, string vppname, string Url)
        {
            string IsChecked = null;
            string IsBackuped = null;
            string versionName = null;
            string[] delArr = new string[] { branchName, _XmlSecNodeName, vppname };
            // opxml.OprationOpmlFile(file, delArr, "Rollback", "Del", null, null);// 
            opxml.WriteXMLAbortRollback(xmlfilepath, null, _branchName, _XmlSecNodeName, "Del_All", _vppName, null);
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                versionName = this.dataGridView1.Rows[i].Cells[1].Value.ToString();
                IsChecked = this.dataGridView1.Rows[i].Cells[2].EditedFormattedValue.ToString();
                IsBackuped= this.dataGridView1.Rows[i].Cells[3].EditedFormattedValue.ToString();
               // opxml.WriteXMLAbortRollback(file, null, branchName, _XmlSecNodeName, "Del", vppname, versionName);
              
                opxml.WriteXML(file, "Rollback", branchName, _XmlSecNodeName, vppname, versionName, IsChecked+"_"+IsBackuped);//这里不需要删除XML中已经存在的项，如果XML里含有的话，则会直接更新。
            }
            if (this.dataGridView1.Rows.Count > 0)
            {
                _newversionNam = this.dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells[1].Value.ToString();//默认最后一个版本为最新版本，但需要考虑是否为选中项为最终版本。
            }
        }

        private void CreateVersion_Load(object sender, EventArgs e)
        {
            xmlfilepath = _formj.OpmlFile;
            _XmlSecNodeName = FormJ.inspectionNodeName;//常量需要用类名来引入
            _inspFold = _formj.InspFold;
            _backupFold = _formj.ChangeAndRollbackFold;
            UploadVerison(xmlfilepath, _branchName, _vppName, null);
        }

        public void transPara(TreeView docTree,string file, string branchName, string title, int index,string Url)
        {
            _docTree = docTree;
            xmlfilepath = file;
            _branchName = branchName;
            _vppName = title;
            _vppIndex=index;

            return;
        }
        public void UploadVerison(string file,string branchName,string title,string Url)
        {
            try
            {
                this.Group_InspName.Text = title;
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlfilepath);

                XmlNode body = doc.SelectSingleNode("//Rollback");//找到根节点

                XmlNode inspCombobox = body.SelectSingleNode(branchName);
                XmlNode secondNode = inspCombobox.SelectSingleNode(_XmlSecNodeName);
                if (secondNode == null)
                    return;
                XmlNode checkListCandidate = secondNode.SelectSingleNode(title);
                if (checkListCandidate == null)
                    return;
                XmlNodeList condidateList = checkListCandidate.ChildNodes;
                this.dataGridView1.DataSource = null;//清空当前datagridview

                DataGridViewTextBoxColumn IndexCol = new DataGridViewTextBoxColumn();
                IndexCol.HeaderText = "Index";
                dataGridView1.Columns.Add(IndexCol);
                DataGridViewTextBoxColumn VersionCol = new DataGridViewTextBoxColumn();
                VersionCol.HeaderText = "Version Name";
                dataGridView1.Columns.Add(VersionCol);
                DataGridViewColumn myCol = new DataGridViewCheckBoxColumn();
                myCol.HeaderText = "Checked";
                dataGridView1.Columns.Add(myCol);
                DataGridViewColumn Isbackup = new DataGridViewCheckBoxColumn();
                Isbackup.HeaderText = "Backuped";
                dataGridView1.Columns.Add(Isbackup);

                IsCurrVersionBackuped = new bool[condidateList.Count];
                for (int m = 0; m < condidateList.Count; m++)
                {
                    
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[m].Cells[0].Value = (m + 1).ToString();
                    dataGridView1.Rows[m].Cells[1].Value = condidateList[m].Name;
                    bool[] statsStr = AnalyzeStrings(condidateList[m].InnerText);//0_0:false:false;0_1:false_true;1_0:true_false;1_1:true_true.
                    if(statsStr.Length>1)
                    {
                        //if(dataGridView1.Rows[m].Cells[2].ReadOnly==true)
                        //{
                        //    dataGridView1.Rows[m].Cells[2].ReadOnly = false;
                        //    dataGridView1.Rows[m].Cells[3].ReadOnly = false;
                        //}
                        dataGridView1.Rows[m].Cells[2].Value = statsStr[0];
                        dataGridView1.Rows[m].Cells[3].Value = statsStr[1];
                        IsCurrVersionBackuped[m] = statsStr[1];
                        //dataGridView1.Rows[m].Cells[2].ReadOnly = true;
                        //dataGridView1.Rows[m].Cells[3].ReadOnly = true;
                    }                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private bool[] AnalyzeStrings(string innerStr)
        {
            string[] strArr = innerStr.Trim().Split('_');
            bool[] statusArr=new bool[strArr.Length];
            for(int i=0;i<strArr.Length;i++)
            {
                statusArr[i] = bool.Parse(strArr[i]);
            }
            return statusArr;
        }
        
        //Save button
        private void button1_Click(object sender, EventArgs e)
        {
            string verName = null;
            int inspectionIndex =_vppIndex;
            string vppName = _vppName;      
            bool isChecked = false;
            bool isBackuped = false;

            MdiDocManager.DeleteAllVersionInfo();
            Tuple<string, int, string, bool, bool> tuple;
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                verName = this.dataGridView1.Rows[i].Cells[1].Value.ToString();
                isChecked =(bool) this.dataGridView1.Rows[i].Cells[2].EditedFormattedValue;
                isBackuped = (bool)this.dataGridView1.Rows[i].Cells[3].EditedFormattedValue;
                tuple = new Tuple<string, int, string, bool, bool>(vppName, inspectionIndex, verName, isChecked, isBackuped);
                MdiDocManager.AddNewVersion(tuple);//将所有的版本信息重新写入到对象的list中去。
            }
           
            WriteVersiontoXML(xmlfilepath, _branchName, _vppName, null);//更新XML信息，如果修改了名称，则直接修改xml中得version名称。
            _formj.UpdateRollbackInfo(xmlfilepath);//这里主要更新rollback对应得界面。

            //考虑备份函数，对每一个version对于的状态进行判断，如果某个version的状态需要备份，
            //再检测其名称，如果对应名称的文件已经备份，则不用再备份，但如果其名称发生变化，则进行重新备份。


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
            _formj.AcquireSelectedNodeInfo(_docTree.SelectedNode);

            DataGridViewRow selectedRow = dataGridView1.SelectedCells[0].OwningRow;
            string SelectVerName = null;
            if (selectedRow == null)
            {
                return;
            }
            else
            {
                if (string.IsNullOrEmpty(Convert.ToString(selectedRow.Cells[1].Value)))
                    return;
                else
                {
                    SelectVerName = selectedRow.Cells[1].Value.ToString();
                }
            }
            if ( SelectVerName== _newversionNam)//判断选中的是否为最后一行??selectedRow.IsNewRow ??
            {
             if((bool)selectedRow.Cells[3].EditedFormattedValue == true)
                rollback.CopyToFile(selectFilePath, targetFilePath, 1);
            }
            else
            {
                switch (MessageBox.Show("You selected row is not the new Row!Do you want to recheck?", "Backup",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes://yes 则返回重新选择
                        break;
                    case DialogResult.No://no 则准备继续判断备份当前所选的inspection
                        if ((bool)selectedRow.Cells[3].EditedFormattedValue == true)
                        {
                            targetFilePath._FileVersion = SelectVerName;
                            rollback.CopyToFile(selectFilePath, targetFilePath, 1);//考虑加入进度条
                        }
                        return;
                }
            }
           
        }

        //逻辑：当rtf界面编辑完毕后，Ctrl+S可以快速保存RTF文件，此时右键Close会直接关闭当前RTF编辑框，对应的后台不创建version以及backup文件，同时
        //保证在关闭总的close按钮时，不再提醒用户是否需要保存；
        public void SaveCurVersionList()
        {
            string verName = null;
            int inspectionIndex = _vppIndex;
            string vppName = _vppName;
            bool isChecked = false;
            bool isBackuped = false;

            MdiDocManager.DeleteAllVersionInfo();
            Tuple<string, int, string, bool, bool> tuple;
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                verName = this.dataGridView1.Rows[i].Cells[1].Value.ToString();
                isChecked = (bool)this.dataGridView1.Rows[i].Cells[2].EditedFormattedValue;
                isBackuped = (bool)this.dataGridView1.Rows[i].Cells[3].EditedFormattedValue;
                tuple = new Tuple<string, int, string, bool, bool>(vppName, inspectionIndex, verName, isChecked, isBackuped);
                MdiDocManager.AddNewVersion(tuple);//将所有的版本信息重新写入到对象的list中去。
            }

            WriteVersiontoXML(xmlfilepath, _branchName, _vppName, null);//修改version名称并在XML文件中进行更新。
            _formj.UpdateRollbackInfo(xmlfilepath);//需要考虑对应得备份文件是否修改名称。


            //考虑备份函数，对每一个version对于的状态进行判断，如果某个version的状态需要备份，
            //再检测其名称，如果对应名称的文件已经备份，则不用再备份，但如果其名称发生变化，则进行重新备份。
            RollBackFilePath selectFilePath = new RollBackFilePath();
            selectFilePath._CurrentUseRootPath = _formj.AppPath + "\\" + "Inspections";
            selectFilePath._FileName = "Inspection_" + _vppIndex.ToString();
            selectFilePath._BackupRootPath = null;
            selectFilePath._BranchName = null;
            selectFilePath._InspectionName = _vppName;
            selectFilePath._inspectionIndex = _vppIndex;
            selectFilePath.IsBackuped = false;
            selectFilePath._FileVersion = _newversionNam;

            RollBackFilePath targetFilePath = new RollBackFilePath();
            targetFilePath._CurrentUseRootPath = null;
            targetFilePath._FileName = "Inspection_" + _vppIndex.ToString();
            targetFilePath._BackupRootPath = _formj.AppPath + "\\" + "Change Record";
            targetFilePath._BranchName = _branchName;
            targetFilePath._InspectionName = _vppName;
            targetFilePath._inspectionIndex = _vppIndex;
            targetFilePath.IsBackuped = false;
            targetFilePath._FileVersion = _newversionNam;

            //这个TreeNode在双击rtf文件时会更新，在tab各个选项切换时也会更新，所有它能保证是当前操作的最真实的selectNode。
            //但需要考虑的是，如果我们打开一个show模式，但此时又操作其他的rtf选项，这个form的内容会不会实时更新？？？
            _formj.AcquireSelectedNodeInfo(_docTree.SelectedNode);
            DataGridViewRow selectedRow = dataGridView1.SelectedCells[0].OwningRow;
            string SelectVerName = null;
            if (selectedRow == null)
            {
                return;//如果没有版本信息，则不浪费时间，直接返回。
            }
            else
            {
                if (string.IsNullOrEmpty(Convert.ToString(selectedRow.Cells[1].Value)))
                    return;//如果版本信息为空，也直接返回。
                else
                {
                    SelectVerName = selectedRow.Cells[1].Value.ToString();
                }
            }
            if (SelectVerName == _newversionNam)//判断选中的是否为最后一行??selectedRow.IsNewRow ??
            {
                if ((bool)selectedRow.Cells[3].EditedFormattedValue == true)//判断当前得版本是否需要备份，检测第四列标签状态。
                    rollback.CopyToFile(selectFilePath, targetFilePath, 1);//
            }
            else
            {
                switch (MessageBox.Show("You selected row is not the new Row!Do you want to recheck?", "Backup",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes://yes 则返回重新选择
                        break;
                    case DialogResult.No://no 则准备继续判断备份当前所选的inspection
                        if ((bool)selectedRow.Cells[3].EditedFormattedValue == true)
                        {
                            targetFilePath._FileVersion = SelectVerName;
                            rollback.CopyToFile(selectFilePath, targetFilePath, 1);//考虑加入进度条
                        }
                        return;
                }
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (i != e.RowIndex && dataGridView1.CurrentCell.ColumnIndex == 2)
                {
                    //if (dataGridView1.CurrentCell.ReadOnly == true)
                    //    continue;
                    DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells[2];
                    cell.Value = false;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string str = "Cell content double click!";
        }

        //当双击某个cell中的内容时，光标停在此处准备修改对应的信息，修改完毕后，执行对应的备份文件操作。
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string str = "Cell   double click!";
            //string old = e.
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string str = "Cell value changed!";
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private string preTextContent = string.Empty;
        private string editTextContent = string.Empty;
        private void editingControl_Enter(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            preTextContent = t.Text;
        }

        private void editingControl_TextChanged(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            editTextContent = t.Text;
            if (editTextContent != preTextContent)
            {
                string str = "Compare the changed str!";
                String appPath = _formj.AppPath + "\\" + "Change Record"+"\\"+_branchName+"\\"+"Backup\\Vpp_ChangeList" ;
                string _vppname =this._vppName;
                int _index = this._vppIndex;
                string oldversionName = preTextContent;
                string newversionName = editTextContent;
                File.Move(appPath +"\\"+_vppname+"\\"+"Inspection_"+ _index .ToString()+"_"+ oldversionName+".vpp",
                    appPath + "\\" + _vppname + "\\" + "Inspection_" + _index.ToString() + "_" + newversionName + ".vpp");
                File.SetLastWriteTime(appPath + "\\" + _vppname + "\\" + "Inspection_" + _index.ToString() + "_" + newversionName + ".vpp", DateTime.Now);
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control.GetType().Equals(typeof(DataGridViewTextBoxEditingControl)))//cell为类TextBox时
            {
                e.CellStyle.BackColor = Color.FromName("window");
                DataGridViewTextBoxEditingControl editingControl = e.Control as DataGridViewTextBoxEditingControl;
                editingControl.Leave += new EventHandler(editingControl_TextChanged);
                editingControl.Enter  += new EventHandler(editingControl_Enter);
            }
        }
    }
}
