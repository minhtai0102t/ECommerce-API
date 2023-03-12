using ECommerce.API.Models;

namespace ECommerce.API.DataAccess
{
    public interface IDataAccess
    {
        // Categories
        List<ProductCategory> GetProductCategories();
        ProductCategory GetProductCategory(int id);

        // Products
        List<Product> GetProducts(string category, string subcategory, int count); 
        Product GetProduct(int id);
        bool UpdateProduct(int id);
        bool DeleteProduct(int id);

        //User
        bool InsertUser(User user);
        string IsUserPresent(string email, string password);
        User GetUser(int id);

        //Review
        void InsertReview(Review review);
        List<Review> GetProductReviews(int productId);

        //Shopping cart
        bool InsertCartItem(int userId, int productId);
        Cart GetActiveCartOfUser(int userid);
        Cart GetCart(int cartid);
        List<Cart> GetAllPreviousCartsOfUser(int userid);

        //Payment method
        List<PaymentMethod> GetPaymentMethods();
        int InsertPayment(Payment payment);
        int InsertOrder(Order order);

        //Offder
        Offer GetOffer(int id);
    }
}
