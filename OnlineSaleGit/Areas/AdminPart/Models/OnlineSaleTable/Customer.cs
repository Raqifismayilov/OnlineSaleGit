using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineSale.Areas.AdminPart.Models.OnlineSaleTable
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public bool PurchaseStatus { get; set; }
        public Customer() { }
        public Customer(string firstName, string lastName, string email, string mobile, DateTime dtofPurch)
        {
            Firstname = firstName;
            Lastname = lastName;
            Email = email;
            MobileNumber = mobile;
            DateOfPurchase = dtofPurch;
        }
    }
}