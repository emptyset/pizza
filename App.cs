using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Refactoring
{
	class Program
	{
		static void Main(string[] args)
		{
			var order = new Order("John Doe");
			order.AddItem(new WeighedProductItem {
				ProductName = "Pulled Pork",
				Price = 6.99m,
				Weight = 0.5m
			});
			order.AddItem(new UnitProductItem {
				ProductName = "Coke",
				Price = 3m,
				Quantity = 2
			});
			
			Console.WriteLine(order.toSummary());
			Console.ReadKey();
		}
	}

	public class Order {
		public String CustomerName;
		private List<ProductItem> items;
		public decimal Total {
			get { 
				return items.Sum(item => item.CalculateItemPrice());
			}
		}

		public Order(String customerName) {
			this.CustomerName = customerName;
			this.items = new List<ProductItem>();
		}

		public void AddItem(ProductItem item) {
			items.Add(item);
		}

		public String toSummary() {
			StringBuilder builder = new StringBuilder();
			builder.AppendLine("ORDER SUMMARY FOR " + CustomerName + ": ");
			items.ForEach(item => builder.AppendLine(item.Description));
			builder.AppendLine().AppendLine(String.Format("Total Price: {0:C}", Total));

			return builder.ToString();
		}		
	}

	public abstract class ProductItem
	{
		// TODO: break out accessors/modifiers
		public string ProductName;
		public decimal Price;

		// TODO: review error handling on nullable types
		public decimal? Weight;
		public int? Quantity;

		protected abstract String descriptionFormat { get; }
		public abstract String Description { get; }

		public abstract decimal CalculateItemPrice();
	}

	public class WeighedProductItem : ProductItem {
		protected override String descriptionFormat { get { return "{0} {1:C} ({2} pounds at {3:C} per pound)"; } }
		public override String Description {
			get {
				return String.Format(descriptionFormat, new Object[] { ProductName, CalculateItemPrice(), Weight.Value, Price });
			}
		}

		public override decimal CalculateItemPrice() {
			return Weight.Value * Price;
		}
	}

	public class UnitProductItem : ProductItem {
		protected override String descriptionFormat { get { return "{0} {1:C} ({2} items at {3:C} each)"; } }
		public override String Description {
			get {
				return String.Format(descriptionFormat, new Object[] { ProductName, CalculateItemPrice(), Quantity.Value, Price });
			}
		}

		public override decimal CalculateItemPrice() {
			return Quantity.Value * Price;
		}
	}
}
