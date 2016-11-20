//////////////////////////////////////////////////////////////////////////////////
//  Managed UPnP
//	Written by Aaron Lee Murgatroyd (http://home.exetel.com.au/amurgshere/)
//	A CodePlex project (http://managedupnp.codeplex.com/)
//  Released under the Microsoft Public License (Ms-PL) .
//////////////////////////////////////////////////////////////////////////////////

using System.Text;
using ManagedUPnP;
using ManagedUPnP.CodeGen;
using ManagedUPnP.Descriptions;
using System.Collections.Generic;
using System;

namespace ManagedUPnPTest
{
    /// <summary>
    /// Encapsulates the info control for UPnP service.
    /// </summary>
    public partial class ctlUPnPServiceInfo : ctlUPnPInfo
    {
        #region Constant Locals

        /// <summary>
        /// The constant textual value for Yes
        /// </summary>
        private const string csYes = "Yes";

        /// <summary>
        /// The constant textual value for No
        /// </summary>
        private const string csNo = "No";

        /// <summary>
        /// The constant textual value for Service
        /// </summary>
        private const string csService = "Service";

        /// <summary>
        /// The constant textual value for Device Service
        /// </summary>
        private const string csDeviceService = "Device Service";

        #endregion

        #region Protected Locals

        /// <summary>
        /// True if code generation has already been created.
        /// </summary>
        protected bool mbCodeGenerationDone = false;

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Adds a line to a string builder if its set.
        /// </summary>
        /// <param name="info">The info to add to.</param>
        /// <param name="value">The value to add if its not null or empty.</param>
        /// <param name="name">The name to use for the value.</param>
        private static void AddLine(StringBuilder info, string value, string name)
        {
            if (!String.IsNullOrEmpty(value)) info.AppendLine(string.Format("{0}: {1}", name, value));
        }

        #endregion

        #region Initialisation

        /// <summary>
        /// Creats a new UPnP service info control.
        /// </summary>
        public ctlUPnPServiceInfo()
        {
            InitializeComponent();
        }

        #endregion

        #region Protected Overrides

        /// <summary>
        /// Updates the data in the control from its item.
        /// </summary>
        protected override void UpdateData()
        {
            StringBuilder lsbInfo = new StringBuilder();

            if (miItem != null)
            {
                UPnPServiceTreeItem ldiItem = (UPnPServiceTreeItem)miItem;
                Service lsService = ((Service)(miItem.LinkedObject));
                RootDescription lrdRoot = lsService.Device.RootDeviceDescription();
                ServiceDescription lsdDescription = lsService.Description();
                DeviceServiceDescription ldsdDescription = lsService.DeviceServiceDescription();

                lsbInfo.AppendLine("Service URLs");
                AddLine(lsbInfo, ldsdDescription.GetSCPDURL(lrdRoot), "SCPD Url");
                AddLine(lsbInfo, ldsdDescription.GetControlURL(lrdRoot), "Control Url");
                AddLine(lsbInfo, ldsdDescription.GetEventSubURL(lrdRoot), "Event Sub Url");
                lsbInfo.AppendLine();
                lsbInfo.AppendLine(lsdDescription.ToString());

                AddDescriptionProperties(csService, lsdDescription);
                AddDescriptionProperties(csDeviceService, ldsdDescription);
            }

            rtbInfo.Text = lsbInfo.ToString();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Adds properties for a description object.
        /// </summary>
        /// <param name="source">The name of the source from which the description came.</param>
        /// <param name="description">The description object to add the properties for.</param>
        private void AddDescriptionProperties(String source, Description description)
        {
            foreach (var lsPropertyName in description.GetUsedPropertyNames())
                dgProperties.Rows.Add(source, csYes, lsPropertyName, description.GetPropertyString(lsPropertyName));
            foreach (var lkvProperty in description.GetUnusedProperties())
                dgProperties.Rows.Add(source, csNo, lkvProperty.Key, lkvProperty.Value);
        }

        /// <summary>
        /// Generates code for the device and displays it.
        /// </summary>
        /// <param name="force">True to force generation even if its already been done once.</param>
        private void GenerateCode(bool force = false)
        {
            if (!mbCodeGenerationDone || force)
            {
                if (miItem != null)
                {
                    mbCodeGenerationDone = true;

                    Service lsService = ((Service)(miItem.LinkedObject));

                    StringBuilder lsbClass = new StringBuilder();

                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                    try
                    {
                        lsbClass.Append(
                            lsService.GenerateClassFor(
                                cgpProvider.CodeGenProvider,
                                null, "UPnPServices", ClassScope.Internal, false, false));

                        rtbClass.Text = lsbClass.ToString();
                    }
                    finally
                    {
                        this.Cursor = this.DefaultCursor; 
                    }
                }
                else
                    rtbClass.Text = "(Code generation not available)";
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Occurs when the tab control page is changed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void tcMain_Selected(object sender, System.Windows.Forms.TabControlEventArgs e)
        {
            if (e.TabPage == tpClass)
                GenerateCode();
        }

        /// <summary>
        /// Occurs when the user changes the code generation provider.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void cgpProvider_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (tcMain.SelectedTab == tpClass)
                GenerateCode(true);
        }

        #endregion
    }
}
