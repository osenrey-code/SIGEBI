using SIGEBI.AppEscritorio.Services.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace SIGEBI.AppEscritorio.Views.Shared
{
    public partial class LoginForm : Form
    {
        private readonly IAuthService _auth;
        public LoginForm(IAuthService auth)
        {
            InitializeComponent();
            _auth = auth;
        }


    }
}
