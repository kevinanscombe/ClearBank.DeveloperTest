using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Data
{
    public class AccountDataStore : IAccountDataStore
    {
        readonly string _connectionString;

        public AccountDataStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Account GetAccount(string accountNumber)
        {
            // Access database to retrieve account, code removed for brevity 
            return new Account(); // _connectionString and accountNumber would be used here
        }

        public void UpdateAccount(Account account)
        {
            // Update account in database, code removed for brevity
            // _connectionString and account would be used here
        }
    }
}
