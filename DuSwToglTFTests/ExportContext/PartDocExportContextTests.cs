using DuSwToglTF;
using DuSwToglTF.ExportContext;
using DuSwToglTFTests.ExportContextTests;
using NUnit.Framework;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuSwToglTFTests.ExportContextTests
{
    [TestFixture()]
    public class PartDocExportContextTests : ExportTestBase
    {
        public PartDocExportContextTests() 
        {
        }

        [Test()]
        public void PartDocExportContextTest()
        {
            Console.WriteLine("开始" + DateTime.Now.ToString());
            //mytest();
            Export(SwApp(), ".SLDPRT","part2",(diretory) => new PartDocExportContext(diretory));
        }

        public SldWorks SwApp()
        {
            Console.WriteLine("new SldWorks()开始" + DateTime.Now.ToString());
            SldWorks swApp = new SldWorks();
            swApp.CommandInProgress = true;
            swApp.Visible = true;
            Console.WriteLine("new SldWorks()借宿" + DateTime.Now.ToString());
            return swApp;
        }

        public void mytest()

        {
            //SldWorks swApp = new SldWorksClass();

            SldWorks swApp = new SldWorks();

            swApp.Visible = true;

            OpenSWDoc2("E:\\user\\download\\DuSwToglTF-1.1.0\\DuSwToglTFTests\\SolidWorksModel\\part2\\DT6Z2-89.sldprt",false, swApp);
            ModelDoc2 swDoc = swApp.IActiveDoc2;

            swDoc.Extension.SelectByID2("Top Plane", "PLANE", 0, 0, 0, false, 0, null, 0);

            swDoc.InsertSketch2(false);

            swDoc.SketchRectangle(0, 0.04, 0, 0.01, 0, 0, true);

            swDoc.FeatureManager.FeatureExtrusion(true, false, false, 0, 0, 0.09, 0, false, false, false, false, 0.0, 0.0, false, false, false, false, true, false, false);

        }

        //filePath文件路径
        //isVisible是否可见
        public static ModelDoc2 OpenSWDoc2(string filePath, bool isVisible, ISldWorks app)
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