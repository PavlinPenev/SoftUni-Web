﻿namespace Store_Ge.Data.Models
{
    public class OrderProduct
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}
