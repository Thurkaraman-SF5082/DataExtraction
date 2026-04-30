using System.ComponentModel;

namespace DataExtraction.Enums
{
    public enum PaymentFrequency
    {
        [Description("Single one-time payment")]
        SingleOneTimePayment = 1117, //1117

        [Description("Monthly Payments")]
        MonthlyPayments = 1118, //1118

        [Description("Quarterly Payments")]
        QuarterlyPayments = 1119, //1119

        [Description("Custom Payment Schedule")]
        CustomPaymentSchedule = 1120, //1120

        [Description("Upfront payment + Monthly Payments")]
        UpfrontPaymentAndMonthlyPayments = 1121, //1121

        [Description("Annual Payment")]
        AnnualPayment = 1122 //1122

    }
}