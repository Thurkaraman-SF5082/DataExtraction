using DataExtraction.Enums;
using DataExtraction.Interfaces;

namespace DataExtraction.Models
{
    public class PaymentOccurency : IPaymentOccurency
    {
        public int GetPaymentOccurencyPerYear(string paymentFrequency)
        {
            PaymentFrequency convertedEnum = (PaymentFrequency)Enum.Parse(typeof(PaymentFrequency), paymentFrequency, ignoreCase: true);

            switch (convertedEnum)
            {
                case PaymentFrequency.SingleOneTimePayment:
                    {
                        return 0;
                    }
                case PaymentFrequency.MonthlyPayments:
                    {
                        return 12;
                    }
                case PaymentFrequency.QuarterlyPayments:
                    {
                        return 4;
                    }
                case PaymentFrequency.CustomPaymentSchedule:
                    {
                        return 0;
                    }
                case PaymentFrequency.UpfrontPaymentAndMonthlyPayments:
                    {
                        return 0;
                    }
                case PaymentFrequency.AnnualPayment:
                    {
                        return 1;
                    }
            }
            return 0;
        }
    }
}