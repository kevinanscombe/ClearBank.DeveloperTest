using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Rules
{
    public class PaymentRules : IPaymentRules
    {
        //Check the account is in a valid state to make the payment
        public bool PaymentValid(MakePaymentRequest request, Account account)
        {
            return
            account != null
            &&
            (
                (request.PaymentScheme == PaymentScheme.Bacs && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
                ||
                (request.PaymentScheme == PaymentScheme.FasterPayments && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
                ||
                (request.PaymentScheme == PaymentScheme.Chaps && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
            );
        }
    }
}
