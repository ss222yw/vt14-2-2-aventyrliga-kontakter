using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aventyrliga_Kontakter.Model.DAL;
using System.ComponentModel.DataAnnotations;


namespace Aventyrliga_Kontakter.Model
{
    public class Service
    {
        private ContactDAL _contactDAL;

        private ContactDAL ContactDAL
        {
            get { return _contactDAL ?? (_contactDAL = new ContactDAL()); }
        }

        public void DeleteContact(int contactId)
        {
            ContactDAL.DeleteContact(contactId);
        }

        //Contact sparas genom att ny kontaktuppgift skapas eller uppdaters en kontaktuppgift.
        public void SaveContact(Contact contact)
        {
            //Validering i affärslogiklagret.

            ICollection<ValidationResult> validationResluts;
            if (!contact.validate(out validationResluts))
            {
                var ex = new ValidationException("Objektet klarar inte validering.");
                ex.Data.Add("ValidationResults", validationResluts);
                throw ex;
            }

            if (contact.ContactId == 0)
            {
                ContactDAL.InsertContact(contact);
            }
            else
            {
                ContactDAL.UpdateContact(contact);
            }

        }




        public IEnumerable<Contact> GetContacts()
        {
            return ContactDAL.GetContacts();
        }



        public Contact GetContact(int contactId)
        {
            return ContactDAL.GetContactById(contactId);
        }


        public IEnumerable<Contact> GetContactsPageWise(int maximumRows, int startRowIndex, out int totalRowCount)
        {
            return ContactDAL.GetContactsPageWise(maximumRows, startRowIndex, out totalRowCount);
        }


    }
}