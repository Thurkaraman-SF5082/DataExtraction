using DataExtraction.Enums;
using DataExtraction.Interfaces;

namespace DataExtraction.Services
{
    public class CustomFieldCode : ICustomField
    {
        public int GetOperationsCustomFieldCode(string paymentFrequency)
        {
            PaymentFrequency convertedEnum = (PaymentFrequency)Enum.Parse(typeof(PaymentFrequency), paymentFrequency, ignoreCase: true);

            switch (convertedEnum)
            {
                case PaymentFrequency.SingleOneTimePayment:
                    {
                        return 1117;
                    }
                case PaymentFrequency.MonthlyPayments:
                    {
                        return 1118;
                    }
                case PaymentFrequency.QuarterlyPayments:
                    {
                        return 1119;
                    }
                case PaymentFrequency.CustomPaymentSchedule:
                    {
                        return 1120;
                    }
                case PaymentFrequency.UpfrontPaymentAndMonthlyPayments:
                    {
                        return 1121;
                    }
                case PaymentFrequency.AnnualPayment:
                    {
                        return 1122;
                    }
            }
            return 0;
        }
    }
}