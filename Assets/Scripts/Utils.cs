using UnityEngine;
using System.Collections;
using System;

public class AssertionException : Exception
{
	public AssertionException(Func<bool> assertion) : base("Assertion failed")
	{
	}
}

public static class Utils
{
	public static void assert(Func<bool> assertion)
	{
		if (!assertion.Invoke()) throw new AssertionException(assertion);
	}
}
