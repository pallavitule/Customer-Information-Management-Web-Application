using System.Data;
using System.Xml.Linq;
namespace MVCDHProject5.Models
{
    public class CustomerXmlDAL: ICustomerDAL
    {
        DataSet ds;
        
        public CustomerXmlDAL()
        {
            ds = new DataSet();

            if (!File.Exists("Customer.xml"))
            {
                Console.WriteLine("Customer.xml not found. Creating a new dataset.");
                DataTable dt = new DataTable("Customer");
                dt.Columns.Add("Custid", typeof(int));
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Balance", typeof(decimal));
                dt.Columns.Add("City", typeof(string));
                dt.Columns.Add("Status", typeof(bool));
                dt.PrimaryKey = new DataColumn[] { dt.Columns["Custid"] };
                ds.Tables.Add(dt);
                ds.WriteXml("Customer.xml"); // Save the default XML structure
            }
            else
            {
                ds.ReadXml("Customer.xml");

                if (ds.Tables.Count == 0 || ds.Tables[0].Columns["Custid"] == null)
                {
                    throw new Exception("Customer table or Custid column is missing from dataset.");
                }

                ds.Tables[0].PrimaryKey = new DataColumn[] { ds.Tables[0].Columns["Custid"] };
            }
        }

        public List<Customer> Customers_Select()
        {
            List<Customer> customers = new List<Customer>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Customer obj = new Customer
                {
                    Custid = Convert.ToInt32(dr["Custid"]),
                    Name = (string)dr["Name"],
                    Balance = Convert.ToDecimal(dr["Balance"]),
                    City = (string)dr["City"],
                    Status = Convert.ToBoolean(dr["Status"])
                };
                customers.Add(obj);
            }
            return customers;
        }
        public Customer Customer_Select(int Custid)
        {
            DataRow dr = ds.Tables[0].Rows.Find(Custid);
            Customer customer = new Customer
            {
                Custid = Convert.ToInt32(dr["Custid"]),
                Name = Convert.ToString(dr["Name"]),
                Balance = Convert.ToDecimal(dr["Balance"]),
                City = Convert.ToString(dr["City"]),
                Status = Convert.ToBoolean(dr["Status"])
            };
            return customer;
        }
      
        public void Customer_Insert(Customer customer)
        {
            if (customer.Custid <= 0)
            {
                int maxCustId = ds.Tables[0].Rows.Count > 0
                    ? ds.Tables[0].AsEnumerable().Max(r => r.Field<int>("Custid"))
                    : 0;
                customer.Custid = maxCustId + 1;  // Assign new Custid
            }

            DataRow dr = ds.Tables[0].NewRow();
            dr["Custid"] = customer.Custid;
            dr["Name"] = customer.Name;
            dr["Balance"] = customer.Balance;
            dr["City"] = customer.City;
            dr["Status"] = customer.Status;

            ds.Tables[0].Rows.Add(dr);
            ds.WriteXml("Customer.xml");
        }       

        public void Customer_Update(Customer customer)
        {
            Console.WriteLine($"Updating Customer with Custid: {customer.Custid}");

            if (customer.Custid <= 0)
            {
                Console.WriteLine("Error: Custid must be greater than 0.");
                throw new Exception("Invalid Custid. Custid must be greater than 0.");
            }

            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                Console.WriteLine("Error: Dataset is empty.");
                throw new Exception("Dataset is empty or not loaded properly.");
            }

            if (ds.Tables[0].PrimaryKey.Length == 0)
            {
                Console.WriteLine("Error: Primary key is not set.");
                throw new Exception("Primary key is not set for the dataset.");
            }

            DataRow dr = ds.Tables[0].Rows.Find(customer.Custid);

            if (dr == null)
            {
                Console.WriteLine($"Error: Customer with Custid {customer.Custid} not found.");
                throw new Exception($"Customer with Custid {customer.Custid} not found.");
            }

            Console.WriteLine("Updating values...");
            dr["Name"] = customer.Name;
            dr["Balance"] = customer.Balance;
            dr["City"] = customer.City;

            ds.WriteXml("Customer.xml");

            Console.WriteLine("Customer updated successfully.");
        }


        public void Customer_Delete(int Custid)
        {
            //Finding a DataRow basedonits PrimaryKeyvalue
            DataRow dr = ds.Tables[0].Rows.Find(Custid);
            //Finding the Indexof DataRow bycallingIndexOf method
            int Index = ds.Tables[0].Rows.IndexOf(dr);
            //Deleting the DataRow fromDataTable byusingIndex
            ds.Tables[0].Rows[Index].Delete();
            //Saving data back toXMLfile
            ds.WriteXml("Customer.xml");
        }
    }
}
