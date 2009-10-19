
using System;

namespace Molecule.GConf
{

	/// <summary>
	/// This GConf helper must be used instead of gconf-sharp to avoid crash when accessing
	/// gconf out of a desktop environment.
	/// </summary>
	public static class Helper
	{

		public static string Read(string key)
		{
			try {
				var process = new System.Diagnostics.Process();
	            process.StartInfo.FileName = "gconftool-2";
	            process.StartInfo.Arguments = "-g "+key;
				process.StartInfo.UseShellExecute = false;
	            process.StartInfo.RedirectStandardOutput = true;
	            process.Start();
	            var output = process.StandardOutput.ReadToEnd();
	            process.WaitForExit();
	            return output.Trim();
			}
			catch {
				return null;	
			}
		}
	}
}
