using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using Tuan4_LeHuuVang_1911065701.Models;

namespace Tuan4_LeHuuVang_1911065701.Controllers
{
    public class GioHangController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstgiohang = Session["Giohang"] as List<GioHang>;
            if (lstgiohang == null)
            {
                lstgiohang = new List<GioHang>();
                Session["Giohang"] = lstgiohang;
            }
            return lstgiohang;
        }
        public ActionResult ThemGioHang(int id, string strURL)
        {
            List<GioHang> lstGioHang = LayGioHang();
            var sach = data.Saches.FirstOrDefault(p => p.masach == id);
            GioHang sanpham = lstGioHang.Find(n => n.masach == id);
            if (sanpham == null)
            {
                sanpham = new GioHang(id);
                lstGioHang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                if (sanpham.isoluong < sach.soluongton)
                {
                    sanpham.isoluong++;
                    return Redirect(strURL);
                }
                else
                {
                    MessageBox.Show("Không đủ sách bán!!!");
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        private int TongSoLuong()
        {
            int tsl = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                tsl = lstGioHang.Sum(n => n.isoluong);
            }
            return tsl;
        }
        private int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Count;
            }
            return tsl;
        }
        private double TongTien()
        {
            double tt = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                tt = lstGiohang.Sum(n => n.dthanhtien);
            }
            return tt;
        }
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGioHang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return PartialView();
        }
        public ActionResult XoaGioHang(int id)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.masach == id);
            if (sanpham != null)
            {
                lstGioHang.RemoveAll(n => n.masach == id);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapNhatGioHang(int id, FormCollection collection)
        {
            List<GioHang> lstGioHang = LayGioHang();
            var sach = data.Saches.FirstOrDefault(p => p.masach == id);
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.masach == id);
            if (sanpham != null)
            {
                sanpham.isoluong = int.Parse(collection["txtsl"].ToString());
                if (sanpham.isoluong > sach.soluongton)
                {
                    MessageBox.Show("Không còn đủ sách để bán");
                    sanpham.isoluong = 1;
                }
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatCaGioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("GioHang");
        }
        public ActionResult DatHang()
        {
            List<GioHang> listGioHang = LayGioHang();
            var ds = listGioHang;

            foreach (var item in ds)
            {
                var change = data.Saches.Where(x => x.masach == item.masach).FirstOrDefault();
                if (change != null)
                {
                    if (change.soluongton >= item.isoluong)
                    {
                        var tempsl = change.soluongton - item.isoluong;
                        change.soluongton = tempsl;
                        UpdateModel(change);
                        data.SubmitChanges();
                    }
                }
            }
            return View(listGioHang);
            listGioHang.Clear();
        }
    }
}