namespace SampleWebsite
{

    using System;

    /// <summary>
    ///     A simple web page that shows the time.
    /// </summary>
    public partial class HelloWorld : System.Web.UI.Page
    {

        /// <summary>
        ///     Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///     The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            TimeLabel.Text = DateTime.Now.ToString();
        }

    }

}