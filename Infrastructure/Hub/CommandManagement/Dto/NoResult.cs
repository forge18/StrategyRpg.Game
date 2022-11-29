using System;
using System.Threading.Tasks;

namespace Infrastructure.HubMediator
{
    public class NoResult
    {
        public static implicit operator Task(NoResult v)
        {
            throw new NotImplementedException();
        }
    }
}