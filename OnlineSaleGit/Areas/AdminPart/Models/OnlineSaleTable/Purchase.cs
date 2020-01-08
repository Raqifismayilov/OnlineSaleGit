using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineSale.Areas.AdminPart.Models.OnlineSaleTable
{
    public class Purchase
    {
        public int PurchaseId { get; set; }
        public int CustomerId { get; set; }
        public string ProductList { get; set; }
        public bool PurchaseStatus { get; set; }
        public Purchase() { }
        public Purchase(int cid, string prdList,bool prcStats)
        {
            CustomerId = cid;
            ProductList = prdList;
            PurchaseStatus = prcStats;
        }
    }
}