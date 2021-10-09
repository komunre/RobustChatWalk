using Robust.Shared.GameObjects;
using Robust.Shared.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Shared.GameObjects
{
    [Serializable, NetSerializable]
    public class AuthRequest : EntityEventArgs
    {
        public EntityUid RequestUser;
        public string Password;
    }
    class SharedAuthSystem
    {
    }
}
