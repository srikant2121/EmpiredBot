using System;
namespace EmpiredBot
{
    [Serializable]
    public class MojoBillEnquiry
    {
        public string Name { get; internal set; }
        public string Month { get; internal set; }
        public string Year { get; internal set; }
    }
}