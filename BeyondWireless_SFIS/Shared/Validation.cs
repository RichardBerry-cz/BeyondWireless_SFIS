using System;
using System.Text.RegularExpressions;
using BeyondWireless_SFIS.Models;

namespace BeyondWireless_SFIS.Shared
{
    public class Validation
    {
        public static bool IsValidDate(string dateString)
        {
            DateTime dateValue;
            bool isValid = DateTime.TryParse(dateString, out dateValue);
            return isValid;
        }

        public static bool IsValidNumber(string numberString)
        {
            double numberValue;
            bool isValid = double.TryParse(numberString, out numberValue);
            return isValid;
        }

        public static bool IsValidEmail(string emailString)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(emailString);
                return !string.IsNullOrEmpty(addr.Address);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPanelSerial(string serialNumber)
        {
            // 13477131T42567CS00412
            // 13477123T12569HK00036
            // 13656042FW258DCS00001
            return Regex.IsMatch(serialNumber, @"^\d{8}\w{8}\d{5}$");
        }

        public static CkdPcbaSerialInfo IsValidCkdPcbaSerial(string input)
        {
            var serialInfo = new CkdPcbaSerialInfo();

            // Format: G0301-000624-Z00*80087*00025*2542*ELJ000002A*85P8K;
            // Format: G0301-000624-Z00*80087*00025*2542*ELJ000002A*85P8K;

            string pattern = @"^(?<partNumber>[A-Z0-9]{5}-[A-Z0-9]{6}-[A-Z0-9]{3})\*(?<pid>[A-Z0-9]{2,7})\*(?<serial>(?!0{5})[0-9]{5})\*(?<dateCode>[0-9]{4})\*(?<odf>[A-Z]{3}[0-9]{5}[A-Z0-9]{2})\*(?<model>[A-Z0-9]{5,7});$";

            //Match match = Regex.Match(input,
            //    @"^(?<partNumber>[A-Z0-9]+(?:-[A-Z0-9]+)+)\*" +  // PartNumber
            //    @"(?<pid>[0-9]{5})\*" +                          // PID
            //    @"(?<serialPart>[0-9]{5})\*" +                   // SerialPart
            //    @"(?<dateCode>(2[0-9]|3[0-9])(0[1-9]|[1-4][0-9]|5[0-3])\*" + // DateCode YYWW (20-39YY, 01-53WW)
            //    @"(?<odf>[A-Z0-9]{8,10})\*" +                    // Odf
            //    @"(?<model>[A-Z0-9]{5});$",                      // Model
            //    RegexOptions.None);

            Match match = Regex.Match(input, pattern, RegexOptions.None);

            if (match.Success)
            {
                serialInfo.IsValid = true;
                serialInfo.SerialNumber = input;
                serialInfo.PartNumber = match.Groups["partNumber"].Value;
                serialInfo.Pid = match.Groups["pid"].Value;
                serialInfo.SerialPart = match.Groups["serial"].Value;
                serialInfo.DateCode = match.Groups["dateCode"].Value;
                serialInfo.Odf = match.Groups["odf"].Value;
                serialInfo.Model = match.Groups["model"].Value;
            }
            else
            {
                serialInfo.IsValid = false;
            }

            return serialInfo;
        }

        public static ModuleSerialInfo IsValidPcbaSerial(string input)
        {
            //Internal PCBA Serial: G0301-000624-Z00*211070*00217*2533*ELJ243687B*55P8K;
            //Internal PCBA Serial: G0301-000624-Z00*211070*00217*2533*ELJ243687B*75A400U;

            ModuleSerialInfo serialInfo = new ModuleSerialInfo();

            Match match = Regex.Match(input, @"^(?<partNumber>[A-Z0-9]{5}-[A-Z0-9]{6}-[A-Z0-9]{3})\*(?<pid>[A-Z0-9]{2,7})\*(?<serial>(?!0{5})[0-9]{5})\*(?<dateCode>[0-9]{4})\*(?<odf>[A-Z]{3}[0-9]{5}[A-Z0-9]{2})\*(?<model>[A-Z0-9]{5,7});$");

            if (match.Success)
            {
                serialInfo.IsValid = true;
                serialInfo.SerialNumber = input;
                serialInfo.PartNumber = match.Groups["partNumber"].Value;
                serialInfo.Pid = match.Groups["pid"].Value.ToString();
                serialInfo.SerialPart = match.Groups["serial"].Value;
                serialInfo.Odf = match.Groups["odf"].Value;
            }
            else
            {
                serialInfo.IsValid = false;
            }
            return serialInfo;
        }

        //public static VehicleLicenceDiscInfo GetLicenceDiscInfoFromBarcode(string barcode)
        //{
        //    var match = Regex.Match(barcode, @"^\s*%(?<BarcodeVersion>[^%]*)%(?<AuthorityCode>[^%]*)%(?<TransactionCode>[^%]*)%(?<StatusCode>[^%]*)%(?<DiscNumber>[^%]*)%(?<RegistrationNumber>[^%]*)%(?<VehicleRegisterNumber>[^%]*)%(?<Description>[^%]*)( / )\w+%(?<Make>[^%]*)%(?<Model>[^%]*)%(?<Colour>[^%]*)( / )\w+%(?<VIN>[^%]*)%(?<EngineNumber>[^%]*)%(?<ExpiryDate>[^%]*)%\s*$");
        //    if (match.Success)
        //    {
        //        return new VehicleLicenceDiscInfo()
        //        {
        //            IsValid = true,
        //            BarcodeVersion = match.Groups["BarcodeVersion"].Value,
        //            AuthorityCode = match.Groups["AuthorityCode"].Value,
        //            TransactionCode = match.Groups["TransactionCode"].Value,
        //            StatusCode = match.Groups["StatusCode"].Value,
        //            DiscNumber = match.Groups["DiscNumber"].Value,
        //            RegistrationNumber = match.Groups["RegistrationNumber"].Value,
        //            VehicleRegisterNumber = match.Groups["VehicleRegisterNumber"].Value,
        //            Description = match.Groups["Description"].Value,
        //            Make = match.Groups["Make"].Value,
        //            Model = match.Groups["Model"].Value,
        //            Colour = match.Groups["Colour"].Value,
        //            VIN = match.Groups["VIN"].Value,
        //            EngineNumber = match.Groups["EngineNumber"].Value,
        //            ExpiryDate = match.Groups["ExpiryDate"].Value

        //        };
        //    }
        //    else
        //    {
        //        return new VehicleLicenceDiscInfo { IsValid = false };
        //    }
        //}
    }
}
