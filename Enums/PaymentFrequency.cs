using System.ComponentModel;

namespace DataExtraction.Enums
{
    public enum PaymentFrequency
    {
        [Description("Annual Payment")]
        AnnualPayment=1,
        [Description("Single one-time payment")]
        SingleOneTimePayment=2,
        [Description("Upfront payment + Monthly Payments")]
        UpfrontPaymentAndMonthlyPayments=3,
        [Description("Quarterly Payments")]
        QuarterlyPayments=4,
        [Description("Monthly Payments")]
        MonthlyPayments=5,
        [Description("Custom Payment Schedule")]
        CustomPaymentSchedule=6
    }
}