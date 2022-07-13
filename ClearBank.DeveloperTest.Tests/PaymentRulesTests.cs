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
}
