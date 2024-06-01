using MVVMlab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMlab.Validators
{
    public class NewItemNameSpecification : ISpecification<NewItem>
    {
        public bool IsSatisfiedBy(NewItem item)
        {
            return !string.IsNullOrWhiteSpace(item.Name);
        }
    }

    public class NewItemPriceSpecification : ISpecification<NewItem>
    {
        public bool IsSatisfiedBy(NewItem item)
        {
            return item.Price > 0;
        }
    }

    public class NewItemCategorySpecification : ISpecification<NewItem>
    {
        public bool IsSatisfiedBy(NewItem item)
        {
            return !string.IsNullOrWhiteSpace(item.Category);
        }
    }

    public class NewItemQuantitySpecification : ISpecification<NewItem>
    {
        public bool IsSatisfiedBy(NewItem item)
        {
            return item.Quantity > 0;
        }
    }

}
