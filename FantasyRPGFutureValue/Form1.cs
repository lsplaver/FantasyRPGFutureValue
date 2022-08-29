using System.Globalization;

namespace FantasyRPGFutureValue
{
    public partial class frmFutureValue : Form
    {
        public frmFutureValue()
        {
            InitializeComponent();
        }
        //    gold,   silver, copper, intrate, years, months, totalG, totalS, totalC
        List<(string, string, string, string, string, string, string, string, string)> values = new List<(string, string, string, string, string, string, string, string, string)>();

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidData())
                {
                    long monthlyInvestmentGold = long.Parse(txtMonthlyGold.Text, NumberStyles.Number);
                    long monthlyInvestmentSilver = long.Parse(txtMonthlySilver.Text, NumberStyles.Number);
                    long monthlyInvestmentCopper = long.Parse(txtMonthlyCopper.Text, NumberStyles.Number);
                    decimal interestRate = decimal.Parse(txtInterestRate.Text, NumberStyles.Number);
                    int years = int.Parse(txtYears.Text, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite);
                    int monthsPerYear = int.Parse(txtMonths.Text, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite);

                    decimal monthlyInvestment = MonthlyInvestmentDecimal(monthlyInvestmentGold, monthlyInvestmentSilver, monthlyInvestmentCopper);

                    monthlyInvestment = CalculateFutureValue(monthlyInvestment, interestRate, years, monthsPerYear);

                    string[] strMonthlyInvestment = monthlyInvestment.ToString().Split(".");
                    string temp = String.Format("{0:c}", strMonthlyInvestment[0]);
                    string gold = temp + " gp";
                    temp = String.Format("{0:c}", strMonthlyInvestment[1].Substring(0, 1));
                    string silver = temp + " sp";
                    temp = String.Format("{0:c}", strMonthlyInvestment[1].Substring(1, 1));
                    string copper = temp + " cp";
                    txtValueGold.Text = gold;
                    txtValueSilver.Text = silver;
                    txtValueCopper.Text = copper;

                    values.Add(((monthlyInvestmentGold.ToString() + " gp"), (monthlyInvestmentSilver.ToString() + " sp"), (monthlyInvestmentCopper.ToString() + " cp"), (interestRate / 100).ToString("P2"),
                        years.ToString(), monthsPerYear.ToString(), gold, silver, copper));
                }
            }
            catch (OverflowException)
            {
                MessageBox.Show("The value is too large. Enter a smaller value", "Entry Error");
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

            for (int x = 0; x < months; x++)
            {
                result = (result + monthlyInvestment) * (1 + monthlyInterestRate);
            }

            return result;
        }

        private decimal MonthlyInvestmentDecimal(long monthlyInvestmentGold, long monthlyInvestmentSilver, long monthlyInvestmentCopper)
        {
            decimal monthlyInvestment = 0;

            monthlyInvestment = (monthlyInvestmentCopper / 100) + (monthlyInvestmentSilver / 10) + monthlyInvestmentGold;

            return monthlyInvestment;
        }

        private bool IsValidData()
        {
            bool success = true;
            string errorMessage = "";

            if (txtMonthlyGold.Text == "" && txtMonthlySilver.Text == "" && txtMonthlyCopper.Text == "")
            {
                errorMessage += "Monthly Gold, Monthly Silver and Monthly Copper can't be all null.\n";
            }
            if (txtMonthlyGold.Text == "")
            {
                txtMonthlyGold.Text = "0";
            }
            if (txtMonthlySilver.Text == "")
            {
                txtMonthlySilver.Text = "0";
            }
            if (txtMonthlyCopper.Text == "")
            {
                txtMonthlyCopper.Text = "0";
            }

            errorMessage += IsDecimal(txtMonthlyGold.Text, txtMonthlyGold.Tag.ToString());
            errorMessage += IsWithinRange(txtMonthlyGold.Text, txtMonthlyGold.Tag.ToString(), 0, 100000);

            errorMessage += IsDecimal(txtMonthlySilver.Text, txtMonthlySilver.Tag.ToString());
            errorMessage += IsWithinRange(txtMonthlySilver.Text, txtMonthlySilver.Tag.ToString(), 0, 100000);

            errorMessage += IsDecimal(txtMonthlyCopper.Text, txtMonthlyCopper.Tag.ToString());
            errorMessage += IsWithinRange(txtMonthlyCopper.Text, txtMonthlyCopper.Tag.ToString(), 0, 100000);

            errorMessage += IsDecimal(txtInterestRate.Text, txtInterestRate.Tag.ToString());
            errorMessage += IsWithinRange(txtInterestRate.Text, txtInterestRate.Tag.ToString(), 1, 20);

            errorMessage += IsInt32(txtYears.Text, txtYears.Tag.ToString());
            errorMessage += IsWithinRange(txtYears.Text, txtYears.Tag.ToString(), 1, 100);

            errorMessage += IsInt32(txtMonths.Text, txtMonths.Tag.ToString());
            errorMessage += IsWithinRange(txtMonths.Text, txtMonths.Tag.ToString(), 1, 20);

            if (errorMessage != "")
            {
                success = false;
                MessageBox.Show(errorMessage, "Entry Error");
            }
            return success;
        }

        private string IsInt32(string value, string? name)
        {
            string msg = "";
            
            if (!Int32.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out _))
            {
                msg += name + " must be a valid integer value.\n";
            }

            return msg;
        }

        private string IsWithinRange(string value, string? name, decimal min, decimal max)
        {
            string msg = "";

            if (Decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal number))
            {
                if (number < min || number > max)
                {
                    msg += name + " must be between " + min + " and " + max + ".\n";
                }
            }

            return msg;
        }

        private string IsDecimal(string value, string? name)
        {
            string msg = "";

            if (!Decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out _))
            {
                msg += name + " must be a valid decimal value.\n";
            }

            return msg;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            string msg = "";

            msg = "Monthly Gold \tMonthly Silver \tMonthly Copper \tInt Rate \tYears \tMonths Per Year \tGold \tSilver \tCopper\n";

            foreach (var list in values)
            {
                msg += list.Item1 + " \t" + list.Item2 + " \t" + list.Item3 + " \t" + list.Item4 + " \t" + list.Item5 + " \t" + list.Item6 + " \t" + list.Item7 + "\t" + list.Item8 + "\t" + list.Item9 + "\n";
            }

            MessageBox.Show(msg, "Future Value Calculations");
            Close();
        }
    }
}