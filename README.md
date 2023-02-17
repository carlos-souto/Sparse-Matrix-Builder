# Sparse-Matrix-Builder
Build large sparse matrices faster in Matlab!

Usage:
```
% reset workspace
clear, clc, close all

% example sparse matrix:
% | 1.0 0.0 0.0 0.0 2.0 |
% | 0.0 3.0 0.0 4.0 0.0 |
% | 0.0 0.0 0.0 0.0 0.0 |
% | 9.0 2.0 0.0 0.0 2.0 |

% approach 1 (slower)
% add element by element
smb = SparseMatrixBuilder(4, 5);
smb.add_val(1, 1, 1.0);
smb.add_val(1, 5, 2.0);
smb.add_val(2, 2, 3.0);
smb.add_val(2, 4, 4.0);
smb.add_val(4, 1, 4.0); % notice that
smb.add_val(4, 1, 5.0); % repeated entries are added
smb.add_val(4, 2, 2.0);
smb.add_val(4, 5, 2.0);
smb.squeeze(); % release unused storage
sparse_matrix = smb.to_matlab()

% approach 2 (faster)
% add multiple elements
smb = SparseMatrixBuilder(4, 5);
smb.add_vals([1, 1, 2], [1, 5, 2], [1.0, 2.0, 3.0]);
smb.add_vals([2, 4, 4, 4], [4, 1, 1, 2], [4.0, 4.0, 5.0, 2.0]);
smb.add_val(4, 5, 2.0); % or smb.add_vals([4], [5], [2.0]);
smb.squeeze(); % release unused storage
sparse_matrix = smb.to_matlab()

% adding multiple values is faster than adding each value individually
```

Notice that repeated entries are added together.

It is faster to set multiple values at once using `add_vals`.

Nevertheless, this implementation should perform better than Matlab's `sparse` during a loop building process.
