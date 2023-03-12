using System;
using ECommerce.API.Models;

namespace ECommerce.API.DataAccess
{
	public interface IReviewService
	{
        void InsertReview(Review review);
        List<Review> GetProductReviews(int productId);
	}
}

