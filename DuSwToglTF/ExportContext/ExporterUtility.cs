using DuSwToglTF.Extension;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Diagnostics;
using System.Linq;
using static DuSwToglTF.SwExtension.CustomPropertiesExtension;

namespace DuSwToglTF.ExportContext
{
    public static class ExporterUtility
    {
        public static void ExportData(IModelDoc2 doc,glTFExportContext context,Action<int,string> progressAction = null,bool hasObj = false,bool hasglTF = false,bool hasglb = true)
        {

            try
            {
                context.Start();
                Console.WriteLine("ExportData开始" + DateTime.Now.ToString());
                //写入实体网格
                var bodies = doc.GetBodyDataModels().ToList();
                Console.WriteLine("ExportData写入实体网格" + DateTime.Now.ToString());
                progressAction?.Invoke(10, "Read SolidWorks's SolidBody...");
                var material = doc.GetMaterialBuilder();
                progressAction?.Invoke(10, "Read SolidWorks Doc's Material...");
  
                int i = 0;
                int count = bodies.Count;

                Console.WriteLine("总共bodys数=" + count + DateTime.Now.ToString());
                foreach (var item in bodies)
                {
                    //i++;
                    int progressValue = (i++)* 100 / count + 10;
                    progressAction?.Invoke(progressValue, i + "/" + count + "Bodys,Name=" + item.DispalyName);

                    Console.WriteLine(i + "/" + count + "Bodys,Name=" + item.DispalyName + DateTime.Now.ToString());
                    context.OnBodyBegin(item.Body, item.BodyMaterialBuilder ?? material, item.Location, item.DispalyName);


                    progressAction?.Invoke(progressValue, $"Finish {item.Body.Name}...");
                }

                //写入自定义属性
                //var properties = doc.GetCustomProperties();
                //foreach (var prop in properties)
                //{
                //Console.WriteLine(prop.Name +","+ prop.Value);

                //context.OnCustomPropertyBegin(prop);
                //}

                progressAction?.Invoke(90, "Saving");
                //Console.WriteLine($"Finish:出文件开始" + DateTime.Now.ToString());
                context.Finish(hasObj, hasglTF, hasglb);
                //Console.WriteLine($"Finish:出文件借宿" + DateTime.Now.ToString());
                progressAction?.Invoke(100, $"Suceess, FileDirectory:{context.SavePathName}.*");

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
