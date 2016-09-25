using System;
using System.Collections.Generic;
using System.Text;

namespace TradeTigerAPI
{
    interface IStruct
    {
        byte[] StructToByte();
        void ByteToStruct(byte[] ByteStructure);

             
    }
}
