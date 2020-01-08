using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using OnlineSale.Areas.AdminPart.Models.OnlineSaleData;
using OnlineSale.Areas.AdminPart.Models.OnlineSaleTable;
using OnlineSale.Areas.AdminPart.Models.ModelView;

namespace OnlineSale.Areas.AdminPart.Controllers
{
    public class ShoppingController : Controller
    {
        OnlineSaleDB db = new OnlineSaleDB("OnlineSale");
        ShoppingView shoppingView = new ShoppingView();
        // GET: AdminPart/Shopping
        [HttpGet]
        public ActionResult GetShopping()
        {
            if (User.Identity.IsAuthenticated)
            {
                shoppingView.AdminPanel = db.getAdminPanel();
                shoppingView.CustomerList = db.getCustomerList();
                return View(shoppingView);
            }
            else
                return RedirectToAction("Login", "Logon");
        }

        [HttpGet]
        public ActionResult ShoppingCart(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                shoppingView.AdminPanel = db.getAdminPanel();
                shoppingView.CustomerId = id;               
                Purchase dataPrc = db.getPurchase(id);
                JavaScriptSerializer productList = new JavaScriptSerializer();
                List<Stock> desrlzProductList = productList.Deserialize<List<Stock>>(dataPrc.ProductList);
                shoppingView.ProductList = desrlzProductList;
                return View(shoppingView);
            }
            else
                return RedirectToAction("Login", "Logon");
        }

        [HttpGet]
        public ActionResult CustomerSearch()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("GetShopping");
            return RedirectToAction("Login", "Logon");
        }

        [HttpPost]
        public ActionResult CustomerSearch(FindCustomer customerSrc)
        {
            if (User.Identity.IsAuthenticated)
            {
                shoppingView.AdminPanel = db.getAdminPanel();
                shoppingView.CustomerList = db.getCustomerList(customerSrc);
                return View("GetShopping", shoppingView);
            }
            else
                return RedirectToAction("Login", "Logon");
        }

        [HttpPost]
        public ActionResult OrderIsProcessed(int CustomerId)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (CustomerId != 0)
                {
                    bool result = db.UpdatePurchase(CustomerId);
                    if (result)
                        return RedirectToAction("GetShopping");
                    return HttpNotFound();
                }
                else
                    return HttpNotFound();
            }
            else
                return RedirectToAction("Login", "Logon");
        }
    }
}