﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public IEnumerable<ShoppingCart>  ListCart { get; set; }
        public OrderHeader OrderHeader { get; set; }
        public double CartTotal { get; set; }
    }
}
