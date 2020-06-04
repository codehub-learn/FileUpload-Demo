using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Models
{
    public enum Category
    {
        ARTS,
        ENVIRONMENT,
        OTHER,
    }
    public class Product
    {
        public Category Category { get; set; }
        public string Name { get; set; }
    }

    public class ProductViewModel
    {
        public int ID { set; get; }
        public string Name { set; get; }
    }

    public class OrderViewModel
    {
        public int OrderNumber { set; get; }
        public List<ProductViewModel> Products { set; get; }
        public int SelectedProductId { set; get; }
    }
    public class ReturnModel
    {
        public int ReturnValue { set; get; }
    }

public class OrderModel
    {
        public int CustomerId { set; get; }
        public string Name{ set; get; }
    }


}
