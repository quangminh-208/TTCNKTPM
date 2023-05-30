using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDT.Models;
using WebDT.Models.EF;

namespace WebDT.Areas.admin.Controllers
{
    public class CategoryController : Controller
    {
        private WebMayTinhEntities db = new WebMayTinhEntities();
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var model = db.Categories.OrderByDescending(x => x.id).ToPagedList(page, pageSize);
            return View(model);
        }

        //Xóa tài khoản
        public JsonResult Delete(long ID)
        {

            try
            {
                var category = db.Categories.Find(ID);
            //Xóa file cũ
                var file = Request.MapPath("~" + category.link);
                System.IO.File.Delete(file);
                db.Categories.Remove(category);
                db.SaveChanges();
                return Json(new
                {
                    status = true
                });
            }
            catch
            {
                return Json(new
                {
                    status = false
                });
            }

        }


        [HttpPost]
        public ActionResult addCategory(Category entity, HttpPostedFileBase img)
        {
            try
            {
                var prv = new Category();
                prv.name = entity.name;
                prv.meta = Str_Metatitle(entity.name);
                prv.hide = true;
                prv.datebegin = DateTime.Now;

                var path = Path.Combine(Server.MapPath("/Content/img"), img.FileName);
                if (System.IO.File.Exists(path))
                {
                    string extensionName = Path.GetExtension(img.FileName);
                    string filename = img.FileName + DateTime.Now.ToString("ddMMyyyy") + extensionName;
                    path = Path.Combine(Server.MapPath("/Content/img"), filename);
                    img.SaveAs(path);
                    prv.link = "/Content/img/" + filename;
                }
                else
                {
                    img.SaveAs(path);
                    prv.link = "/Content/img/" + img.FileName;
                }

                db.Categories.Add(prv);
                db.SaveChanges();
                TempData["add_success"] = "Thêm danh mục sản phẩm thành công";
                return RedirectToAction("Index");

            }
            catch
            {
                TempData["add_success"] = "Thêm danh mục sản phẩm KHÔNG thành công";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult editCategory(Category entity, HttpPostedFileBase img)
        {
            try
            {
                var prv = db.Categories.Find(entity.id);
                
                if (img != null )
                {
                    var OriginLink = "/Content/img/" + img.FileName;
                    if (prv.link != OriginLink)
                    {
                        System.IO.File.Delete(Request.MapPath("~" + prv.link));
                        //Thêm hình ảnh
                        var path = Path.Combine(Server.MapPath("/Content/img"), img.FileName);
                        if (System.IO.File.Exists(path))
                        {
                            string extensionName = Path.GetExtension(img.FileName);
                            string filename = img.FileName + DateTime.Now.ToString("ddMMyyyy") + extensionName;
                            path = Path.Combine(Server.MapPath("/Content/img"), filename);
                            img.SaveAs(path);
                            prv.link = "/Content/img/" + filename;
                        }
                        else
                        {
                            img.SaveAs(path);
                            prv.link = "/Content/img/" + img.FileName;
                        }
                    }
                }
                prv.name = entity.name;
                prv.meta = Str_Metatitle(entity.name);
                db.SaveChanges();
                TempData["add_success"] = "Sửa danh mục sản phẩm thành công";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["add_success"] = "Sửa danh mục sản phẩm KHÔNG thành công";
                return RedirectToAction("Index");
            }
        }

        public JsonResult GetCategoryByID(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var category = db.Categories.Find(ID);
            return Json(category, JsonRequestBehavior.AllowGet);
        }


        public string Str_Metatitle(string str)
        {
            string[] VietNamChar = new string[]
            {
                "aAeEoOuUiIdDyY",
                "áàạảãâấầậẩẫăắằặẳẵ",
                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ",
                "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ",
                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ",
                "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ",
                "ÍÌỊỈĨ",
                "đ",
                "Đ",
                "ýỳỵỷỹ",
                "ÝỲỴỶỸ:/"
            };
            //Thay thế và lọc dấu từng char      
            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                    str = str.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
            }
            string str1 = str.ToLower();
            string[] name = str1.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string meta = null;
            //Thêm dấu '-'
            foreach (var item in name)
            {
                meta = meta + item + "-";
            }
            return meta;
        }
    }
}