using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schoolxm
{
    public class Order
    {
        public int Id { get; set; }
        public string StuAccount { get; set; }//学号
        public string StuName { get; set; }//姓名
        public string Date { get; set; }
        public string Time { get; set; }
        public int NotOrderNumber { get; set; }
        public int OrderNumber { get; set; }
        public string State { get; set; }
        public string OrderYesOrNo { get; set; }
        public string ButtonValue { get; set; }
    }
}