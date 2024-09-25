using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsContacts
{
    public partial class Form1 : Form
    {
        private BussinessLogicLayer _bussinessLogicLayer;
        private DataAccessLayer _dataAccessLayer;
        public Form1()
        {
            InitializeComponent();
            _bussinessLogicLayer = new BussinessLogicLayer();
            
        }

        #region EVENTS

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ContactDetails contactDetails = new ContactDetails();
            //abre la instancia del formulario contactDetails
            ContactDetailsDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //capturamos la celda pinchada, y aseguramos que sea de tipo link
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewLinkCell cell)
            {
                if (cell.Value.ToString() == "Edit")
                {
                    ContactDetails contactDetails = new ContactDetails();
                    //abre la instancia del formulario contactDetails
                    contactDetails.LoadContact(new Contact
                    {
                        Id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()),
                        FirstName = (dataGridView1.Rows[e.RowIndex].Cells[1]).Value.ToString(),
                        LastName = (dataGridView1.Rows[e.RowIndex].Cells[2]).Value.ToString(),
                        Phone = (dataGridView1.Rows[e.RowIndex].Cells[3]).Value.ToString(),
                        Address = (dataGridView1.Rows[e.RowIndex].Cells[4]).Value.ToString()
                    });

                    contactDetails.ShowDialog(this);
                }
                else if (cell.Value.ToString() == "Delete")
                {
                    int id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                    DeleteContact(id);
                }
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            PopulateContacts(txtSearch.Text);
            txtSearch.Clear();
        }

        #endregion


        #region PRIVATE METHODS

        private void ContactDetailsDialog()
        {
            ContactDetails contactDetails = new ContactDetails();
            contactDetails.ShowDialog(this);
        }

        private void ResizeCells()
        {
            int anchoTotal = 0;
            int altoTotal = 470;

            //se ajusta el ancho de cada columna al contenido
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AutoSize = true;

            //se ajusta el ancho de la ventana al datagrid
            foreach (DataGridViewColumn columna in dataGridView1.Columns)
            {
                if (columna.Visible)
                {
                    anchoTotal += columna.Width;
                }
            }

            if (dataGridView1.RowHeadersVisible)
            {
                anchoTotal += dataGridView1.RowHeadersWidth;
            }

            // Ajustar el ancho del DataGridView
            dataGridView1.Width = anchoTotal;

            this.ClientSize = new Size(dataGridView1.DisplayRectangle.Width, altoTotal);
        }
        private void DeleteContact(int id)
        {
            _bussinessLogicLayer.DeleteContact(id);
            PopulateContacts();
            MessageBox.Show("Data deleted successfully", "Info");
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            //ResizeCells();
            PopulateContacts();
        }

        //el argumento es opcional cuando se asigna null
        public void PopulateContacts(string searchText = null)
        {
            //obtenemos los datos
            List<Contact> contacts = _bussinessLogicLayer.GetAllContacts(searchText);

            // indicamos al dataGrid el origen de datos
            dataGridView1.DataSource = contacts;
        }   
    }
}
