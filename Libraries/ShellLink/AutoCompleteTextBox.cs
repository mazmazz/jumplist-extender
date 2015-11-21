using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace vbAccelerator.Controls.TextBox
{
	/// <summary>
	/// Adds Shell File System and URL AutoCompletion facilities to 
	/// a text box
	/// </summary>
	public class AutoCompleteTextBox : System.Windows.Forms.TextBox 
	{
		#region Unmanaged Code
		[Flags]
		public enum SHAutoCompleteFlags : uint
		{	
			SHACF_DEFAULT = 0x0,                          // Currently (SHACF_FILESYSTEM | SHACF_URLALL)
			SHACF_FILESYSTEM = 0x1,                        // This includes the File System as well as the rest of the shell (Desktop\My Computer\Control Panel\)
			SHACF_URLHISTORY = 0x2,                        // URLs in the User's History
			SHACF_URLMRU = 0x4,                            // URLs in the User's Recently Used list.
			SHACF_USETAB = 0x8,                            // Use the tab to move thru the autocomplete possibilities instead of to the next dialog/window control.
			SHACF_URLALL = (SHACF_URLHISTORY | SHACF_URLMRU),
			SHACF_FILESYS_ONLY = 0x10,                     // This includes the File System
			SHACF_FILESYS_DIRS = 0x20,                     // Same as SHACF_FILESYS_ONLY except it only includes directories, UNC servers, and UNC server shares.
			SHACF_AUTOSUGGEST_FORCE_ON = 0x10000000,       // Ignore the registry default and force the feature on.
			SHACF_AUTOSUGGEST_FORCE_OFF = 0x20000000,      // Ignore the registry default and force the feature off.
			SHACF_AUTOAPPEND_FORCE_ON = 0x40000000,        // Ignore the registry default and force the feature on. (Also know as AutoComplete)
			SHACF_AUTOAPPEND_FORCE_OFF = 0x80000000       // Ignore the registry default and force the feature off. (Also know as AutoComplete)
		}
		
		[DllImport("shlwapi.dll")]
		private static extern int SHAutoComplete (
			IntPtr hwndEdit, 
			AutoCompleteTextBox.SHAutoCompleteFlags dwFlags );

		#endregion

		#region Member Variables
		private AutoCompleteTextBox.SHAutoCompleteFlags autoCompleteFlags = 
			SHAutoCompleteFlags.SHACF_FILESYS_ONLY;
		private bool flagsSet = false;
		private bool handleCreated = false;
		#endregion

		#region Implementation

		/// <summary>
		/// Gets/sets the flags controlling automcompletion for the 
		/// text box
		/// </summary>
		public AutoCompleteTextBox.SHAutoCompleteFlags AutoCompleteFlags
		{
			get
			{
				return this.autoCompleteFlags;
			}
			set
			{
				this.autoCompleteFlags = value;
				this.flagsSet = true;
				if (handleCreated)
				{
					SetAutoComplete();
				}
			}
		}

		protected override void OnHandleCreated ( System.EventArgs e )
		{
			// call this first as SHAutoComplete may not be supported
			// on the OS
			base.OnHandleCreated(e);
			// don't do anything if we're in design mode:
			if (!this.DesignMode)
			{
				// remember we've created the handle for any future 
				// get/ set
				handleCreated = true;

				// if we've provided some flags then start autocompletion:
				if (flagsSet)
				{
					SetAutoComplete();
				}
			}
		}

		private void SetAutoComplete()
		{
			SHAutoComplete(this.Handle, this.autoCompleteFlags);
		}

		/// <summary>
		///  Constructs an auto-complete capable text box but
		///  does not automatically start auto-completion.
		/// </summary>
		public AutoCompleteTextBox() : base()
		{
		}

		/// <summary>
		/// Constructs an auto-complete capable text box and
		/// starts auto-completion with the specified flags.
		/// </summary>
		/// <param name="autoCompleteFlags">Flags controlling
		/// auto-completion</param>
		public AutoCompleteTextBox(
			AutoCompleteTextBox.SHAutoCompleteFlags autoCompleteFlags
			) : this()
		{
			// Handle will not be available at this point; we need
			// to wait for HandleCreated:
			this.autoCompleteFlags = autoCompleteFlags;
			this.flagsSet = true;
		}

		#endregion
	}
}

