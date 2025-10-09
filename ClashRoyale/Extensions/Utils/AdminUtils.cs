using System;
using System.Collections.Generic;
using System.Text;

namespace ClashRoyale.Extensions.Utils
{
    public class AdminUtils
    {
        public static bool CheckIfAdmin(int id)
        {
            return Resources.Configuration.Admins.Contains(id);
        }
    }
}