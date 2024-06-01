using MVVMlab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMlab.Validators
{
    public class Validator
    {
        public List<string> ValidateNewItem(NewItem item)
        {
            var errorMessages = new List<string>();

            var specifications = new List<(ISpecification<NewItem> spec, string errorMessage)>
    {
        (new NewItemNameSpecification(), "Wprowadź nazwę produktu."),
        (new NewItemPriceSpecification(), "Wprowadź poprawną cenę produktu."),
        (new NewItemCategorySpecification(), "Wprowadź kategorię produktu."),
        (new NewItemQuantitySpecification(), "Wprowadź poprawną ilość produktu.")
    };

            foreach (var (spec, errorMessage) in specifications)
            {
                if (!spec.IsSatisfiedBy(item))
                    errorMessages.Add(errorMessage);
            }

            return errorMessages;
        }

        public List<string> ValidateItem(Item item)
        {
            var errorMessages = new List<string>();

            var specifications = new List<(ISpecification<Item> spec, string errorMessage)>
    {
        (new ItemNameSpecification(), "Wprowadź nazwę produktu."),
        (new ItemPriceSpecification(), "Wprowadź poprawną cenę produktu."),
        (new ItemCategorySpecification(), "Wprowadź kategorię produktu."),
        (new ItemQuantitySpecification(), "Wprowadź poprawną ilość produktu.")
    };

            foreach (var (spec, errorMessage) in specifications)
            {
                if (!spec.IsSatisfiedBy(item))
                    errorMessages.Add(errorMessage);
            }

            return errorMessages;
        }
    }
}
