using System.Collections.Generic;
using System.Linq.Expressions;
using NSubstitute;
using NUnit.Framework;

namespace IsolatedByInheritanceAndOverride.Test
{
    /// <summary>
    /// OrderServiceTest 的摘要描述
    /// </summary>
    [TestFixture]
    public class OrderServiceTest
    {
        [Test]
        public void Test_SyncBookOrders_3_Orders_Only_2_book_order()
        {
            // hard to isolate dependency to unit test

            var target = new FakeOrderService();
            target.SetOrder(new List<Order>()
            {
                new Order(){Type = "Book"},
                new Order(){Type = "CD"},
                new Order(){Type = "Book"}
            });

            var bookDao = Substitute.For<IBook>();
            target.SetBookDao(bookDao);

            target.SyncBookOrders();
            bookDao.Received(2).Insert(Arg.Is<Order>(x => x.Type == "Book"));
        }

        public class FakeOrderService : OrderService
        {
            private List<Order> _orders;
            private IBook _book;
           
            public void SetOrder(List<Order> orders)
            {
                _orders = orders;
            }

            public void SetBookDao(IBook book)
            {
                _book = book;
            }
            protected override List<Order> GetOrders()
            {
                return _orders;
            }

            protected override IBook GetBookDao()
            {
                return _book;
            }
        }
    }
}