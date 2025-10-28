using System;
using System.Collections.Generic;

public static class Utils
{
	private static readonly Random _random = new();
	
	public static void Shuffle<T>(this IList<T> list)  
	{  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = _random.Next(n + 1);  
			(list[k], list[n]) = (list[n], list[k]);
		}  
	}
}
