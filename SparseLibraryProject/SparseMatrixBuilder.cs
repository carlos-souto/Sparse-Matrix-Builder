using scg = System.Collections.Generic;

namespace SparseLibrary
{
	public class SparseMatrixBuilder
	{
		#region Private Readonly Instance Fields
		private readonly scg::Dictionary<(int, int), double> data;
		private readonly int numberOfRows, numberOfColumns;
		#endregion

		#region Public Readonly Instance Properties
		public int NumberOfRows => numberOfRows;
		public int NumberOfColumns => numberOfColumns;
		public int NumberOfNonZeros => data.Count;
		#endregion

		#region Public Instance Constructor
		public SparseMatrixBuilder(int numberOfRows, int numberOfColumns)
		{
			this.numberOfRows = numberOfRows;
			this.numberOfColumns = numberOfColumns;
			this.data = new scg::Dictionary<(int, int), double>();
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
		public (int[], int[], double[]) ToCOO(int indexBase = 0)
		{
			var rows = new int[NumberOfNonZeros];
			var cols = new int[NumberOfNonZeros];
			var vals = new double[NumberOfNonZeros];

			int i = 0;
			foreach (var keyValuePair in data)
			{
				rows[i] = keyValuePair.Key.Item1 + indexBase;
				cols[i] = keyValuePair.Key.Item2 + indexBase;
				vals[i] = keyValuePair.Value;
				i++;
			}
			return (rows, cols, vals);
		}
		public double Get(int row, int column)
		{
			CheckIndexOutOfRangeException(row, column);

			(int, int) key = (row, column);

			if (data.ContainsKey(key))
				return data[key];
			else
				return 0.0;
		}
		public double[] Get(int[] rows, int[] columns)
		{
			if (rows.Length != columns.Length)
				throw new System.ArgumentException();

			double[] values = new double[rows.Length];

			for (int i = 0; i < values.Length; i++)
			{
				int row = rows[i];
				int column = columns[i];

				CheckIndexOutOfRangeException(row, column);

				(int, int) key = (row, column);

				if (data.ContainsKey(key))
					values[i] = data[key];
			}

			return values;
		}
		public void Set(int row, int column, double value)
		{
			CheckIndexOutOfRangeException(row, column);

			(int, int) key = (row, column);

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
		public void Set(int[] rows, int[] columns, double[] values)
		{
			if (rows.Length != columns.Length || rows.Length != values.Length)
				throw new System.ArgumentException();

			for (int i = 0; i < values.Length; i++)
			{
				int row = rows[i];
				int column = columns[i];
				double value = values[i];

				CheckIndexOutOfRangeException(row, column);

				(int, int) key = (row, column);

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
		}
		public void Add(int row, int column, double value)
		{
			CheckIndexOutOfRangeException(row, column);

			(int, int) key = (row, column);

			if (value != 0.0)
			{
				if (data.ContainsKey(key))
					data[key] += value;
				else
					data.Add(key, value);
			}
		}
		public void Add(int[] rows, int[] columns, double[] values)
		{
			if (rows.Length != columns.Length || rows.Length != values.Length)
				throw new System.ArgumentException();

			for (int i = 0; i < values.Length; i++)
			{
				int row = rows[i];
				int column = columns[i];
				double value = values[i];

				CheckIndexOutOfRangeException(row, column);

				(int, int) key = (row, column);

				if (value != 0.0)
				{
					if (data.ContainsKey(key))
						data[key] += value;
					else
						data.Add(key, value);
				}
			}
		}
		public void SetDiagonal(double value)
		{
			int numberOfDiagonalElements = numberOfRows < numberOfColumns ? numberOfRows : numberOfColumns;

			if (value == 0.0)
			{
				for (int i = 0; i < numberOfDiagonalElements; i++)
					if (data.ContainsKey((i, i)))
						data.Remove((i, i));
			}
			else
			{
				for (int i = 0; i < numberOfDiagonalElements; i++)
					if (data.ContainsKey((i, i)))
						data[(i, i)] = value;
					else
						data.Add((i, i), value);
			}
		}
		public double[] GetDiagonal()
		{
			int numberOfDiagonalElements = numberOfRows < numberOfColumns ? numberOfRows : numberOfColumns;

			double[] diagonal = new double[numberOfDiagonalElements];
			foreach (var keyValuePair in data)
				if (keyValuePair.Key.Item1 == keyValuePair.Key.Item2)
					diagonal[keyValuePair.Key.Item1] = keyValuePair.Value;
			return diagonal;
		}
		public void SetRow(int row, double value)
		{
			if (value == 0.0)
			{
				for (int i = 0; i < numberOfColumns; i++)
					if (data.ContainsKey((row, i)))
						data.Remove((row, i));
			}
			else
			{
				for (int i = 0; i < numberOfColumns; i++)
					if (data.ContainsKey((row, i)))
						data[(row, i)] = value;
					else
						data.Add((row, i), value);
			}
		}
		public double[] GetRow(int row)
		{
			double[] values = new double[numberOfColumns];

			foreach (var keyValuePair in data)
				if (keyValuePair.Key.Item1 == row)
					values[keyValuePair.Key.Item2] = keyValuePair.Value;
			return values;
		}
		public void SetColumn(int column, double value)
		{
			if (value == 0.0)
			{
				for (int i = 0; i < numberOfRows; i++)
					if (data.ContainsKey((i, column)))
						data.Remove((i, column));
			}
			else
			{
				for (int i = 0; i < numberOfRows; i++)
					if (data.ContainsKey((i, column)))
						data[(i, column)] = value;
					else
						data.Add((i, column), value);
			}
		}
		public double[] GetColumn(int column)
		{
			double[] values = new double[numberOfRows];

			foreach (var keyValuePair in data)
				if (keyValuePair.Key.Item2 == column)
					values[keyValuePair.Key.Item1] = keyValuePair.Value;
			return values;
		}
		#endregion

		#region Private Instance Methods
		private void CheckIndexOutOfRangeException(int row, int column)
		{
			if (row < 0 || row >= numberOfRows || column < 0 || column >= numberOfColumns)
				throw new System.IndexOutOfRangeException();
		}
		#endregion
	}
}
