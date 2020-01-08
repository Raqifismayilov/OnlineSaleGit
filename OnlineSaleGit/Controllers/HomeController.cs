using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineSale.Areas.AdminPart.Models.OnlineSaleTable;
using OnlineSale.Areas.AdminPart.Models.OnlineSaleData;
using OnlineSale.Areas.AdminPart.Models.ModelView;
using OnlineSale.Models;
using System.Web.Script.Serialization;

namespace OnlineSale.Controllers
{
    public class HomeController : Controller
    {
        OnlineSaleDB db = new OnlineSaleDB("OnlineSale");
        HomeView homeView = new HomeView();
        RandomProduct ranProduct = new RandomProduct();
        TopProductSlide topProduct = null;

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Allin.az:Elektronika və məişət texnika internet mağazası";
            homeView.AdminPanel = db.getAdminPanel();
            homeView.SubProductsCategory = db.getSubProducts();
            homeView.ProductsCategorys = db.getProductsCategory();
            topProduct = new TopProductSlide(db.getStockStrNewTop(), db.getStockStrEndirimTop(), ranProduct.getNewStockList(db.getStockStrList()));
            homeView.TopSlidePruduct = topProduct;
            homeView.FooterList = db.getFooters();
            homeView.SocialMediaList = db.getSocialMedia();
            homeView.FeedbackEmail = db.getContactList().FirstOrDefault().Email;
            homeView.MainSlideList = db.getMainSlide();
            homeView.StockList = db.weekDiscount();
            ViewBag.Category = new SelectList(homeView.ProductsCategorys, "Id", "ProductName");
            return View(homeView);
        }
        [HttpGet]
        public ActionResult GetAbout()
        {
            ViewBag.Title = "Haqqımızda";
            homeView.AdminPanel = db.getAdminPanel();
            homeView.AboutList = db.getAboutList();
            homeView.SubProductsCategory = db.getSubProducts();
            homeView.ProductsCategorys = db.getProductsCategory();
            topProduct = new TopProductSlide(db.getStockStrNewTop(), db.getStockStrEndirimTop(), ranProduct.getNewStockList(db.getStockStrList()));
            homeView.TopSlidePruduct = topProduct;
            homeView.FooterList = db.getFooters();
            homeView.SocialMediaList = db.getSocialMedia();
            homeView.FeedbackEmail = db.getContactList().FirstOrDefault().Email;
            return View(homeView);
        }
        [HttpGet]
        public ActionResult GetContact()
        {
            ViewBag.Title = "Əlaqə";
            homeView.ContactList = db.getContactList();
            homeView.AdminPanel = db.getAdminPanel();
            homeView.SubProductsCategory = db.getSubProducts();
            homeView.ProductsCategorys = db.getProductsCategory();
            topProduct = new TopProductSlide(db.getStockStrNewTop(), db.getStockStrEndirimTop(), ranProduct.getNewStockList(db.getStockStrList()));
            homeView.TopSlidePruduct = topProduct;
            homeView.FooterList = db.getFooters();
            homeView.SocialMediaList = db.getSocialMedia();
            homeView.FeedbackEmail = db.getContactList().FirstOrDefault().Email;
            return View(homeView);
        }
        [HttpGet]
        public ActionResult GetLocation()
        {
            ViewBag.Title = "Xəritə";
            homeView.ContactList = db.getContactList();
            homeView.AdminPanel = db.getAdminPanel();
            homeView.SubProductsCategory = db.getSubProducts();
            homeView.ProductsCategorys = db.getProductsCategory();
            topProduct = new TopProductSlide(db.getStockStrNewTop(), db.getStockStrEndirimTop(), ranProduct.getNewStockList(db.getStockStrList()));
            homeView.TopSlidePruduct = topProduct;
            homeView.FooterList = db.getFooters();
            homeView.SocialMediaList = db.getSocialMedia();
            homeView.FeedbackEmail = db.getContactList().FirstOrDefault().Email;
            return View(homeView);
        }
        [HttpGet]
        public ActionResult GetProduct(int id)
        {
            ViewBag.Title = "Məhsullar";
            homeView.AdminPanel = db.getAdminPanel();//Navbar menu
            homeView.SubProductsCategory = db.getSubProducts();//Menu Categoriya
            homeView.ProductsCategorys = db.getProductsCategory();//Menu Sub categoriya
            topProduct = new TopProductSlide(db.getStockStrNewTop(), db.getStockStrEndirimTop(), ranProduct.getNewStockList(db.getStockStrList()));
            homeView.TopSlidePruduct = topProduct;
            homeView.StockList = db.getStockWherePrd(id);
            homeView.FooterList = db.getFooters();
            homeView.SocialMediaList = db.getSocialMedia();
            homeView.FeedbackEmail = db.getContactList().FirstOrDefault().Email;
            return View(homeView);
        }
        [HttpGet]
        public ActionResult GetProductDetail(int id)
        {
            if (id != 0)
            {
                SubImgAndDet subImgAndDet = new SubImgAndDet(new List<SlideMod>(db.getSlideList(id)), new List<ProductDetail>(db.getProductDetail(id)));
                if (subImgAndDet.SubDetail.Count != 0 && subImgAndDet.SubSlide.Count != 0)
                {
                    homeView.AdminPanel = db.getAdminPanel();
                    homeView.SubProductsCategory = db.getSubProducts();
                    homeView.ProductsCategorys = db.getProductsCategory();
                    topProduct = new TopProductSlide(db.getStockStrNewTop(), db.getStockStrEndirimTop(), ranProduct.getNewStockList(db.getStockStrList()));
                    homeView.TopSlidePruduct = topProduct;
                    homeView.FooterList = db.getFooters();
                    homeView.SubImageAndDetail = subImgAndDet;
                    homeView.FeedbackEmail = db.getContactList().FirstOrDefault().Email;
                    homeView.SocialMediaList = db.getSocialMedia();
                    Stock slctdStc = db.getStock(id);
                    slctdStc.Quantity = 1;
                    homeView.SelectedStock = slctdStc;
                    return View(homeView);
                }
                else
                    return RedirectToAction("Index");
            }
            else
                return HttpNotFound();
        }
        [HttpPost]
        public ActionResult SearchProduct(SearchInCategory srcPrd)
        {
            ViewBag.Title = "Axtarış";
            List<StockUser> stkFind = db.searchProduct(srcPrd);
            homeView.AdminPanel = db.getAdminPanel();//Navbar menu
            homeView.SubProductsCategory = db.getSubProducts();//Menu Categoriya
            homeView.ProductsCategorys = db.getProductsCategory();//Menu Sub categoriya
            topProduct = new TopProductSlide(db.getStockStrNewTop(), db.getStockStrEndirimTop(), ranProduct.getNewStockList(db.getStockStrList()));
            homeView.TopSlidePruduct = topProduct;
            homeView.StockList = stkFind;
            homeView.FooterList = db.getFooters();
            homeView.SocialMediaList = db.getSocialMedia();
            homeView.FeedbackEmail = db.getContactList().FirstOrDefault().Email;
            if (srcPrd != null && stkFind.Count != 0)
            {
                return View(homeView);
            }
            else
            {
                ViewBag.Result = "Sorğu üzrə “" + srcPrd.ProductName + "” heç nә tapılmadı";
                ViewBag.SubResult = "Sorğunu dәyişdirmәk üçün cәhd edin vә ya mәhsul kataloqundan istifadә edin";
                return View(homeView);
            }
        }
        public ActionResult Question(Question question)
        {
            return RedirectToAction("GetAbout");
        }
        [HttpGet]
        public ActionResult GetCart()
        {
            HttpCookie cookieInBrowser = Request.Cookies["ProductListCookie"];
            ViewBag.Title = "Səbət";
            homeView.AdminPanel = db.getAdminPanel();
            homeView.SubProductsCategory = db.getSubProducts();
            homeView.ProductsCategorys = db.getProductsCategory();
            topProduct = new TopProductSlide(db.getStockStrNewTop(), db.getStockStrEndirimTop(), ranProduct.getNewStockList(db.getStockStrList()));
            homeView.TopSlidePruduct = topProduct;
            homeView.FooterList = db.getFooters();
            homeView.SocialMediaList = db.getSocialMedia();
            homeView.FeedbackEmail = db.getContactList().FirstOrDefault().Email;
            if (cookieInBrowser != null)
            {
                string[] cookieSplit = cookieInBrowser["id"].Split(',');
                List<Stock> stockList = new List<Stock>();
                int convertedId;
                bool viewResult = false;
                for (int i = 0; i < cookieSplit.Length - 1; i += 2)
                {
                    bool result = int.TryParse(cookieSplit[i], out convertedId);
                    if (result)
                    {
                        Stock selectedStock = db.getStock(Convert.ToInt32(cookieSplit[i]));
                        selectedStock.Quantity = Convert.ToInt32(cookieSplit[i + 1]);
                        stockList.Add(selectedStock);
                        viewResult = true;
                    }
                    else
                    {
                        homeView.FeedBackMessage = "Xəta";
                        break;
                    }
                }
                if (viewResult)
                    homeView.StockIdList = stockList;
            }
            else
                homeView.FeedBackMessage = "Sizin səbət artıq boşdur";
            return View(homeView);
        }
        [HttpPost]
        public ActionResult AddToCart(Stock stIdandQuantity)
        {
            HttpCookie cookieInBrowser = Request.Cookies["ProductListCookie"];
            string cookieDb = string.Empty;
            if (cookieInBrowser != null)
            {
                cookieDb = cookieInBrowser["id"].ToString();
                string[] cookieSplit = cookieInBrowser["id"].Split(',');
                int cookieDbItemResult = 0;
                for (int i = 0; i < cookieSplit.Length - 1; i += 2)
                {
                    if (cookieSplit[i] == stIdandQuantity.Id.ToString())
                    {
                        cookieDbItemResult++;
                        break;
                    }
                }
                if (cookieDbItemResult == 0)
                {
                    HttpCookie cookie = new HttpCookie("ProductListCookie");
                    cookie["id"] = cookieDb + "" + stIdandQuantity.Id.ToString() + "," + stIdandQuantity.Quantity + ",";
                    cookie.Expires = DateTime.Now.AddDays(5);
                    Response.Cookies.Add(cookie);
                }
                else
                    homeView.FeedBackMessage = "Seçdiginiz məhsul artıq səbətdədir";
            }
            else
            {
                HttpCookie cookie = new HttpCookie("ProductListCookie");
                cookie["id"] = stIdandQuantity.Id.ToString() + "," + stIdandQuantity.Quantity + ",";
                cookie.Expires = DateTime.Now.AddDays(5);
                Response.Cookies.Add(cookie);
            }
            return RedirectToAction("GetCart");
        }
        [HttpPost]
        public ActionResult CheckOut(Customer customer)
        {
            HttpCookie cookieInBrowser = Request.Cookies["ProductListCookie"];
            JavaScriptSerializer jsonSrz = new JavaScriptSerializer();
            int customerId = db.insertCustomer(customer);
            if (cookieInBrowser != null && customerId != 0)
            {
                string[] cookieSplit = cookieInBrowser["id"].Split(',');
                List<Stock> stockList = new List<Stock>();

                for (int i = 0; i < cookieSplit.Length - 1; i += 2)
                {
                    Stock selectedStock = db.getStock(Convert.ToInt32(cookieSplit[i]));
                    if (selectedStock != null)
                    {
                        selectedStock.Quantity = Convert.ToInt32(cookieSplit[i + 1]);
                        stockList.Add(selectedStock);
                    }
                }
                string jsonResult;
                if (stockList != null)
                {
                    jsonResult = jsonSrz.Serialize(stockList);
                    Purchase prcs = new Purchase()
                    {
                        CustomerId = customerId,
                        ProductList = jsonResult
                    };
                    bool result = db.insertPurchase(prcs);
                    if (result)
                    {
                        cookieInBrowser.Expires = DateTime.Now.AddDays(-5);
                        Response.Cookies.Add(cookieInBrowser);
                        homeView.FeedBackMessage = "Bizi seçdiyiniz üçün sizə təşşəkür edirik";
                        return RedirectToAction("GetCart");
                    }
                }

            }
            return View();
        }
        [HttpGet]
        public ActionResult RemoveItem(int id)
        {
            HttpCookie cookieInBrowser = Request.Cookies["ProductListCookie"];
            if (cookieInBrowser != null)
            {
                List<string> cookieList = new List<string>();
                cookieList.AddRange(cookieInBrowser["id"].Split(','));
                cookieList.RemoveAt(cookieList.Count - 1);
                int removedItem = cookieList.FindIndex(m => m == id.ToString());
                if ((removedItem == 0) || (removedItem != -1 && removedItem % 2 == 0))
                {
                    cookieList.RemoveRange(removedItem, 2);
                }
                string result = null;
                for (int i = 0; i < cookieList.Count; i++)
                    result = result + cookieList[i] + ",";
                HttpCookie addCookieinPage = new HttpCookie("ProductListCookie");
                if (result != null)
                {
                    addCookieinPage["id"] = result;
                    addCookieinPage.Expires = DateTime.Now.AddDays(5);
                    Response.Cookies.Add(addCookieinPage);
                }
                else
                {
                    addCookieinPage.Expires = DateTime.Now.AddDays(-5);
                    Response.Cookies.Add(addCookieinPage);
                }
            }
            return RedirectToAction("GetCart");

        }
        [HttpGet]
        public ActionResult CleanCart()
        {
            HttpCookie cookieInBrowser = Request.Cookies["ProductListCookie"];
            if (cookieInBrowser != null)
                cookieInBrowser.Expires = DateTime.Now.AddDays(-5);
            Response.Cookies.Add(cookieInBrowser);
            return RedirectToAction("GetCart");
        }
    }
}