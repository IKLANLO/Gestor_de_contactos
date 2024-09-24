using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsContacts
{
    public class BussinessLogicLayer
    {
        private DataAccessLayer _dataAccesslayer;

        public BussinessLogicLayer() {
            //conectamos con la capa de datos
            _dataAccesslayer = new DataAccessLayer();
        }
        public Contact SaveContact(Contact contact)
        {
            if (contact.Id == 0)
                _dataAccesslayer.InsertContact(contact);
            else
                _dataAccesslayer.UpdateContact(contact);

            return contact;
        }
        public List<Contact> GetAllContacts(string searchText = null)
        {
            return _dataAccesslayer.GetContacts(searchText);
        }

        public void DeleteContact(int id)
        {
            _dataAccesslayer.DeleteContact(id);
        }
    }
}
