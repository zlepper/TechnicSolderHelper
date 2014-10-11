using System;

namespace TechnicSolderHelper
{
	public class globalfunctions
	{
		public globalfunctions ()
		{
		}

		public static Boolean isUnix() {
			return Environment.OSVersion.ToString ().ToLower ().Contains ("unix");
		}
	}
}

