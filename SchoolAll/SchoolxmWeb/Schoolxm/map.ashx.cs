using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Schoolxm
{
    /// <summary>
    /// map 的摘要说明
    /// </summary>
    public class map : IHttpHandler
    {
        DateTime[] time;
        static DataTable users;
        string lineto = null;
        int[] nodex = new int[100];
        int[] nodey = new int[100];
        public void dra(int i, int a)
        {
            if (nodex[i] > nodex[i - 1] && nodey[i] > nodey[i - 1])
            {
                if (a % 2 != 0)
                {
                    while (nodex[i] > nodex[i - 1] && nodey[i] > nodey[i - 1])
                    {
                        lineto = lineto + @"
                 setTimeout(""arrayx[j]=arrayx[j]+2;arrayy[j]=arrayy[j]+3;context.lineTo(arrayx[j],arrayy[j]);context.stroke()"",times); times=times+100;
                   ";
                        nodex[i - 1] = nodex[i - 1] + 2; nodey[i - 1] = nodey[i - 1] + 2;
                    }
                }
                if (a == 0 || a % 2 == 0)
                {
                    while (nodex[i] > nodex[i - 1] && nodey[i] > nodey[i - 1])
                    {
                        lineto = lineto + @"
                 setTimeout(""arrayx[j]=arrayx[j]+2;arrayy[j]=arrayy[j]+2;context.lineTo(arrayx[j],arrayy[j]);context.stroke()"",times); times=times+100;
                   ";
                        nodex[i - 1] = nodex[i - 1] + 2; nodey[i - 1] = nodey[i - 1] + 2;
                    }
                }
                //lineto = lineto + "j=j+1;";
            }
            if (nodex[i] < nodex[i - 1] && nodey[i] < nodey[i - 1])
            {
                if (a % 2 != 0)
                {
                    while (nodex[i] < nodex[i - 1] && nodey[i] < nodey[i - 1])
                    {
                        lineto = lineto + @"
                 setTimeout(""arrayx[j]=arrayx[j]-2;arrayy[j]=arrayy[j]-3;context.lineTo(arrayx[j],arrayy[j]);context.stroke()"",times); times=times+100;
                   ";
                        nodex[i - 1] = nodex[i - 1] - 2; nodey[i - 1] = nodey[i - 1] - 2;
                    }
                }
                if (a == 0 || a % 2 == 0)
                {
                    while (nodex[i] < nodex[i - 1] && nodey[i] < nodey[i - 1])
                    {
                        lineto = lineto + @"
                 setTimeout(""arrayx[j]=arrayx[j]-2;arrayy[j]=arrayy[j]-2;context.lineTo(arrayx[j],arrayy[j]);context.stroke()"",times); times=times+100;
                   ";
                        nodex[i - 1] = nodex[i - 1] - 2; nodey[i - 1] = nodey[i - 1] - 2;
                    }
                }
                //lineto = lineto + "j=j+1;";
            }
            if (nodex[i] > nodex[i - 1] && nodey[i] < nodey[i - 1])
            {
                if (a % 2 != 0)
                {
                    while (nodex[i] > nodex[i - 1] && nodey[i] < nodey[i - 1])
                    {
                        lineto = lineto + @"
                 setTimeout(""arrayx[j]=arrayx[j]+2;arrayy[j]=arrayy[j]-3;context.lineTo(arrayx[j],arrayy[j]);context.stroke()"",times); times=times+100;
                   ";
                        nodex[i - 1] = nodex[i - 1] + 2; nodey[i - 1] = nodey[i - 1] - 2;
                    }
                }
                if (a == 0 || a % 2 == 0)
                {
                    while (nodex[i] > nodex[i - 1] && nodey[i] < nodey[i - 1])
                    {
                        lineto = lineto + @"
                 setTimeout(""arrayx[j]=arrayx[j]+2;arrayy[j]=arrayy[j]-2;context.lineTo(arrayx[j],arrayy[j]);context.stroke()"",times); times=times+100;
                   ";
                        nodex[i - 1] = nodex[i - 1] + 2; nodey[i - 1] = nodey[i - 1] - 2;
                    }
                }
                //lineto = lineto + "j=j+1;";
            }
            if (nodex[i] < nodex[i - 1] && nodey[i] > nodey[i - 1])
            {
                if (a % 2 != 0)
                {
                    while (nodex[i] < nodex[i - 1] && nodey[i] > nodey[i - 1])
                    {
                        lineto = lineto + @"
                 setTimeout(""arrayx[j]=arrayx[j]-2;arrayy[j]=arrayy[j]+3;context.lineTo(arrayx[j],arrayy[j]);context.stroke()"",times); times=times+100;
                   ";
                        nodex[i - 1] = nodex[i - 1] - 2; nodey[i - 1] = nodey[i - 1] + 2;
                    }
                }
                if (a == 0 || a % 2 == 0)
                {
                    while (nodex[i] < nodex[i - 1] && nodey[i] > nodey[i - 1])
                    {
                        lineto = lineto + @"
                 setTimeout(""arrayx[j]=arrayx[j]-2;arrayy[j]=arrayy[j]+2;context.lineTo(arrayx[j],arrayy[j]);context.stroke()"",times); times=times+100;
                   ";
                        nodex[i - 1] = nodex[i - 1] - 2; nodey[i - 1] = nodey[i - 1] + 2;
                    }
                }
                //lineto = lineto + "j=j+1;";
            }
            if (nodex[i] == nodex[i - 1] && nodey[i] > nodey[i - 1])
            {
                if (a % 2 != 0)
                {
                    while (nodex[i] == nodex[i - 1] && nodey[i] > nodey[i - 1])
                    {
                        lineto = lineto + @"
                 setTimeout(""arrayx[j]=arrayx[j]+1;arrayy[j]=arrayy[j]+2;context.lineTo(arrayx[j],arrayy[j]);context.stroke()"",times); times=times+100;
                   ";
                        nodey[i - 1] = nodey[i - 1] + 2;
                    }
                }
                if (a == 0 || a % 2 == 0)
                {
                    while (nodex[i] == nodex[i - 1] && nodey[i] > nodey[i - 1])
                    {
                        lineto = lineto + @"
                 setTimeout(""arrayy[j]=arrayy[j]+2;context.lineTo(arrayx[j],arrayy[j]);context.stroke()"",times); times=times+100;
                   ";
                        nodey[i - 1] = nodey[i - 1] + 2;
                    }
                }
                // lineto = lineto + "j=j+1;";
            }
            if (nodex[i] == nodex[i - 1] && nodey[i] < nodey[i - 1])
            {
                if (a % 2 != 0)
                {
                    while (nodex[i] == nodex[i - 1] && nodey[i] < nodey[i - 1])
                    {
                        lineto = lineto + @"
                 setTimeout(""arrayx[j]=arrayx[i]+1;arrayy[j]=arrayy[j]-2;context.lineTo(arrayx[j],arrayy[j]);context.stroke()"",times); times=times+100;
                   ";
                        nodey[i - 1] = nodey[i - 1] - 2;
                    }
                }
                if (a == 0 || a % 2 == 0)
                {
                    while (nodex[i] == nodex[i - 1] && nodey[i] < nodey[i - 1])
                    {
                        lineto = lineto + @"
                 setTimeout(""arrayy[j]=arrayy[j]-2;context.lineTo(arrayx[j],arrayy[j]);context.stroke()"",times); times=times+100;
                   ";
                        nodey[i - 1] = nodey[i - 1] - 2;
                    }
                }
                // lineto = lineto + "j=j+1;";
            }
            if (nodex[i] < nodex[i - 1] && nodey[i] == nodey[i - 1])
            {
                if (a % 2 != 0)
                {
                    while (nodex[i] < nodex[i - 1] && nodey[i] == nodey[i - 1])
                    {
                        lineto = lineto + @"
                 setTimeout(""arrayy[j]=arrayy[j]+1;arrayx[j]=arrayx[j]-2;context.lineTo(arrayx[j],arrayy[j]);context.stroke()"",times); times=times+100;
                   ";
                        nodex[i - 1] = nodex[i - 1] - 2;
                    }
                }
                if (a == 0 || a % 2 == 0)
                {
                    while (nodex[i] < nodex[i - 1] && nodey[i] == nodey[i - 1])
                    {
                        lineto = lineto + @"
                 setTimeout(""arrayx[j]=arrayx[j]-2;context.lineTo(arrayx[j],arrayy[j]);context.stroke()"",times); times=times+100;
                   ";
                        nodex[i - 1] = nodex[i - 1] - 2;
                    }
                }
                // lineto = lineto + "j=j+1;";
            }
            if (nodex[i] > nodex[i - 1] && nodey[i] == nodey[i - 1])
            {
                if (a % 2 != 0)
                {
                    while (nodex[i] > nodex[i - 1] && nodey[i] == nodey[i - 1])
                    {
                        lineto = lineto + @"
                 setTimeout(""arrayy[j]=arrayy[j]+1;arrayx[j]=arrayx[j]+2;context.lineTo(arrayx[j],arrayy[j]);context.stroke()"",times); times=times+100;
                   ";
                        nodex[i - 1] = nodex[i - 1] + 2;
                    }
                }
                if (a == 0 || a % 2 == 0)
                {
                    while (nodex[i] > nodex[i - 1] && nodey[i] == nodey[i - 1])
                    {
                        lineto = lineto + @"
                 setTimeout(""arrayx[j]=arrayx[j]+2;context.lineTo(arrayx[j],arrayy[j]);context.stroke()"",times); times=times+100;
                   ";
                        nodex[i - 1] = nodex[i - 1] + 2;
                    }
                }
                // lineto = lineto + "j=j+1;";
            }
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            users = SqlHelper.ExecuteDataTable("select * from T_UserRFID300");
            time = new DateTime[users.Rows.Count];
            for (int j = 0; j < users.Rows.Count; j++)
            {

                DateTime time2 = Convert.ToDateTime(users.Rows[j]["Time"]);
                DateTime time1 = DateTime.Now;
                TimeSpan ts1 = new TimeSpan(time1.Ticks);
                TimeSpan ts2 = new TimeSpan(time2.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                int h = (int)ts.Days;
                //if(h<12)
                time[j] = time2;
            }
            Sort(time);
            string[] node = new string[users.Rows.Count];
            for (int j = 0; j < users.Rows.Count; j++)
            {
                node[j] = SqlHelper.ExecuteScalar("Select Node_name from T_UserRFID300 where Time=@Time", new SqlParameter("@Time", time[j])).ToString();
            }
            //var data = new { Title = "", Node = node };
            //string html = CommonHelper.RenderHtml("../html/map.htm", data);
            string html1 = @"<html><head><title></title><canvas id = ""myCanvas"" width = '500' height = '500'>Canvas画线技巧</canvas>
                           <script type=""text/javascript"">
 var x0=50;
         var y0=50;
         var x1=100;
         var y1=100;
         var times=300;
         var fla=0;
         var i=0;
         var arrayx = new Array();　//创建一个数组
         var arrayy = new Array();　//创建一个数组
         arrayx. push(10);
         arrayy. push(10);
var myCanvas = document.getElementById(""myCanvas"");
          var context = myCanvas.getContext(""2d"");
          var gradient=context.createLinearGradient(0,0,myCanvas.width,myCanvas.height);
gradient.addColorStop(0,""black"");
gradient.addColorStop(""0.3"",""magenta"");
gradient.addColorStop(""0.5"",""blue"");
gradient.addColorStop(""0.6"",""green"");
gradient.addColorStop(""0.8"",""yellow"");
gradient.addColorStop(1,""red"");
context.lineJoin=""round"";
          context.strokeStyle =gradient;
          context.globalAlpha=0.5; 
                            function dra() {
          context.lineWidth = 3; //设置线宽
          context.beginPath();
          context.moveTo(10, 10);
          ";


            nodex[0] = 10; nodey[0] = 10;
            for (int j = 0; j < users.Rows.Count; j++)
            {
                if (node[j] == "智慧精灵读卡器")
                {
                    nodex[j + 1] = 50; nodey[j + 1] = 50;
                    lineto = lineto + "arrayx. push(50);arrayy. push(50);";
                }
                if (node[j] == "手环自动发放机")
                {
                    nodex[j + 1] = 100; nodey[j + 1] = 100;
                    lineto = lineto + "arrayx. push(100);arrayy. push(100);";
                }
            }
            lineto = lineto + "j=0;";
            for (int j = 0; j < users.Rows.Count; j++)
            {
                if (node[j] == "智慧精灵读卡器")
                {

                    dra(j + 1, j);

                }
                if (node[j] == "手环自动发放机")

                    dra(j + 1, j);

                if (node[j] == "体验导览读卡器")
                    //lineto = lineto + ""
                    ;
            }


            //            string ren = @"if(flag){
            //                          clearInterval(zq);
            //                          flag=false;}
            //                         if(!flag){
            //                         ks();
            //                         flag=true;}";
            string html2 = lineto + "}" + @"
          
          
          //context.closePath(); //可以把这句注释掉再运行比较下不同
         //context.stroke(); //画线框
          //context.fill(); //填充颜色
 
  </script>
</head>
<body onload=""dra()"">
<canvas id = ""myCanvas"" width = '500' height = '500'>Canvas画线技巧</canvas>
</body>
</html>";
            string html = html1 + html2;
            context.Response.Write(html);
        }

        public void Sort(DateTime[] time)
        {
            for (int i = 1; i < time.Length; i++)
            {
                DateTime dt1 = time[i];
                int j = i;
                while ((j > 0) && (DateTime.Compare(time[j - 1], dt1) > 0))
                { time[j] = time[j - 1]; --j; }
                time[j] = dt1;
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}