using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;
using ECommerseTemplate.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace ECommerseTemplate.Areas.Admin.Controllers
{
	[Area(SD.Roles.Admin)]
	[Authorize(Roles = SD.Roles.Admin)]
	public class OrderController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public OrderController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Details(int orderHeaderId)
		{
			OrderVM orderVM = new OrderVM()
			{
				OrderHeader = _unitOfWork.OrderHeader.Get(oh => oh.Id == orderHeaderId, includeProperties: "ApplicationUser"),
				OrderDetails = _unitOfWork.OrderDetails.GetAll(od => od.OrderHeaderId == orderHeaderId, includeProperties: "Product")
			};

			return View(orderVM);
		}

		[HttpPost]
		public IActionResult UpdateOrderDetail(OrderVM orderVM)
		{
			OrderHeader orderHeaderFromView = orderVM.OrderHeader;
			OrderHeader orderHeaderFromDb = _unitOfWork.OrderHeader.Get(oh => oh.Id == orderVM.OrderHeader.Id);
			orderHeaderFromDb.Name = orderHeaderFromView.Name;
			orderHeaderFromDb.PhoneNumber = orderHeaderFromView.PhoneNumber;
			orderHeaderFromDb.StreetAddress = orderHeaderFromView.StreetAddress;
			orderHeaderFromDb.City = orderHeaderFromView.City;
			orderHeaderFromDb.Country = orderHeaderFromView.Country;
			orderHeaderFromDb.PostalCode = orderHeaderFromView.PostalCode;

			_unitOfWork.OrderHeader.Update(orderHeaderFromDb);
			_unitOfWork.Save();
			TempData["success"] = "Order Details updated Succussfully";
			return RedirectToAction(nameof(Index), new { status = "all" });
		}

		[HttpPost]
		public IActionResult StartProcessing(OrderHeader orderHeader)
		{
			_unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.OrderStatuses.InProcess);
			_unitOfWork.Save();
			TempData["success"] = "Order Details updated Succussfully";
			return RedirectToAction(nameof(Details), new { orderHeaderId = orderHeader.Id });
		}

		[HttpPost]
		public IActionResult ShipOrder(OrderHeader orderHeader)
		{
			OrderHeader orderHeaderFromDb = _unitOfWork.OrderHeader.Get(oh => oh.Id == orderHeader.Id);
			if (orderHeaderFromDb == null)
			{
				return NotFound();
			}

			// Update fields from the view model
			orderHeaderFromDb.Name = orderHeader.Name;
			orderHeaderFromDb.PhoneNumber = orderHeader.PhoneNumber;
			orderHeaderFromDb.StreetAddress = orderHeader.StreetAddress;
			orderHeaderFromDb.City = orderHeader.City;
			orderHeaderFromDb.Country = orderHeader.Country;
			orderHeaderFromDb.PostalCode = orderHeader.PostalCode;
			orderHeaderFromDb.Carrier = orderHeader.Carrier;
			orderHeaderFromDb.TrackingNumber = orderHeader.TrackingNumber;

			orderHeaderFromDb.OrderStatus = SD.OrderStatuses.Shipped;
			orderHeaderFromDb.ShippingDate = DateTime.Now;
			if (orderHeader.PaymentStatus == SD.PaymentStatuses.DelayedPayment)
			{
				orderHeaderFromDb.PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));
			}

			_unitOfWork.OrderHeader.Update(orderHeaderFromDb);
			_unitOfWork.Save();

			TempData["Success"] = "Order Shipped Successfully";
			return RedirectToAction(nameof(Details), new { orderHeaderId = orderHeader.Id });
		}

		[HttpPost]
		public IActionResult Cancel(OrderHeader orderHeader)
		{
			if (orderHeader.PaymentStatus == SD.PaymentStatuses.Approved)
			{
				RefundCreateOptions options = new RefundCreateOptions
				{
					PaymentIntent = orderHeader.PaymentIntentId,
					Reason = RefundReasons.RequestedByCustomer
				};

				RefundService service = new RefundService();
				Refund refund = service.Create(options);
				if (refund.Status == "succeeded")
				{
					_unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.OrderStatuses.Cancelled, SD.PaymentStatuses.Refunded);
				}
			}
			else
			{
				_unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.OrderStatuses.Cancelled, SD.PaymentStatuses.Cancelled);
			}

			_unitOfWork.Save();
			TempData["Success"] = "Order Cancelled Successfully";
			return RedirectToAction(nameof(Details), new { orderHeaderId = orderHeader.Id });
		}

		[HttpPost]
		public IActionResult PayNow(OrderHeader orderHeader)
		{
			IEnumerable<OrderDetail> orderDetails = _unitOfWork.OrderDetails.GetAll(od => od.OrderHeaderId == orderHeader.Id, includeProperties: "Product");

			const string domain = SD.ApplicationSettings.ApplicationMode == "Development" ? SD.URLs.DevelopmentDomain : SD.URLs.ProductionDomain;
			SessionCreateOptions options = new SessionCreateOptions
			{
				SuccessUrl = domain + $"admin/order/PaymentConfirmation?orderHeaderId={orderHeader.Id}",
				CancelUrl = domain + $"admin/order/details?orderId={orderHeader.Id}",
				LineItems = new List<SessionLineItemOptions>(),
				Mode = "payment",
			};

			// Populate the SessionLineItems
			foreach (OrderDetail orderDetail in orderDetails)
			{
				SessionLineItemOptions sessionLineItem = new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmount = (long)(orderDetail.Price * 100), // 20.50 to 2050
						Currency = "eur",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = orderDetail.Product.Title
						}
					},
					Quantity = orderDetail.Count
				};

				// Add the line item to the options' list
				options.LineItems.Add(sessionLineItem);
			}

			SessionService service = new SessionService();
			Session session = service.Create(options);
			_unitOfWork.OrderHeader.UpdateStripePaymentId(orderHeader.Id, session.Id, session.PaymentIntentId);
			_unitOfWork.Save();

			// Redirect to the Stripe payment panel
			Response.Headers.Add("Location", session.Url);
			return new StatusCodeResult(303);
		}

		#region API CALLS
		[HttpGet]
		public IActionResult GetAll(string status)
		{
			List<OrderHeader> orderHeader;

			if (status == "all")
			{
				orderHeader = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();
			}
			else
			{
				string statusDbFieldName = status switch
				{
					"pending" => SD.OrderStatuses.Pending,
					"approved" => SD.OrderStatuses.Approved,
					"inprocess" => SD.OrderStatuses.InProcess,
					"shipped" => SD.OrderStatuses.Shipped,
					"cancelled" => SD.OrderStatuses.Cancelled,
					"refunded" => SD.OrderStatuses.Refunded,
					"completed" => SD.OrderStatuses.Completed,
					_ => throw new ArgumentException("Invalid status", nameof(status)),
				};

				orderHeader = _unitOfWork.OrderHeader.GetAll(oh => oh.OrderStatus == statusDbFieldName, includeProperties: "ApplicationUser").ToList();
			}

			// If customer or company only show their orders
			if (User.IsInRole(SD.Roles.Customer) || User.IsInRole(SD.Roles.Company))
			{
				ClaimsIdentity userClaimsIdentity = (ClaimsIdentity)User.Identity;
				string userId = userClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
				orderHeader = orderHeader.Where(oh => oh.ApplicationUserId == userId).ToList();
			}

			return Json(new { data = orderHeader });
		}

		#endregion
	}
}
