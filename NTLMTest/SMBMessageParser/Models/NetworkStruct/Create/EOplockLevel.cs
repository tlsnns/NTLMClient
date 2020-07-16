using System;
using System.Collections.Generic;
using System.Text;

namespace SMBMessageParser.Models.NetworkStruct.Create
{
    public enum EOplockLevel : byte
    {
        SMB2_OPLOCK_LEVEL_NONE = 0,
        SMB2_OPLOCK_LEVEL_II = 1,
        SMB2_OPLOCK_LEVEL_EXCLUSIVE = 8,
        SMB2_OPLOCK_LEVEL_BATCH = 9,
        SMB2_OPLOCK_LEVEL_LEASE = 0xff
    }
}
