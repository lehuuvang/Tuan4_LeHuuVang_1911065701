using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tuan4_LeHuuVang_1911065701.Models
{
    public class GioHang
    {
       MyDataDataContext data = new MyDataDataContext();
        public int masach { get; set; }
        [Display(Name ="Tên Sách")]
        public string tensach { get; set; }
        [Display(Name = "Ảnh Bìa")]
        public string hinh { get; set; }
        [Display(Name = "Giá Bán")]
        public double giaban { get; set; }
        [Display(Name = "Số Lượng")]
        [Required]
        public int isoluong { get; set; }
        [Display(Name = "Thành Tiền")]
        public double dthanhtien { 
            get { 
                return isoluong * giaban; 
                }
            }
        public GioHang(int id)
        {
            masach = id;
            Sach sach = data.Saches.Single(m => m.masach == masach);
            tensach = sach.tensach;
            hinh = sach.hinh;
            giaban = double.Parse(sach.giaban.ToString());
            isoluong = 1;
        }
    }
}