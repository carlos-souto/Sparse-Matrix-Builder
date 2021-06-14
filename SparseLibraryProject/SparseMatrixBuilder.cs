using scg = System.Collections.Generic;

namespace SparseLibrary
{
	public class SparseMatrixBuilder
	{
		#region Private Readonly Instance Fields
		private readonly scg::Dictionary<(uint, uint), double> data;
		private readonly uint numberOfRows, numberOfColumns;
		#endregion

		#region Public Readonly Instance Properties
		public uint NumberOfRows => numberOfRows;
		public uint NumberOfColumns => numberOfColumns;
		public uint NumberOfNonZeros => (uint)data.Count;
		#endregion

		#region Public Instance Constructor
		public SparseMatrixBuilder(uint numberOfRows, uint numberOfColumns)
		{
			this.numberOfRows = numberOfRows;
			this.numberOfColumns = numberOfColumns;
			this.data = new scg::Dictionary<(uint, uint), double>();
		}
		#endregion

		#region Public Instance Methods
		public double[,] ToDense()
		{
			double[,] dense = new double[numberOfRows, numberOfColumns];
			foreach (var keyValuePair in data)
			{
				var row = keyValuePair.Key.Item1;
				var col = keyValuePair.Key.Item2;
				var val = keyValuePair.Value;
				dense[row, col] = val;
			}
			return dense;
		}
		public (uint[], uint[], double[]) ToCOO(uint indexBase = 0)
		{
			var rows = new uint[NumberOfNonZeros];
			var cols = new uint[NumberOfNonZeros];
			var vals = new double[NumberOfNonZeros];

			uint i = 0;
			foreach (var keyValuePair in data)
			{
				rows[i] = keyValuePair.Key.Item1 + indexBase;
				cols[i] = keyValuePair.Key.Item2 + indexBase;
				vals[i] = keyValuePair.Value;
				i++;
			}
			return (rows, cols, vals);
		}
		public double Get(uint row, uint column)
		{
			CheckIndexOutOfRangeException(row, column);

			(uint, uint) key = (row, column);

			if (data.ContainsKey(key))
				return data[key];
			else
				return 0.0;
		}
		public void Set(uint row, uint column, double value)
		{
			CheckIndexOutOfRangeException(row, column);

			(uint, uint) key = (row, column);

			if (value == 0.0)
			{
				if (data.ContainsKey(key))
					data.Remove(key);
			}
			else
			{
				if (data.ContainsKey(key))
					data[key] = value;
				else
					data.Add(key, value);
			}
		}
		public void SetDiagonal(double value)
		{
			uint numberOfDiagonalElements = numberOfRows < numberOfColumns ? numberOfRows : numberOfColumns;

			if (value == 0.0)
			{
				for (uint i = 0; i < numberOfDiagonalElements; i++)
					if (data.ContainsKey((i, i)))
						data.Remove((i, i));
			}
			else
			{
				for (uint i = 0; i < numberOfDiagonalElements; i++)
					if (data.ContainsKey((i, i)))
						data[(i, i)] = value;
					else
						data.Add((i, i), value);
			}
		}
		public double[] GetDiagonal()
		{
			uint numberOfDiagonalElements = numberOfRows < numberOfColumns ? numberOfRows : numberOfColumns;

			double[] diagonal = new double[numberOfDiagonalElements];
			foreach (var keyValuePair in data)
				if (keyValuePair.Key.Item1 == keyValuePair.Key.Item2)
					diagonal[keyValuePair.Key.Item1] = keyValuePair.Value;
			return diagonal;
		}
		public void SetRow(uint row, double value)
		{
			if (value == 0.0)
			{
				for (uint i = 0; i < numberOfColumns; i++)
					if (data.ContainsKey((row, i)))
						data.Remove((row, i));
			}
			else
			{
				for (uint i = 0; i < numberOfColumns; i++)
					if (data.ContainsKey((row, i)))
						data[(row, i)] = value;
					else
						data.Add((row, i), value);
			}
		}
		public double[] GetRow(uint row)
		{
			double[] values = new double[numberOfColumns];

			foreach (var keyValuePair in data)
				if (keyValuePair.Key.Item1 == row)
					values[keyValuePair.Key.Item2] = keyValuePair.Value;
			return values;
		}
		public void SetColumn(uint column, double value)
		{
			if (value == 0.0)
			{
				for (uint i = 0; i < numberOfRows; i++)
					if (data.ContainsKey((i, column)))
						data.Remove((i, column));
			}
			else
			{
				for (uint i = 0; i < numberOfRows; i++)
					if (data.ContainsKey((i, column)))
						data[(i, column)] = value;
					else
						data.Add((i, column), value);
			}
		}
		public double[] GetColumn(uint column)
		{
			double[] values = new double[numberOfRows];

			foreach (var keyValuePair in data)
				if (keyValuePair.Key.Item2 == column)
					values[keyValuePair.Key.Item1] = keyValuePair.Value;
			return values;
		}
		#endregion

		#region Private Instance Methods
		private void CheckIndexOutOfRangeException(uint row, uint column)
		{
			if (row >= numberOfRows || column >= numberOfColumns)
				throw new System.IndexOutOfRangeException();
		}
		#endregion
	}
}
