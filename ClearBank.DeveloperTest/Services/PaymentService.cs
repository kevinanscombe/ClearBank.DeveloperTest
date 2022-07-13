using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Rules;
using ClearBank.DeveloperTest.Types;
using System.Configuration;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        readonly IAccountDataStore _accountDataStore;
        readonly IPaymentRules _paymentRules;

        public PaymentService(IAccountDataStore accountDataStore, IPaymentRules paymentRules)
        {
            _accountDataStore = accountDataStore;
            _paymentRules = paymentRules;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var account = _accountDataStore.GetAccount(request.DebtorAccountNumber);

            var result = new MakePaymentResult();

            result.Success = _paymentRules.PaymentValid(request, account); 

            if (result.Success)
            {
                //Deduct the payment amount from the account's balance and update the account in the database
                account.Balance -= request.Amount;
                _accountDataStore.UpdateAccount(account);
            }

            return result;
        }
    }
}
