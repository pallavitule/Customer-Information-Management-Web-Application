using Microsoft.EntityFrameworkCore;

namespace MVCDHProject5.Models
{
    public class CustomerSqlDAL : ICustomerDAL
    {
        private readonly MVCCoreDbContext context;
        public CustomerSqlDAL(MVCCoreDbContext context)
        {
            this.context = context;
        }
        public List<Customer> Customers_Select()
        {
            return context.Customers.Where(c => c.Status).ToList(); // Fetch only active customers
        }      

        public Customer Customer_Select(int Custid)
        {
            Customer customer = context.Customers.Find(Custid);
            if (customer == null)
            {
                throw new Exception("No customer exist's with given Custid.");
            }
            return customer;
        }      

        public void Customer_Insert(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            if (string.IsNullOrEmpty(customer.Name) || string.IsNullOrEmpty(customer.City))
                throw new Exception("Name and City fields are required.");

            customer.Continent ??= "Unknown";
            customer.Country ??= "Unknown";
            customer.State ??= "Unknown";

            try
            {
                // Ensure Custid is not manually set
                customer.Custid = 0; // Reset to default, letting DB auto-generate

                context.Customers.Add(customer);
                context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Insert Error: {ex.InnerException?.Message ?? ex.Message}");
                throw new Exception($"Insert Error: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public void Customer_Update(Customer customer)
        {
            var existingCustomer = context.Customers.Find(customer.Custid);
            if (existingCustomer == null)
                throw new Exception("Customer not found.");

            // Update only the necessary properties
            existingCustomer.Name = customer.Name;
            existingCustomer.City = customer.City;
            existingCustomer.Continent = customer.Continent ?? existingCustomer.Continent;
            existingCustomer.Country = customer.Country ?? existingCustomer.Country;
            existingCustomer.Status = true;

            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Update Error: {ex.InnerException?.Message ?? ex.Message}");
                throw;
            }
        }
        
        public void Customer_Delete(int Custid)
        {
            var customer = context.Customers.Find(Custid);
            if (customer == null)
                throw new Exception("Customer not found.");

            customer.Status = false; // Soft delete: Mark inactive

            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Delete Error: {ex.InnerException?.Message ?? ex.Message}");
                throw;
            }
        }
    }
}
