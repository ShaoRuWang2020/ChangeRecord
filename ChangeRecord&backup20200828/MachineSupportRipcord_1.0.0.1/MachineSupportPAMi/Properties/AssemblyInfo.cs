using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// 有关程序集的常规信息通过以下
// 特性集控制。更改这些特性值可修改
// 与程序集关联的信息。
[assembly: AssemblyTitle("MachineSupportPAMi")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("COGNEX")]
[assembly: AssemblyProduct("MachineSupportPAMi")]
[assembly: AssemblyCopyright("Copyright ©  2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// 将 ComVisible 设置为 false 使此程序集中的类型
// 对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型，
// 则将该类型上的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("81f9883e-dc82-46d4-890c-6efad3886d87")]

// 程序集的版本信息由下面四个值组成:
//
//      主版本
//      次版本 
//      生成号
//      修订号
//
// 可以指定所有这些值，也可以使用“生成号”和“修订号”的默认值，
// 方法是按如下所示使用“*”:

//version update note
//4.1.0.1
//target plateform from AnyCPU to x64
//delete camera 2 related code
//@TCPJobParser(CommandAndInfo command) T2,1
//@PAMReCheck2
//@SimulatorCommandStrings
//@SimulatorCommandParserIDs

//[assembly: AssemblyVersion("1.0.*")]
//[assembly: AssemblyVersion("4.1.0.1")]
//[assembly: AssemblyFileVersion("4.1.0.1")]

//added for getting module wings bar light images 20170201
//add T1,2 simulate command and process method "PAMReCheck2"
//add images in one process group function
//change simulate command to      
//strings.Add("T1,1,1-5,1");
//strings.Add("T1,1,6-10,6");
//strings.Add("T1,2");

//[assembly: AssemblyVersion("4.1.0.2")]
//[assembly: AssemblyFileVersion("4.1.0.2")]

//add separate cavity use separate auto calibratio function
//Cal,1,2...10 command auto cal
//Cal,0 end calibration and save inspection

//[assembly: AssemblyVersion("4.1.0.3")]
//[assembly: AssemblyFileVersion("4.1.0.3")]

//EVT update version

//[assembly: AssemblyVersion("5.0.0.0")]
//[assembly: AssemblyFileVersion("5.0.0.0")]

//CRB update version
//[assembly: AssemblyVersion("6.0.0.0")]
//[assembly: AssemblyFileVersion("6.0.0.0")]


//N84 PAMi Template Version

[assembly: AssemblyVersion("1.1.0.1")]
[assembly: AssemblyFileVersion("1.1.0.1")]