using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoBaHBuilder
{
    class SoBaHBuilderCalc:Form
    {

        //singelton
        private static SoBaHBuilderCalc instance;
        private SoBaHBuilderCalc(){}

        public static SoBaHBuilderCalc Instance
        {
            get
            {
                if (instance == null)
                {
                    instance=new SoBaHBuilderCalc();
                }
                return instance;
            }
        }

        //attributes
        private string qualityScore;
        private string combatScore;
        private string specialAbilitiesScore;
        private string quanitityScore;
        private double totalCostValue;
        private double specialAbilitiesTotal;

      
        public string QualityScore
        {
            set { qualityScore = value; }
            get { return qualityScore; }
        }
        public string CombatScore {
            set { combatScore = value; }
            get { return combatScore; }
        }
        public string SpecialAbilitiesScore
        {
            set { specialAbilitiesScore = value; }
            get { return specialAbilitiesScore; }
        }
        public string QuantityScore
        {
            get { return quanitityScore; }
            set { quanitityScore = value; }
        }
        public double TotalCostValue
        {
            set { totalCostValue = value; }
            get { return totalCostValue; }
        }
        //read in comboCheckBox string and take out numbers only
        public double parseSpecialAbilities(){
            
            MatchCollection matches = Regex.Matches(SpecialAbilitiesScore, "-?[0-9]+");
            List<double> output = new List<double>();
            foreach(Match x in matches){
                
                output.Add(double.Parse(x.Value));
                Debug.WriteLine(x.Value);
                Debug.WriteLine(x.GetType());
            }

            
            foreach (double y in output)
            {
                Debug.WriteLine(y);
            }
            specialAbilitiesTotal = output.Sum();
            Debug.WriteLine(specialAbilitiesTotal);

            return specialAbilitiesTotal;
        }

        public double calculatePoints()
        {
            if (CombatScore != "" && QualityScore != "")
            {
                    parseSpecialAbilities();

                if (quanitityScore == "")
                {
                    quanitityScore = "1";
                }

                double combatScoreValue = Convert.ToDouble(combatScore);
                double qualityScoreValue = Convert.ToDouble(qualityScore.Substring(0, 1));
                double quantityScoreValue = Convert.ToDouble(quanitityScore);

                double subTotalCostValue = Math.Ceiling(((combatScoreValue * 5 + specialAbilitiesTotal) * (7 - qualityScoreValue)) / 2);
                totalCostValue = subTotalCostValue * quantityScoreValue;

                return totalCostValue;

            }
            else
            {
                return 0;
            }
                
        }
    }
}
