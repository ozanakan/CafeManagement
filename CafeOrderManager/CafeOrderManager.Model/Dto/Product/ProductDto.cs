using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Enums;
using System;
using System.Collections.Generic;

namespace CafeOrderManager.Model.Dto.Product
{
    public class ProductDto : BaseDto
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int? StockQuantity { get; set; }
        public int CategoryId { get; set; }

    }
}