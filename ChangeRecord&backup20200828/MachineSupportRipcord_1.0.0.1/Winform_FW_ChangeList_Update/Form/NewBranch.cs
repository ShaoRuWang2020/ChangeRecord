using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cognex.VS.Utility;

namespace CS_Note1
{
    public partial class NewBranch : Form
    {
        FormJ formj = new FormJ();
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

        FormJ _parentForm;
        public NewBranch()
        {
            InitializeComponent();
        }

        public NewBranch(FormJ parentForm)
        {
            InitializeComponent();

            _parentForm = parentForm;
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


        private void button1_Click(object sender, EventArgs e)
        {
            bool IsNameQualify = true;
           
            for (int i = 0; i < _parentForm.branchNameList.Count; i++)
            {
                if (_parentForm.branchNameList[i] == NameEdit.Text || NameEdit.Text == "")
                { 
                    IsNameQualify = false;
                    break;
                }
            }
            if(IsNameQualify)
            {
                _parentForm.CurTab_Closing();//关闭当前的tab，在关闭前进行保存。
                _parentForm.AddNewBranch(NameEdit.Text);
                this.DialogResult = DialogResult.Yes;
                //this.Dispose();
            }              
            else
            {
                //因为主窗体的dialogue的属性没有设置为OK，所以设置的对话框不关闭！
                MessageBox.Show("Please Check the enter name!");
                NameEdit.Focus();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void NewBranch_FormClosed(object sender, FormClosedEventArgs e)
        {
            //this.Dispose();
        }

        private void NewBranch_Load(object sender, EventArgs e)
        {

        }
    }
}
