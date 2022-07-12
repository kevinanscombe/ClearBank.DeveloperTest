using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Rules;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace ClearBank.DeveloperTest.Tests
{
    public class PaymentRulesTests
    {
        private IPaymentRules _paymentRules;

        [SetUp]
        public void Setup()
        {
            _paymentRules = new PaymentRules();
        }

        [Test]
        public void Payment_WhenAccountNull_ShouldBeInvalid()
        {
            bool valid = _paymentRules.PaymentValid(new MakePaymentRequest(), null);
            valid.ShouldBeFalse();
        }

        [Test]
        public void BacsPayment_WhenDisallowed_ShouldBeInvalid()
        {
            bool valid = _paymentRules.PaymentValid(new MakePaymentRequest { PaymentScheme = PaymentScheme.Bacs }, new Account());
            valid.ShouldBeFalse();
        }

        [Test]
        public void BacsPayment_WhenAllowed_ShouldBeValid()
        {
            bool valid = _paymentRules.PaymentValid(new MakePaymentRequest { PaymentScheme = PaymentScheme.Bacs }, new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs });
            valid.ShouldBeTrue();
        }

        [Test]
        public void ChapsPayment_WhenDisallowed_ShouldBeInvalid()
        {
            bool valid = _paymentRules.PaymentValid(new MakePaymentRequest { PaymentScheme = PaymentScheme.Chaps }, new Account());
            valid.ShouldBeFalse();
        }

        [Test]
        public void ChapsPayment_WhenAllowed_ShouldBeValid()
        {
            bool valid = _paymentRules.PaymentValid(new MakePaymentRequest { PaymentScheme = PaymentScheme.Chaps }, new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps });
            valid.ShouldBeTrue();
        }

        [Test]
        public void FasterPaymentsPayment_WhenDisallowed_ShouldBeInvalid()
        {
            bool valid = _paymentRules.PaymentValid(new MakePaymentRequest { PaymentScheme = PaymentScheme.FasterPayments }, new Account());
            valid.ShouldBeFalse();
        }

        [Test]
        public void FasterPaymentsPayment_WhenAllowed_ShouldBeValid()
        {
            bool valid = _paymentRules.PaymentValid(new MakePaymentRequest { PaymentScheme = PaymentScheme.FasterPayments }, new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments });
            valid.ShouldBeTrue();
        }
    }

    public class PaymentServiceTests
    {
        const decimal OpeningBalance = 100, Payment = 10;
        const string AccountNumber = "1234567890";

        private Mock<IAccountDataStore> _accountDataStore;
        private Mock<IPaymentRules> _paymentRules;
        private IPaymentService _paymentService;
        private Account _account;
        private MakePaymentRequest _request;

        [SetUp]
        public void Setup()
        {
            _accountDataStore = new Mock<IAccountDataStore>();
            _paymentRules = new Mock<IPaymentRules>();
            _paymentService = new PaymentService(_accountDataStore.Object, _paymentRules.Object);
            _account = new Account { Balance = OpeningBalance, AccountNumber = AccountNumber };
            _accountDataStore.Setup(x => x.GetAccount(AccountNumber)).Returns(_account);
            _request = new MakePaymentRequest { DebtorAccountNumber = _account.AccountNumber, Amount = Payment };
        }

        [Test]
        public void Payment_WhenInvalid_ShouldFailAndNotUpdateAccount()
        {
            _paymentRules.Setup(x => x.PaymentValid(_request, _account)).Returns(false);
            var result = _paymentService.MakePayment(_request);
            result.Success.ShouldBeFalse();
            _accountDataStore.Verify(x => x.UpdateAccount(_account), Times.Never);  // _accountDataStore.UpdateAccount method should not have been called
            _account.Balance.ShouldBe(OpeningBalance); // account balance shouldn't have changed
        }

        [Test]
        public void Payment_WhenValid_ShouldSucceedAndUpdateAccount()
        {
            _paymentRules.Setup(x => x.PaymentValid(_request, _account)).Returns(true);
            var result = _paymentService.MakePayment(_request);
            result.Success.ShouldBeTrue();
            _accountDataStore.Verify(x => x.UpdateAccount(_account), Times.Once); // _accountDataStore.UpdateAccount method should have been called once
            _account.Balance.ShouldBe(OpeningBalance - Payment); // account should have been debited
        }

    }
}
