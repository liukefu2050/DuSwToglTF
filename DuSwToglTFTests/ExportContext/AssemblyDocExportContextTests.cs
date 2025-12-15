using DuSwToglTF;
using DuSwToglTF.ExportContext;
using DuSwToglTFTests.ExportContextTests;
using NUnit.Framework;
using SolidWorks.Interop.sldworks;
using System;


namespace DuSwToglTFTests.ExportContextTests
{
    [TestFixture()]
    public class AssemblyDocExportContextTests:  ExportTestBase
    {
        public AssemblyDocExportContextTests() 
        {
        }

        [Test()]
        public void AssemblyDocExportContextTest()
        {
            ISldWorks app = SwApp();
            Export(app,".SLDASM","Assembly",(diretory) => new AssemblyDocExportContext(diretory));

        }

        public SldWorks SwApp()
        {
            SldWorks swApp;
            Type type = Type.GetTypeFromProgID("SldWorks.Application");
            swApp = Activator.CreateInstance(type) as SldWorks;
            swApp.Visible = true;
            swApp.CommandInProgress = true;
            return swApp;
        }
    }
}