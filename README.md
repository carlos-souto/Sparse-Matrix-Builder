# Sparse-Matrix-Builder

### By Carlos Souto (csouto@fe.up.pt)

Much faster routines for creating and build huge sparse matrices in MATLAB.

Usage example:

```matlab
% reset workspace
clear all; close all; clc;

% load the .NET assembly
dll = [pwd '\SparseLibrary.dll'];
assembly = NET.addAssembly(dll);

% create a sparse matrix builder
m = 8;
n = 4;
matrix = SparseMatrixBuilder(m, n);

% set some values
matrix.Set(2, 3, 5.0);
matrix.Set(2, 4, 6.0);
matrix.Set(7, 1, 4.0);
matrix.Set(7, 3, 9.0);
%matrix.Set(9, 3, 1.0); % Error: Index was outside the bounds of the array.

% get some values (print)
matrix.Get(7, 1)
matrix.Get(1, 1)
%matrix.Get(8, 5) % Error: Index was outside the bounds of the array.

% set all values in the diagonal
matrix.SetDiagonal(5.0)

% get the diagonal (print)
matrix.GetDiagonal()

% set and get a row
matrix.SetRow(2, 6.0);
matrix.GetRow(2) % will print

% set and get a column
matrix.SetColumn(2, 8.0);
matrix.GetColumn(2) % will print

% to dense (print)
matrix.ToDense()

% to COO format (print)
matrix.ToCOO()

% to MATLAB sparse (print)
matrix.ToMatlabSparse()
```

![a](https://user-images.githubusercontent.com/83190503/121953799-eb634d80-cd55-11eb-9835-818a1ad1b818.png)
