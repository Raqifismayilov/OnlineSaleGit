using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineSale.Areas.AdminPart.Models.OnlineSaleTable
{
    public class FindCustomer
    {
        public bool? SelectedValue { get; set; }
        public DateTime SelectedDate { get; set; }
        public FindCustomer() { }
        public FindCustomer(bool? slctValue, DateTime slctDate)
        {
            SelectedValue = slctValue;
            SelectedDate = slctDate;
        }
    }
}