using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMW_Client.Views
{

    public class LoginMenuItem
    {
        public LoginMenuItem()
        {
            TargetType = typeof(LoginDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}