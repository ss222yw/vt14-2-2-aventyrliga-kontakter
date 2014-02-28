using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aventyrliga_Kontakter.Model;

namespace Aventyrliga_Kontakter
{
    public partial class Default : System.Web.UI.Page
    {

        private Service _service;

        private Service Service
        {
            get { return _service ?? (_service = new Service()); }
        }
        private string Message
        {
            get
            {
                string message = Session["MessageEx"] as string;
                Session.Remove("MessageEx");
                return message;
            }
            set
            {
                Session["MessageEx"] = value;
            }

        }

        public bool MessageExists
        {
            get
            {
                return Session["MessageEx"] != null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (MessageExists)
            {
                MessagePanel.Visible = true;
                Label1.Text = Message;
            }
        }

        public IEnumerable<Contact> ContactListView_GetDataFromDataBase(int maximumRows, int startRowIndex, out int totalRowCount)
        {
            return Service.GetContactsPageWise(maximumRows, startRowIndex, out totalRowCount);
        }


        public void ContactListView_InsertItem(Contact contact)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    Service.SaveContact(contact);
                    Message = String.Format("KontaktUppgiften '{0} {1} {2}' Lades till.", contact.FirstName,contact.LastName,contact.EmailAddress);
                    Response.Redirect(Request.Path);
                   
                }
                catch (Exception)
                {
                    ModelState.AddModelError(String.Empty, "Ett fel inträffade då kontakten skulle läggas till.");
                }
            }
        }
         
          
        public void ContactListView_UpdateItem(int contactId)
        {
                try
                {
                    var contact = Service.GetContact(contactId);
                    if (contact == null)
                    {

                        ModelState.AddModelError(String.Empty,
                            String.Format("Kontakten med kontaktnummer {0} hittades inte.", contactId));
                        return;
                    }
                    
                    if (TryUpdateModel(contact))
                    {
                        Service.SaveContact(contact);
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError(String.Empty, "Ett oväntat fel inträffade då kontaktuppgiften skulle uppdateras.");
                }
            
        }


        public void ContactListView_DeleteItem(int contactId)
        {
            try
            {
                Service.DeleteContact(contactId);
            }
            catch (Exception)
            {
                ModelState.AddModelError(String.Empty, "Ett oväntat fel inträffade då kontaktuppgiften skulle tas bort.");
            }
        }

    }
}