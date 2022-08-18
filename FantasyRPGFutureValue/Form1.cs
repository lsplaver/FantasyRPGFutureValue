namespace FantasyRPGFutureValue
{
    public partial class frmFutureValue : Form
    {
        public frmFutureValue()
        {
            InitializeComponent();
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidData())
                {
                    long monthlyInvestmentGold = Convert.ToInt64(txtMonthlyGold.Text);
                    long monthlyInvestmentSilver = Convert.ToInt64(txtMonthlySilver.Text);
                    long monthlyInvestmentCopper = Convert.ToInt64(txtMonthlyCopper.Text);
                    decimal interestRate = Convert.ToDecimal(txtInterestRate.Text);
                    int years = Convert.ToInt32(txtYears.Text);
                    int monthsPerYear = Convert.ToInt32(txtMonths.Text);

                    decimal monthlyInvestment = MonthlyInvestmentDecimal(monthlyInvestmentGold, monthlyInvestmentSilver, monthlyInvestmentCopper);

                    monthlyInvestment = CalculateFutureValue(monthlyInvestment, interestRate, years, monthsPerYear);


                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.GetType().ToString() + "\n" + ex.StackTrace, "Exception");
            }
        }

        private decimal CalculateFutureValue(decimal monthlyInvestment, decimal interestRate, int years, int monthsPerYear)
        {
            decimal result = 0;
            decimal monthlyInterestRate = ((interestRate / monthsPerYear) / 100);
            int months = monthsPerYear * years;

            for (int x = 0; x < months; months++)
            {
                result += monthlyInvestment * (1 + monthlyInterestRate);
            }

            return result;
        }

        private decimal MonthlyInvestmentDecimal(long monthlyInvestmentGold, long monthlyInvestmentSilver, long monthlyInvestmentCopper)
        {
            decimal monthlyInvestment = 0;
            monthlyInvestment = (((monthlyInvestmentGold * 100) + (monthlyInvestmentSilver * 10) + monthlyInvestmentCopper) / 100);
            return monthlyInvestment;
        }

        private bool IsValidData()
        {
            bool success = true;
            string errorMessage = "";

            errorMessage += IsDecimal(txtMonthlyGold.Text, txtMonthlyGold.Tag.ToString());

            if (errorMessage != "")
            {
                success = false;
                MessageBox.Show(errorMessage, "Entry Error");
            }
            return success;
        }

        private string IsDecimal(string text, string? v)
        {
            throw new NotImplementedException();
        }
    }
}