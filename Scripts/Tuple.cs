using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]

public class Tuple<T1, T2>
{
	public T1 thing1;
	public T2 thing2;
	
	private static readonly IEqualityComparer Item1Comparer = EqualityComparer<T1>.Default;
	private static readonly IEqualityComparer Item2Comparer = EqualityComparer<T2>.Default;
	
	public Tuple(T1 thing1, T2 thing2)
	{
		this.thing1 = thing1;
		this.thing2 = thing2;
	}
	
	public override string ToString()
	{
		return string.Format("<{0}, {1}>", thing1, thing2);
	}
	
	public static bool operator ==(Tuple<T1, T2> a, Tuple<T1, T2> b)
	{
		if (Tuple<T1, T2>.IsNull(a) && !Tuple<T1, T2>.IsNull(b))
			return false;
		
		if (!Tuple<T1, T2>.IsNull(a) && Tuple<T1, T2>.IsNull(b))
			return false;
		
		if (Tuple<T1, T2>.IsNull(a) && Tuple<T1, T2>.IsNull(b))
			return true;
		
		return a.thing1.Equals(b.thing1) && a.thing2.Equals(b.thing2);
	}
	
	public static bool operator !=(Tuple<T1, T2> a, Tuple<T1, T2> b)
	{
		return !(a == b);
	}
	
	public override int GetHashCode()
	{
		int hash = 17;
		hash = hash * 23 + thing1.GetHashCode();
		hash = hash * 23 + thing2.GetHashCode();
		return hash;
	}
	
	public override bool Equals(object obj)
	{
		var other = obj as Tuple<T1, T2>;
		if (object.ReferenceEquals(other, null))
			return false;
		else
			return Item1Comparer.Equals(thing1, other.thing1) &&
				Item2Comparer.Equals(thing2, other.thing2);
	}
	
	private static bool IsNull(object obj)
	{
		return object.ReferenceEquals(obj, null);
	}
}
