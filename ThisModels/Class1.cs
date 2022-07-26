using System.Net.NetworkInformation;

namespace ThisModels
{
    public class Class1
    {
        public string Info1 { get; set; } //IP
        public string[] Info2 { get; set; } //addrr fisicos
        public string Info3 { get; set; } //mac1
        public PhysicalAddress Info4 { get; set; } //mac2 physical add
        public string Info5 { get; set; } //mac3
        public string Info6 { get; set; } //mac4
        public string Info7 { get; set; } //mac5
        public ICollection<string> Info8 { get; set; } //mac6
        public string[] Info9 { get; set; } //desk files
        public string[] Info10 { get; set; } //my docs files
    }
}