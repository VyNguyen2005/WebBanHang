using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanHang.Models;

namespace WebBanHang.Areas.Customer.Models
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
    public class Cart
    {
        private List<CartItem> _items;
        public Cart()
        {
            _items = new List<CartItem>();
        }
        public List<CartItem> Items { get { return _items; } }
        public void Add(Product p, int quantity)
        {
            var item = _items.FirstOrDefault(x => x.Product.Id == p.Id);
            if(item == null)
            {
                _items.Add(new CartItem { Product = p, Quantity = quantity });
            }
            else
            {
                item.Quantity += quantity;
            }
        }
        public void Update(int productId, int quantity)
        {
            var item = _items.FirstOrDefault(x => x.Product.Id == productId);
            if(item != null)
            {
                if(quantity > 0)
                {
                    item.Quantity = quantity;
                }
                else
                {
                    _items.Remove(item);
                }
            }
        }
        public void Remove(int productId)
        {
            var item = _items.FirstOrDefault(x => x.Product.Id == productId);
            if(item != null)
            {
                _items.Remove(item);
            }
        }
        public double Total { get {
                double total = _items.Sum(x => x.Quantity * x.Product.Price);
                return total;
            } }
        public double Quantity { get {
                int total = _items.Sum(x => x.Quantity);
                return total;
            } }
    }
}
