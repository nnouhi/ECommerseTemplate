using ECommerseTemplate.DataAccess.Data;
using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerseTemplate.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader orderHeader)
        {
            _db.Update(orderHeader);
        }

		public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
		{
            OrderHeader orderHeader = _db.OrderHeaders.FirstOrDefault(oh => oh.Id == id);
            if (orderHeader is not null)
            {
                orderHeader.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    orderHeader.PaymentStatus = paymentStatus;
                }
			}
		}

		public void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId)
		{
			OrderHeader orderHeader = _db.OrderHeaders.FirstOrDefault(oh => oh.Id == id);
			if (orderHeader is not null)
			{
                if (!string.IsNullOrEmpty(sessionId))
                { 
                    orderHeader.SessionId = sessionId;
                }

				if (!string.IsNullOrEmpty(paymentIntentId))
				{
					orderHeader.PaymentIntentId = paymentIntentId;
                    orderHeader.PaymentDate = DateTime.Now;
				}
			}
		}
	}
}
