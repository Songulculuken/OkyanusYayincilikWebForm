using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using OkyanusYayincilikWebForm.Models;
namespace OkyanusYayincilikWebForm.Controllers
{
    public class HomeController : Controller
    {
      
        soncuEntities db = new soncuEntities();
        Class1 cs = new Class1();
        public ActionResult Order()
        {
          
            cs.City = new SelectList(db.City, "IlId", "IlAdi");
            cs.Town = new SelectList(db.Town, "IlceId", "IlceAdi");
            cs.Institution = new SelectList(db.Kurumlar, "KurumId", "KurumAdi");
            cs.Product = new SelectList(db.Products, "UrunId", "Name");
            return View(cs);
            
        }
        public JsonResult townGet(int cty)
        {
            var town = (from t in db.Town
                        join c in db.City on t.IlId equals c.IlId
                        where t.IlId==cty
                        select new

                        {
                            Text = t.IlceAdi,
                            Value = t.IlceId.ToString()
                        }).ToList(); 
            return Json(town, JsonRequestBehavior.AllowGet);    
        }
        public JsonResult institutionGet(int ins)
        {
            var institution = (from i in db.Kurumlar
                               join t in db.Town on i.IlceId equals t.IlceId
                               where i.IlceId == ins
                               select new

                               {
                                   Text = i.KurumAdi,
                                   Value = i.Kategori
                                   


                               }).ToList();
            return Json(institution, JsonRequestBehavior.AllowGet);
        }
        public JsonResult productGet(string pro)
        {
            
            var product = (from p in db.Products
                               where p.Kategori == pro
                               select new

                               {
                                   Text = p.Name,
                                   Value = p.UrunId.ToString()



                               }).ToList();

            return Json(product, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Order(Class1 class1)
        {
            Orders or = class1.orders;
            
            var order = (from o in db.Orders where o.customerNameSurname == or.customerNameSurname /*&& o.customerPhone == or.customerPhone*/ select o).ToList();

            if (order.Count == 0)
            {
                int i = int.Parse(or.Il);
                var city = (from o in db.City where o.IlId == i select o).ToList();
                or.Il = city.First().IlAdi;
                int il = int.Parse(or.Ilce);
                var town = (from o in db.Town where o.IlceId == il select o).ToList();
                or.Ilce = town.First().IlceAdi;

                db.sp_OrdrInsert(or.orderDate, or.customerNameSurname, or.customerPhone, or.Il, or.Ilce, or.KurumId, or.UrunId);
                ViewBag.mesaj = "İşlem Başarılı";
            }
            else
            { 

                ViewBag.mesaj = "İşlem Başarısız.Kullanıcı Tekrar Sipariş Veremez";
            }
            cs.City = new SelectList(db.City, "IlId", "IlAdi");
            cs.Town = new SelectList(db.Town, "IlceId", "IlceAdi");
            cs.Institution = new SelectList(db.Kurumlar, "KurumId", "KurumAdi");
            cs.Product = new SelectList(db.Products, "UrunId", "Name");
            return View(cs);
        }



    }
}