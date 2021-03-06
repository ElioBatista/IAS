﻿//«Copyright 2014 Balcazz HT, http://www.balcazzht.com»

//This file is part of IAS | Insurance Advanced Services.

//IAS is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//IAS is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with Foobar.  If not, see <http://www.gnu.org/licenses/>.


using IAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IAS.Admin {

    public partial class Alarms : System.Web.UI.Page {

        protected void Page_Load( object sender, EventArgs e ) {            
            if ( !IsPostBack ) {
                GetWorkflows();
                if (Request.QueryString["WorkflowID"] != null)
                {
                    ddlWorkflows.SelectedValue = Request.QueryString["WorkflowID"];
                }
            }
        }

        public IQueryable<Workflow> GetWorkflows() {
            var db = new ApplicationDbContext();
            var query = db.Workflows;
            return query;
        }

        public IQueryable<State> GetStates() {
            var db = new ApplicationDbContext();
            var query = db.States.OrderBy( s => s.StateName );
            return query;
        }

        public IQueryable<Alarm> GetData( [QueryString( "WorkflowID" )] long? workflowID,
            [Control( "ddlWorkflows" )] long? postBackWorkflowID ) {
            
            var db = new ApplicationDbContext();
            IQueryable<Alarm> query = db.Alarms;

            if (postBackWorkflowID.HasValue)
                query = query.Where(wt => wt.WorkflowID == postBackWorkflowID.Value);
            else if (workflowID.HasValue)
                query = query.Where(wt => wt.WorkflowID == workflowID.Value);

            return query;
        
        }

        public void Update( Alarm subject ) {
            
            try{
                var db = new ApplicationDbContext();
                var alarm = db.Alarms.Where( s => s.AlarmID == subject.AlarmID ).SingleOrDefault();
                alarm.StateID = subject.StateID;
                alarm.Interval = subject.Interval;
                db.SaveChanges();
                ErrorLabel.Text = String.Empty;
            }
            catch ( Exception exp ) {
                ErrorLabel.Visible = true;
                ErrorLabel.Text = exp.Message;
            }
        }

        public void Delete( Alarm subject ) {
            try {
                var db = new ApplicationDbContext();
                var alarm = db.Alarms.Where( s => s.AlarmID == subject.AlarmID ).SingleOrDefault();
                db.Alarms.Remove( alarm );
                db.SaveChanges();
                ErrorLabel.Text = String.Empty;
            }
            catch ( Exception exp ) {
                ErrorLabel.Visible = true;
                ErrorLabel.Text = exp.Message;
            }
        }

        protected void AlarmsListView_ItemEditing( object sender, ListViewEditEventArgs e ) {
            AlarmsListView.EditIndex = e.NewEditIndex;
        }

        protected void AlarmsListView_ItemCanceling( object sender, ListViewCancelEventArgs e ) {
            AlarmsListView.EditIndex = -1;
        }

        public void Insert( [Control( "ddlWorkflows" )] long workflowID ) {
            var db = new ApplicationDbContext();
            var alarm = new Alarm();
            TryUpdateModel( alarm );
            alarm.WorkflowID = workflowID;
            if ( ModelState.IsValid ) {
                try {
                    db.Alarms.Add( alarm );
                    db.SaveChanges();
                    ErrorLabel.Text = String.Empty;
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