using Android.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DLuOvBamG.Services
{
    public class GeoService
    {
        public Point GetGeoLocations(string filePath)
        {
            Point point = new Point();
            if (!File.Exists(filePath))
            {
                throw new ArgumentException("file does not exist path:{0}", filePath);
            }
            ExifInterface exif = new ExifInterface(filePath);
            point = GetGeoDegrees(exif);
            return point;

        }
        Point GetGeoDegrees(ExifInterface exif)
        {
            double Latitude;
            double Longitude;
            String attrLATITUDE = exif.GetAttribute(ExifInterface.TagGpsLatitude);
            String attrLATITUDE_REF = exif.GetAttribute(ExifInterface.TagGpsLatitudeRef);
            String attrLONGITUDE = exif.GetAttribute(ExifInterface.TagGpsLongitude);
            String attrLONGITUDE_REF = exif.GetAttribute(ExifInterface.TagGpsLongitudeRef);

            if ((attrLATITUDE != null)
              && (attrLATITUDE_REF != null)
              && (attrLONGITUDE != null)
              && (attrLONGITUDE_REF != null))
            {
                
                if (attrLATITUDE_REF.Equals("N"))
                {
                    Latitude = convertToDegree(attrLATITUDE);
                }
                else
                {
                    Latitude = 0 - convertToDegree(attrLATITUDE);
                }

                if (attrLONGITUDE_REF.Equals("E"))
                {
                    Longitude = convertToDegree(attrLONGITUDE);
                }
                else
                {
                    Longitude = 0 - convertToDegree(attrLONGITUDE);
                }

                return new Point(Latitude, Longitude);
            }
            return new Point();
        }

        private Double convertToDegree(String stringDMS)
        {
            // stringDMS is rational. Format is "num1/denom1,num2/denom2,num3/denom3".
            Double result;
            String[] DMS = stringDMS.Split(',');

            String[] stringD = DMS[0].Split('/');
            Double D0 = Convert.ToDouble(stringD[0]);
            Double D1 = Convert.ToDouble(stringD[1]);
            Double FloatD = D0 / D1;

            String[] stringM = DMS[1].Split('/');
            Double M0 = Convert.ToDouble(stringM[0]);
            Double M1 = Convert.ToDouble(stringM[1]);
            Double FloatM = M0 / M1;

            String[] stringS = DMS[2].Split('/');
            Double S0 = Convert.ToDouble(stringS[0]);
            Double S1 = Convert.ToDouble(stringS[1]);
            Double FloatS = S0 / S1;

            result = FloatD + (FloatM / 60) + (FloatS / 3600);

            return result;
        }

        public async Task<Placemark> GetPlacemark(double latitude, double longitude)
        {
            Placemark empty = new Placemark();
            try
            {
                IEnumerable<Placemark> placemarks = await Geocoding.GetPlacemarksAsync(latitude, longitude);
                Placemark placemark = placemarks?.FirstOrDefault();;
                return placemark;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                return empty;
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                return empty;
                // Handle exception that may have occurred in geocoding
            }
        }
    }
}
