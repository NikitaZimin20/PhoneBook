using PhoneBook.Models;
using SwitchingViews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchingViews.Stores
{
    public static class UserStore
    {
        public static void BroadCast(UserModel message)
        {
            if (OnMessageTransmitted != null)
                OnMessageTransmitted(message);
        }

        public static  event Action<UserModel> OnMessageTransmitted;
    }
}
