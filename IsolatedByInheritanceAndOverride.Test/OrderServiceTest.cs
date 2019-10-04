using System.Collections.Generic;
using System.Linq;
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
        private FakeOrderService _target;
        private IBook _bookDao;

        [SetUp]
        public void Setup()
        {
            _target = new FakeOrderService();
            _bookDao = Substitute.For<IBook>();
            _target.SetBookDao(_bookDao);
        }

        [Test]
        public void Test_SyncBookOrders_3_Orders_Only_2_book_order()
        {
            // hard to isolate dependency to unit test
            GivenOrders(new Order() { Type = "Book" },
                        new Order() { Type = "CD" },
                        new Order() { Type = "Book" });
            WhenSyncOrders();
            BooksShouldHave(2);
        }

        private void BooksShouldHave(int times)
        {
            _bookDao.Received(times).Insert(Arg.Is<Order>(x => x.Type == "Book"));
        }

        private void WhenSyncOrders()
        {
            _target.SyncBookOrders();
        }

        private void GivenOrders(params Order[] orders)
        {
            _target.SetOrder(orders.ToList());
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