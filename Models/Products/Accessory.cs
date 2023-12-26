using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace PaintShopMVC.Models.Products
{
    public class Accessory : Product
    {
       
        public AccessoryCategory? Category { get; set; }
        [Display(Name = "Nazwa kategorii")]
        public int? CategoryId { get; set; }
    }
}
