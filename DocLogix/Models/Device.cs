using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DocLogix.Models
{
    public class Device
    {
        private string _deviceVendor;
        private string _deviceProduct;
        private string _deviceVersion;
        private string _signatureId;
        private string _severity;
        private string _name;
        private string _start;
        private string _rt;
        private string _msg;
        private string _shost;
        private string _smac;
        private string _dhost;
        private string _dmac;
        private string _suser;
        private string _suid;
        private string _externalId;
        private string _cs1Label;
        private string _cs1;

        public Device()
        {

        }

        public Device(string deviceVendor, string deviceProduct, string deviceVersion, string signatureId, string severity, string name, string start, string rt, string msg, string shost, string smac, string dhost, string dmac, string suser, string suid, string externalId, string cs1Label, string cs1)
        {
            DeviceVendor = deviceVendor;
            DeviceProduct = deviceProduct;
            DeviceVersion = deviceVersion;
            SignatureId = signatureId;
            Severity = severity;
            Name = name;
            Start = start;
            Rt = rt;
            Msg = msg;
            Shost = shost;
            Smac = smac;
            Dhost = dhost;
            Dmac = dmac;
            Suser = suser;
            Suid = suid;
            ExternalId = externalId;
            Cs1Label = cs1Label;
            Cs1 = cs1;
        }

        public string DeviceVendor { get => _deviceVendor; set => _deviceVendor = value; }
        public string DeviceProduct { get => _deviceProduct; set => _deviceProduct = value; }
        public string DeviceVersion { get => _deviceVersion; set => _deviceVersion = value; }
        public string SignatureId { get => _signatureId; set => _signatureId = value; }
        public string Severity { get => _severity; set => _severity = value; }
        public string Name { get => _name; set => _name = value; }
        public string Start { get => _start; set => _start = value; }
        public string Rt { get => _rt; set => _rt = value; }
        public string Msg { get => _msg; set => _msg = value; }
        public string Shost { get => _shost; set => _shost = value; }
        public string Smac { get => _smac; set => _smac = value; }
        public string Dhost { get => _dhost; set => _dhost = value; }
        public string Dmac { get => _dmac; set => _dmac = value; }
        public string Suser { get => _suser; set => _suser = value; }
        public string Suid { get => _suid; set => _suid = value; }
        public string ExternalId { get => _externalId; set => _externalId = value; }
        public string Cs1Label { get => _cs1Label; set => _cs1Label = value; }
        public string Cs1 { get => _cs1; set => _cs1 = value; }

        public override string ToString()
        {
            return DeviceVendor + " " + DeviceProduct + " " + DeviceVersion + " " + SignatureId;
        }
    }
}
