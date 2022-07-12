using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Rules
{
    public interface IPaymentRules
    {
        bool PaymentValid(MakePaymentRequest request, Account account);
    }
}
