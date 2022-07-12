using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Rules;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace ClearBank.DeveloperTest.Tests
{
    public class PaymentServiceTests
    {
        private Mock<IAccountDataStore> _accountDataStore;
        private Mock<IPaymentRules> _paymentRules;
        private IPaymentService _paymentService;

        [SetUp]
        public void Setup()
        {
            _accountDataStore = new Mock<IAccountDataStore>();
            _paymentRules = new Mock<IPaymentRules>();
            _paymentService = new PaymentService(_accountDataStore.Object, _paymentRules.Object);
        }

        [Test]
        public void Payment_WhenInvalid_ShouldFail()
        {
            _paymentRules.Setup(x => x.PaymentValid(new MakePaymentRequest(), new Account())).Returns(false);
            var result = _paymentService.MakePayment(new MakePaymentRequest());
            result.Success.ShouldBe(false);
        }

        //[Test]
        //public void Payment_WhenValid_ShouldSucceed()
        //{
        //    _paymentRules.Setup(x => x.PaymentValid(new MakePaymentRequest(), new Account())).Returns(true);
        //    var result = _paymentService.MakePayment(new MakePaymentRequest());
        //    result.Success.ShouldBe(true);
        //}

        [Test]
        public void Payment_WhenInvalid_ShouldNotChangeBalance()
        {
            const decimal openingBalance = 100;
            var account = new Account { Balance = openingBalance };
            _paymentRules.Setup(x => x.PaymentValid(new MakePaymentRequest(), account)).Returns(false);
            var result = _paymentService.MakePayment(new MakePaymentRequest());
            account.Balance.ShouldBe(openingBalance);
        }

        //DOES / DOES NOT CALL UpdateAccount!!


        [Test]
        public void Payment_WhenValid_ShouldDebitAccountCorrectAmount()
        {
            const decimal openingBalance = 100, payment = 10;
            const string accountNumber = "1234567890";
            var account = new Account { Balance = openingBalance, AccountNumber = accountNumber };
            _accountDataStore.Setup(x => x.GetAccount(accountNumber)).Returns(account);
            var request = new MakePaymentRequest { DebtorAccountNumber = accountNumber, Amount = payment };
            _paymentRules.Setup(x => x.PaymentValid(request, account)).Returns(true);
            var result = _paymentService.MakePayment(request);
            account.Balance.ShouldBe(openingBalance - payment);
        }

        //[Test]
        //public void BacsPayment_WhenNotAllowed_ShouldFail()
        //{
        //    const decimal openingBalance = 100, payment = 10;
        //    const string accountNumber = "1234567890";
        //    var account = new Account { Balance = openingBalance, AccountNumber = accountNumber };
        //    _accountDataStore.Setup(x => x.GetAccount(accountNumber)).Returns(account);
        //    var request = new MakePaymentRequest { DebtorAccountNumber = accountNumber, Amount = payment };
        //    _paymentRules.Setup(x => x.PaymentValid(request, account)).Returns(true);
        //    var result = _paymentService.MakePayment(request);
        //    account.Balance.ShouldBe(openingBalance - payment);
        //}

    }
}
