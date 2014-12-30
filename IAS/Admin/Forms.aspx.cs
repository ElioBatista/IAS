﻿using IAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity.Validation;

namespace IAS.Admin {

    public partial class Forms : System.Web.UI.Page {
  
        protected void Page_Load( object sender, EventArgs e ) {

        }

        public IQueryable<Form> GetData() {
            
            var db = new ApplicationDbContext();
            var query = db.Forms.OrderBy(frm => frm.FormName);
            return query;            

        }

        public void Update( Form subject ) {

            try {
                var db = new ApplicationDbContext();
                var form = db.Forms.Where( s => s.FormID == subject.FormID ).SingleOrDefault();
                form.FormName = subject.FormName;
                form.Url = subject.Url;
                db.SaveChanges();
                ErrorLabel.Text = String.Empty;
            }
            catch (DbEntityValidationException ex)
            {
                ErrorLabel.Visible = true;
                ErrorLabel.Text = EventLogManager.LogError(ex);
            }
            catch ( Exception exp ) {
                ErrorLabel.Visible = false;
                ErrorLabel.Text = exp.Message;
            }
        }

        public void Delete( Form subject ) {
            try {
                var db = new ApplicationDbContext();
                var form = db.Forms.Where( s => s.FormID == subject.FormID ).SingleOrDefault();
                db.Forms.Remove( form );
                db.SaveChanges();
                ErrorLabel.Text = String.Empty;
            }
            catch (DbEntityValidationException ex)
            {
                ErrorLabel.Visible = true;
                ErrorLabel.Text = EventLogManager.LogError(ex);
            }
            catch ( Exception exp ) {
                ErrorLabel.Visible = false;
                ErrorLabel.Text = exp.Message;
            }
        }

        protected void FormsListView_ItemEditing( object sender, ListViewEditEventArgs e ) {
            FormsListView.EditIndex = e.NewEditIndex;
        }

        protected void FormsListView_ItemCanceling( object sender, ListViewCancelEventArgs e ) {
            FormsListView.EditIndex = -1;
        }

        public void Insert() {

            var db = new ApplicationDbContext();
            var form = new Form();
            
            TryUpdateModel( form );
            
            if ( ModelState.IsValid ) {
                
                try {
                    db.Forms.Add( form );
                    db.SaveChanges();
                    ErrorLabel.Text = String.Empty;
                }
                catch (DbEntityValidationException ex)
                {
                    ErrorLabel.Visible = true;
                    ErrorLabel.Text = EventLogManager.LogError(ex);
                }
                catch ( Exception exp ) {
                    ErrorLabel.Visible = true;
                    ErrorLabel.Text = exp.Message;
                }
            }
            else {
                ErrorLabel.Visible = true;
                ErrorLabel.Text = "Complete todos los campos.";
            }
        }
    }
}