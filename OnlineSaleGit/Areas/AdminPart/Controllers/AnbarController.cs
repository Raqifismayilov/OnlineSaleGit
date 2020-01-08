using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using OnlineSale.Areas.AdminPart.Models.OnlineSaleTable;
using OnlineSale.Areas.AdminPart.Models.OnlineSaleData;
using OnlineSale.Areas.AdminPart.Models.ModelView;

namespace OnlineSale.Areas.AdminPart.Controllers
{
    public class AnbarController : Controller
    {
        OnlineSaleDB db = new OnlineSaleDB("OnlineSale");
        StockView stockView = new StockView();
        //ProductDetail productDetail = new ProductDetail();
        public ActionResult GetAnbar()
        {
            if (User.Identity.IsAuthenticated)
            {
                stockView.AdminPanel = db.getAdminPanel();
                stockView.StockList = db.getStockList();
                stockView.prdSubCategorylist = db.getSubProducts();
                stockView.PrdCategory = db.getProductsCategory();
                stockView.ColorList = db.getColors();
                stockView.EndrimList = db.getEndrims();
                stockView.ValutaList = db.getValutas();
                return View(stockView);
            }
            else
                return RedirectToAction("Login", "Logon");
        }
        [HttpGet]
        public ActionResult AddAnbar()
        {
            if (User.Identity.IsAuthenticated)
            {
                stockView.AdminPanel = db.getAdminPanel();
                stockView.StockList = db.getStockList();
                stockView.prdSubCategorylist = db.getSubProducts();
                stockView.ColorList = db.getColors();
                stockView.EndrimList = db.getEndrims();
                stockView.ValutaList = db.getValutas();
                stockView.Stock = new Stock();
                return View(stockView);
            }
            else return RedirectToAction("Login", "Logon");
        }
        [HttpPost]
        public ActionResult AddAnbar(HttpPostedFileBase AnbarUploadFile)
        {
            if (User.Identity.IsAuthenticated)
            {
                string uploadFileName = "noimage.jpg";
                if (AnbarUploadFile != null && AnbarUploadFile.ContentLength != 0 && AnbarUploadFile.FileName != "noimage.jpg")
                    uploadFileName = DateTime.Now.ToString("yyyy_MM_dd") + Path.GetFileName(AnbarUploadFile.FileName);
                Stock stock = new Stock();
                stock.Id = Convert.ToInt32(Request.Form["Id"].ToString());
                stock.ProductName = Request.Form["ProductName"].ToString();
                stock.SubProductCategoryId = Convert.ToInt32(Request.Form["AnbarSubPrdSelect"].ToString());
                stock.SubColorId = Convert.ToInt32(Request.Form["AnbarColorSelect"].ToString());
                stock.MainPhotoPath = uploadFileName;
                stock.Price = Convert.ToDouble(Request.Form["Price"].ToString().Replace('.', ','));
                stock.SubValutaId = Convert.ToInt32(Request.Form["AnbarValutaSelect"].ToString());
                stock.Quantity = Convert.ToInt32(Request.Form["Quantity"].ToString());
                stock.Endirim = Convert.ToDouble(Request.Form["Endirim"].ToString().Replace('.', ','));
                int result;
                if (int.TryParse(Request.Form["AnbarEndirimSelect"], out result))
                    stock.SubEndirimId = result;
                else
                    stock.SubEndirimId = null;
                stock.RowsNumber = Convert.ToInt32(Request.Form["RowsNumber"].ToString());
                if (Request.Form["ProductCondition"].ToString() == "1")
                    stock.ProductCondition = true;
                else stock.ProductCondition = false;
                stock.ProductCode = Request.Form["ProductCode"].ToString();
                if (stock != null)
                {
                    if (stock.Id == 0)
                    {
                        stock.ProductId = 1;
                        stock.SubEndirimId = 2;
                        bool insertResult = db.insertStock(stock);
                        if (insertResult)
                        {
                            if (uploadFileName != "noimage.jpg")
                                AnbarUploadFile.SaveAs(Server.MapPath("~/Content/ProductMainImg/" + uploadFileName));
                            return RedirectToAction("GetAnbar");
                        }
                        else return HttpNotFound();
                    }
                    else
                    {
                        bool updateResult = db.updateStock(stock);
                        if (updateResult)
                        {
                            try
                            {
                                string exsistFile = Server.MapPath("~/Content/ProductMainImg/" + uploadFileName);
                                if (!System.IO.File.Exists(exsistFile) && uploadFileName != "noimage.jpg")
                                {
                                    AnbarUploadFile.SaveAs(exsistFile);
                                }
                                return RedirectToAction("GetAnbar");
                            }
                            catch (Exception)
                            {
                                throw new Exception("Error");
                            }
                        }
                        else
                            return HttpNotFound();
                    }
                }
                else
                    return HttpNotFound();
            }
            else return RedirectToAction("Login", "Logon");
        }
        public ActionResult UpdateAnbar(int Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (Id != 0)
                {
                    if ((stockView.Stock = db.getStock(Id)) != null)
                    {
                        stockView.AdminPanel = db.getAdminPanel();
                        stockView.EndrimList = db.getEndrims();
                        stockView.ValutaList = db.getValutas();
                        stockView.ColorList = db.getColors();
                        stockView.prdSubCategorylist = db.getSubProducts();
                        return View("AddAnbar", stockView);
                    }
                    else
                        return HttpNotFound();
                }
                else
                    return HttpNotFound();
            }
            else return RedirectToAction("Login", "Logon");
        }
        public ActionResult DeleteAnbar(int Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                Stock deleteStock = db.getStock(Id) as Stock;
                if (Id != 0)
                {
                    bool result = db.deleteStock(Id);
                    if (result)
                    {
                        string exsistFile = Server.MapPath("~/Content/ProductMainImg/" + deleteStock.MainPhotoPath);
                        if (System.IO.File.Exists(exsistFile))
                        {
                            System.IO.File.Delete(exsistFile);
                        }
                        return RedirectToAction("GetAnbar");
                    }
                    else
                        return HttpNotFound();
                }
                else
                    return HttpNotFound();
            }
            else return RedirectToAction("Login", "Logon");
        }
        [HttpGet]
        public ActionResult SearchInAnbar(string product)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<Stock> st = db.searchInStock(product.Trim());
                stockView.StockList = st;
                stockView.AdminPanel = db.getAdminPanel();
                stockView.prdSubCategorylist = db.getSubProducts();
                stockView.ColorList = db.getColors();
                stockView.EndrimList = db.getEndrims();
                stockView.ValutaList = db.getValutas();
                return View("GetAnbar", stockView);
            }
            else return RedirectToAction("Login", "Logon");
        }
        public ActionResult GetSubImage(int stockId)
        {
            if (User.Identity.IsAuthenticated)
            {
                stockView.AdminPanel = db.getAdminPanel();
                List<SlideMod> slideList = db.getSlideList(stockId);
                stockView.SlideList = slideList;
                stockView.PrdCategory = db.getProductsCategory();
                stockView.prdSubCategorylist = db.getSubProducts();
                stockView.Slide = new SlideMod() {StockId = stockId };
                stockView.SubImageCount = slideList.Count;
                return View(stockView);
            }
            else return RedirectToAction("Login", "Logon");
        }
        [HttpPost]
        public ActionResult SubImageAdd(SlideMod slide, HttpPostedFileBase PhotoPath)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (slide != null && PhotoPath != null)
                {
                    string imagePath = (DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second + "" + PhotoPath.FileName).ToString();
                    bool result;
                    if (slide.Id == 0)
                    {
                        slide.PhotoPath = imagePath;
                        result = db.insertSlide(slide);
                        if (result)
                        {
                            PhotoPath.SaveAs(Server.MapPath("/Content/SlideImage/" + imagePath));
                            return RedirectToAction("GetSubImage", new { stockId = slide.StockId });
                        }
                        else return HttpNotFound();

                    }
                    else
                    {
                        slide.PhotoPath = imagePath;
                        result = db.updateSlide(slide);
                        if (result)
                        {
                            if (System.IO.File.Exists(Server.MapPath("/Content/SlideIamge/" + ViewBag.SubImgForDelete)))
                            {
                                System.IO.File.Delete(Server.MapPath("/Content/SlideIamge/" + ViewBag.SubImgForDelete));
                            }
                            else
                                return HttpNotFound();
                            return HttpNotFound();
                        }
                        else return HttpNotFound();

                    }
                }
                else
                    return HttpNotFound();
            }
            else return RedirectToAction("Login", "Logon");
        }
        [HttpGet]
        public ActionResult SubImageUpdate(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id != 0)
                {
                    SlideMod slideUpdate = db.getSlide(id) as SlideMod;
                    if (slideUpdate != null)
                    {
                        stockView.AdminPanel = db.getAdminPanel();
                        stockView.SlideList = db.getSlideList(slideUpdate.StockId);
                        stockView.PrdCategory = db.getProductsCategory();
                        stockView.prdSubCategorylist = db.getSubProducts();
                        stockView.Slide = slideUpdate;
                        stockView.SubImageCount = stockView.SlideList.Count;
                        ViewBag.SubImgForDelete = slideUpdate.PhotoPath;
                        return View("GetSubImage", stockView);
                    }
                    else
                        return HttpNotFound();
                }
                else
                    return HttpNotFound();
            }
            else return RedirectToAction("Login", "Logon");
        }
        [HttpGet]
        public ActionResult DeleteSubImage(int Id, int StockId)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (Id != 0)
                {
                    SlideMod slShow = db.getSlide(Id) as SlideMod;
                    bool result = db.deleteSubImage(Id);
                    if (result && slShow != null)
                    {
                        string imgPath = Server.MapPath("~/Content/SlideImage/" + slShow.PhotoPath);
                        if (System.IO.File.Exists(imgPath))
                            System.IO.File.Delete(imgPath);
                        else
                            return RedirectToAction("GetSubImage", new { Id = StockId });
                    }
                    else
                        HttpNotFound();
                }
                return HttpNotFound();
            }
            else
                return RedirectToAction("Login", "Logon");
        }
        [HttpGet]
        public ActionResult GetDetail(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                stockView.AdminPanel = db.getAdminPanel();
                stockView.productsDetails = db.getProductDetail(id);
                stockView.SubPrdDetail = new ProductDetail() { StockId = id };
                return View(stockView);
            }
            else return RedirectToAction("Login", "Logon");
        }
        [HttpPost]
        public ActionResult AddDetail(ProductDetail prdDetail)
        {
            if (User.Identity.IsAuthenticated)
            {
                bool result;
                if (prdDetail != null && !string.IsNullOrEmpty(prdDetail.Name) && !string.IsNullOrEmpty(prdDetail.Values))
                {
                    if (prdDetail.ProductId == 0)
                        result = db.insertProductDetail(prdDetail);
                    else
                        result = db.updateProductDetail(prdDetail);
                    if (result)
                        return RedirectToAction("GetDetail", new { id = prdDetail.StockId });
                    else
                        return HttpNotFound();
                }
                return RedirectToAction("GetDetail", new { id = prdDetail.StockId });
            }
            else return RedirectToAction("Login", "Logon");
        }
        public ActionResult UpdateDetail(int ProductId)
        {
            if (ProductId != 0)
            {
                if (db.getSubPrdDetail(ProductId) is ProductDetail updPrdctDetail)
                {
                    int stockId = updPrdctDetail.StockId;
                    stockView.AdminPanel = db.getAdminPanel();
                    stockView.productsDetails = db.getProductDetail(stockId);
                    stockView.SubPrdDetail = updPrdctDetail;
                    return View("GetDetail", stockView);
                }
                else
                    return HttpNotFound();
            }
            else
                return HttpNotFound();
        }
        public ActionResult DeleteDetail(int ProductId, int stockId)
        {
            if (ProductId != 0 && stockId != 0)
            {
                bool result = db.deleteProductDetail(ProductId);
                if (result)
                {
                    string redMethod = string.Format("GetDetail/{0}", stockId);
                    return RedirectToAction(redMethod);
                }
                return HttpNotFound();
            }
            else
                return HttpNotFound();
        }

    }
}