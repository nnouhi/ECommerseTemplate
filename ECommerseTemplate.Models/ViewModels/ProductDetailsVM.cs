using X.PagedList;

namespace ECommerseTemplate.Models.ViewModels
{
    public class ProductDetailsVM
    {
        public ShoppingCart ShoppingCart { get; set; }

        public IPagedList<ProductReview> Reviews { get; set; }

        public ProductReview Review { get; set; } // Model

        public int ProductId { get; set; }

        // Add RatingOverview as a nested class
        public RatingMetrics RatingOverview { get; set; }

        public string ReviewFilter { get; set; }

        public string ActiveReviewFilter => GetDropdownValue(ReviewFilter);

        private string GetDropdownValue(string reviewFilter)
        {
            switch (reviewFilter)
            {
                case "latest":
                    return "View latest";
                case "oldest":
                    return "View oldest";
                case "include-pictures":
                    return "View with pictures";
                case "highest-rating":
                    return "View highest rating";
                case "lowest-rating":
                    return "View lowest pictures";
                default:
                    return "View latest";
            }
        }

        public class RatingMetrics
        {
            public int ReviewCount { get; set; }

            public int AverageRating { get; set; }

            public Dictionary<int, int> CountOfVotesPerStar { get; set; }

            public Dictionary<int, int> PercentageOfVotesPerStar { get; set; }
        }
    }
}
