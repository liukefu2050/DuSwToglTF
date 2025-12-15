using DuSwToglTF.ExportContext;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.IO;

namespace WinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SldWorks swApp = new SldWorks();
            swApp.CommandInProgress = true;
            swApp.Visible = true;

            var orgFilePath = this.textBox1.Text;

            var doc = OpenSWDoc(orgFilePath, true, swApp);
            var fileName = Path.GetFileNameWithoutExtension(orgFilePath);
            Console.WriteLine("OpenSWDoc完成" + DateTime.Now.ToString());
            var diretory = @"C:\3d\";
            var aa = Path.Combine(diretory, fileName); 

            ExporterUtility.ExportData(doc, new PartDocExportContext(aa));
            swApp.ExitApp();
        }

        public void PartDocExportContextTest()
        {
            Console.WriteLine("开始" + DateTime.Now.ToString());
            //mytest();
            //Export(SwApp(), ".SLDPRT", "part2", (diretory) => new PartDocExportContext(diretory));
        }

        public SldWorks SwApp()
        {
            Console.WriteLine("new SldWorks()开始" + DateTime.Now.ToString());
            SldWorks swApp = new SldWorks();
            swApp.Visible = true;
            Console.WriteLine("new SldWorks()借宿" + DateTime.Now.ToString());
            return swApp;
        }

        //filePath文件路径SldWorks.AssemblyDoc
        //isVisible是否可见
        public static ModelDoc2 OpenSWDoc(string filePath, bool isVisible, ISldWorks app)
        {
            swDocumentTypes_e type = swDocumentTypes_e.swDocNONE;
            string ext = Path.GetExtension(filePath).ToUpper().Substring(1);
            if (ext == "SLDASM")
            {
                type = swDocumentTypes_e.swDocASSEMBLY;
            }
            else if (ext == "SLDPRT")
            {
                type = swDocumentTypes_e.swDocPART;
            }
            else if (ext == "SLDDRW")
            {
                type = swDocumentTypes_e.swDocDRAWING;
            }
            else
            {
                return null;
            }
            int Errors = 0;
            int Warnings = 0;
            ModelDoc2 modelDoc2 = app.OpenDoc6(filePath, (int)type, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref Errors, ref Warnings);
            
            return modelDoc2;
        }
    }
}
