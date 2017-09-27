#if NET461

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceFabric.Utils.Logging
{
    public struct EventId
    {
        private int _id;
        private string _name;

        public EventId(int id, string name = null)
        {
            _id = id;
            _name = name;
        }

        public int Id
        {
            get
            {
                return _id;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public static implicit operator EventId(int i)
        {
            return new EventId(i);
        }

        public override string ToString()
        {
            if (_name != null)
            {
                return _name;
            }
            else
            {
                return _id.ToString();
            }
        }
    }
}

#endif