using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Cognex.VS.Utility;
using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;
using Cognex.VS.Garbo;
using System.Windows.Forms;

namespace Cognex.VS.MachineSupport
{
    public class MachineSupportFactory
    {
        public static IMachineSupport Create()
        {
            return new MachineSupportPAMi();
        }
    }

    public class MachineSupportPAMi : IMachineSupport
    {
        private IFrameworkSupport m_framework;

        string partName;

        public void Prepare() { }
        public void Init(IFrameworkSupport framework)
        {
            CogGarbo.Check("AssemblyPlusSolutions");
            m_framework = framework;
            //m_framework.ReLoadInspectionsWithGUI();
            ToolStripMenuItem Item1 = new ToolStripMenuItem("CustomTool");
            ToolStripMenuItem Item2 = new ToolStripMenuItem("ChangeRecord");
            Item2.Click += (object sendor, EventArgs e) =>
            {
                CS_Note1.FormJ AutoRunImageModule = new CS_Note1.FormJ(m_framework);
                //AutoRunImageModule.ShowDialog();
                AutoRunImageModule.Show();

            };
            Item1.DropDownItems.Add(Item2);
            m_framework.AddToolStripMenuItem(Item1);
        }
        public struct Packet
        {
            public double X;
            public double Y;
            public double Angle;
        }
        public string MachineName
        {
            get { return "Ripcord"; }
        }

        public void ForceRecordFlush()
        {
        }

        public void Shutdown()
        {
            ForceRecordFlush();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

      public ProfinetUserData ProfinetJobParser(CommandAndInfo command)
        {
          return new ProfinetUserData(60);
        }

        public List<int> SimulatorCommandParserIDs
        {
            get
            {
                List<int> ids = new List<int>();

                ids.Add(1);
                ids.Add(1);
               // ids.Add(1);
                return ids;
            }
        }

        public List<string> SimulatorCommandStrings
        {
            get
            {
                List<string> strings = new List<string>();
                // run times
                strings.Add("T1,SN");
                strings.Add("T21,SN");
                strings.Add("T22,SN");
                strings.Add("T3,Train,0,0,0");
                strings.Add("T4,Train,0,0,0");
                strings.Add("T3,1,Box,0,0,0");
                strings.Add("T4,1,Box,0,0,0");
                strings.Add("T3,1,SN,0,0,0");
                strings.Add("T4,1,SN,0,0,0");
                strings.Add("T5,SN");
                // calibration
                strings.Add("T1,1");
                strings.Add("T21,1");
                strings.Add("T22,1");
                strings.Add("T5,1");
                return strings;
            }
        }

        public List<Tuple<String, Int16>> SimulatorPLCCommands
        {
            get
            {
                /*
                List<Tuple<String, Int16>> commandNumbers = new List<Tuple<String, Int16>>();
                return commandNumbers;
                */
                return null;
            }
        }

        public CogGraphicCollection GetAcquisitionSetupOverlayGraphics(int inspectionID, int inputID, int cameraID, int productID)
        {
             CogGraphicCollection overlayGraphics = new CogGraphicCollection();

            // Define the target image height and width
            int width = 2592;
            int height = 1944;

            // Draw some crosshairs through the center of the image
            CogLine vertCrosshair = new CogLine();
            vertCrosshair.SetXYRotation(width / 2, height / 2, 0);
            vertCrosshair.SelectedSpaceName = "#";
            overlayGraphics.Add(vertCrosshair);

            CogLine horizCrosshair = new CogLine();
            horizCrosshair.SetXYRotation(width / 2, height / 2, Math.PI / 2.0);
            horizCrosshair.SelectedSpaceName = "#";
            overlayGraphics.Add(horizCrosshair);

            return overlayGraphics;

        }

        public string TCPAcqArmed(CommandAndInfo command)
        {
            // If we are in async mode, send an AcqArmed response on acqarmed
            if (FrameworkConfiguration.gOnly.AllowAsyncOperation)
                return "AcqArmed";
            // Otherwise send nothing at all.
            else
                return null;
        }

        public string TCPAcqComplete(CommandAndInfo command)
        {
            // If we are in async mode, send an AcqComplete response on acqcomplete
            if (FrameworkConfiguration.gOnly.AllowAsyncOperation)
                return "AcqComplete";
            // Otherwise send nothing at all.
            else
                return null;
        }

        public string TCPJobParser(CommandAndInfo command)
        {
            String strSend = "Unknown Command";
            int JobIndex = 0;
            String strCmd = command.Command; // command
            string[] strArray = strCmd.Split(','); //array of command
            int.TryParse(strArray[1], out JobIndex);

            try
            {
                switch (strArray[0].ToUpper())
                {
                    case "T1":
                        {
                            if (strArray[1] == "1")                 //CheckBoard calibration
                            {
                                bool success = CalibrationCamera(command);
                                if (success)
                                {
                                    strSend = strArray[0] + ",1";
                                }
                                else
                                {
                                    strSend = strArray[0] + ",0";
                                }
                            }
                            else                                    //runtimes
                            {
                                strSend = Cam1Job1(command, 0);
                            }
                            
                            return strSend;
                        }
                    case "T21":
                        {
                            if (strArray[1] == "1")//CheckBoard calibration
                            {
                                bool success = CalibrationCamera(command);
                                if (success)
                                {
                                    strSend = strArray[0] + ",1";
                                }
                                else
                                {
                                    strSend = strArray[0] + ",0";
                                }
                            }
                            else
                            {
                                strSend = Cam2Job1(command, 1);
                            }
                            return strSend;
                        }
                    case "T22":
                        {
                            if (strArray[1] == "1")//CheckBoard calibration
                            {
                                bool success = CalibrationCamera(command);
                                if (success)
                                {
                                    strSend = strArray[0] + ",1";
                                }
                                else
                                {
                                    strSend = strArray[0] + ",0";
                                }
                            }
                            else
                            {
                                strSend = Cam2Job2(command, 2);
                            }
                            return strSend;
                        }
                    case "T3":
                        {
                            if (strArray[1] == "Train" || strArray[2] == "Box")
                            {
                                strSend = Cam3Job1(command, 3);
                            }
                            else
                            {
                                strSend = Cam3Job2(command, 5);
                            }
                            return strSend;
                        }
                    case "T4":
                        {
                            if (strArray[1] == "Train" || strArray[2] == "Box")
                            {
                                strSend = Cam4Job1(command, 4);
                            }
                            else
                            {
                                strSend = Cam4Job2(command, 6);
                            }
                            return strSend;
                        }
                    case "T5":
                        {
                            if (strArray[1] == "1")//CheckBoard calibration
                            {
                                 bool success = CalibrationCamera(command);
                                if (success)
                                {
                                    strSend = strArray[0] + ",1";
                                }
                                else
                                {
                                    strSend = strArray[0] + ",0";
                                }
                            }
                            else
                            {
                                strSend = Cam5Job1(command, 7);
                            }
                            
                            return strSend;
                        }
                    case "GROUP":
                        {
                            List<String> sList = new List<string>();
                            for (int i = 2; i < strArray.Length; i++)
                            {
                                sList.Add(strArray[i]);
                            }
                            String[] serials = sList.ToArray();
                            ImageSaveQueue.gOnly.SetPartName(strArray[1], serials);
                            return "GROUP,1";
                        }
                    // For any other command not caught by the case statement above, report an unknow command
                    default:
                        {
                            return "Unknown Command";
                        }
                }
            }
            catch (Exception ex)
            {
                MessageManager.gOnly.Alarm("TCP receive data error：" + strCmd, AlarmType.Unknown, ex);
            }
            return strSend;
        }
        //
        public string Cam1Job1(CommandAndInfo Command, int InspectionIndex)
        {
            bool bRunJob = false;
            String strReturn = "RunJob Error,please check inspection !";
            RunJobParameters runJobParams;
            m_framework.Calibrations[0].GetToolBlock().Inputs["strCMD"].Value = Command.Command;
            string strCMD = Command.Command;
            string[] strArray = strCMD.Split(',');          
            UInt64 partID;
            partName = strArray[0] + "_" + strArray[1] + "_" + DateTime.Now.ToString("HHmmss");
            partID = ImageSaveQueue.gOnly.NewPart(partName);
            runJobParams = new RunJobParameters(InspectionIndex, partID);
            try
            {
                m_framework.SetInspectionTerminalValue(0, new CogToolBlockTerminal("strCMD", Command.Command));
                bRunJob = m_framework.RunJob(Command, LightControlAction.AutoOnAndOff, true, runJobParams);
                if (bRunJob)
                    strReturn = (String)runJobParams.Outputs["ResultString"];
                else
                    strReturn = (String)runJobParams.Outputs["DefaultString"];
            }
            catch (Exception ex)
            {
                MessageManager.gOnly.Alarm("RunJob Error,please check inspection !", AlarmType.Unknown, ex);
            }

            return strReturn;
        }
        public string Cam2Job1(CommandAndInfo Command, int InspectionIndex)
        {
            bool bRunJob = false;
            String strReturn = "RunJob Error,please check inspection !";
            RunJobParameters runJobParams;
            //m_framework.Calibrations[1].GetToolBlock().Inputs["strCMD"].Value = Command.Command;
            CogToolBlock tbCalib = null;
            tbCalib = (CogToolBlock)m_framework.Calibrations[1  ].GetToolBlock();
            tbCalib.Inputs["StrCMD"].Value = Command.Command;
            string strCMD = Command.Command;
            string[] strArray = strCMD.Split(',');
            UInt64 partID;
            partName = strArray[0] + "_" + strArray[1] + "_" + DateTime.Now.ToString("HHmmss");
            partID = ImageSaveQueue.gOnly.NewPart(partName);
            runJobParams = new RunJobParameters(InspectionIndex, partID);
            try
            {
                bRunJob = m_framework.RunJob(Command, LightControlAction.AutoOnAndOff, true, runJobParams);
                if (bRunJob)
                    strReturn = (String)runJobParams.Outputs["ResultString"];
                else
                    strReturn = (String)runJobParams.Outputs["DefaultString"];
            }
            catch (Exception ex)
            {
                MessageManager.gOnly.Alarm("RunJob Error,please check inspection !", AlarmType.Unknown, ex);
            }
            return strReturn;
        }
        public string Cam2Job2(CommandAndInfo Command, int InspectionIndex)
        {
            bool bRunJob = false;
            String strReturn = "RunJob Error,please check inspection !";
            RunJobParameters runJobParams;
           // m_framework.Calibrations[2].GetToolBlock().Inputs["strCMD"].Value = Command.Command;
            CogToolBlock tbCalib = null;
            tbCalib = (CogToolBlock)m_framework.Calibrations[2].GetToolBlock();
            tbCalib.Inputs["StrCMD"].Value = Command.Command;
            string strCMD = Command.Command;
            string[] strArray = strCMD.Split(',');
            UInt64 partID;
            partName = strArray[0] + "_" + strArray[1] + "_" + DateTime.Now.ToString("HHmmss");
            partID = ImageSaveQueue.gOnly.NewPart(partName);
            runJobParams = new RunJobParameters(InspectionIndex, partID);
            try
            {
                bRunJob = m_framework.RunJob(Command, LightControlAction.AutoOnAndOff, true, runJobParams);
                if (bRunJob)
                    strReturn = (String)runJobParams.Outputs["ResultString"];
                else
                    strReturn = (String)runJobParams.Outputs["DefaultString"];
            }
            catch (Exception ex)
            {
                MessageManager.gOnly.Alarm("RunJob Error,please check inspection !", AlarmType.Unknown, ex);
            }
            return strReturn;
        }
        public string Cam3Job1(CommandAndInfo Command, int InspectionIndex)
        {
            bool bRunJob = false;
            String strReturn = "RunJob Error,please check inspection !";
            RunJobParameters runJobParams;
            string strCMD = Command.Command;
            string[] strArray = strCMD.Split(',');
            UInt64 partID;
            partName = strArray[0] + "_" + strArray[1] + "_" + DateTime.Now.ToString("HHmmss");
            partID = ImageSaveQueue.gOnly.NewPart(partName);
            runJobParams = new RunJobParameters(InspectionIndex, partID);
            try
            {
                if (strArray.Length >= 4)
                {

                    runJobParams.Inputs["strCMD"] = Command.Command;

                    //Run T1,2 Inspection

                    bRunJob = m_framework.RunJob(Command, LightControlAction.AutoOnAndOff, true, runJobParams);
                }

                if (bRunJob)
                {
                    strReturn = (String)runJobParams.Outputs["ResultString"];
                    m_framework.SaveInspection(InspectionIndex);
                }                
                else
                    strReturn = (String)runJobParams.Outputs["DefaultString"];

                //if (strArray.Length >= 4)
                //ImageSaveQueue.gOnly.SetPartName(partName, partName);

            }
            catch (Exception ex)
            {
                MessageManager.gOnly.Alarm("RunJob Error,please check inspection !", AlarmType.Unknown, ex);
            }

            return strReturn;
        }
        public string Cam3Job2(CommandAndInfo Command, int InspectionIndex)
        {
            bool bRunJob = false;
            String strReturn = "RunJob Error,please check inspection !";
            RunJobParameters runJobParams;
            string strCMD = Command.Command;
            string[] strArray = strCMD.Split(',');
            UInt64 partID;
            partName = strArray[0] + "_" + strArray[1] + "_" + strArray[2] + "_" + DateTime.Now.ToString("HHmmss");
            CogTransform2DLinear RobotPose = new CogTransform2DLinear();
            if (strArray.Length >= 6)
            {
                RobotPose.TranslationX = double.Parse(strArray[3]);
                RobotPose.TranslationY = double.Parse(strArray[4]);
                RobotPose.Rotation  = CogMisc.DegToRad (double.Parse(strArray[5]));
            }
            else
            {
                MessageManager.gOnly.Alarm("SI Send Command Error! Please check!");
                return strReturn;
            }
            partID = ImageSaveQueue.gOnly.NewPart(partName);
            runJobParams = new RunJobParameters(InspectionIndex, partID);
            CogTransform2DLinear robotPose = new CogTransform2DLinear();
            CogTransform2DLinear destPartpose_Station1 = new CogTransform2DLinear();
            CogTransform2DLinear destPartpose_Station2 = new CogTransform2DLinear();
            try
            {
                if (strArray.Length >= 4)
                {
                    runJobParams.Inputs["strCMD"] = Command.Command;
                    //Run T1,2 Inspection
                    //Calc Box position partDestPose
                    CogTransform2DLinear robotPose_Nozzle =(CogTransform2DLinear)m_framework.Inspections[ConstType.T3_TrainNozzleBox].GetToolBlock().Outputs["RobotPose_Nozzle"].Value ;
                    CogTransform2DLinear partPose_Nozzle = (CogTransform2DLinear)m_framework.Inspections[ConstType.T3_TrainNozzleBox].GetToolBlock().Outputs["PartPose_Nozzle"].Value;
                    CogTransform2DLinear robotDestPose1 = (CogTransform2DLinear)m_framework.Inspections[ConstType.T3_TrainNozzleBox].GetToolBlock().Outputs["DestPose_BOX1"].Value;
                    CogTransform2DLinear robotDestPose2 = (CogTransform2DLinear)m_framework.Inspections[ConstType.T3_TrainNozzleBox].GetToolBlock().Outputs["DestPose_BOX2"].Value;
                    CogLineSegment seg = (CogLineSegment)m_framework.Inspections[ConstType.T3_TrainNozzleBox].GetToolBlock().Outputs["TrainImageSegment"].Value;
                    CogCircle circle = (CogCircle)m_framework.Inspections[ConstType.T3_TrainNozzleBox].GetToolBlock().Outputs["TrainImageCircle"].Value;
                  

                    destPartpose_Station1 = robotDestPose1.Compose(robotPose_Nozzle.Invert()).Compose(partPose_Nozzle);
                    destPartpose_Station2 = robotDestPose2.Compose(robotPose_Nozzle.Invert()).Compose(partPose_Nozzle);

                    runJobParams.Inputs["destPartpose_station1"] = (CogTransform2DLinear)destPartpose_Station1;
                    runJobParams.Inputs["destPartpose_station2"] = (CogTransform2DLinear)destPartpose_Station2;
                    runJobParams.Inputs["robotPose_Nozzle"] = (CogTransform2DLinear)robotPose_Nozzle;
                    runJobParams.Inputs["partPose_Nozzle"] = (CogTransform2DLinear)partPose_Nozzle;
                    runJobParams.Inputs["TrainImageSegment"] = (CogLineSegment)seg;
                    runJobParams.Inputs["TrainImageCircle"] = (CogCircle)circle;

                    bRunJob = m_framework.RunJob(Command, LightControlAction.AutoOnAndOff, true, runJobParams);
                }

                if (bRunJob)
                {
                    CogTransform2DLinear currentPartPose = new CogTransform2DLinear();
                    //currentPartPose = (CogTransform2DLinear)runJobParams.Outputs["PartPose_Nozzle"];
                   // robotPose = CalculateRobotDestinationPose(RobotPose, currentPartPose, destPartpose);

                    strReturn = (String)runJobParams.Outputs["ResultString"];
                }
                else
                    strReturn = (String)runJobParams.Outputs["DefaultString"];

                //if (strArray.Length >= 4)
                //ImageSaveQueue.gOnly.SetPartName(partName, partName);

            }
            catch (Exception ex)
            {
                MessageManager.gOnly.Alarm("RunJob Error,please check inspection !", AlarmType.Unknown, ex);
            }

            return strReturn;
        }
        public string Cam4Job1(CommandAndInfo Command, int InspectionIndex)
        {
            bool bRunJob = false;
            String strReturn = "RunJob Error,please check inspection !";
            RunJobParameters runJobParams;

          //  m_framework.Calibrations[0].GetToolBlock().Inputs["strCMD"].Value = Command.Command;
            string strCMD = Command.Command;
            string[] strArray = strCMD.Split(',');
            UInt64 partID;
            partName = strArray[0] + "_" + strArray[1] + "_" + DateTime.Now.ToString("HHmmss");
            partID = ImageSaveQueue.gOnly.NewPart(partName);
            runJobParams = new RunJobParameters(InspectionIndex, partID);
            try
            {
                if (strArray.Length >= 4)
                {

                    runJobParams.Inputs["strCMD"] = Command.Command;

                    //Run T1,2 Inspection

                    bRunJob = m_framework.RunJob(Command, LightControlAction.AutoOnAndOff, true, runJobParams);
                }

                if (bRunJob)
                {
                    strReturn = (String)runJobParams.Outputs["ResultString"];
                    m_framework.SaveInspection(InspectionIndex);
                }                   
                else
                    strReturn = (String)runJobParams.Outputs["DefaultString"];

                //if (strArray.Length >= 4)
                //ImageSaveQueue.gOnly.SetPartName(partName, partName);

            }
            catch (Exception ex)
            {
                MessageManager.gOnly.Alarm("RunJob Error,please check inspection !", AlarmType.Unknown, ex);
            }

            return strReturn;
        }
        public string Cam4Job2(CommandAndInfo Command, int InspectionIndex)
        {
            bool bRunJob = false;
            String strReturn = "RunJob Error,please check inspection !";
            RunJobParameters runJobParams;

           // m_framework.Calibrations[0].GetToolBlock().Inputs["strCMD"].Value = Command.Command;
            string strCMD = Command.Command;
            string[] strArray = strCMD.Split(',');
            UInt64 partID;
            partName = strArray[0] + "_" + strArray[1] + "_" + strArray[2] + "_" + DateTime.Now.ToString("HHmmss");
            partID = ImageSaveQueue.gOnly.NewPart(partName);
            runJobParams = new RunJobParameters(InspectionIndex, partID);
            CogTransform2DLinear RobotPose = new CogTransform2DLinear();
            if (strArray.Length >= 5)
            {
                RobotPose.TranslationX = double.Parse(strArray[3]);
                RobotPose.TranslationY = double.Parse(strArray[4]);
                RobotPose.Rotation = CogMisc.DegToRad(double.Parse(strArray[5]));
            }
            else
            {
                MessageManager.gOnly.Alarm("SI Send Command Error! Please check!");
                return strReturn;
            }
            partID = ImageSaveQueue.gOnly.NewPart(partName);
            runJobParams = new RunJobParameters(InspectionIndex, partID);
            CogTransform2DLinear robotPose = new CogTransform2DLinear();
            CogTransform2DLinear destPartpose_Station1 = new CogTransform2DLinear();
            CogTransform2DLinear destPartpose_Station2 = new CogTransform2DLinear();
            try
            {
                if (strArray.Length >= 4)
                {

                    runJobParams.Inputs["strCMD"] = Command.Command;

                    //Run T1,2 Inspection
                    CogTransform2DLinear robotPose_Nozzle = (CogTransform2DLinear)m_framework.Inspections[ConstType.T4_TrainNozzleBox].GetToolBlock().Outputs["RobotPose_Nozzle"].Value;
                    CogTransform2DLinear partPose_Nozzle = (CogTransform2DLinear)m_framework.Inspections[ConstType.T4_TrainNozzleBox].GetToolBlock().Outputs["PartPose_Nozzle"].Value;
                    CogTransform2DLinear robotDestPose1 = (CogTransform2DLinear)m_framework.Inspections[ConstType.T4_TrainNozzleBox].GetToolBlock().Outputs["DestPose_BOX1"].Value;
                    CogTransform2DLinear robotDestPose2 = (CogTransform2DLinear)m_framework.Inspections[ConstType.T4_TrainNozzleBox].GetToolBlock().Outputs["DestPose_BOX2"].Value;
                    CogLineSegment seg = (CogLineSegment)m_framework.Inspections[ConstType.T4_TrainNozzleBox].GetToolBlock().Outputs["TrainImageSegment"].Value;
                    CogCircle circle = (CogCircle)m_framework.Inspections[ConstType.T4_TrainNozzleBox].GetToolBlock().Outputs["TrainImageCircle"].Value;
                  

                    destPartpose_Station1 = robotDestPose1.Compose(robotPose_Nozzle.Invert()).Compose(partPose_Nozzle);
                    destPartpose_Station2 = robotDestPose2.Compose(robotPose_Nozzle.Invert()).Compose(partPose_Nozzle);

                    runJobParams.Inputs["destPartpose_station1"] = (CogTransform2DLinear)destPartpose_Station1;
                    runJobParams.Inputs["destPartpose_station2"] = (CogTransform2DLinear)destPartpose_Station2;
                    runJobParams.Inputs["robotPose_Nozzle"] = (CogTransform2DLinear)robotPose_Nozzle;
                    runJobParams.Inputs["partPose_Nozzle"] = (CogTransform2DLinear)partPose_Nozzle;
                    runJobParams.Inputs["TrainImageSegment"] = (CogLineSegment)seg;
                    runJobParams.Inputs["TrainImageCircle"] = (CogCircle)circle;

                    bRunJob = m_framework.RunJob(Command, LightControlAction.AutoOnAndOff, true, runJobParams);
                }

                if (bRunJob)
                {
                    //Calc Box position partDestPose
                   
                    CogTransform2DLinear currentPartPose = new CogTransform2DLinear();
                   // currentPartPose = (CogTransform2DLinear)runJobParams.Outputs["PartPose_Nozzle"];
                   // robotPose = CalculateRobotDestinationPose(RobotPose, currentPartPose, destPartpose);
                    strReturn = (String)runJobParams.Outputs["ResultString"];
               }  
                else
                    strReturn = (String)runJobParams.Outputs["DefaultString"];

                //if (strArray.Length >= 4)
                //ImageSaveQueue.gOnly.SetPartName(partName, partName);

            }
            catch (Exception ex)
            {
                MessageManager.gOnly.Alarm("RunJob Error,please check inspection !", AlarmType.Unknown, ex);
            }

            return strReturn;
        }
        public string Cam5Job1(CommandAndInfo Command, int InspectionIndex)
        {
            bool bRunJob = false;
            String strReturn = "RunJob Error,please check inspection !";
            RunJobParameters runJobParams;
            CogToolBlock tbCalib = null;
            tbCalib = (CogToolBlock)m_framework.Calibrations[5].GetToolBlock();
            tbCalib.Inputs["StrCMD"].Value = Command.Command;
            string strCMD = Command.Command;
            string[] strArray = strCMD.Split(',');
            UInt64 partID;
            partName = strArray[0] + "_" + strArray[1] + "_" + DateTime.Now.ToString("HHmmss");
            partID = ImageSaveQueue.gOnly.NewPart(partName);
            runJobParams = new RunJobParameters(InspectionIndex, partID);
            try
            {
                runJobParams.Inputs["strCMD"] = Command.Command;
                bRunJob = m_framework.RunJob(Command, LightControlAction.AutoOnAndOff, true, runJobParams);
                if (bRunJob)
                    strReturn = (String)runJobParams.Outputs["ResultString"];
                else
                    strReturn = (String)runJobParams.Outputs["DefaultString"];
            }
            catch (Exception ex)
            {
                MessageManager.gOnly.Alarm("RunJob Error,please check inspection !", AlarmType.Unknown, ex);
            }
            return strReturn;
        }
        public string ImageSavedToDisk(bool savedToDisk, string savedToDiskPath, bool movedOnce, ref CommandDestination destination)
        {
           // throw new NotImplementedException();
            return null;
        }
        public CogTransform2DLinear  CalculateRobotDestinationPose(CogTransform2DLinear robotCurrPose, CogTransform2DLinear partCurrPose, CogTransform2DLinear partDestPose)
        {
            CogTransform2DLinear currRobotPose = new CogTransform2DLinear();
            CogTransform2DLinear currPartPose = new CogTransform2DLinear();
            CogTransform2DLinear destPartPose = new CogTransform2DLinear();
            CogTransform2DLinear destRobotPose = new CogTransform2DLinear();

            currRobotPose = robotCurrPose;
            currPartPose = partCurrPose;
            destPartPose = partDestPose; 

            destRobotPose = destPartPose.Compose(currPartPose.Invert().Compose(currRobotPose));
 
            return destRobotPose;
        }
        public string TCPRjpResultReady(CommandAndInfo command, RunJobParameters parameters)
        {
           // throw new NotImplementedException();
            return null;
        }
        private bool CalibrationCamera(CommandAndInfo Command)
        {
            //white light
            m_framework.ChangeProduct(0);

            string[] strArr = Command.Command.Split(',');
            int indexInsp = 0;//base 0
            switch (strArr[0])
            {
                case "T1":
                    indexInsp = 8;
                    FrameworkConfiguration.gOnly.SetInspectionInputLink(0, 0, 8, 0);
                    break;
                case "T22":
                    indexInsp = 8;
                    FrameworkConfiguration.gOnly.SetInspectionInputLink(2, 0, 8, 0);
                    break;
                case "T21":
                    indexInsp = 8;
                    FrameworkConfiguration.gOnly.SetInspectionInputLink(1, 0, 8, 0);
                    break;
                case "T5":
                    indexInsp = 8;
                    FrameworkConfiguration.gOnly.SetInspectionInputLink(5, 0, 8, 0);
                    break;


            }
            CogToolBlock tbCalib = new CogToolBlock();

            if (strArr[0] == "T1")
                {
                    tbCalib = (CogToolBlock)m_framework.Calibrations[0].GetToolBlock();
                    tbCalib.Inputs["StrCMD"].Value = Command.Command;
                }
                else if (strArr[0] == "T21")
                {
                    tbCalib = (CogToolBlock)m_framework.Calibrations[1].GetToolBlock();
                    tbCalib.Inputs["StrCMD"].Value = Command.Command;
                }
                else if (strArr[0] == "T22")
                {
                    tbCalib = (CogToolBlock)m_framework.Calibrations[2].GetToolBlock();
                    tbCalib.Inputs["StrCMD"].Value = Command.Command;
                }
                else if (strArr[0] == "T5")
                {
                    tbCalib = (CogToolBlock)m_framework.Calibrations[5].GetToolBlock();
                    tbCalib.Inputs["StrCMD"].Value = Command.Command;
                }
            RunJobParameters runJobParams = new RunJobParameters(indexInsp, ImageSaveQueue.gOnly.NewPart(Command.Command));

            bool isSuccess = m_framework.RunJob(Command, LightControlAction.AutoOnAndOff, false, runJobParams);
            m_framework.PostRecordToDisplay(Command, 0, tbCalib.CreateLastRunRecord().SubRecords[0]);
            if (isSuccess)
            {
                if (strArr[0] == "T1")
                {
                    for (int i = 0; i < 1; i++)
                    {
                        m_framework.SaveCalibration(i);
                        MessageManager.gOnly.Info("Save Calibration_" + (i + 1).ToString() + ".vpp" + " successfully!");
                    }

                }
                else if (strArr[0] == "T21")
                {
                    for (int i = 1; i < 2; i++)
                    {
                        m_framework.SaveCalibration(i);
                        MessageManager.gOnly.Info("Save Calibration_" + (i + 1).ToString() + ".vpp" + " successfully!");
                    }
                }
                else if (strArr[0] == "T22")
                {
                    for (int i = 2; i < 3; i++)
                    {
                        m_framework.SaveCalibration(i);
                        MessageManager.gOnly.Info("Save Calibration_" + (i + 1).ToString() + ".vpp" + " successfully!");
                    }
                }
                else if (strArr[0] == "T5")
                {
                    m_framework.SaveCalibration(5);
                    MessageManager.gOnly.Info("Save Calibration_" + (6).ToString() + ".vpp" + " successfully!");
                }
                else
                {
                    MessageManager.gOnly.AlarmFormat("Input checkboard command is error, please check!");
                }
            }
            else
            {
                MessageManager.gOnly.Info("calibration" + strArr[0] + " station failed !");
            }
            return isSuccess;
        }
    }

    
}
