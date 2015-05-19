using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using SoBaHBuilder;
using System.Drawing.Printing;

namespace SoBaHBuilder
{
    public partial class Form1 : Form
    {
        //form control lists
        List<TextBox> nameTextBoxList=new List<TextBox>();
        List<ComboBox> qualityComboBoxList = new List<ComboBox>();
        List<ComboBox> combatComboBoxList = new List<ComboBox>();
        List<CheckedComboBox> sPComboCheckBoxList = new List<CheckedComboBox>();
        List<TextBox> quantityTextBoxList = new List<TextBox>();
        List<TextBox> pointsTextBoxList = new List<TextBox>();
        Bitmap memoryImage;
        private PrintDocument printDocument1 = new PrintDocument();
        
        int controlCounter = 0;
        int rowCounter = 2;
        
        private List<string> abilitiesList=new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);

            //add initial form controls to lists
            nameTextBoxList.Add(nameTextBox);
            qualityComboBoxList.Add(qualityComboBox);
            combatComboBoxList.Add(combatComboBox);
            sPComboCheckBoxList.Add(sPComboCheckBox);
            quantityTextBoxList.Add(quantityTextBox);
            pointsTextBoxList.Add(pointsTextBox);

            controlCounter++;

            //read in special abilities textfile
            var reader = new StreamReader(File.OpenRead(System.IO.Path.GetFullPath("Abilities.txt")));
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();

                abilitiesList.Add(line);
            }
            reader.Close();

            //add list to sPComboCheckBox
            for (int i = 0; i < abilitiesList.Count; i++)
            {
                CCBoxItem item = new CCBoxItem(abilitiesList[i], i);
                sPComboCheckBox.Items.Add(item);
            }
            // If more then 8 items, add a scroll bar to the dropdown.
            sPComboCheckBox.MaxDropDownItems = 8;
            // Make the "Name" property the one to display, rather than the ToString() representation.
            sPComboCheckBox.DisplayMember = "Name";
            sPComboCheckBox.ValueSeparator = ", ";
            // Check the first 2 items.
            //SPComboCheckBox.SetItemChecked(0, true);
            //checkedComboBox1.SetItemChecked(1, true);            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tableLayoutPanel1.Visible = false;
            this.SuspendLayout();

            //add row to tableLayout
            this.tableLayoutPanel1.RowCount = 1 + rowCounter;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(886, 72 + (32 * (rowCounter - 1)));

            //add Name text box to List and Form
            nameTextBoxList.Add(new TextBox());
            nameTextBoxList[controlCounter].Anchor = System.Windows.Forms.AnchorStyles.None;
            nameTextBoxList[controlCounter].Name = "nameTextBox"+controlCounter;
            nameTextBoxList[controlCounter].Size = new System.Drawing.Size(154, 21);
            this.Controls.Add(nameTextBoxList[controlCounter]);
            this.tableLayoutPanel1.Controls.Add(nameTextBoxList[controlCounter], 0, rowCounter);

            //add Quality Combo Box to List and Form
            qualityComboBoxList.Add(new ComboBox());
            qualityComboBoxList[controlCounter].Anchor = System.Windows.Forms.AnchorStyles.None;
            qualityComboBoxList[controlCounter].FormattingEnabled = true;
            qualityComboBoxList[controlCounter].Items.AddRange(new object[] {
                "2+",
                "3+",
                "4+",
                "5+"});
            qualityComboBoxList[controlCounter].Name = "qualityComboBox"+controlCounter;
            qualityComboBoxList[controlCounter].Size = new System.Drawing.Size(46, 22);
            Controls.Add(qualityComboBoxList[controlCounter]);
            this.tableLayoutPanel1.Controls.Add(qualityComboBoxList[controlCounter], 1, rowCounter);
            this.qualityComboBoxList[controlCounter].SelectedIndexChanged += new System.EventHandler(this.qualityComboBox_SelectedIndexChanged);

            //add combat combo box to list and Form
            combatComboBoxList.Add(new ComboBox());
            combatComboBoxList[controlCounter].Anchor = System.Windows.Forms.AnchorStyles.None;
            combatComboBoxList[controlCounter].FormattingEnabled = true;
            combatComboBoxList[controlCounter].Items.AddRange(new object[] {
                "0",
                "1",
                "2",
                "3",
                "4",
                "5",
                "6"});
            combatComboBoxList[controlCounter].Name = "combatComboBox"+controlCounter;
            combatComboBoxList[controlCounter].Size = new System.Drawing.Size(45, 22);
            Controls.Add(combatComboBoxList[controlCounter]);
            this.tableLayoutPanel1.Controls.Add(combatComboBoxList[controlCounter], 2, rowCounter);
            this.combatComboBoxList[controlCounter].SelectedIndexChanged += new System.EventHandler(this.controlChange_SelectedIndexChanged);

            //special abilities combo check box
            sPComboCheckBoxList.Add(new SoBaHBuilder.CheckedComboBox());
            sPComboCheckBoxList[controlCounter].Anchor = System.Windows.Forms.AnchorStyles.None;
            sPComboCheckBoxList[controlCounter].CheckOnClick = true;
            sPComboCheckBoxList[controlCounter].DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            sPComboCheckBoxList[controlCounter].DropDownHeight = 1;
            sPComboCheckBoxList[controlCounter].FormattingEnabled = true;
            sPComboCheckBoxList[controlCounter].IntegralHeight = false;
            sPComboCheckBoxList[controlCounter].Name = "SPComboCheckBox" + controlCounter;
            sPComboCheckBoxList[controlCounter].Size = new System.Drawing.Size(366, 21);
            sPComboCheckBoxList[controlCounter].ValueSeparator = ", ";
            Controls.Add(sPComboCheckBoxList[controlCounter]);
            this.tableLayoutPanel1.Controls.Add(sPComboCheckBoxList[controlCounter], 3, rowCounter);
            for (int i = 0; i < abilitiesList.Count; i++)
            {
                CCBoxItem item = new CCBoxItem(abilitiesList[i], i);
                sPComboCheckBoxList[controlCounter].Items.Add(item);
            }
            sPComboCheckBoxList[controlCounter].DisplayMember = "Name";

            //add Quantity text box to list and Form
            quantityTextBoxList.Add(new TextBox());
            quantityTextBoxList[controlCounter].Anchor = System.Windows.Forms.AnchorStyles.None;
            quantityTextBoxList[controlCounter].Name = "quantityTextBox" + controlCounter;
            quantityTextBoxList[controlCounter].Size = new System.Drawing.Size(35, 21);
            quantityTextBoxList[controlCounter].Text = "1";
            Controls.Add(quantityTextBoxList[controlCounter]);
            this.tableLayoutPanel1.Controls.Add(quantityTextBoxList[controlCounter], 4, rowCounter);
            this.quantityTextBoxList[controlCounter].TextChanged += new System.EventHandler(this.quantityTextBox_TextChanged);

            //add Points text box to list and Form
            pointsTextBoxList.Add(new TextBox());
            pointsTextBoxList[controlCounter].Anchor = System.Windows.Forms.AnchorStyles.None;
            pointsTextBoxList[controlCounter].Enabled = true;
            pointsTextBoxList[controlCounter].ReadOnly = true;
            pointsTextBoxList[controlCounter].Name = "pointsTextBox" + controlCounter;
            pointsTextBoxList[controlCounter].Size = new System.Drawing.Size(45, 21);
            pointsTextBoxList[controlCounter].Text = "0";
            Controls.Add(pointsTextBoxList[controlCounter]);
            this.tableLayoutPanel1.Controls.Add(pointsTextBoxList[controlCounter], 5, rowCounter);

            this.ClientSize = new System.Drawing.Size(921, 346);

            this.ResumeLayout(true);
            tableLayoutPanel1.Visible = true;
            controlCounter++;
            rowCounter++;
            SoBaHBuilderCalc.Instance.QualityScore = "";
            SoBaHBuilderCalc.Instance.CombatScore = "";
            SoBaHBuilderCalc.Instance.SpecialAbilitiesScore = "";
            SoBaHBuilderCalc.Instance.QuantityScore = "";
            

        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel1.RowCount > 2)
            {
                tableLayoutPanel1.Visible = false;
                this.SuspendLayout();
                int rowIndex = tableLayoutPanel1.RowCount;
                int rowHeight = tableLayoutPanel1.GetRowHeights()[rowIndex - 1];

                tableLayoutPanel1.RowStyles.RemoveAt(rowIndex - 1);

                for (int columnIndex = 0; columnIndex < tableLayoutPanel1.ColumnCount; columnIndex++)
                {
                    var control = tableLayoutPanel1.GetControlFromPosition(columnIndex, rowCounter - 1);
                    tableLayoutPanel1.Controls.Remove(control);
                }

                tableLayoutPanel1.RowCount--;
                rowCounter--;
                controlCounter--;

                nameTextBoxList.RemoveAt(nameTextBoxList.Count-1);
                qualityComboBoxList.RemoveAt(qualityComboBoxList.Count-1);
                combatComboBoxList.RemoveAt(combatComboBoxList.Count-1);
                sPComboCheckBoxList.RemoveAt(sPComboCheckBoxList.Count-1);
                quantityTextBoxList.RemoveAt(quantityTextBoxList.Count-1);
                pointsTextBoxList.RemoveAt(pointsTextBoxList.Count-1);

                tableLayoutPanel1.Height = tableLayoutPanel1.Height - rowHeight;
                this.ResumeLayout(true);
                tableLayoutPanel1.Visible = true;
            }
        }
        private void controlChange_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < combatComboBoxList.Count; i++)
            {
                Console.WriteLine(combatComboBoxList.Count);
                SoBaHBuilderCalc.Instance.QualityScore = qualityComboBoxList[i].Text;
                SoBaHBuilderCalc.Instance.CombatScore = combatComboBoxList[i].Text;
                SoBaHBuilderCalc.Instance.SpecialAbilitiesScore = sPComboCheckBoxList[i].Text;
                SoBaHBuilderCalc.Instance.QuantityScore = quantityTextBoxList[i].Text;
                SoBaHBuilderCalc.Instance.calculatePoints();
                pointsTextBoxList[i].Text = SoBaHBuilderCalc.Instance.TotalCostValue.ToString();
                SoBaHBuilderCalc.Instance.TotalCostValue = 0;
                SoBaHBuilderCalc.Instance.QualityScore = "";
                SoBaHBuilderCalc.Instance.CombatScore = "";
                SoBaHBuilderCalc.Instance.SpecialAbilitiesScore = "";
                SoBaHBuilderCalc.Instance.QuantityScore = "";
            }  
        }

        private void qualityComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
                controlChange_SelectedIndexChanged(sender, e);
        }

        private void quantityTextBox_TextChanged(object sender, EventArgs e)
        {
            controlChange_SelectedIndexChanged(sender, e);
        }
        //special abilities comboCheckBox change event
        protected override void OnActivated(EventArgs e)
        {
            for (int i = 0; i < combatComboBoxList.Count; i++)
            {
                SoBaHBuilderCalc.Instance.QualityScore = qualityComboBoxList[i].Text;
                SoBaHBuilderCalc.Instance.CombatScore = combatComboBoxList[i].Text;
                if (sPComboCheckBoxList[i].Text == "")
                {
                    SoBaHBuilderCalc.Instance.SpecialAbilitiesScore = "0";
                }
                else
                {
                    SoBaHBuilderCalc.Instance.SpecialAbilitiesScore = sPComboCheckBoxList[i].Text;
                }
                SoBaHBuilderCalc.Instance.QuantityScore = quantityTextBoxList[i].Text;
                SoBaHBuilderCalc.Instance.calculatePoints();
                pointsTextBoxList[i].Text = SoBaHBuilderCalc.Instance.TotalCostValue.ToString();
                SoBaHBuilderCalc.Instance.TotalCostValue = 0;
                SoBaHBuilderCalc.Instance.QualityScore = "";
                SoBaHBuilderCalc.Instance.CombatScore = "";
                SoBaHBuilderCalc.Instance.SpecialAbilitiesScore = "";
                SoBaHBuilderCalc.Instance.QuantityScore = "";
            }
        }

        //capture form for printing
        private void CaptureScreen()
        {
            Graphics myGraphics = this.CreateGraphics();
            Size s = this.Size;
            memoryImage = new Bitmap(s.Width, s.Height, myGraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, s);
        }

        //print event
        private void printDocument1_PrintPage(System.Object sender,
               System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, 0, 0);
        }

        //print click event
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CaptureScreen();
            PrintDialog printDialog1 = new PrintDialog();
            printDialog1.Document = printDocument1;
            DialogResult result = printDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }
    }
}
