using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using DuSwToglTF.ExportContext;
using System.IO;
using System.Linq;
using NUnit.Framework;
using System.Runtime.CompilerServices;

namespace DuSwToglTFTests.ExportContextTests
{
    public class ExportTestBase
    {
        public ExportTestBase()
        {
            
        }

        protected void Export(ISldWorks app, string fileExtension,string childFolder,Func<string,glTFExportContext > contextFunc)
        {
            Console.WriteLine("Export开始" + DateTime.Now.ToString());
            var dirInfo = new DirectoryInfo(Path.GetDirectoryName(typeof(PartDocExportContextTests).Assembly.Location));
            
            var diretory = Path.Combine(dirInfo.Parent.FullName, "DuSwToglTFTests", "SolidWorksModel", childFolder);
            //var diretory = Path.Combine("E:\\3d\\input");
            var files = Directory.GetFiles(diretory).Where(p => p.ToUpper().EndsWith(fileExtension) && p.Contains("~$") == false);
            //var files = Directory.GetFiles(diretory).Where(p => p.ToUpper().EndsWith(fileExtension) && p.Contains("~$")==false && p.Contains("SK7420A-750.SLDASM") == true);
            Console.WriteLine("文件路径完成" + DateTime.Now.ToString());
            foreach (var item in files)
            {
                Console.WriteLine("OpenSWDoc开始" + DateTime.Now.ToString());

                int errors = 0;
                //var doc = app.OpenDoc2(item, (int)swDocumentTypes_e.swDocASSEMBLY, true,false,true,ref errors) as IModelDoc2;
                var doc = OpenSWDoc(item, true, app);
                var fileName = Path.GetFileNameWithoutExtension(item);
                Console.WriteLine("OpenSWDoc完成" + DateTime.Now.ToString());

                ExporterUtility.ExportData(doc, contextFunc.Invoke(Path.Combine(diretory,fileName)));

               // Assert.IsTrue(File.Exists(diretory + ".gltf"));
                // Assert.IsTrue(File.Exists(diretory + ".glb"));

            }
            Console.WriteLine("总完成" + DateTime.Now.ToString());

            app.ExitApp();

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
