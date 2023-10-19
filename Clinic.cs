using System;
namespace ClinicalCoordinationApplication
{
    public interface IClinic
    {
        string ClinicName { get; }
        int Distance { get; }
    }

    public class Clinic : IClinic
    {
        private readonly string _clinicName;
        private readonly int _distance;

        public Clinic(string clinicName, int distance)
        {
            _clinicName = clinicName;
            _distance = distance;
        }

        public string ClinicName => _clinicName;

        public int Distance => _distance;
    }

}

