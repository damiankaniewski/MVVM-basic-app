using MVVMlab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMlab.Validators
{
    public class ItemNameSpecification : ISpecification<Item>
    {
        public bool IsSatisfiedBy(Item item)
        {
            return !string.IsNullOrWhiteSpace(item.Name);
        }
    }

    public class ItemPriceSpecification : ISpecification<Item>
    {
        public bool IsSatisfiedBy(Item item)
        {
            return item.Price > 0;
        }
    }

    public class ItemCategorySpecification : ISpecification<Item>
    {
        public bool IsSatisfiedBy(Item item)
        {
            return !string.IsNullOrWhiteSpace(item.Category);
        }
    }

    public class ItemQuantitySpecification : ISpecification<Item>
    {
        public bool IsSatisfiedBy(Item item)
        {
            return item.Quantity > 0;
        }
    }
}
