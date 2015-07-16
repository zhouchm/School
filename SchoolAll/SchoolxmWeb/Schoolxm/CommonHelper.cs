using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NVelocity.App;
using NVelocity.Runtime;
using NVelocity;

namespace Schoolxm
{
    public class CommonHelper
    {
        public static string RenderHtml(string name, object data)
        {
            VelocityEngine vltEngine = new VelocityEngine();
            vltEngine.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            vltEngine.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, System.Web.Hosting.HostingEnvironment.MapPath("~/html"));//模板文件所在的文件夹
            vltEngine.Init();

            VelocityContext vltContext = new VelocityContext();
            vltContext.Put("Model", data);//设置参数，在模板中可以通过$data来引用

            Template vltTemplate = vltEngine.GetTemplate(name);
            System.IO.StringWriter vltWriter = new System.IO.StringWriter();
            vltTemplate.Merge(vltContext, vltWriter);
            return vltWriter.GetStringBuilder().ToString();
        }
    }
}