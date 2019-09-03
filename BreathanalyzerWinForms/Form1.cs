using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BreathanalyzerWinForms
{
    public partial class Form1 : Form
    {
        private List<double> _alcohols = new List<double>();
        private const double ManCoefficient = 0.7;
        private const double WomanCoefficient = 0.6;
        private double _coefficient;
        private const double alcoholConsumptionIndicator = 0.13;
        private TimeSpan _timeElapsed;

        public Form1()
        {
            InitializeComponent();
            InitComponents();

            _timeElapsed = dateTimePicker.Value - DateTime.Now;
        }

        private void InitComponents()
        {
            BeerAlcoholPercentageNumericUpDown.Text = 5.ToString();
            ChampagneAlcoholPercentageNumericUpDown.Text = 9.ToString();
            WineAlcoholPercentageNumericUpDown.Text = 11.ToString();
            VodkaAlcoholPercentageNumericUpDown.Text = 40.ToString();
            OtherAlcoholPercentageNumericUpDown.Text = 30.ToString();

            BeerVolumeComboBox.Text = 500.ToString();
            ChampagneVolumeComboBox.Text = 100.ToString();
            WineVolumeComboBox.Text = 100.ToString();
            VodkaVolumeComboBox.Text = 50.ToString();
            OtherAlcoholVolumeComboBox.Text = 100.ToString();

            dateTimePicker.Format = DateTimePickerFormat.Custom;
            dateTimePicker.CustomFormat = Application.CurrentCulture.DateTimeFormat.FullDateTimePattern;
            dateTimePicker.MaxDate = DateTime.Now;
        }

        private double CalculateDrunkAlcoholQuantity(ComboBox alcoholVolume, NumericUpDown alcoholPercentage,
            NumericUpDown alcoholQuantity)
        {
            double.TryParse(alcoholVolume.Text, out var volume);
            double.TryParse(alcoholPercentage.Text, out var percentage);
            double.TryParse(alcoholQuantity.Text, out var quantity);
            var percentToFraction = percentage / 100;
            return volume * percentToFraction * quantity / 1.25;
        }

        private double CalculateAlcoholBloodLevel()
        {
            var a = _alcohols.Sum();
            var kw = _coefficient * double.Parse(WeightNumericUpDown.Text);
            var result = a / kw;
            return result;
        }

        private double CalculateTimeElapsed()
        {
            return (DateTime.Now - dateTimePicker.Value).TotalHours;
        }

        private double CalculateBurnedAlcohol()
        {
            return CalculateTimeElapsed() * alcoholConsumptionIndicator;
        }

        private double CalculateAlcoholLevelNow()
        {
            var alcoholLevel = CalculateAlcoholBloodLevel() - CalculateBurnedAlcohol();
            if (alcoholLevel < 0)
            {
                return 0;
            }

            return alcoholLevel / 10;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _alcohols.Clear();
            _alcohols.Add(CalculateDrunkAlcoholQuantity(BeerVolumeComboBox, BeerAlcoholPercentageNumericUpDown,
                BeerQuantityNumericUpDown));
            _alcohols.Add(CalculateDrunkAlcoholQuantity(WineVolumeComboBox, WineAlcoholPercentageNumericUpDown,
                WineQuantityNumericUpDown));
            _alcohols.Add(CalculateDrunkAlcoholQuantity(VodkaVolumeComboBox, VodkaAlcoholPercentageNumericUpDown,
                VodkaQuantityNumericUpDown));
            _alcohols.Add(CalculateDrunkAlcoholQuantity(ChampagneVolumeComboBox,
                ChampagneAlcoholPercentageNumericUpDown,
                ChampainQuantityNumericUpDown));
            _alcohols.Add(CalculateDrunkAlcoholQuantity(OtherAlcoholVolumeComboBox, OtherAlcoholPercentageNumericUpDown,
                OtherAlcoholQuantityNumericUpDown));
            ResultLbl.Text = $@"Twój wynik to: {CalculateAlcoholLevelNow():N2} ‰";
        }

        private void WomanRadioBt_CheckedChanged(object sender, EventArgs e)
        {
            _coefficient = WomanCoefficient;
        }

        private void ManRadioBt_CheckedChanged(object sender, EventArgs e)
        {
            _coefficient = ManCoefficient;
        }

        
    }
}