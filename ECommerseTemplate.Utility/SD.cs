public static class SD
{
	public static class Roles
	{
		public const string Customer = "Customer";
		public const string Company = "Company";
		public const string Admin = "Admin";
		public const string Employee = "Employee";
	}

	public static class OrderStatuses
	{
		public const string Pending = "Pending";
		public const string Approved = "Approved";
		public const string InProcess = "Processing";
		public const string Shipped = "Shipped";
		public const string Cancelled = "Cancelled";
		public const string Refunded = "Refunded";
		public const string Completed = "Completed";
	}

	public static class PaymentStatuses
	{
		public const string Pending = "Pending";
		public const string Approved = "Approved";
		public const string DelayedPayment = "ApprovedForDelayedPayment";
		public const string Rejected = "Rejected";
		public const string Refunded = "Refunded";
		public const string Cancelled = "Cancelled";
	}


	public static class URLs
	{
		public const string DevelopmentDomain = "https://localhost:7080/";
		public const string ProductionDomain = "https://example.com/";
	}

	public static class ApplicationSettings
	{
		public const string ApplicationMode = "Development";
	}

	public static class SessionKeys
	{
		public const string NumOfShoppingCarts = "NumOfShoppingCarts";
	}
}
