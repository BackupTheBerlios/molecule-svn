// Copyright (c) 2009 Pascal Fresnay (dev.molecule@free.fr) - Mickael Renault (dev.molecule@free.fr) 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;

using Molecule.Collections;

using com.drew.metadata;
using com.drew.metadata.iptc;
using com.drew.imaging.jpg;
using com.drew.lang;

namespace Molecule.Metadata
{
	public  class JpegMetadataReader
	{	
		Molecule.Collections.Dictionary<string, string> commonMetadatas;
		FileInfo jpegFileInfo;

        public static JpegMetadataReader RetreiveFromFile(string filePath)
        {
            var reader = new JpegMetadataReader(filePath);
            reader.RetrieveMetadatas();
            return reader;
        }

		public JpegMetadataReader(string filePath)
		{
			jpegFileInfo = new FileInfo(filePath); 
			
		}
		
		public void RetrieveMetadatas()
		{
			commonMetadatas = new Molecule.Collections.Dictionary<string,string>();
			
			// Loading all meta data
			com.drew.metadata.Metadata photoMedatas = com.drew.imaging.jpg.JpegMetadataReader.ReadMetadata(jpegFileInfo);	

			if (photoMedatas.ContainsDirectory( typeof(com.drew.metadata.exif.ExifDirectory)))
			{
				
				// Make
				AbstractDirectory exifDirectory =  photoMedatas.GetDirectory(typeof(com.drew.metadata.exif.ExifDirectory).ToString());
				if( exifDirectory.ContainsTag(com.drew.metadata.exif.ExifDirectory.TAG_MAKE))
				{
					commonMetadatas.Add(exifDirectory.GetTagName(com.drew.metadata.exif.ExifDirectory.TAG_MAKE), 
					                    exifDirectory.GetString( com.drew.metadata.exif.ExifDirectory.TAG_MAKE));
				}
				
				// Model
				if( exifDirectory.ContainsTag(com.drew.metadata.exif.ExifDirectory.TAG_MODEL))
				{
					commonMetadatas.Add(exifDirectory.GetTagName(com.drew.metadata.exif.ExifDirectory.TAG_MODEL), 
					                    exifDirectory.GetString( com.drew.metadata.exif.ExifDirectory.TAG_MODEL));
				}				
				
				// Model
				if( exifDirectory.ContainsTag(com.drew.metadata.exif.ExifDirectory.TAG_APERTURE))
				{
					commonMetadatas.Add(exifDirectory.GetTagName(com.drew.metadata.exif.ExifDirectory.TAG_APERTURE), 
					                    exifDirectory.GetDescription( com.drew.metadata.exif.ExifDirectory.TAG_APERTURE));
				}
				
				if( exifDirectory.ContainsTag(com.drew.metadata.exif.ExifDirectory.TAG_FLASH))
				{
					commonMetadatas.Add(exifDirectory.GetTagName(com.drew.metadata.exif.ExifDirectory.TAG_FLASH), 
					                    exifDirectory.GetDescription( com.drew.metadata.exif.ExifDirectory.TAG_FLASH));
				}				

				if( exifDirectory.ContainsTag(com.drew.metadata.exif.ExifDirectory.TAG_ISO_EQUIVALENT))
				{
					commonMetadatas.Add(exifDirectory.GetTagName(com.drew.metadata.exif.ExifDirectory.TAG_ISO_EQUIVALENT), 
					                    exifDirectory.GetString( com.drew.metadata.exif.ExifDirectory.TAG_ISO_EQUIVALENT));
				}					

				if( exifDirectory.ContainsTag(com.drew.metadata.exif.ExifDirectory.TAG_FOCAL_LENGTH))
				{
					commonMetadatas.Add(exifDirectory.GetTagName(com.drew.metadata.exif.ExifDirectory.TAG_FOCAL_LENGTH), 
					                    exifDirectory.GetString( com.drew.metadata.exif.ExifDirectory.TAG_FOCAL_LENGTH));
				}						
				
				
				if( exifDirectory.ContainsTag(com.drew.metadata.exif.ExifDirectory.TAG_FOCAL_LENGTH_IN_35MM_FILM))
				{
					commonMetadatas.Add(exifDirectory.GetTagName(com.drew.metadata.exif.ExifDirectory.TAG_FOCAL_LENGTH_IN_35MM_FILM), 
					                    exifDirectory.GetString( com.drew.metadata.exif.ExifDirectory.TAG_FOCAL_LENGTH_IN_35MM_FILM));
				}						
			}
			if (photoMedatas.ContainsDirectory( typeof(com.drew.metadata.exif.GpsDirectory)))
			{
				AbstractDirectory gpsDirectory =  photoMedatas.GetDirectory(typeof(com.drew.metadata.exif.GpsDirectory).ToString());
				if( gpsDirectory.ContainsTag(com.drew.metadata.exif.GpsDirectory.TAG_GPS_LATITUDE) && 
				   gpsDirectory.ContainsTag(com.drew.metadata.exif.GpsDirectory.TAG_GPS_LONGITUDE))
				{
					Rational[] latitudeRationals = gpsDirectory.GetRationalArray(com.drew.metadata.exif.GpsDirectory.TAG_GPS_LATITUDE);
					
					this.ContainsGPSInformation = true;
					
					this.Latitude = 
						(gpsDirectory.GetString( com.drew.metadata.exif.GpsDirectory.TAG_GPS_LONGITUDE_REF).Equals("N")? -1 : 1	)*
						ConvertDMSToDecimalDegrees(latitudeRationals[0].DoubleValue(), 
					                           latitudeRationals[1].DoubleValue(), 
					                           latitudeRationals[2].DoubleValue());
					Console.WriteLine(	this.Latitude);	
					
					Rational[] longitudeRationals = gpsDirectory.GetRationalArray(com.drew.metadata.exif.GpsDirectory.TAG_GPS_LONGITUDE);
					this.Longitude = 
						(gpsDirectory.GetString( com.drew.metadata.exif.GpsDirectory.TAG_GPS_LONGITUDE_REF).Equals("W")? -1 : 1	)*	
						ConvertDMSToDecimalDegrees(longitudeRationals[0].DoubleValue(), 
					                                           longitudeRationals[1].DoubleValue(), 
					                                           longitudeRationals[2].DoubleValue());	
					Console.WriteLine(this.Longitude);
					
				}
			}
		}

		private double ConvertDMSToDecimalDegrees(double degrees, double minutes, double seconds)
		{
			
			return (degrees + (minutes / (double)60.0) + (seconds / (double)3600.0));
		}		
		
		
		public IKeyedEnumerable<string,string> CommonMetadatas
		{
			get
			{
				return ( IKeyedEnumerable<string,string> ) commonMetadatas;
			}
			
		}
		
		
		public bool ContainsGPSInformation  
		{
			get;
			private set;
				
		}
		
		public double Longitude
		{
			
			private set;
			get;
		}
		
		public double Latitude
		{
			
			private set;
			get;
		}
				
	}
}
