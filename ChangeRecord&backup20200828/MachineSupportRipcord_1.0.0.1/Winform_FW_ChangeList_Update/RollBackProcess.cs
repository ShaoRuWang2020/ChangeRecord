using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 
using System.ComponentModel; 
using System.Data;
using System.IO;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary; 
using System.Text.RegularExpressions;
using Cognex.VS.Utility;


namespace CS_Note1
{
    public class RollBackProcess
    {
        FormJ _parentForm;
        public RollBackProcess(FormJ parentForm)
        {
            _parentForm = parentForm;
        }
        public RollBackProcess()
        {

        }


        //SelectedFilePath: 代表被选中的、准备替换其他文件的备份文件；
        //TargetFilePath：代表最终被替换的文件。
        //copyMode 1:from inspections fold to ChangeRecord//Branch1.0//Backup
        //         2:from Backup fold to inspections fold.
        public bool CopyToFile(RollBackFilePath SelectedFilePath, RollBackFilePath TargetFilePath, int copyMode)
        {
            string Frompath = SelectedFilePath._BackupRootPath;
            string directoryPath = TargetFilePath._CurrentUseRootPath;
            try
            {
                switch (copyMode)
                {
                    case 1:
                        string toRollbackFile = null;
                        if (Directory.Exists(SelectedFilePath._CurrentUseRootPath))//判断COGNEX//Inspections文件夹是否存在
                        {
                            string vppFilePath = SelectedFilePath._CurrentUseRootPath + "\\" + SelectedFilePath._FileName + ".vpp";

                            if (!Directory.Exists(TargetFilePath._BackupRootPath))
                            {
                                Directory.CreateDirectory(TargetFilePath._BackupRootPath + "\\" + TargetFilePath._BranchName + "\\" + "Backup");
                            }
                            else
                            {
                                if (!Directory.Exists(TargetFilePath._BackupRootPath + "\\" + TargetFilePath._BranchName))
                                {
                                    Directory.CreateDirectory(TargetFilePath._BackupRootPath + "\\" + TargetFilePath._BranchName + "\\" + "Backup");
                                }
                                else
                                {
                                    if (!Directory.Exists(TargetFilePath._BackupRootPath + "\\" + TargetFilePath._BranchName + "\\" + "Backup"))
                                    {
                                        Directory.CreateDirectory(TargetFilePath._BackupRootPath + "\\" + TargetFilePath._BranchName + "\\" + "Backup"+"\\VPP_ChangeList\\" + TargetFilePath._InspectionName);
                                          toRollbackFile = TargetFilePath._BackupRootPath + "\\" + TargetFilePath._BranchName + "\\" + "Backup" + "\\VPP_ChangeList\\" + TargetFilePath._InspectionName + "\\" + "Inspection_" + TargetFilePath._inspectionIndex + "_" + TargetFilePath._FileVersion+".vpp";
                                        if (File.Exists(vppFilePath))
                                        {
                                            File.Copy(vppFilePath, toRollbackFile,true);//action copy
                                            File.SetLastWriteTime(toRollbackFile, DateTime.Now);
                                        }
                                        else
                                        {
                                            MessageBox.Show("The inspection file has not been create!");
                                        }
                                    }
                                    else
                                    {
                                        if (!Directory.Exists(TargetFilePath._BackupRootPath + "\\" + TargetFilePath._BranchName + "\\" + "Backup" + "\\VPP_ChangeList\\" + TargetFilePath._InspectionName))
                                        {
                                            Directory.CreateDirectory(TargetFilePath._BackupRootPath + "\\" + TargetFilePath._BranchName + "\\" + "Backup" + "\\VPP_ChangeList\\" + TargetFilePath._InspectionName);
                                              toRollbackFile = TargetFilePath._BackupRootPath + "\\" + TargetFilePath._BranchName + "\\" + "Backup" + "\\VPP_ChangeList\\" + TargetFilePath._InspectionName + "\\" + "Inspection_" + TargetFilePath._inspectionIndex + "_" + TargetFilePath._FileVersion+".vpp";
                                            if (File.Exists(vppFilePath))
                                            {
                                                File.Copy(vppFilePath, toRollbackFile,true);//action copy
                                                File.SetLastWriteTime(toRollbackFile, DateTime.Now);
                                            }
                                            else
                                            {
                                                MessageBox.Show("The inspection file has not been create!");
                                            }
                                        }
                                        else
                                        {
                                              toRollbackFile = TargetFilePath._BackupRootPath + "\\" + TargetFilePath._BranchName + "\\" + "Backup" + "\\VPP_ChangeList\\" + TargetFilePath._InspectionName + "\\" + "Inspection_" + TargetFilePath._inspectionIndex + "_" + TargetFilePath._FileVersion+".vpp";
                                            if (File.Exists(vppFilePath))
                                            {
                                                File.Copy(vppFilePath, toRollbackFile,true);//action copy
                                                File.SetLastWriteTime(toRollbackFile, DateTime.Now);
                                            }
                                            else
                                            {
                                                MessageBox.Show("The inspection file has not been create!");
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            MessageBox.Show("The Inspections Fold has not been created! Please check!");
                        }
                        break;
                    case 2:
                        if (Directory.Exists(TargetFilePath._CurrentUseRootPath))
                        {

                            toRollbackFile = TargetFilePath._CurrentUseRootPath + "\\" + TargetFilePath._FileName+".vpp";

                        }
                        else
                        {
                            break;
                        }


                        if (!Directory .Exists(SelectedFilePath._BackupRootPath))
                        {
                            Directory.CreateDirectory(SelectedFilePath._BackupRootPath + "\\" + SelectedFilePath._BranchName + "\\" +   "Backup" + "\\VPP_ChangeList\\" + SelectedFilePath._InspectionName);
                        }
                        else
                        {
                            if(!Directory.Exists(SelectedFilePath._BackupRootPath + "\\" + SelectedFilePath._BranchName))
                            {
                                Directory.CreateDirectory(SelectedFilePath._BackupRootPath + "\\" + SelectedFilePath._BranchName + "\\" + "Backup" + "\\VPP_ChangeList\\" + SelectedFilePath._InspectionName);

                            }
                            else
                            {
                                if(!Directory.Exists(SelectedFilePath._BackupRootPath + "\\" + SelectedFilePath._BranchName + "\\" + "Backup" + "\\VPP_ChangeList\\" + SelectedFilePath._InspectionName))
                                {
                                    Directory.CreateDirectory(SelectedFilePath._BackupRootPath + "\\" + SelectedFilePath._BranchName + "\\" + "Backup" + "\\VPP_ChangeList\\" + SelectedFilePath._InspectionName);

                                    string vppFilePath = SelectedFilePath._BackupRootPath + "\\" + SelectedFilePath._BranchName + "\\" + "Backup" + "\\VPP_ChangeList\\" + SelectedFilePath._InspectionName + "\\" + "Inspection_" + SelectedFilePath._inspectionIndex + "_" + SelectedFilePath._FileVersion + ".vpp";
                                    if (File.Exists(vppFilePath))
                                    {
                                        //File.Replace(vppFilePath, toRollbackFile, toRollbackFile + "_temp");//action copy
                                        File.Copy(vppFilePath, toRollbackFile, true);//action copy
                                        File.SetLastWriteTime(toRollbackFile, DateTime.Now);
                                    }
                                    else
                                    {
                                        MessageBox.Show("The inspection file has not been create!");
                                    }
                                }
                                else
                                {

                                    string vppFilePath = SelectedFilePath._BackupRootPath + "\\" + SelectedFilePath._BranchName + "\\" + "Backup" + "\\VPP_ChangeList\\" + SelectedFilePath._InspectionName + "\\" + "Inspection_" + SelectedFilePath._inspectionIndex + "_" + SelectedFilePath._FileVersion+".vpp";
                                    if (File.Exists(vppFilePath))
                                    {
                                        File.Copy(vppFilePath, toRollbackFile,true);//action copy
                                        File.SetLastWriteTime(toRollbackFile, DateTime.Now);
                                    }
                                    else
                                    {
                                        MessageBox.Show("The inspection file has not been create!");
                                    }
                                }
                            }
                        }

                        break;
                }

            }
            catch (DirectoryNotFoundException dirNotFound)
            {
                MessageBox.Show(dirNotFound.Message);
            }
            return true;
        }

    }
}
