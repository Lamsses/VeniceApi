using EFDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Dto
{
    public class ProductDtoUpdate
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public bool IsVisible { get; set; } = true;

        public int Quantity { get; set; }
        public ProductType Type { get; set; }
        public string PicturePath { get; set; }

        public int CategoryId { get; set; }

    }

}
