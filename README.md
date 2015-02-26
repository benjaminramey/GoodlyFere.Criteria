# GoodlyFere.Criteria

General library of specification pattern interfaces and classes to build criteria from.

## Example Usage

Encase your business rules in criteria classes (specifications).  I recommend making
one criteria class for one business rule.  You can can combine these rules for more
complex logic.

For example, take two `Invoice` specifications:

    public class PastDue : BaseCriteria<Invoice>
    {
        public override Expression<Func<T, bool>> Satisfier
        {
            get
            {
                return invoice => invoice.DueDate < DateTime.Now;
            }
        }

        public override void Accept(ICriteriaVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }
    
    public class Unpaid : BaseCriteria<Invoice>
    {
        public override Expression<Func<T, bool>> Satisfier
        {
            get
            {
                return invoice => !invoice.Paid;
            }
        }

        public override void Accept(ICriteriaVisitor<T> visitor)
        {
            visitor.Visit(this);
        }
    }

To get all invoices that are past due and unpaid, you can do something like the following:

    ICriteria<Invoice> criteria = new PastDue().And(new Unpaid());
    IList<Invoice> invoices = new List<Invoice>() {
        new Invoice { DueDate = DateTime.Now.AddDays(-1), Unpaid = true },
        new Invoice { DueDate = DateTime.Now.AddDays(2), Unpaid = false },
        new Invoice { DueDate = DateTime.Now.AddDays(10), Unpaid = true },
    };

    List<Invoice> pastDueUnpaidInvoices = new List<Invoice>();
    foreach(Invoice invoice in invoices)
    {
        if (criteria.IsSatisfiedBy(invoice))
        {
            pastDueUnpaidInvoices.Add(invoice);
        }
    }

The `pastDueUnpaidInvoices` list will now contain only the first invoice in this list.

## Combining with Repository pattern
I build my repositories with `Find` methods that take `ICriteria<T>` classes.  I can then
use the `ICriteria<T>.Satisfier` property which is an expression of type `Expression<Func<T,bool>>`
if my data layer supports LINQ for querying.  

See this [gist](https://gist.github.com/benjaminramey/9421272) for an example repository interface that takes `ICriteria<T>`
parameters for the Find methods.

## Version History
- 1.0.3.0: override of ToString method in BaseCriteria
- 1.0.2.0: implemented Accept method in BaseCriteria
- 1.0.0.6: Added .NET 3.5 assembly to nuget package
- 1.0.0.0: initial release, base set of criteria