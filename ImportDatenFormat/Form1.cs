using System.Data;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Schema;

namespace ImportDatenFormat
{
    enum ART
    {
        CSV = 1,
        JSON = 2,
        XML =3
    }
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckPathExists = true;

            ofd.Filter = "CSV files (*.csv)|*.csv|JSON files (*.json)|*.json|XML files (*.xml)|*.xml";
            DialogResult result = ofd.ShowDialog();

            if (result == DialogResult.OK)
            {
                // Create a new DataTable object
                DataTable dt = new DataTable();
                string path = ofd.FileName;

                int index = ofd.FilterIndex;

                switch (index)
                {
                    //csv
                    case (int)ART.CSV:

                        var reihe = File.ReadLines(path);
                        try
                        {
                            string[] lines = File.ReadAllLines(path);
                            if (lines.Length > 0)
                            {
                                string[] headers = lines[0].Split(";");
                                foreach (string header in headers)
                                {
                                    dt.Columns.Add(header);
                                }
                                for (int i = 1; i < lines.Length; i++)
                                {
                                    string[] data1 = lines[i].Split(";");
                                    dt.Rows.Add(data1);
                                }
                            }
                            dataGridView1.DataSource = dt;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error reading CSV file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        dataGridView1.DataSource = dt;


                        break;
                    //json
                    case 2:
                        // Read the JSON file                  
                        string json = File.ReadAllText(path);
                        dt = JsonConvert.DeserializeObject<DataTable>(json);

                        // Deserialize the JSON into a List of objects
                        //List<MyObject> myObjects = JsonConvert.DeserializeObject<List<MyObject>>(json);
                        // Deserialize the JSON into a List of Person
                        //dynamic people = JsonConvert.DeserializeObject<dynamic>(json)!;

                        //dynamic myObjects = JsonConvert.DeserializeObject<dynamic>(json)!;
                        // Create a DataTable from the List of objects
                        //DataTable dataTable = ConvertToDataTable(myObjects);

                        // Bind the DataTable to the DataGridView

                        dataGridView1.DataSource = dt;
                        break;

                    //Xml
                    case 3:
                        // Erstellen einer neuen DataTable
                        DataSet ds = new DataSet();
                        string extension = Path.GetExtension(path);

                       // string xsdPath = ofd.FileName;
                        string xmlPath = path;

                        // Read XML file and validate against XSD schema
                        // XmlSchemaSet schemaSet = new XmlSchemaSet();
                        // schemaSet.Add(null, @"C:\tmp\xsd\myXml.xsd");

                        //XML-Schema erstellen und hinzufügen
                        XmlReaderSettings settings = new XmlReaderSettings();
                        settings.Schemas.Add("", @"C:\tmp\xsd\myXml.xsd");
                        settings.ValidationEventHandler += settingsValidationEventHandler;

                        //XML-Dokument validieren 
                        // settings.ValidationType = ValidationType.Schema;

                        // XmlReader reader = XmlReader.Create(xmlPath, settings);
                        // settings.ValidationEventHandler += settingsValidationEventHandler;

                        XmlReader reader = XmlReader.Create(xmlPath, settings);
                        //XML-Datei laden
                        // XmlDocument document = new XmlDocument();
                        
                        ds.ReadXml(reader);
                        // document.Load(reader);
                        dataGridView1.DataSource = ds.Tables[0];

                        //
                        // ValidationEventHandler eventHandler = new ValidationEventHandler(settingsValidationEventHandler);
                        // bool error = false;
                        // string sError = String.Empty;

                        // document.Validate(eventHandler);

                        // if (error)
                        // {
                        //     // MessageBox.Show("Fehler: " + sError);
                        // }
                        // else
                        // {
                        //     // Create a new DataSet and read the XML document into it
                        //     ds.ReadXml(path);
                        //     dataGridView1.DataSource = ds.Tables[0];
                        // }
                        break;

                    default:
                        MessageBox.Show("Invalid file format selected.");
                        break;
                }
            }
            ofd.RestoreDirectory = true;
        }
        static void settingsValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    MessageBox.Show("Error: {0}", e.Message);
                    break;
                case XmlSeverityType.Warning:
                    MessageBox.Show("Warning: {0}", e.Message);
                    break;
            }
        }

        /*public void ValidateXml(string xmlFilePath, string xsdFilePath)
        {
            // Load the XSD schema
            XmlSchema schema = XmlSchema.Read(new XmlTextReader(xsdFilePath, XmlNodeType.Document, null), ValidationCallBack);
        }*/

        private void control_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Control control = sender as Control;
                // Get the current mouse position relative to the control
                Point mousePosition = new Point(e.X, e.Y);

                // Show the ContextMenuStrip at the current mouse position
                contextMenuStrip1.Show(control, mousePosition);

                // Check if the cursor is over the Exit menu item


            }
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void importToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            importToolStripMenuItem_Click(sender, e);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
