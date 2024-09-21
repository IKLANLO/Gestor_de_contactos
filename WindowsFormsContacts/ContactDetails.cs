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
    public partial class ContactDetails : Form
    {
        private BussinessLogicLayer _bussinessLogiclayer;
        private Contact _contact;
        public ContactDetails()
        {
            InitializeComponent();
            //conectamos con la lógica de negocio
            _bussinessLogiclayer = new BussinessLogicLayer();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //cerramos el formulario
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveContact();
            ((Form1)this.Owner).PopulateContacts();
        }

        private void SaveContact()
        {
            try
            {
                //guardamos en un objeto los datos introducidos por el usuario
                Contact contact = new Contact();
                contact.FirstName = txtFirstName.Text;
                contact.LastName = txtLastName.Text;
                contact.Phone = txtPhone.Text;
                contact.Address = txtAddress.Text;

                //asignamos valor 0 al id si no hay
                contact.Id = _contact != null ? _contact.Id : 0;

                //llamamos a la lógica de negocio
                _bussinessLogiclayer.SaveContact(contact);

                //vaciamos los cuadros de texto
                ClearContactData();
                MessageBox.Show("Data saved successfully", "Info", MessageBoxButtons.OK);
            }
            catch (Exception)
            {
                MessageBox.Show("Error, revisa los datos");
            }
        }

        private void ClearContactData()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
        }

        public void LoadContact(Contact contact)
        {
            _contact = contact;
            if (contact != null)
            {
                ClearContactData();

                txtFirstName.Text = contact.FirstName;
                txtLastName.Text = contact.LastName;
                txtPhone.Text = contact.Phone;
                txtAddress.Text = contact.Address;
            }
        }
    }
}
