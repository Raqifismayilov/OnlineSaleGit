using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineSale.Areas.AdminPart.Models.OnlineSaleTable;

namespace OnlineSale.Areas.AdminPart.Models.ModelView
{
    public class ShoppingView : IAdminPanel
    {
        public List<AdminPanel> AdminPanel { get; set; }
        public List<Customer> CustomerList { get; set; }
        public List<Stock> ProductList { get; set; }
        public FindCustomer SelectedCustomer { get; set; }
        public int CustomerId { get; set; }
        public ShoppingView() { }
        public ShoppingView(List<AdminPanel> adPanel, List<Customer> cstList, List<Stock> prdList)
        {
            AdminPanel = adPanel;
            CustomerList = cstList;
            ProductList = prdList;
        }
    }
}